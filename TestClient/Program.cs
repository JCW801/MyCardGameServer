using System;
using Models;
using MyCardGameServer;
using Newtonsoft.Json;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client c = new Client();
            Console.Read();
        }

        public class Client
        {
            public Client ()
            {
                NetworkController.ConnectToServer(FirstContact, "localhost");
            }

            private void FirstContact(SocketState ss)
            {
                ss.CallBackFunction = GetRequestResult;
                NetworkController.Send(ss, "Login");
                NetworkController.getData(ss);
            }

            private void GetRequestResult(SocketState ss)
            {
                string result = ss.SB.ToString();
                ss.SB.Clear();
                switch (result)
                {
                    case "LoginAccept":
                        Login(ss);
                        break;
                    default:
                        Console.WriteLine(result);
                        break;
                }
            }

            private void Login(SocketState ss)
            {
                Player p = new Player();
                p.AccountName = "TestPlayer1";
                p.Password = "password1";

                NetworkController.Send(ss, JsonConvert.SerializeObject(p));
                ss.CallBackFunction = GetRequestResult;
                NetworkController.getData(ss);
            }

        }
    }
}
