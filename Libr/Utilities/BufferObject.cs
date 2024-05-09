using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Libr
{
    public enum BufferType
    {
        ArrayBuffer = BufferTarget.ArrayBuffer,
        ElementBuffer = BufferTarget.ElementArrayBuffer
    }

    public enum BufferHint
    {
        StaticDraw = BufferUsageHint.StaticDraw,
        DynamicDraw = BufferUsageHint.DynamicDraw
    }

    public sealed class BufferObject : IDisposable
    {
        public int BufferID { private set; get; } = 0;
        private readonly BufferTarget _type;
        private bool _active = false;

        public void SetData<T>(T[] data, BufferHint hint)where T : struct
        {
            if (data.Length == 0)
                throw new ArgumentException("Массив должен содержать хотя бы один элемент", "data");
            Activate();
            GL.BufferData(_type, (IntPtr)(data.Length * Marshal.SizeOf(typeof(T))), data, BufferUsageHint.StaticDraw);
        }

        public BufferObject(BufferType type)
        {
            _type = (BufferTarget)type;
            BufferID = GL.GenBuffer();
        }

        public void Activate()
        {
            _active = true;
            GL.BindBuffer(_type, BufferID);
        }

        public void Deactivate()
        {
            _active = false;
            GL.BindBuffer(_type, 0);
        }

        public bool IsActive()
        {
            return _active;
        }

        public void Delete()
        {
            if (BufferID == -1)
                return;

            Deactivate();

            GL.DeleteBuffer(BufferID);
            BufferID = -1;
        }

        public void Dispose()
        {
            Delete();
            GC.SuppressFinalize(this);
        }
    }
}
