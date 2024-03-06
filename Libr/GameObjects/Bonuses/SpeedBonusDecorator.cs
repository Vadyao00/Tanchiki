using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public class SpeedBonusDecorator(Player player) : BonusDecorator(player)
    {
        private readonly float speedEffect = 0.002f;
        private readonly float speedNormal = 0.006f;

        public override void ActivateBonus()
        {
            if(_player.Speed <= speedNormal)
                _player.Speed += speedEffect;
        }

        public override void DeactivateBonus()
        {
            if(_player.Speed > speedNormal)
                _player.Speed -= speedEffect;
        }
    }
}
