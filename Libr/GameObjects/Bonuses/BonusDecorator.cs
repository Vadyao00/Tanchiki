using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public abstract class BonusDecorator(Player player) : IBonus
    {
        public Player _player = player;
        protected TimeSpan startTime;
        protected TimeSpan duration = TimeSpan.FromMilliseconds(10000);

        public abstract void ActivateBonus();

        public abstract void DeactivateBonus();

        public void StartDurationTracking()
        {
            startTime = TimeSpan.FromMilliseconds(Environment.TickCount);
        }

        public bool IsExpired()
        {
            TimeSpan currentTime = TimeSpan.FromMilliseconds(Environment.TickCount);
            TimeSpan elapsedTime = currentTime - startTime;
            return elapsedTime >= duration;
        }
    }
}
