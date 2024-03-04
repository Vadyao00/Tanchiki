namespace Libr
{
    public class Map
    {
        public Cell[,] Cells {  get;private set; }
        public List<Cell> ListWalls { get; private set; }
        public Map(int width, int height, float cellSize, int[,] data)
        {
            ListWalls = [];
            Cells = new Cell[width, height];
            float halfWidth = width * cellSize / 2;
            float halfHeight = height * cellSize / 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float xPos = (x * cellSize) - halfWidth;
                    float yPos = (y * cellSize) - halfHeight;
                    Cells[x, y] = new Cell(xPos, yPos, cellSize, data[x, y] == 1);
                }
            }

            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    if (Cells[i,j].IsWall)
                        ListWalls.Add(Cells[i, j]);
                }
            }
        }
    }
}
