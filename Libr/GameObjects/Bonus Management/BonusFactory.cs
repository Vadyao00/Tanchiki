using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public class BonusFactory
    {
        private int randomBonus;
        public Bonus CreateBonus(Player player)
        {
            Random random = new();
            randomBonus = random.Next(1, 5);
            return randomBonus switch
            {
                1 => new SpeedBonus(player),
                2 => new FuelBonus(player),
                3 => new DamageBonus(player),
                4 => new ReloadBonus(player),
                _ => new SpeedBonus(player),
            };
        }
    }
}
