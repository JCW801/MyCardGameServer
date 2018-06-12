using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCardGameServer
{
    public class Server
    {
        public Server()
        {
            NetworkController.ServerAwaitingClientLoop(FirstContact);
            Console.WriteLine("服务器开始运行,等待连接中...");
        }

        private void FirstContact(SocketState ss)
        {
            ss.CallBackFunction = AddClient;
            NetworkController.getData(ss);
        }

        private void AddClient(SocketState ss)
        {
            NetworkController.getData(ss);
        }
    }
}
