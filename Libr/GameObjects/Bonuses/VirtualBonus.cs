using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr.GameObjects.Bonuses
{
    public class VirtualBonus
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Size { get; private set; } = 0.11f;
        public bool IsUsed { get; set; } = false;
        public double LifeTime { get; set; } = 0;
        public VirtualBonus()
        {
            Random random = new();
            X = (float)random.NextDouble() * 1.8f - 0.9f;
            Y = (float)random.NextDouble() * 1.8f - 0.9f;
        }

        public float[] GetVertexArray()
        {
            return
            [
             X, Y + Size, 0.0f, 0.0f,1.0f,
             X, Y, 0.0f, 0.0f,0.0f,
             X + Size, Y, 0.0f, 1.0f,0.0f,
             X + Size, Y, 0.0f, 1.0f,0.0f,
             X + Size, Y + Size, 0.0f, 1.0f,1.0f,
             X, Y + Size, 0.0f, 0.0f,1.0f
            ];
        }
    }
}
