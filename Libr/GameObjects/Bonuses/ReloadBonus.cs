using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public class ReloadBonus : Bonus
    {
        private readonly double reloadEffect = 0.2;
        private readonly double reloadNormal = 0.5;
        public ReloadBonus() : base() { }

        public override void ActivateBonus(Player player)
        {
            player.TimeReload = reloadEffect;
        }

        public override void DeactivateBonus(Player player)
        {
            player.TimeReload = reloadNormal;
        }
    }
}
