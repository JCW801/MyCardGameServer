using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class MonsterTransferModel
    {
        public string MonsterName { get; set; }

        public string MonsterSpriteName { get; set; }

        public int MonsterHealth { get; set; }

        public List<string> MonsterCardList { get; set; }

        public List<string> MonsterRelicList { get; set; }
    }
}
