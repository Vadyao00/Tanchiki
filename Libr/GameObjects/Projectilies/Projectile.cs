namespace Libr.GameObjects.Projectilies
{
    public class Projectile
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        private float speedProjectile;
        public Movement direction { get; private set; }
        public Projectile(Movement dir, float[] vertexArray)
        {
            X = vertexArray[0];
            Y = vertexArray[1];
            direction = dir;
            speedProjectile = 0.008f;
        }

        public float[] Move(List<Cell> listWalls, List<Projectile> projectilesToRemove, Player firstPlayer, Player secondPlayer, int idPlayer)
        {
            float x = X, y = Y;
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
            foreach (Cell cell in listWalls)
            {
                if (x < cell.X + cell.Size &&
                x > cell.X &&
                y < cell.Y + cell.Size &&
                y > cell.Y && cell.IsWall)
                {
                    projectilesToRemove.Add(this);
                    return [X, Y];
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
                    return [X, Y];
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
                    return [X, Y];
                }
                else
                {
                    X = x;
                    Y = y;
                }
            }

            return [X, Y];
        }
    }
}
