using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public class SpeedBonus : Bonus
    {
        private readonly float speedEffect = 0.007f;
        private readonly float speedNormal = 0.004f;
        public SpeedBonus() : base() { }

        public override void ActivateBonus(Player player)
        {
            player.Speed = speedEffect;
        }

        public override void DeactivateBonus(Player player)
        {
                player.Speed = speedNormal;
        }
    }
}
