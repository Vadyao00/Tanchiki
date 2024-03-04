namespace Libr
{
    public class Cell(float x, float y, float size, bool isWall)
    {
        public float X { get; set; } = x;
        public float Y { get; set; } = y;
        public float Size { get; set; } = size;
        public bool IsWall { get; set; } = isWall;
    }
}
