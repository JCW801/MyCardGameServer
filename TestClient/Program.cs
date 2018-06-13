using System;
using System.IO;
using Models;
using MyCardGameServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameDic = JsonConvert.DeserializeObject<GameDictionary>(JToken.Parse(File.ReadAllText("GameDic.json")).ToString());

            CardTransferModel card = new CardTransferModel();

            card.CanPlay = true;
            card.CanUpgrade = true;
            card.CardDescription = "Deal 8 damage. Apply Vulnerable for 2 turns.";
            card.CardDescriptionAfterUpgreade = "Deal 10 damage. Apply Vulnerable for 3 turns.";
            card.CardEffectsString = new System.Collections.Generic.List<string>();
            card.CardEffectsStringAfterUpgrade = new System.Collections.Generic.List<string>();
            card.CardManaCost = 2;
            card.CardManaCostAfterUpgrade = 2;
            card.CardName = "Bash";
            card.CardRarity = Card.Rarity.Basic;
            card.CardSpriteName = "Warrior_Bash_Sprite";
            card.Owner = "Warrior";
            card.CardType = Card.Type.Attack;
            
            card.CardEffectsString.Add("SingleAttack 8 1");
            card.CardEffectsString.Add("ApplySingleBuff Vulnerable 2");
            card.CardEffectsStringAfterUpgrade.Add("SingleAttack 10 1");
            card.CardEffectsStringAfterUpgrade.Add("ApplySingleBuff Vulnerable 3");

            gameDic.CardDic.Add(card.CardName, card);

            File.WriteAllText("GameDic.json", JsonConvert.SerializeObject(gameDic));

            Console.WriteLine(JsonConvert.SerializeObject(gameDic));

            //Client c = new Client();
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
                PlayerTransferModel p = new PlayerTransferModel();
                p.AccountName = "TestPlayer2";
                p.Password = "password2";

                NetworkController.Send(ss, JsonConvert.SerializeObject(p));
                ss.CallBackFunction = GetRequestResult;
                NetworkController.getData(ss);
            }

        }
    }
}
