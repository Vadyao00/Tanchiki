namespace Libr.GameObjects.Bonuses
{
    public class VirtualBonus
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Size { get; private set; } = 0.08f;
        public bool IsUsed { get; set; } = false;
        public double LifeTime { get; set; } = 0;
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

        public VirtualBonus(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public float[] GetVertexArray()
        {
            return
            [
             X, Y + Size, 0.0f, 0.0f,1.0f,
             X, Y, 0.0f, 0.0f,0.0f,
             X + Size, Y, 0.0f, 1.0f,0.0f,
             X + Size, Y, 0.0f, 1.0f,0.0f,
             X + Size, Y + Size, 0.0f, 1.0f,1.0f,
             X, Y + Size, 0.0f, 0.0f,1.0f
            ];
        }
    }
}
