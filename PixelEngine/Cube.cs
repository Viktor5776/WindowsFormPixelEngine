
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelEngine
{
    internal class Cube
    {
        public Cube(float size)
        {
            float side = size / 2.0f;
            vertices.Add(new Vec3<float>(-side, -side, -side));
            vertices.Add(new Vec3<float>(side, -side, -side));
            vertices.Add(new Vec3<float>(-side, side, -side));
            vertices.Add(new Vec3<float>(side, side, -side));
            vertices.Add(new Vec3<float>(-side, -side, side));
            vertices.Add(new Vec3<float>(side, -side, side));
            vertices.Add(new Vec3<float>(-side, side, side));
            vertices.Add(new Vec3<float>(side, side, side));
        }
        public IndexedLineList GetLines()
    	{
            IndexedLineList lines = new IndexedLineList();
            lines.vertices = vertices;
            lines.indices = new List<int>
            {
                0,1,  1,3,  3,2,  2,0,
                0,4,  1,5,  3,7,  2,6,
                4,5,  5,7,  7,6,  6,4
            };
            return lines;
        }

        List<Vec3<float>> vertices = new List<Vec3<float>>();
    }
}
