using Libr.GameObjects.Bonuses;
using Libr.GameObjects.Projectilies;
using Libr.Utilities;

namespace Libr
{
    /// <summary>
    /// Перечисление, содержащее направления движения танка.
    /// </summary>
    public enum Movement
    {
        Left,Top,Right, Bottom
    }
    /// <summary>
    /// Класс танка.
    /// </summary>
    public class Tank

    {
        /// <summary>
        /// Левая нижняя координата X.
        /// </summary>
        public float X {  get; private set; }
        /// <summary>
        /// Левая нижняя координата Y.
        /// </summary>
        public float Y { get; private set; }
        /// <summary>
        /// Размер танка.
        /// </summary>
        public float Size { get; private set; } = 0.08f;
        /// <summary>
        /// Количество жизней танка.
        /// </summary>
        public float Health { get; set; } = 100f;
        /// <summary>
        /// Значения урона танка.
        /// </summary>
        public float Damage { get; set; } = 20f;
        /// <summary>
        /// Значение скорости движения танка.
        /// </summary>
        public float Speed { get;  set; } = 0.35f;
        /// <summary>
        /// Время перезарядки танка.
        /// </summary>
        public double TimeReload { get; set; } = 0.5;
        /// <summary>
        /// Количество топлива танка.
        /// </summary>
        public float Fuel { get; set; } = 100.0f;
        /// <summary>
        /// Количество снарядов танка.
        /// </summary>
        public int NumProjectiles { get; set; } = 100;
        /// <summary>
        /// Флаг, показыващий, перезаряжается ли танк.
        /// </summary>
        public bool IsReloading { get; private set; } = false;
        /// <summary>
        /// Текущее направление танка.
        /// </summary>
        public Movement? Direction { get;private set; }
        /// <summary>
        /// Коллекция патрон танка, отображающихся на карте.
        /// </summary>
        public List<Projectile> Projectiles {  get; private set; }

        public Tank() : this(1) { }
        /// <summary>
        /// Конструктор, устанавливающий начальное положение танков и их направлеине.
        /// </summary>
        /// <param name="num">Номер игрока.</param>
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
        /// <summary>
        /// Метод, передвигающий танк по игровому полю.
        /// </summary>
        /// <param name="move">Направление танка, в котором он будет двигаться.</param>
        /// <param name="listWalls">Коллекция стен, расположенных на карте.</param>
        /// <param name="virtualBonusesList">Коллекция виртуальных бонусов, расположенных на карте.</param>
        /// <param name="player">Объект второго игрока.</param>
        /// <param name="randomBonusFactory">Объект класса <see cref="RandomBonusFactory"/>, который создает случайный бонус.</param>
        /// <param name="timer">Объект класса <see cref="Timer"/>.</param>
        /// <param name="speedKoef">Коэффициент движения танка, зависящий от частоты обновления экрана.</param>
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
        /// <summary>
        /// Метод, проверяющий собрал ли танк бонус.
        /// </summary>
        /// <param name="futureX">Левая нижняя координата X после движения танка.</param>
        /// <param name="futureY">Левая нижняя координата Y после движения танка.</param>
        /// <param name="virtualBonusesList">Коллекция виртуальных бонусов.</param>
        /// <returns>Возвращает true, если игрок собрал бонус, иначе возвращает false.</returns>
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
        /// <summary>
        /// Метод, првоеряющий столкновение танка со стенами.
        /// </summary>
        /// <param name="futureX">Левая нижняя координата X после движения танка.</param>
        /// <param name="futureY">Левая нижняя координата Y после движения танка.</param>
        /// <param name="listWalls">Коллекция стен на карте.</param>
        /// <returns>Возвращает true, если игрок столкнулся со стеной, иначе возвращает false.</returns>
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
        /// <summary>
        /// Метод, проверяющий столкновения танка с другим танком.
        /// </summary>
        /// <param name="futureX">Левая нижняя координата X после движения танка.</param>
        /// <param name="futureY">Левая нижняя координата Y после движения танка.</param>
        /// <param name="player">Объект другого игрока.</param>
        /// <returns>Возвращает true, если игрок столкнулся с другим игроком, иначе возвращает false.</returns>
        private bool CheckCollisionWithAnotherPlayer(float futureX, float futureY, Tank? player)
        {
            if (futureX < player?.X + player?.Size &&
                    futureX + Size > player?.X &&
                    futureY < player?.Y + player?.Size &&
                    futureY + Size > player?.Y) return true;
            return false;
        }
        /// <summary>
        /// Метод, проверяющий уничтожен ли танк.
        /// </summary>
        /// <returns>Возвращает true, если количество жизней игрока меньше или равно 0, иначе возвращает false.</returns>
        public bool CheckIsDead()
        {
            if (Health <= 0) return true;
            else return false;
        }
        /// <summary>
        /// Метод, производящий выстрел.
        /// </summary>
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
        /// <summary>
        /// Метод, перезаряжающий танк.
        /// </summary>
        public async void Reload()
        {
            await Task.Delay(TimeSpan.FromSeconds(TimeReload));
            IsReloading = false;
        }
    }
}
