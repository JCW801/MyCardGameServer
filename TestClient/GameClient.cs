using Models;
using MyCardGameServer;
using Newtonsoft.Json;
using System;

namespace TestClient
{
    public class GameClient
    {
        public static string ServerIp = "localhost";

        public delegate void PlayerCallbackPlayerDelegate(PlayerTransferModel player);

        public delegate void CallbackDelegate();

        public static GameClient Client
        {
            get
            {
                if (client == null)
                {
                    client = new GameClient();
                }
                return client;
            }
            private set
            {

            }
        }
        private static GameClient client;

        private SocketState socketState;

        private PlayerTransferModel player = new PlayerTransferModel();

        private CallbackDelegate callback;

        private PlayerCallbackPlayerDelegate playerCallback;

        private GameClient() { }

        public void ConnectToServer(CallbackDelegate _callback)
        {
            NetworkController.ConnectToServer(ConnectToServerCallback, ServerIp);
            callback = _callback;
        }
        
        private void ConnectToServerCallback(SocketState ss)
        {
            socketState = ss;
            callback();
        }

        public void Login(String accountName, String password, PlayerCallbackPlayerDelegate _callback)
        {
            if (socketState == null)
            {
                player.TransferState = PlayerTransferModel.TransferStateType.Error;
                player.TransferStateMessage = "没有连接到服务器";
                _callback(player.Clone());
                return;
            }
            player.AccountName = accountName;
            player.Password = password;
            player.TransferRequest = PlayerTransferModel.TransferRequestType.Login;

            socketState.CallBackFunction = LoginCallback;
            NetworkController.Send(socketState, JsonConvert.SerializeObject(player));
            NetworkController.getData(socketState);
            playerCallback = _callback;
        }

        private void LoginCallback(SocketState ss)
        {
            player = JsonConvert.DeserializeObject<PlayerTransferModel>(ss.SB.ToString());
            playerCallback(player.Clone());
            ss.SB.Clear();
        }
    }
}

