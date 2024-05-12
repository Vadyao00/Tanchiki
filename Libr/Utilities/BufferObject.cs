using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Libr
{
    /// <summary>
    /// Представляет тип буфера вершин в OpenGL.
    /// </summary>
    public enum BufferType
    {
        ArrayBuffer = BufferTarget.ArrayBuffer,
        ElementBuffer = BufferTarget.ElementArrayBuffer
    }

    /// <summary>
    /// Представляет подсказку использования буфера вершин в OpenGL.
    /// </summary>
    public enum BufferHint
    {
        StaticDraw = BufferUsageHint.StaticDraw,
        DynamicDraw = BufferUsageHint.DynamicDraw
    }

    /// <summary>
    /// Представляет объект буфера вершин в OpenGL.
    /// </summary>
    public sealed class BufferObject : IDisposable
    {

        /// <summary>
        /// Идентификатор буфера вершин.
        /// </summary>
        public int BufferID { private set; get; } = 0;
        private readonly BufferTarget _type;
        private bool _active = false;

        /// <summary>
        /// Заполняет буфер вершин данными.
        /// </summary>
        /// <typeparam name="T">Тип данных.</typeparam>
        /// <param name="data">Массив данных.</param>
        /// <param name="hint">Подсказка использования буфера.</param>
        /// <exception cref="ArgumentException">Вызывается, если массив данных пуст.</exception>
        public void SetData<T>(T[] data, BufferHint hint)where T : struct
        {
            if (data.Length == 0)
                throw new ArgumentException("Массив должен содержать хотя бы один элемент", "data");
            Activate();
            GL.BufferData(_type, (IntPtr)(data.Length * Marshal.SizeOf(typeof(T))), data, BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BufferObject"/> с указанным типом буфера.
        /// </summary>
        /// <param name="type">Тип буфера вершин.</param>
        public BufferObject(BufferType type)
        {
            _type = (BufferTarget)type;
            BufferID = GL.GenBuffer();
        }

        /// <summary>
        /// Активирует данный буфер вершин.
        /// </summary>
        public void Activate()
        {
            _active = true;
            GL.BindBuffer(_type, BufferID);
        }

        /// <summary>
        /// Деактивирует данный буфер вершин.
        /// </summary>
        public void Deactivate()
        {
            _active = false;
            GL.BindBuffer(_type, 0);
        }

        /// <summary>
        /// Проверяет, является ли данный буфер вершин активным.
        /// </summary>
        /// <returns>Значение true, если буфер вершин активен, в противном случае - false.</returns>
        public bool IsActive()
        {
            return _active;
        }

        /// <summary>
        /// Удаляет данный буфер вершин.
        /// </summary>
        public void Delete()
        {
            if (BufferID == -1)
                return;

            Deactivate();

            GL.DeleteBuffer(BufferID);
            BufferID = -1;
        }

        /// <summary>
        /// Освобождает ресурсы, используемые данным объектом буфера вершин.
        /// </summary>
        public void Dispose()
        {
            Delete();
            GC.SuppressFinalize(this);
        }
    }
}
