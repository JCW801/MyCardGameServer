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

            //gameDic.BuffDic = new System.Collections.Generic.Dictionary<string, BuffTransferModel>();
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

            /*
            BuffTransferModel buff = new BuffTransferModel();

            buff.BuffName = "Vulnerable";
            buff.BuffDescription = "All attack damage received will be increased by 50% (this amount can be modified by relics).";
            buff.BuffEffects = new System.Collections.Generic.List<string>();
            buff.BuffSpriteName = "Buff_Vulnerable_Sprite";
            buff.IsDebuff = true;
            buff.IsPower = false;

            buff.BuffEffects.Add("Vulnerable");

            gameDic.BuffDic.Add(buff.BuffName, buff);
            */

            /*
            DungeonTransferModel dungeon = new DungeonTransferModel();
            dungeon.DungeonName = "TestDungeon";
            dungeon.DungeonDepth = 16;
            dungeon.BonfireRoomLevel = new System.Collections.Generic.HashSet<int> {15, 12, 9};
            dungeon.EliteMonsterRoomLevel = new System.Collections.Generic.HashSet<int> {13, 11, 9, 6};
            dungeon.EventRoomLevel = new System.Collections.Generic.HashSet<int> { 14, 13, 12, 10, 8, 5, 4, 3, 2, 1 };
            dungeon.NormalMonsterRoomLevel = new System.Collections.Generic.HashSet<int> { 14, 11, 10, 8, 5, 4, 3, 2, 1, 0 };
            dungeon.ShoppingRoomLevel = new System.Collections.Generic.HashSet<int> {12, 8, 4};
            dungeon.TreasureRoomLevel = new System.Collections.Generic.HashSet<int> { 7 };

            gameDic.DungeonDic.Add("TestDungeon", dungeon);
            */

            //File.WriteAllText("GameDic.json", JsonConvert.SerializeObject(gameDic));
            //Console.WriteLine(JsonConvert.SerializeObject(gameDic));

            GameClient.Client.ConnectToServer(FirstContact);

            Console.Read();
        }

        private static void FirstContact()
        {
            GameClient.Client.Login("TestPlayer", "password1", Login);
        }

        private static void Login(PlayerTransferModel player)
        {
            if (player.TransferState != PlayerTransferModel.TransferStateType.Accept)
            {
                Console.WriteLine(player.TransferMessage);
            }
            else
            {
                Console.WriteLine(player.PlayerName + " " + player.PlayerHeroList[0]);
            }

            CardPlayerTransferModel cardPlayer = new CardPlayerTransferModel();
            cardPlayer.MainHero = "Warrior";
            cardPlayer.CardDic = GameClient.Client.GameDic.HeroDic["Warrior"].HeroBasicCard;

            //GameClient.Client.Login("TestPlayer2", "password2", Login);
        }

        private static void EnterDungeon(PlayerTransferModel player)
        {
            
        }
    }
}
