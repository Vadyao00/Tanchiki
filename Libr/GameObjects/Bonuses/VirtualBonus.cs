namespace Libr.GameObjects.Bonuses
{
    /// <summary>
    /// Класс виртуального бонуса.
    /// </summary>
    public class VirtualBonus
    {
        /// <summary>
        /// Координата X левого нижнего угла.
        /// </summary>
        public float X { get; private set; }
        /// <summary>
        /// Координата Y левого нижнего угла.
        /// </summary>
        public float Y { get; private set; }
        /// <summary>
        /// Размер бонуса, его длина и ширина.
        /// </summary>
        public float Size { get; private set; } = 0.08f;
        /// <summary>
        /// Флаг, показывающий использован ли бонус.
        /// </summary>
        public bool IsUsed { get; set; } = false;
        /// <summary>
        /// Время, которое бонус находится на карте.
        /// </summary>
        public double LifeTime { get; set; } = 0;
        /// <summary>
        /// Конструктор, задающий параметры X и Y для бонуса, вне стен.
        /// </summary>
        /// <param name="listWalls">Коллекция стен, которые находятся на карте.</param>
        public VirtualBonus(List<Wall> listWalls)
        {
            Random random = new();
            bool checkCollision = true;
            while(checkCollision)
            {
                X = (float)random.NextDouble() * 1.8f - 0.9f;
                Y = (float)random.NextDouble() * 1.8f - 0.9f;
                checkCollision = listWalls.Any(cell =>
                    X < cell.X + cell.Size &&
                    X + Size > cell.X &&
                    Y < cell.Y + cell.Size &&
                    Y + Size > cell.Y
                );
            }
        }
        /// <summary>
        /// Конструктор, задающий параметры X и Y для бонуса.
        /// </summary>
        /// <param name="X">Координата X левого нижнего угла.</param>
        /// <param name="Y">Координата Y левого нижнего угла.</param>
        public VirtualBonus(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
