using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;

namespace Libr
{
    public enum Movement
    {
        Left,Top,Right, Bottom
    }
    public class Player
    {
        public float X {  get; private set; }
        public float Y { get; private set; }
        public float Size { get; private set; } = 0.08f;
        public float Health { get; set; } = 100f;
        public float Damage { get; private set; } = 20f;
        public float Speed { get; private set; } = 0.0045f;
        public double TimeReload { get; private set; }
        public float Fuel { get; private set; } = 100.0f;
        public int NumShells { get; private set; } = 20;
        public bool IsReloading { get; private set; } = false;
        public bool IsChanged { get; set; } = true;
        public Movement direction { get;private set; }

        public List<Projectile> projectiles {  get; private set; }

        public Player(int num)
        {
            if (num == 1)
            {
                TimeReload = 0.7;
                X = 0.0f;
                Y = 0.76f;
                direction = Movement.Bottom;
            }
            else
            {
                TimeReload = 0.7;
                X = 0.0f;
                Y = -0.76f;
                direction = Movement.Top;
            }
            projectiles = new List<Projectile>();
        }

        public Player(float hlt, float dmg, float spd, int shl)
        {
            Health = hlt;
            Damage = dmg;
            Speed = spd;
            NumShells = shl;
            projectiles = new List<Projectile>();
        }

        public float[] GetVertColorArray()
        {
            switch(direction)
            {
                case Movement.Bottom:
                      return
                      [
                            X, Y + Size, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
                            X, Y, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
                            X + Size, Y, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
                            X + Size, Y, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
                            X + Size, Y + Size, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
                            X, Y + Size, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f
                      ];
                case Movement.Left:
                      return
                      [   
                            X,Y + Size, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f,1.0f,
                             X,Y, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
                            X + Size,Y,0.0f,0.0f,0.0f,0.0f,1.0f,0.0f,0.0f,
                            X + Size,Y,0.0f,0.0f,0.0f,0.0f,1.0f,0.0f,0.0f,
                            X + Size,Y + Size,0.0f,0.0f,0.0f,0.0f,1.0f,1.0f,0.0f,
                            X,Y + Size,0.0f,0.0f,0.0f,0.0f,1.0f,1.0f,1.0f
                      ];
                case Movement.Right:
                     return
                     [
                         X, Y + Size, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
                         X, Y, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
                         X + Size, Y, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
                         X + Size, Y, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
                         X + Size, Y + Size, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
                         X, Y + Size, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f
                     ];
                case Movement.Top:
                    return
                    [
                        X, Y + Size, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
                        X, Y, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
                        X + Size, Y, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
                        X + Size, Y, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
                        X + Size, Y + Size, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
                        X, Y + Size, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f
                    ];
                default:
                    return
                    [
                        X, Y + Size, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
                        X, Y, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
                        X + Size, Y, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
                        X + Size, Y, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
                        X + Size, Y + Size, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
                        X, Y + Size, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f
                    ];
            }

        }
        public void PlayerMove(Movement move, List<Cell> mapCells)
        {
            if(Fuel <= 0) return;
            float futureX = X;
            float futureY = Y;
            direction = move;
            switch (move)
            {
                case Movement.Left:
                    futureX -= Speed;
                    direction = Movement.Left;
                    break;
                case Movement.Top:
                    futureY += Speed;
                    direction = Movement.Top;
                    break;
                case Movement.Right:
                    futureX += Speed;
                    direction = Movement.Right;
                    break;
                case Movement.Bottom:
                    futureY -= Speed;
                    direction = Movement.Bottom;
                    break;
            }

            foreach (var cell in mapCells)
            {
                if (futureX < cell.X + cell.Size &&
                    futureX + Size > cell.X &&
                    futureY < cell.Y + cell.Size &&
                    futureY + Size > cell.Y &&
                    cell.IsWall) return;
            }

            X = futureX;
            Y = futureY;
            IsChanged = true;
            Fuel -= 0.02f;
        }

        public float[] PointerAndReloadLine()
        {
            float x=0, y=0, xStart=X, xEnd=X + Size, yLine = Y + Size + 0.02f;
            switch(direction)
            {
                case Movement.Left:
                    x = X;
                    y = Y + Size/2;
                    break;
                case Movement.Top:
                    x = X + Size/2;
                    y = Y + Size;
                    break;
                case Movement.Right:
                    x = X + Size;
                    y = Y + Size/2;
                    break;
                case Movement.Bottom:
                    x = X + Size/2;
                    y = Y;
                    break;
            }
            return [x, y, xStart, xEnd, yLine];
        }

        public bool CheckIsDead()
        {
            if (Health <= 0) return true;
            else return false;
        }

        public void Shoot()
        {
            if (NumShells != 0 && !IsReloading)
            {
                projectiles.Add(new Projectile(direction, PointerAndReloadLine()[0], PointerAndReloadLine()[1]));
                NumShells--;
                IsReloading = true;
                Reload();
            }
        }

        public async void Reload()
        {
            await Task.Delay(TimeSpan.FromSeconds(TimeReload));
            IsReloading = false;
        }
    }
}
