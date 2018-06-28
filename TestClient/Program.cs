using System;
using System.Collections.Generic;
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


            /*
            CardTransferModel card = new CardTransferModel();

            card.CanPlay = true;
            card.CanUpgrade = true;
            card.CardDescription = "";
            card.CardDescriptionAfterUpgreade = "";
            card.CardEffectsString = new System.Collections.Generic.List<string>();
            card.CardEffectsStringAfterUpgrade = new System.Collections.Generic.List<string>();
            card.CardManaCost = 1;
            card.CardManaCostAfterUpgrade = 1;
            card.CardName = "YJT_AttackCard";
            card.CardRarity = Card.Rarity.UnavailableForPlayer;
            card.CardSpriteName = "";
            card.Owner = "异教徒";
            card.CardType = Card.Type.Attack;
            
            card.CardEffectsString.Add("SingleAttack 6 1");
            //card.CardEffectsString.Add("ApplySingleSelfBuff PowerCeremony 3");
            card.CardEffectsStringAfterUpgrade.Add("SingleAttack 9 1");
            //card.CardEffectsStringAfterUpgrade.Add("ApplySingleSelfBuff PowerCeremony 5");

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

            buff.BuffName = "力量";
            buff.BuffDescription = "增加攻击时的伤害.";
            buff.BuffEffects = new System.Collections.Generic.List<string>();
            buff.BuffSpriteName = "Buff_Power_Sprite";
            buff.IsDebuff = false;
            buff.IsPower = true;

            buff.BuffEffects.Add("Power");

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
            dungeon.DungeonAllowedCardCount = 10;

            dungeon.HighLevelNormalMsonterRoomList = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, int>>();
            dungeon.LowLevelNormalMonsterRoomList = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, int>>();
            dungeon.BossMonsterRoomList = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, int>>();
            dungeon.EliteMonsterRoomList = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, int>>();
            dungeon.EventRoomList = new System.Collections.Generic.List<string>();
            dungeon.LowLevelThreshold = 7;

            System.Collections.Generic.Dictionary<string, int> room = new System.Collections.Generic.Dictionary<string, int>();
            room.Add("异教徒", 1);
            dungeon.LowLevelNormalMonsterRoomList.Add(room);

            gameDic.DungeonDic.Add("TestDungeon", dungeon);
            */

            /*
            MonsterTransferModel monster = new MonsterTransferModel();
            monster.MonsterName = "异教徒";
            monster.MonsterHealth = 55;
            monster.MonsterSpriteName = "Monster_YJT_Spirte";
            monster.MonsterCardList = new System.Collections.Generic.List<string>();
            monster.MonsterRelicList = new System.Collections.Generic.List<string>();

            monster.MonsterCardList.Add("YJT_PowerCeremonyCard");
            monster.MonsterCardList.Add("YJT_AttackCard");

            gameDic.MonsterDic.Add("异教徒", monster);
            */

            //File.WriteAllText("GameDic.json", JsonConvert.SerializeObject(gameDic));
            //Console.WriteLine(JsonConvert.SerializeObject(gameDic));

            GameClient.Client.ConnectToServer(FirstContact);

            Console.Read();
        }

        private static void FirstContact()
        {
            GameClient.Client.Login("TestPlayer1", "password1", Login);
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

            GameClient.Client.EnterDungeon("TestDungeon", cardPlayer, EnterDungeon);

            //GameClient.Client.Login("TestPlayer2", "password2", Login);
        }

        private static void EnterDungeon(PlayerTransferModel player)
        {
            var t = GameClient.Client.Player.GetRoomMap();
            int i = 0;
            foreach (var item in t[0])
            {
                i = item.Key;
                break;
            }
            GameClient.Client.EnterDungeonRoom(i,EnterDungeonRoom);
        }

        private static void EnterDungeonRoom(PlayerTransferModel player)
        {
            Player p = GameClient.Client.Player;
        }
    }
}
