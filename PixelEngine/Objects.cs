
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
            vertices.Add(new Vec3<float>(-side, -side, -side));
            vertices.Add(new Vec3<float>(side, -side, -side));
            vertices.Add(new Vec3<float>(side, -side, side));
            vertices.Add(new Vec3<float>(-side, -side, side));
            vertices.Add(new Vec3<float>(0, side, 0));
        }
        public IndexedLineList GetLines()
        {
            IndexedLineList lines = new IndexedLineList();
            lines.vertices = vertices;
            lines.indices = new List<int>
            {
                0,1,  1,2,  2,3,  3,0,
                0,4,  1,4,  2,4,  3,4
            };
            return lines;
        }

        List<Vec3<float>> vertices = new List<Vec3<float>>();
    }

    internal class Tie
    {
        public Tie(float size)
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

            // 8 mid left, 9 mid left wíng
            vertices.Add(new Vec3<float>(-side, 0, 0));
            vertices.Add(new Vec3<float>(-2 * side, 0, 0));

            // 10 mid right, 11 mid right wíng
            vertices.Add(new Vec3<float>(side, 0, 0));
            vertices.Add(new Vec3<float>(2 * side, 0, 0));

            //12 left win
            vertices.Add(new Vec3<float>(-2 * side, 1.5f * side, 1.5f * side));
            vertices.Add(new Vec3<float>(-2 * side, 1.5f * side, -1.5f * side));
            vertices.Add(new Vec3<float>(-2 * side, -1.5f * side, 1.5f * side));
            vertices.Add(new Vec3<float>(-2 * side, -1.5f * side, -1.5f * side));

            //16 right win
            vertices.Add(new Vec3<float>(2 * side, 1.5f * side, 1.5f * side));
            vertices.Add(new Vec3<float>(2 * side, 1.5f * side, -1.5f * side));
            vertices.Add(new Vec3<float>(2 * side, -1.5f * side, 1.5f * side));
            vertices.Add(new Vec3<float>(2 * side, -1.5f * side, -1.5f * side));
        }
        public IndexedLineList GetLines()
        {
            IndexedLineList lines = new IndexedLineList();
            lines.vertices = vertices;
            lines.indices = new List<int>
            {
                0,1,  1,3,  3,2,  2,0,
                0,4,  1,5,  3,7,  2,6,
                4,5,  5,7,  7,6,  6,4,

                8,9,
                12,13,12,14,
                14,15,15,13,

                10,11,
                16,17,16,18,
                18,19,19,17

            };
            return lines;
        }

        List<Vec3<float>> vertices = new List<Vec3<float>>();
    }
}
