using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Libr
{
    public class Map
    {
        public Cell[,] cells {  get;private set; }
        public Map(int width, int height, float cellSize, int[,] data)
        {
            cells = new Cell[width, height];
            float halfWidth = width * cellSize / 2;
            float halfHeight = height * cellSize / 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float xPos = (x * cellSize) - halfWidth;
                    float yPos = (y * cellSize) - halfHeight;
                    cells[x, y] = new Cell(xPos, yPos, cellSize, data[x, y] == 1);
                }
            }
        }

        public float[] GetVertColorArray()
        {
            List<float> result = new List<float>();

            foreach (Cell cell in cells)
            {
                float[] cellVertColorArr = cell.GetVertColorArray();
                foreach (float vertColor in cellVertColorArr)
                    if(cell.IsWall)
                        result.Add(vertColor);
            }

            return result.ToArray();
        }

        public List<Cell> GetListCells()
        {
            List<Cell> list = new List<Cell>();

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    list.Add(cells[i, j]);
                }
            }
            return list;
        }
    }
}
