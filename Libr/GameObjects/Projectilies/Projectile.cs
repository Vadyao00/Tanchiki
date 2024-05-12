namespace Libr.GameObjects.Projectilies
{
    /// <summary>
    /// Класс снаряда.
    /// </summary>
    /// <param name="direction">Направление, в котором движется снаряд.</param>
    /// <param name="vertexArray">Массив, состоящий из координат X и Y.</param>
    public class Projectile(Movement? direction, float[] vertexArray)
    {
        /// <summary>
        /// Координата X снаряда.
        /// </summary>
        public float X { get; private set; } = vertexArray[0];
        /// <summary>
        /// Координата Y снаряда.
        /// </summary>
        public float Y { get; private set; } = vertexArray[1];
        /// <summary>
        /// Скорость движения снаряда.
        /// </summary>

        private readonly float speedProjectile = 0.7f;
        /// <summary>
        /// Направление движения снаряда.
        /// </summary>
        public Movement? Direction { get; private set; } = direction;
        /// <summary>
        /// Метод, двигающий снаряд по игровому полю.
        /// </summary>
        /// <param name="listWalls">Коллекция стен, расположенных на карте.</param>
        /// <param name="projectilesToRemove">Коллекция снарядов для удаления.</param>
        /// <param name="firstPlayer">Объект первого игрока.</param>
        /// <param name="secondPlayer">Объект второго игрока.</param>
        /// <param name="idPlayer">Номер игрока, которому принадлежит снаряд.</param>
        /// <param name="koef">Коэффициент движения снаряда, зависящий от частоты обновления экрана.</param>
        public void Move(List<Wall> listWalls, List<Projectile> projectilesToRemove, Tank firstPlayer, Tank secondPlayer, int idPlayer,float koef)
        {
            float x = X, y = Y;
            switch (Direction)
            {
                case Movement.Left:
                    x -= speedProjectile*koef;
                    break;
                case Movement.Top:
                    y += speedProjectile*koef;
                    break;
                case Movement.Right:
                    x += speedProjectile*koef;
                    break;
                case Movement.Bottom:
                    y -= speedProjectile*koef;
                    break;
            }
            foreach (Wall cell in listWalls)
            {
                if (x < cell.X + cell.Size &&
                x > cell.X &&
                y < cell.Y + cell.Size &&
                y > cell.Y)
                {
                    projectilesToRemove.Add(this);
                }
            }
            if (idPlayer == 2)
            {
                if (x < firstPlayer.X + firstPlayer.Size &&
                        x > firstPlayer.X &&
                        y < firstPlayer.Y + firstPlayer.Size &&
                        y > firstPlayer.Y)
                {
                    projectilesToRemove.Add(this);
                    firstPlayer.Health -= secondPlayer.Damage;
                }
                else
                {
                    X = x;
                    Y = y;
                }
            }
            if (idPlayer == 1)
            {
                if (x < secondPlayer.X + secondPlayer.Size &&
                        x > secondPlayer.X &&
                        y < secondPlayer.Y + secondPlayer.Size &&
                        y > secondPlayer.Y)
                {
                    projectilesToRemove.Add(this);
                    secondPlayer.Health -= firstPlayer.Damage;
                }
                else
                {
                    X = x;
                    Y = y;
                }
            }
        }
    }
}