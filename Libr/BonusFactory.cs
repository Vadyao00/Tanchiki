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
        public Bonus createBonus()
        {
            Random random = new Random();
            randomBonus = random.Next(1, 5);
            switch (randomBonus)
            {
                case 1:
                    return new SpeedBonus();
                case 2:
                    return new FuelBonus();
                case 3:
                    return new DamageBonus();
                case 4:
                    return new ReloadBonus();
                default:
                    return new SpeedBonus();
            }
        }
    }
}
