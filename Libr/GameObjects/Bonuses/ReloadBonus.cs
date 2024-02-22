using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public class ReloadBonus(Player player) : Bonus(player)
    {
        private readonly double reloadEffect = 0.2;
        private readonly double reloadNormal = 0.5;

        public override void ActivateBonus()
        {
            _player.TimeReload = reloadEffect;
        }

        public override void DeactivateBonus()
        {
            _player.TimeReload = reloadNormal;
        }
    }
}
