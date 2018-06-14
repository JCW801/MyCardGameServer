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
        private Dictionary<SocketState,Player> connectedClient;
        private string sqlConnectionString;
        string errorString;
        GameDictionary GameDic;

        public Server()
        {
            clients = new List<SocketState>();
            NetworkController.ServerAwaitingClientLoop(FirstContact);
            GameDic = JsonConvert.DeserializeObject<GameDictionary>(JToken.Parse(File.ReadAllText("GameDic.json")).ToString());
            sqlConnectionString = ConfigurationManager.ConnectionStrings["MyCardGameServer.Properties.Settings.GameDatabaseConnectionString"].ConnectionString;
            Console.WriteLine("Waiting for first connection...");
        } 

        private void FirstContact(SocketState ss)
        {
            ss.CallBackFunction = GetRequestAfterFirstContact;
            NetworkController.getData(ss);
        }

        private void GetRequestAfterFirstContact(SocketState ss)
        {
            Console.WriteLine("A new Client has contacted the Server.");
            lock (clients)
            {
                clients.Add(ss);
            }
            if (ss.SB.ToString().Equals("Login"))
            {
                ss.SB.Clear();
                ss.CallBackFunction = ClientLogin;
                NetworkController.Send(ss,"LoginAccept");
                NetworkController.getData(ss);
            }
            else
            {
                errorString = "illegal request.";
                NetworkController.Send(ss, "Illegal request", DisconnectClient);
            }
        }

        private void ClientLogin(SocketState ss)
        {
            PlayerTransferModel player = null;
            try
            {
                player = JsonConvert.DeserializeObject<PlayerTransferModel>(ss.SB.ToString());
                ss.SB.Clear();
            }
            catch (Exception){}

            if (player != null && player.AccountName != null && player.Password != null)
            {
                player.AccountName = player.AccountName.ToLower();
                Console.WriteLine("New Client wants to login as " + player.AccountName + ".");

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
                                }
                            }
                            else
                            {
                                errorString = "wrong AccountName/Password pair";
                                NetworkController.Send(ss, "Wrong AccountName/Password pair", DisconnectClient);
                                return;
                            }
                        }
                    }

                    player.PlayerHeroList = new List<HeroTransferModel>();

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
                                    player.PlayerHeroList.Add(GameDic.HeroDic[reader["HeroName"].ToString()]);
                                    Console.WriteLine(String.Format("User data has sent to {0}({1}), waiting for client action.", player.AccountName, player.PlayerName));
                                    ss.CallBackFunction = WaitForClientAction;
                                    NetworkController.Send(ss, JsonConvert.SerializeObject(player));
                                    NetworkController.getData(ss);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                errorString = "illegal data";
                NetworkController.Send(ss, "Illegal data", DisconnectClient);
            }
        }

        private void WaitForClientAction(SocketState ss)
        {

        }

        private void DisconnectClient(SocketState ss)
        {
            Console.WriteLine("New Client has been disconnected cause "+ errorString);
            ss.TheSocket.Close();
            ss.TheSocket.Dispose();
        }
    }
}
