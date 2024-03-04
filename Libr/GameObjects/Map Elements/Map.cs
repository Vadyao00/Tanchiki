namespace Libr
{
    public class Map
    {
        public List<Wall> ListWalls { get; private set; }
        public Map(int width, int height, float cellSize, int[,] data)
        {
            ListWalls = [];
            float halfWidth = width * cellSize / 2;
            float halfHeight = height * cellSize / 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float xPos = (x * cellSize) - halfWidth;
                    float yPos = (y * cellSize) - halfHeight;
                    if (data[x,y] == 1)
                        ListWalls.Add(new Wall(xPos, yPos, cellSize));
                }
            }
        }
    }
}
