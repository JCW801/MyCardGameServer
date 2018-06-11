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
            NetworkController.ServerAwaitingClientLoop(AddClient);
            Console.WriteLine("服务器开始运行,等待连接中...");
        }

        /// <summary>
        /// Add a new client.
        /// </summary>
        /// <param name="ss"></param>
        private void AddClient(SocketState ss)
        {
            NetworkController.getData(ss);
        }
    }
}
