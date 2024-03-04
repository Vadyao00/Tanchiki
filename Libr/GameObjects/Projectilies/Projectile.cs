namespace Libr.GameObjects.Projectilies
{
    public class Projectile(Movement direction, float[] vertexArray)
    {
        public float X { get; private set; } = vertexArray[0];
        public float Y { get; private set; } = vertexArray[1];

        private readonly float speedProjectile = 0.008f;
        public Movement Direction { get; private set; } = direction;

        public void Move(List<Wall> listWalls, List<Projectile> projectilesToRemove, Player firstPlayer, Player secondPlayer, int idPlayer)
        {
            float x = X, y = Y;
            switch (Direction)
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
