﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;


namespace Libr
{
    public enum AttribType
    {
        Float = VertexAttribPointerType.Float
    }

    public class ArrayObject : IDisposable
    {
        public int ArrayID { private set; get; }

        private bool _active = false;

        private List<int> _attribsList;

        private List<BufferObject> _buffersList;
        public ArrayObject()
        {
            _attribsList = new List<int>();
            _buffersList = new List<BufferObject>();
            ArrayID = GL.GenVertexArray();
        }

        public void Activate()
        {
            _active = true;
            GL.BindVertexArray(ArrayID);
        }

        public void Deactivate()
        {
            _active = false;
            GL.BindVertexArray(0);
        }

        public bool IsActive()
        {
            return _active;
        }

        public void AttachBufer(BufferObject buffer)
        {
            if (IsActive() != true)
                Activate();
            buffer.Activate();
            _buffersList.Add(buffer);
        }

        public void AttribPointer(int index, int elementsPerVertex, AttribType type, int stride, int offset)
        {
            _attribsList.Add(index);
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, elementsPerVertex, (VertexAttribPointerType)type, false, stride, offset);
        }

        public void Draw(int start, int count)
        {
            Activate();
            GL.DrawArrays(PrimitiveType.Triangles, start, count);
            Dispose();
        }

        public void DrawPoligon(int start, int count)
        {
            Activate();
            GL.DrawArrays(PrimitiveType.Polygon, start, count);
            Dispose();
        }

        public void DisableAttribAll()
        {
            foreach(int attrib in _attribsList)
                GL.DisableVertexAttribArray(attrib);
        }

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

        public void Dispose()
        {
            Delete();
            GC.SuppressFinalize(this);
        }
    }
}
