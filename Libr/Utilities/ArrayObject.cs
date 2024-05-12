using OpenTK.Graphics.OpenGL;


namespace Libr
{
    /// <summary>
    /// Представляет объект массива вершин в OpenGL.
    /// </summary>
    public class ArrayObject : IDisposable
    {
        /// <summary>
        /// Идентификатор массива вершин.
        /// </summary>
        public int ArrayID { private set; get; }
        /// <summary>
        /// Флаг, показывающий активен ли vao
        /// </summary>

        private bool _active = false;

        private List<int> _attribsList;

        private List<BufferObject> _buffersList;
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ArrayObject"/>.
        /// </summary>
        public ArrayObject()
        {
            _attribsList = new List<int>();
            _buffersList = new List<BufferObject>();
            ArrayID = GL.GenVertexArray();
        }
        /// <summary>
        /// Активирует данный массив вершин.
        /// </summary>
        public void Activate()
        {
            _active = true;
            GL.BindVertexArray(ArrayID);
        }
        /// <summary>
        /// Деактивирует данный массив вершин.
        /// </summary>
        public void Deactivate()
        {
            _active = false;
            GL.BindVertexArray(0);
        }
        /// <summary>
        /// Проверяет, является ли данный массив вершин активным.
        /// </summary>
        /// <returns>Значение true, если массив вершин активен, в противном случае - false.</returns>
        public bool IsActive()
        {
            return _active;
        }
        /// <summary>
        /// Присоединяет буфер к данному массиву вершин.
        /// </summary>
        /// <param name="buffer">Буфер объекта.</param>
        public void AttachBufer(BufferObject buffer)
        {
            if (IsActive() != true)
                Activate();
            buffer.Activate();
            _buffersList.Add(buffer);
        }
        /// <summary>
        /// Устанавливает указатель атрибута вершин.
        /// </summary>
        /// <param name="index">Индекс атрибута вершины.</param>
        /// <param name="elementsPerVertex">Количество элементов в вершине.</param>
        /// <param name="type">Тип данных атрибута.</param>
        /// <param name="stride">Шаг между атрибутами.</param>
        /// <param name="offset">Смещение атрибута.</param>
        public void AttribPointer(int index, int elementsPerVertex, VertexAttribPointerType type, int stride, int offset)
        {
            _attribsList.Add(index);
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, elementsPerVertex, type, false, stride, offset);
        }
        /// <summary>
        /// Рисует примитивы с использованием данного массива вершин.
        /// </summary>
        /// <param name="start">Начальный индекс вершины.</param>
        /// <param name="count">Количество вершин.</param>
        public void Draw(int start, int count)
        {
            Activate();
            GL.DrawArrays(PrimitiveType.Triangles, start, count);
            Dispose();
        }
        /// <summary>
        /// Рисует полигон с использованием данного массива вершин.
        /// </summary>
        /// <param name="start">Начальный индекс вершины.</param>
        /// <param name="count">Количество вершин.</param>
        public void DrawPolygon(int start, int count)
        {
            Activate();
            GL.DrawArrays(PrimitiveType.Polygon, start, count);
            Dispose();
        }
        /// <summary>
        /// Отключает все атрибуты вершин.
        /// </summary>
        public void DisableAttribAll()
        {
            foreach(int attrib in _attribsList)
                GL.DisableVertexAttribArray(attrib);
        }
        /// <summary>
        /// Удаляет данный массив вершин и связанные с ним буферы.
        /// </summary>
        public void Delete()
        {
            if (ArrayID == -1)
                return;

            Deactivate();

            GL.DeleteVertexArray(ArrayID);

            foreach(BufferObject buffer in _buffersList)
                buffer.Dispose();

            ArrayID = -1;
        }
        /// <summary>
        /// Освобожождает ресурсы, используемые данным объектом массива вершин.
        /// </summary>
        public void Dispose()
        {
            Delete();
            GC.SuppressFinalize(this);
        }
    }
}
