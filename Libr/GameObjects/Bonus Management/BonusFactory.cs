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
            randomBonus = random.Next(1, 9);
            return randomBonus switch
            {
                1 => new SpeedBonus(player),
                2 => new FuelBonus(player),
                3 => new DamageBonus(player),
                4 => new ReloadBonus(player),
                5 => new ShellBonus(player),
                6 => new HarmfulSpeedBonus(player),
                7 => new HarmfulShellBonus(player),
                8 => new HarmfulReloadBonus(player),
                _ => new SpeedBonus(player),
            };
        }
    }
}
