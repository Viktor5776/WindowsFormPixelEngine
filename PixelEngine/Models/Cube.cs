using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PixelEngine.Pipeline;

namespace PixelEngine.Models
{
    internal class Cube
    {
        public static IndexedTriangleList<Vertex> GetSkinned(float size = 1.0f)
        {
            Func<float, float, Vec2<float>> ConverTextCoord = (u, v) =>
            {
                return new Vec2<float>((u + 1.0f) / 3.0f, v / 4.0f);
            };

            float side = size / 2.0f;

            List<Vec3<float>> vertices = new List<Vec3<float>>();
            List<Vec2<float>> tc = new List<Vec2<float>>();

            vertices.Add(new Vec3<float>(-side, -side, -side)); // 0
            tc.Add(ConverTextCoord(1.0f, 0.0f));
            vertices.Add(new Vec3<float>(side, -side, -side)); // 1
            tc.Add(ConverTextCoord(0.0f, 0.0f));
            vertices.Add(new Vec3<float>(-side, side, -side)); // 2
            tc.Add(ConverTextCoord(1.0f, 1.0f));
            vertices.Add(new Vec3<float>(side, side, -side)); // 3
            tc.Add(ConverTextCoord(0.0f, 1.0f));
            vertices.Add(new Vec3<float>(-side, -side, side)); // 4
            tc.Add(ConverTextCoord(1.0f, 3.0f));
            vertices.Add(new Vec3<float>(side, -side, side)); // 5
            tc.Add(ConverTextCoord(0.0f, 3.0f));
            vertices.Add(new Vec3<float>(-side, side, side)); // 6
            tc.Add(ConverTextCoord(1.0f, 2.0f));
            vertices.Add(new Vec3<float>(side, side, side)); // 7
            tc.Add(ConverTextCoord(0.0f, 2.0f));
            vertices.Add(new Vec3<float>(-side, -side, -side)); // 8
            tc.Add(ConverTextCoord(1.0f, 4.0f));
            vertices.Add(new Vec3<float>(side, -side, -side)); // 9
            tc.Add(ConverTextCoord(0.0f, 4.0f));
            vertices.Add(new Vec3<float>(-side, -side, -side)); // 10
            tc.Add(ConverTextCoord(2.0f, 1.0f));
            vertices.Add(new Vec3<float>(-side, -side, side)); // 11
            tc.Add(ConverTextCoord(2.0f, 2.0f));
            vertices.Add(new Vec3<float>(side, -side, -side)); // 12
            tc.Add(ConverTextCoord(-1.0f, 1.0f));
            vertices.Add(new Vec3<float>(side, -side, side)); // 13
            tc.Add(ConverTextCoord(-1.0f, 2.0f));

            IndexedTriangleList<Vertex> triangles = new IndexedTriangleList<Vertex>();

            List<Vertex> verts = new List<Vertex>();
            for (int i = 0; i < vertices.Count; i++)
            {
                verts.Add(new Vertex());
                verts[i].pos = vertices[i];
                verts[i].t = tc[i];
            }

            triangles.vertices = verts;

            triangles.indices = new List<int>
            {
                0,2,1,   2,3,1,
                4,8,5,   5,8,9,
                2,6,3,   3,6,7,
                4,5,7,   4,7,6,
                2,10,11, 2,11,6,
                12,3,7,  12,7,13
            };

            return triangles;
        }

    }
}
