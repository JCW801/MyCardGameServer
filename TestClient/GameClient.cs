using Models;
using MyCardGameServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

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
                client = value;
            }
        }
        private static GameClient client;

        private SocketState socketState;

        public Player Player { get; private set; }

        public GameDictionary GameDic { get; private set; }

        private CallbackDelegate callback;

        private PlayerCallbackPlayerDelegate playerCallback;

        private GameClient()
        {
            GameDic = JsonConvert.DeserializeObject<GameDictionary>(JToken.Parse(File.ReadAllText("GameDic.json")).ToString());
        }

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
            var playerModel = new PlayerTransferModel();
            if (socketState == null)
            {
                playerModel.TransferState = PlayerTransferModel.TransferStateType.Error;
                playerModel.TransferMessage = "没有连接到服务器";
                _callback(playerModel);
                return;
            }
            playerModel.AccountName = accountName;
            playerModel.Password = password;
            playerModel.TransferRequest = PlayerTransferModel.TransferRequestType.Login;

            socketState.CallBackFunction = LoginCallback;
            NetworkController.Send(socketState, JsonConvert.SerializeObject(playerModel));
            NetworkController.getData(socketState);
            playerCallback = _callback;
        }

        private void LoginCallback(SocketState ss)
        {
            socketState = ss;
            PlayerTransferModel playerModel = null;
            try
            {
                playerModel = JsonConvert.DeserializeObject<PlayerTransferModel>(ss.SB.ToString());
            }
            catch
            {
                NetworkController.getData(ss);
                return;
            }

            if (playerModel.TransferState == PlayerTransferModel.TransferStateType.Accept)
            {
                Player = new Player(playerModel, GameDic);
            }
            playerCallback(playerModel);
            ss.SB = new System.Text.StringBuilder();
        }

        public void EnterDungeon(String dungeonName, CardPlayerTransferModel cardPlayer, PlayerCallbackPlayerDelegate _callback)
        {
            var playerModel = new PlayerTransferModel();
            if (socketState == null || Player == null)
            {
                playerModel.TransferState = PlayerTransferModel.TransferStateType.Error;
                playerModel.TransferMessage = "没有连接到服务器";
                _callback(playerModel);
                return;
            }
            playerModel.TransferRequest = PlayerTransferModel.TransferRequestType.EnterDungeon;
            playerModel.TransferMessage = dungeonName;
            playerModel.CardPlayer = cardPlayer;

            socketState.CallBackFunction = EnterDungeonCallback;
            NetworkController.Send(socketState, JsonConvert.SerializeObject(playerModel));
            NetworkController.getData(socketState);
            playerCallback = _callback;
        }

        private void EnterDungeonCallback(SocketState ss)
        {
            socketState = ss;
            PlayerTransferModel playerModel = null;
            try
            {
               playerModel = JsonConvert.DeserializeObject<PlayerTransferModel>(ss.SB.ToString());
            }
            catch
            {
                NetworkController.getData(ss);
                return;
            }

            if (Player != null && playerModel.TransferState == PlayerTransferModel.TransferStateType.Accept)
            {
                Player.EnterDungeon(playerModel.Dungeon, playerModel.CardPlayer, GameDic);
            }
            playerCallback(playerModel);
            ss.SB = new System.Text.StringBuilder();
        }
    }

}

