using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
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
        public float Damage { get;  set; } = 20f;
        public float Speed { get;  set; } = 0.004f;
        public double TimeReload { get; set; } = 0.5;
        public float Fuel { get; set; } = 100.0f;
        public int NumShells { get; private set; } = 100;
        public bool IsReloading { get; private set; } = false;
        public Movement direction { get;private set; }
        public double TimeSpeedBonus { get; set; } = 0;
        public bool isSpeedBonusActive { get; set; } = false;
        public double TimeDamageBonus { get; set; } = 0;
        public bool isDamageBonusActive { get; set; } = false;
        public double TimeReloadBonus { get; set; } = 0;
        public bool isReloadBonusActive { get; set; } = false;
        public List<Projectile> projectiles {  get; private set; }

        public Player(int num)
        {
            if (num == 1)
            {
                X = -0.04f;
                Y = 0.76f;
                direction = Movement.Bottom;
            }
            else
            {
                X = -0.04f;
                Y = -0.84f;
                direction = Movement.Top;
            }
            projectiles = new List<Projectile>();
        }

        public float[] GetVertColorArray()
        {
            switch(direction)
            {
                case Movement.Bottom:
                      return
                      [
                            X, Y + Size, 0.0f, 1.0f, 0.0f,
                            X, Y, 0.0f, 1.0f, 1.0f,
                            X + Size, Y, 0.0f, 0.0f, 1.0f,
                            X + Size, Y, 0.0f, 0.0f, 1.0f,
                            X + Size, Y + Size, 0.0f, 0.0f, 0.0f,
                            X, Y + Size, 0.0f, 1.0f, 0.0f
                      ];
                case Movement.Left:
                      return
                      [   
                            X,Y + Size, 0.0f,1.0f,1.0f,
                             X,Y, 0.0f,0.0f, 1.0f,
                            X + Size,Y,0.0f,0.0f,0.0f,
                            X + Size,Y,0.0f,0.0f,0.0f,
                            X + Size,Y + Size,0.0f,1.0f,0.0f,
                            X,Y + Size,0.0f,1.0f,1.0f
                      ];
                case Movement.Right:
                     return
                     [
                         X, Y + Size, 0.0f, 1.0f, 0.0f,
                         X, Y, 0.0f, 0.0f, 0.0f,
                         X + Size, Y, 0.0f, 0.0f, 1.0f,
                         X + Size, Y, 0.0f, 0.0f, 1.0f,
                         X + Size, Y + Size, 0.0f, 1.0f, 1.0f,
                         X, Y + Size, 0.0f, 1.0f, 0.0f
                     ];
                case Movement.Top:
                    return
                    [
                        X, Y + Size, 0.0f, 0.0f, 1.0f,
                        X, Y, 0.0f, 0.0f, 0.0f,
                        X + Size, Y, 0.0f, 1.0f, 0.0f,
                        X + Size, Y, 0.0f, 1.0f, 0.0f,
                        X + Size, Y + Size, 0.0f, 1.0f, 1.0f,
                        X, Y + Size, 0.0f, 0.0f, 1.0f
                    ];
                default:
                    return
                    [
                        X, Y + Size, 0.0f, 1.0f, 1.0f,
                        X, Y, 0.0f, 0.0f, 1.0f,
                        X + Size, Y, 0.0f, 0.0f, 0.0f,
                        X + Size, Y, 0.0f, 0.0f, 0.0f,
                        X + Size, Y + Size, 0.0f, 1.0f, 0.0f,
                        X, Y + Size, 0.0f, 1.0f, 1.0f
                    ];
            }

        }
        public void PlayerMove(Movement move, List<Cell> mapCells, List<Bonus> bonusList, Player? player)
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

            foreach (Bonus bonus in bonusList)
            {
                if (futureX < bonus.X + bonus.Size &&
                    futureX + Size > bonus.X &&
                    futureY < bonus.Y + bonus.Size &&
                    futureY + Size > bonus.Y)
                {
                    if (bonus is SpeedBonus)
                    {
                        bonus.isUsed = true;
                        bonus.ActivateBonus(this);
                        TimeSpeedBonus = 0;
                        isSpeedBonusActive = true;
                    }
                    if (bonus is FuelBonus)
                    {
                        bonus.isUsed = true;
                        bonus.ActivateBonus(this);
                    }
                    if (bonus is DamageBonus)
                    {
                        bonus.isUsed = true;
                        bonus.ActivateBonus(this);
                        TimeDamageBonus = 0;
                        isDamageBonusActive = true;
                    }
                    if (bonus is ReloadBonus)
                    {
                        bonus.isUsed = true;
                        bonus.ActivateBonus(this);
                        TimeReloadBonus = 0;
                        isReloadBonusActive = true;
                    }
                }
            }
            if (futureX < player?.X + player?.Size &&
                    futureX + Size > player?.X &&
                    futureY < player?.Y + player?.Size &&
                    futureY + Size > player?.Y) return;
            X = futureX;
            Y = futureY;
            Fuel -= 0.03f;
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
