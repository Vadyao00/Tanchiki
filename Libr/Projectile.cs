using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public class Projectile
    {
        public float X { get;private set; }
        public float Y { get;private set; }
        private float speedProjectile;
        public Movement direction { get; private set; }
        public Projectile(Movement dir, float x,float y)
        {
            X = x;
            Y = y;
            direction = dir;
            speedProjectile = 0.008f;
        }

        public float[] Move(Cell[,] cells, List<Projectile> projectiles)
        {
            float x=X, y=Y;
            switch (direction)
            {
                case Movement.Left:
                    x -= speedProjectile;
                    break;
                case Movement.Top:
                    y += speedProjectile;
                    break;
                case Movement.Right:
                    x += speedProjectile;
                    break;
                case Movement.Bottom:
                    y -= speedProjectile;
                    break;
            }
            foreach(Cell cell in cells)
            {
                if (x < cell.X + cell.Size &&
                x  > cell.X &&
                y < cell.Y + cell.Size &&
                y  > cell.Y && cell.IsWall)
                {
                    projectiles.Add(this);
                    return [X, Y];
                }
                else
                {
                    X = x;
                    Y = y;
                }
            }
            return [X,Y];
        }
    }
}
