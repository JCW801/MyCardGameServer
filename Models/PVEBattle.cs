using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PVEBattle
    {
        private CardPlayer player;
        private MonsterDungeonRoom room;

        public PVEBattle (CardPlayer _player, MonsterDungeonRoom _room)
        {
            player = _player;
            room = _room;
        }
    }
}
