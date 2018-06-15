﻿using System;
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
        private Dictionary<string, Player> playerDic;
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
                
                switch (player.TransferRequest)
                {
                    case PlayerTransferModel.TransferRequestType.Login:
                        ClientLogin(player, ss);
                        break;
                }

                NetworkController.getData(ss);

            }
            catch
            {
                errorString = "illegal data";
                PlayerTransferModel player = new PlayerTransferModel();
                player.TransferState = PlayerTransferModel.TransferStateType.Error;
                player.TransferStateMessage = "Illegal data";
                NetworkController.Send(ss, JsonConvert.SerializeObject(player), DisconnectClient);
            }
            
        }

        private void ClientLogin(PlayerTransferModel player,SocketState ss)
        {
            if (player != null && player.AccountName != null && player.Password != null)
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
                                errorString = "wrong AccountName/Password pair";
                                player = new PlayerTransferModel();
                                player.TransferState = PlayerTransferModel.TransferStateType.Decline;
                                player.TransferStateMessage = "Wrong AccountName/Password pair";
                                NetworkController.Send(ss, JsonConvert.SerializeObject(player), DisconnectClient);
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
                }

                Console.WriteLine(String.Format("User data has sent to {0}({1}), waiting for client action.", player.AccountName, player.PlayerName));
                NetworkController.Send(ss, JsonConvert.SerializeObject(player));
                Player p = new Player(player, GameDic);
            }
            else
            {
                throw new Exception();
            }
        }

        private void DisconnectClient(SocketState ss)
        {
            Console.WriteLine("New Client has been disconnected cause "+ errorString);
            ss.TheSocket.Close();
            ss.TheSocket.Dispose();
        }
    }
}
