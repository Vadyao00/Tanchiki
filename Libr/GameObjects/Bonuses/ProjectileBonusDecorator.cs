using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public class ProjectileBonusDecorator(Tank player) : BonusDecorator(player)
    {
        private readonly int Shells = 30;

        public override void ActivateBonus()
        {
            if (_player.NumProjectiles <= 70)
                _player.NumProjectiles += Shells;
            else _player.NumProjectiles = 100;
        }

        public override void DeactivateBonus() { }
    }
}

