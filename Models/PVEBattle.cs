using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class PVEBattle
    {
        private CardPlayer player;
        private MonsterRoom room;

        public PVEBattle (CardPlayer _player, MonsterRoom _room)
        {
            player = _player;
            room = _room;
        }
    }
}
