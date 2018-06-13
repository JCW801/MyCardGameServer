using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MyCardGameServer
{
    public class Server
    {
        private List<SocketState> clients;
        private Dictionary<SocketState,Player> connectedClient;
        private string sqlConnectionString;
        string errorString;

        public Server()
        {
            clients = new List<SocketState>();
            NetworkController.ServerAwaitingClientLoop(FirstContact);
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
            Player player = null;
            try
            {
                player = JsonConvert.DeserializeObject<Player>(ss.SB.ToString());
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
                                    Console.WriteLine(String.Format("New Client successfully login as {0}({1})",player.AccountName,player.PlayerName));
                                }
                            }
                            else
                            {
                                errorString = "wrong AccountName/Password pair";
                                NetworkController.Send(ss, "Wrong AccountName/Password pair", DisconnectClient);
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

        private void DisconnectClient(SocketState ss)
        {
            Console.WriteLine("New Client has been disconnected cause "+ errorString);
            ss.TheSocket.Close();
            ss.TheSocket.Dispose();
        }
    }
}
