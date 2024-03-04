using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public abstract class BonusFactory(Player player)
    {
        protected Player player = player;
        public abstract Bonus CreateBonus();
    }
}
