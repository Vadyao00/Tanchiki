using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Libr.GameObjects.Bonuses;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using Timer = Libr.GameObjects.Bonus_Management.Timer;

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
        public Movement Direction { get;private set; }
        public List<Projectile> Projectiles {  get; private set; }

        public Player(int num)
        {
            if (num == 1)
            {
                X = -0.04f;
                Y = 0.76f;
                Direction = Movement.Bottom;
            }
            else
            {
                X = -0.04f;
                Y = -0.84f;
                Direction = Movement.Top;
            }
            Projectiles = new List<Projectile>();
        }

        public float[] GetVertColorArray()
        {
            switch(Direction)
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
        public void PlayerMove(Movement move, List<Cell> mapCells, List<VirtualBonus> virtualBonusesList, Player? player, BonusFactory bonusFactory, Timer timer)
        {
            if(Fuel <= 0) return;
            float futureX = X;
            float futureY = Y;
            Direction = move;
            switch (move)
            {
                case Movement.Left:
                    futureX -= Speed;
                    Direction = Movement.Left;
                    break;
                case Movement.Top:
                    futureY += Speed;
                    Direction = Movement.Top;
                    break;
                case Movement.Right:
                    futureX += Speed;
                    Direction = Movement.Right;
                    break;
                case Movement.Bottom:
                    futureY -= Speed;
                    Direction = Movement.Bottom;
                    break;
            }

            if (CheckCollisoinCells(futureX, futureY, mapCells))
                return;

            foreach (VirtualBonus bonus in virtualBonusesList)
            {
                if (futureX < bonus.X + bonus.Size &&
                    futureX + Size > bonus.X &&
                    futureY < bonus.Y + bonus.Size &&
                    futureY + Size > bonus.Y)
                { 
                    bonus.IsUsed = true;
                    timer.AddBonus(bonusFactory.CreateBonus(this));
                }
            }

            if (CheckCollisionWithAnotherPlayer(futureX, futureY, player))
                return;

            X = futureX;
            Y = futureY;
            Fuel -= 0.03f;
        }


        private bool CheckCollisoinCells(float futureX, float futureY, List<Cell> mapCells)
        {
            foreach (var cell in mapCells)
            {
                if (futureX < cell.X + cell.Size &&
                    futureX + Size > cell.X &&
                    futureY < cell.Y + cell.Size &&
                    futureY + Size > cell.Y &&
                    cell.IsWall) return true;
            }
            return false;
        }

        private bool CheckCollisionWithAnotherPlayer(float futureX, float futureY, Player? player)
        {
            if (futureX < player?.X + player?.Size &&
                    futureX + Size > player?.X &&
                    futureY < player?.Y + player?.Size &&
                    futureY + Size > player?.Y) return true;
            return false;
        }

        public float[] PointerAndReloadLine()
        {
            float x=0, y=0, xStart=X, xEnd=X + Size, yLine = Y + Size + 0.02f;
            switch(Direction)
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
                Projectiles.Add(new Projectile(Direction, PointerAndReloadLine()[0], PointerAndReloadLine()[1]));
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
