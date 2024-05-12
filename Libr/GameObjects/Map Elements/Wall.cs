namespace Libr
{
    /// <summary>
    /// Класс стены.
    /// </summary>
    /// <param name="x">Нижняя левая координата X.</param>
    /// <param name="y">Нижняя левая координата Y.</param>
    /// <param name="size">Размер стены</param>
    public class Wall(float x, float y, float size)
    {
        /// <summary>
        /// Нижняя левая координата X.
        /// </summary>
        public float X { get; set; } = x;
        /// <summary>
        /// Нижняя левая координата Y.
        /// </summary>
        public float Y { get; set; } = y;
        /// <summary>
        /// Размер стены.
        /// </summary>
        public float Size { get; set; } = size;
    }
}
