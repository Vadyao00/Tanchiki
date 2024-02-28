using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public class ShellBonus(Player player) : Bonus(player)
    {
        private readonly int Shells = 30;

        public override void ActivateBonus()
        {
            if (_player.NumShells <= 70)
                _player.NumShells += Shells;
            else _player.NumShells = 100;
        }

        public override void DeactivateBonus() { }
    }
}

