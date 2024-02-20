using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public abstract class Bonus : IBonus
    {
        public float X { get;private set; }
        public float Y { get; private set; }
        public float Radius { get; private set; } = 0.05f;
        public double BonusTime { get; private set; } = 5;
        public bool isUsed { get; set; } = false;
        public double LifeTime { get; set; } = 0;

        public Bonus()
        {
            Random random = new Random();
            X = (float)random.NextDouble() * 1.8f - 0.9f;
            Y = (float)random.NextDouble() * 1.8f - 0.9f;
        }

        public abstract void ActivateBonus(Player player);

        public abstract void DeactivateBonus(Player player);
    }
}
