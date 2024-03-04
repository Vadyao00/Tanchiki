using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public class HarmfulShellBonusDecorator(Player player) : BonusDecorator(player)
    {
        private readonly int Shells = 15;

        public override void ActivateBonus()
        {
            if (_player.NumShells <= Shells)
                _player.NumShells = 0;
            else _player.NumShells -= Shells;
        }

        public override void DeactivateBonus() { }
    }
}

