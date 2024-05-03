using System.Windows.Media;
using Libr.GameObjects.Bonuses;
using Libr.GameObjects.Projectilies;
using Libr.Utilities;

namespace Libr
{
    public enum Movement
    {
        Left,Top,Right, Bottom
    }
    public class Tank

    {
        public float X {  get; private set; }
        public float Y { get; private set; }
        public float Size { get; private set; } = 0.08f;
        public float Health { get; set; } = 100f;
        public float Damage { get; set; } = 20f;
        public float Speed { get;  set; } = 0.35f;
        public double TimeReload { get; set; } = 0.5;
        public float Fuel { get; set; } = 100.0f;
        public int NumProjectiles { get; set; } = 100;
        public bool IsReloading { get; private set; } = false;
        public Movement? Direction { get;private set; }
        public List<Projectile> Projectiles {  get; private set; }

        public Tank() : this(1) { }

        public Tank(int num)
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
            Projectiles = [];
        }

        public void Move(Movement? move, List<Wall> listWalls, List<VirtualBonus> virtualBonusesList, Tank? player, RandomBonusFactory randomBonusFactory, Timer timer, float speedKoef)
        {
            Direction = move;
            if(Fuel <= 0) return;
            float futureX = X;
            float futureY = Y;
            switch (move)
            {
                case Movement.Left:
                    futureX -= Speed*speedKoef;
                    Direction = Movement.Left;
                    break;
                case Movement.Top:
                    futureY += Speed*speedKoef;
                    Direction = Movement.Top;
                    break;
                case Movement.Right:
                    futureX += Speed*speedKoef;
                    Direction = Movement.Right;
                    break;
                case Movement.Bottom:
                    futureY -= Speed*speedKoef;
                    Direction = Movement.Bottom;
                    break;
            }

            if (CheckCollisoinCells(futureX, futureY, listWalls))
                return;

            if(CheckCollisionBonus(futureX,futureY, virtualBonusesList))
            {
                timer.AddBonus(randomBonusFactory.CreateBonus(this), this);
            }

            if (CheckCollisionWithAnotherPlayer(futureX, futureY, player))
                return;

            X = futureX;
            Y = futureY;
            Fuel -= 0.01f;
        }

        private bool CheckCollisionBonus(float futureX, float futureY, List<VirtualBonus> virtualBonusesList)
        {
            foreach (VirtualBonus bonus in virtualBonusesList)
            {
                if (futureX < bonus.X + bonus.Size &&
                    futureX + Size > bonus.X &&
                    futureY < bonus.Y + bonus.Size &&
                    futureY + Size > bonus.Y)
                {
                    bonus.IsUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool CheckCollisoinCells(float futureX, float futureY, List<Wall> listWalls)
        {
            foreach (var cell in listWalls)
            {
                if (futureX < cell.X + cell.Size &&
                    futureX + Size > cell.X &&
                    futureY < cell.Y + cell.Size &&
                    futureY + Size > cell.Y) return true;
            }
            return false;
        }

        private bool CheckCollisionWithAnotherPlayer(float futureX, float futureY, Tank? player)
        {
            if (futureX < player?.X + player?.Size &&
                    futureX + Size > player?.X &&
                    futureY < player?.Y + player?.Size &&
                    futureY + Size > player?.Y) return true;
            return false;
        }

        public bool CheckIsDead()
        {
            if (Health <= 0) return true;
            else return false;
        }

        public void Shoot()
        {
            if (NumProjectiles != 0 && !IsReloading)
            {
                Projectiles.Add(new Projectile(Direction, VertexGenerator.GetShellVertexArray(this)));
                NumProjectiles--;
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
