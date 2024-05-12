namespace Libr
{
    /// <summary>
    /// Класс карты.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Коллекция стен в ввиде объектов класса <see cref="Wall"/>.
        /// </summary>
        public List<Wall> ListWalls { get; private set; }
        /// <summary>
        /// Конструктор, заполняющий коллекцию стен объектами класса <see cref="Wall"/>.
        /// </summary>
        /// <param name="width">Количество клеток в ширину.</param>
        /// <param name="height">Количество клеток в высоту.</param>
        /// <param name="cellSize">Размер клетки.</param>
        /// <param name="data">Массив, представляющий карту в виде цифр 1 и 0, где 1 это стена.</param>
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
