using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CardPlayerTransferModel
    {
        public String MainHero { get; set; }

        public String SubHero { get; set; }

        public Dictionary<string, int> CardDic { get; set; }
    }
}
