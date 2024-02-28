using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public class HarmfulReloadBonus(Player player) : Bonus(player)
    {
        private readonly double reloadEffect = 0.3;
        private readonly double reloadNormal = 0.5;

        public override void ActivateBonus()
        {
            if(_player.TimeReload <= reloadNormal)
                _player.TimeReload += reloadEffect;
        }

        public override void DeactivateBonus()
        {
            if(_player.TimeReload > reloadNormal)
                _player.TimeReload -= reloadEffect;
        }
    }
}
