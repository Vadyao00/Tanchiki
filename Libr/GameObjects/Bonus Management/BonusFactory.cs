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
        public Bonus createBonus(Player player)
        {
            Random random = new();
            randomBonus = random.Next(1, 5);
            switch (randomBonus)
            {
                case 1:
                    return new SpeedBonus(player);
                case 2:
                    return new FuelBonus(player);
                case 3:
                    return new DamageBonus(player);
                case 4:
                    return new ReloadBonus(player);
                default:
                    return new SpeedBonus(player);
            }
        }
    }
}
