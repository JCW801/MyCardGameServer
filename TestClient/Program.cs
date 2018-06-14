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
            //var gameDic = JsonConvert.DeserializeObject<GameDictionary>(JToken.Parse(File.ReadAllText("GameDic.json")).ToString());

            /*
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
            */

            /*
            HeroTransferModel hero = new HeroTransferModel();

            hero.HeroName = "Warrior";
            hero.HeroSprite = "Warrior_Sprite";
            hero.HeroBasicCard = new System.Collections.Generic.Dictionary<string, int>();
            hero.HeroBasicCard.Add("Bash", 1);
            hero.HeroBasicCard.Add("WarriorStrike", 5);
            hero.HeroBasicCard.Add("WarriorDefend", 4);
            hero.HeroCard = new System.Collections.Generic.List<string>();
            hero.HeroHealth = 80;
            hero.HeroGold = 99;
            hero.HeroBasicRilic = new System.Collections.Generic.List<string>();
            hero.HeroBasicRilic.Add("BurningBlood");

            gameDic.HeroDic.Add("Warrior", hero);
            */

            /*
            RelicTransferModel relic = new RelicTransferModel();

            relic.RelicDescription = "At the end of combat, heal 6 HP.";
            relic.RelicEffectsString = new System.Collections.Generic.List<string>();
            relic.RelicEffectsString.Add("AfterBattleRecover 6");
            relic.RelicName = "BurningBlood";
            relic.RelicRarity = Relic.Rarity.Starter;
            relic.RelicSpriteName = "Relic_BurningBlood_Sprite";

            gameDic.RelicDic.Add(relic.RelicName, relic);
            */

            //File.WriteAllText("GameDic.json", JsonConvert.SerializeObject(gameDic));

            //Console.WriteLine(JsonConvert.SerializeObject(gameDic));

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
                try
                {
                    PlayerTransferModel player = JsonConvert.DeserializeObject<PlayerTransferModel>(result);
                    Console.WriteLine(player.PlayerName);
                }
                catch (Exception)
                {
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
            }

            private void Login(SocketState ss)
            {
                PlayerTransferModel p = new PlayerTransferModel();
                p.AccountName = "TestPlayer1";
                p.Password = "password1";

                NetworkController.Send(ss, JsonConvert.SerializeObject(p));
                ss.CallBackFunction = GetRequestResult;
                NetworkController.getData(ss);
            }

        }
    }
}
