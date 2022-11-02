
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

        public IndexedTriangleList GetTriangles()
	    {
            IndexedTriangleList triangles = new IndexedTriangleList();
            triangles.vertices = vertices;
            triangles.indices = new List<int>
            {
				0,2,1, 2,3,1,
				1,3,5, 3,7,5,
				2,6,3, 3,6,7,
				4,5,7, 4,7,6,
				0,4,2, 2,4,6,
				0,1,4, 1,5,4 
		    };
            triangles.cullFlags = new List<bool>();
            for (int i = 0; i < triangles.indices.Count / 3; i++)
            {
                triangles.cullFlags.Add(false);
            }
            return triangles;
        }

        List<Vec3<float>> vertices = new List<Vec3<float>>();
    }

    internal class Pyramid
    {
        public Pyramid(float size)
        {
            float side = size / 2.0f;
            vertices.Add(new Vec3<float>(0, -side, -0.86f * side));
            vertices.Add(new Vec3<float>(-side, -side, 0.86f * side));
            vertices.Add(new Vec3<float>(side, -side, 0.86f * side));
            vertices.Add(new Vec3<float>(0, side, 0.258f * side));
        }
        public IndexedLineList GetLines()
        {
            IndexedLineList lines = new IndexedLineList();
            lines.vertices = vertices;
            lines.indices = new List<int>
            {
                0,1,  1,2,  2,0,
                0,3,  1,3,  2,3,
            };
            return lines;
        }

        public IndexedTriangleList GetTriangles()
        {
            IndexedTriangleList triangles = new IndexedTriangleList();
            triangles.vertices = vertices;
            triangles.indices = new List<int>
            {
                1,3,0, 0,3,2,
                2,3,1, 2,1,0
            };
            triangles.cullFlags = new List<bool>();
            for (int i = 0; i < triangles.indices.Count / 3; i++)
            {
                triangles.cullFlags.Add(false);
            }
            return triangles;
        }

        List<Vec3<float>> vertices = new List<Vec3<float>>();
    }

}
