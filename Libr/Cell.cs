using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenTK.Graphics.OpenGL;

namespace Libr
{
    public class Cell
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Size { get; set; }
        public bool IsWall { get; set; }
        public Color Color { get; set; }
        public int TextureId { get; set; }
        public Cell(float x, float y, float size, bool isWall)
        {
            X = x;
            Y = y;
            Size = size;
            IsWall = isWall;
            Color = isWall ? Color.Black : Color.White;
        }

        public float[] GetVertColorArray()
        {
            return
            [
             X, Y + Size, 0.0f, Color.R / 255.0f, Color.G / 255.0f, Color.B / 255.0f, 1.0f ,        1.0f,1.0f,
             X, Y, 0.0f, Color.R / 255.0f, Color.G / 255.0f, Color.B / 255.0f, 1.0f ,               0.0f,1.0f,      
             X + Size, Y, 0.0f, Color.R / 255.0f, Color.G / 255.0f, Color.B / 255.0f, 1.0f ,        0.0f,0.0f,

             X + Size, Y, 0.0f, Color.R / 255.0f, Color.G / 255.0f, Color.B / 255.0f, 1.0f,         0.0f,0.0f,
             X + Size, Y + Size, 0.0f, Color.R / 255.0f, Color.G / 255.0f, Color.B / 255.0f, 1.0f,  1.0f,0.0f,
             X, Y + Size, 0.0f, Color.R / 255.0f, Color.G / 255.0f, Color.B / 255.0f, 1.0f      ,    1.0f,1.0f
            ];
        }
    }
}
