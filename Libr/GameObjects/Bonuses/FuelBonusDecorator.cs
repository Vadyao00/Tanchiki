using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public class FuelBonusDecorator(Tank player) : BonusDecorator(player)
    {
        private readonly float fuel = 10.0f;

        public override void ActivateBonus()
        {
            if (_player.Fuel <= 90.0f)
                _player.Fuel += fuel;
            else _player.Fuel = 100.0f;
        }

        public override void DeactivateBonus() { }
    }
}
