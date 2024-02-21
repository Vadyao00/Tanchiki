using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public class FuelBonus : Bonus
    {
        private readonly float fuel = 10.0f;
        public FuelBonus() : base() { }

        public override void ActivateBonus(Player player)
        {
            if (player.Fuel <= 90.0f)
                player.Fuel += fuel;
            else player.Fuel = 100.0f;
        }

        public override void DeactivateBonus(Player player) { }
    }
}
