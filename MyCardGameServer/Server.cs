using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Models;
using System.Configuration;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MyCardGameServer
{
    public class Server
    {
        private List<SocketState> clients;
        private Dictionary<SocketState, Player> playerDic;
        private string sqlConnectionString;
        private GameDictionary GameDic;

        public Server()
        {
            clients = new List<SocketState>();
            playerDic = new Dictionary<SocketState, Player>();
            NetworkController.ServerAwaitingClientLoop(FirstContact);
            GameDic = JsonConvert.DeserializeObject<GameDictionary>(JToken.Parse(File.ReadAllText("GameDic.json")).ToString());
            GameDictionary.GameDic = GameDic;
            sqlConnectionString = ConfigurationManager.ConnectionStrings["MyCardGameServer.Properties.Settings.GameDatabaseConnectionString"].ConnectionString;
            Console.WriteLine("Waiting for first connection...");
        } 

        private void FirstContact(SocketState ss)
        {
            Console.WriteLine("A new client has contacted the server.");
            lock (clients)
            {
                clients.Add(ss);
            }
            ss.CallBackFunction = WaitForClientData;
            NetworkController.getData(ss);
        }
        
        private void WaitForClientData(SocketState ss)
        {
            string s = ss.SB.ToString();
            ss.SB.Clear();
            try
            {
                PlayerTransferModel player = JsonConvert.DeserializeObject<PlayerTransferModel>(s);
                if (player == null)
                {
                    throw new Exception();
                }
                
                switch (player.TransferRequest)
                {
                    case PlayerTransferModel.TransferRequestType.Login:
                        ClientLogin(player, ss);
                        break;
                    case PlayerTransferModel.TransferRequestType.EnterDungeon:
                        ClientEnterDungeon(player, ss);
                        break;
                    case PlayerTransferModel.TransferRequestType.EnterDungeonRoom:
                        ClientEnterDungeonRoom(player, ss);
                        break;
                    default:
                        throw new Exception();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("A client has sent illegal data");
                PlayerTransferModel player = new PlayerTransferModel();
                player.TransferState = PlayerTransferModel.TransferStateType.Error;
                player.TransferMessage = "Illegal data";
                NetworkController.Send(ss, JsonConvert.SerializeObject(player));
            }
            finally
            {
                NetworkController.getData(ss);
            }
        }

        private void ClientLogin(PlayerTransferModel player,SocketState ss)
        {
            if (player.AccountName != null && player.Password != null)
            {
                player.AccountName = player.AccountName.ToLower();
                Console.WriteLine("New client wants to login as " + player.AccountName + ".");

                String query = String.Format("SELECT * FROM PlayerAccountData WHERE AccountName = '{0}' AND Password = '{1}'", player.AccountName,player.Password);
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    player.PlayerName = reader["PlayerName"].ToString();
                                    Console.WriteLine(String.Format("New Client successfully login as {0}({1}).",player.AccountName,player.PlayerName));
                                    player.Password = null;
                                    player.TransferState = PlayerTransferModel.TransferStateType.Accept;

                                }
                            }
                            else
                            {
                                Console.WriteLine("A client wants to login with wrong AccountName/Password pair");
                                player = new PlayerTransferModel();
                                player.TransferState = PlayerTransferModel.TransferStateType.Decline;
                                player.TransferMessage = "Wrong AccountName/Password pair";
                                NetworkController.Send(ss, JsonConvert.SerializeObject(player));
                                return;
                            }
                        }
                    }

                    player.PlayerHeroList = new List<string>();

                    query = String.Format("SELECT * FROM PlayerHeroData WHERE PlayerName = '{0}'", player.PlayerName);
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    player.PlayerHeroList.Add(reader["HeroName"].ToString());
                                }
                            }
                        }
                    }

                    player.PlayerCardList = new Dictionary<string, int>();

                    query = String.Format("SELECT * FROM PlayerCardData WHERE PlayerName = '{0}'", player.PlayerName);
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    player.PlayerCardList.Add(reader["CardName"].ToString(),Convert.ToInt32(reader["CardCount"].ToString()));
                                }
                            }
                        }
                    }

                    connection.Close();
                }

                Console.WriteLine(String.Format("User data has sent to {0}({1}), waiting for client action.", player.AccountName, player.PlayerName));
                NetworkController.Send(ss, JsonConvert.SerializeObject(player));
                Player p = new Player(player);
                lock (playerDic)
                {
                    playerDic.Add(ss, p);
                }
            }
            else
            {
                throw new Exception();
            }
        }

        private void ClientEnterDungeon(PlayerTransferModel player, SocketState ss)
        {
            Console.WriteLine(String.Format("{0} wants to enter {1}.", playerDic[ss].PlayerName, player.TransferMessage));
            if (playerDic.ContainsKey(ss) && player.TransferMessage != null && player.CardPlayer != null && GameDic.DungeonDic.ContainsKey(player.TransferMessage))
            {
                int count = 0;
                string heroName="";
                foreach (var item in player.CardPlayer.CardDic)
                {
                    count += item.Value;
                    if (!GameDic.CardDic.ContainsKey(item.Key))
                    {
                        throw new Exception();
                    }
                    
                    if (heroName == "")
                    {
                        heroName = GameDic.CardDic[item.Key].Owner;
                    }
                    else if (heroName != GameDic.CardDic[item.Key].Owner)
                    {
                        throw new Exception();
                    }

                    if (GameDic.CardDic[item.Key].CardRarity == Card.Rarity.Basic)
                    {
                        if (!GameDic.HeroDic[heroName].HeroBasicCard.ContainsKey(item.Key) || GameDic.HeroDic[heroName].HeroBasicCard[item.Key]<item.Value)
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        if (!playerDic[ss].HasCard(item.Key, item.Value))
                        {
                            throw new Exception();
                        }
                    }
                }
                if (count != GameDic.DungeonDic[player.TransferMessage].DungeonAllowedCardCount)
                {
                    throw new Exception();
                }
                if (!playerDic[ss].HasHero(heroName) || heroName != player.CardPlayer.MainHero)
                {
                    throw new Exception();
                }
                if (player.CardPlayer.SubHero != null && !playerDic[ss].HasHero(player.CardPlayer.SubHero))
                {
                    throw new Exception();
                }
                var temp = GameDic.DungeonDic[player.TransferMessage].Generate();
                if (!playerDic[ss].EnterDungeon(temp, player.CardPlayer))
                {
                    throw new Exception();
                }

                player.TransferState = PlayerTransferModel.TransferStateType.Accept;
                player.Dungeon = temp;
                string s = JsonConvert.SerializeObject(player);
                NetworkController.Send(ss, JsonConvert.SerializeObject(player));
                Console.WriteLine(String.Format("{0} enters {1} successfully.", playerDic[ss].PlayerName, player.TransferMessage));
            }
            else
            {
                throw new Exception();
            }
        }

        private void ClientEnterDungeonRoom(PlayerTransferModel player, SocketState ss)
        {
            Console.WriteLine(String.Format("{0} wants to enter room.", playerDic[ss].PlayerName, player.TransferMessage));
            if (playerDic.ContainsKey(ss) && player.TransferMessage != null)
            {
                int i = Convert.ToInt32(player.TransferMessage);
                if (playerDic[ss].EnterDungeonRoom(i))
                {
                    var room = playerDic[ss].GetCurrentRoom();
                    if (room is MonsterRoom)
                    {
                        playerDic[ss].EnterBattle();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new Exception();
            }
        }

        private void DisconnectClient(SocketState ss)
        {
            ss.TheSocket.Close();
            ss.TheSocket.Dispose();
            if (playerDic.ContainsKey(ss))
            {
                playerDic.Remove(ss);
            }
            clients.Remove(ss);
        }
    }
}
