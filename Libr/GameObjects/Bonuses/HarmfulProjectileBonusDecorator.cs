using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public class HarmfulProjectileBonusDecorator(Tank player) : BonusDecorator(player)
    {
        private readonly int Shells = 15;

        public override void ActivateBonus()
        {
            if (_player.NumProjectiles <= Shells)
                _player.NumProjectiles = 0;
            else _player.NumProjectiles -= Shells;
        }

        public override void DeactivateBonus() { }
    }
}

