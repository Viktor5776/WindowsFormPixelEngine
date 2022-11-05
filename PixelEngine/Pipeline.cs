using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelEngine
{
    internal class Pipeline
    {
        public class Vertex
        {

            public Vertex()
            {
                pos = new Vec3<float>();
                t = new Vec2<float>();
            }
            public Vertex(Vec3<float> pos_in, Vec2<float> t_in)
            {
                pos = pos_in;
                t = t_in;
            }

            public Vertex (Vertex rhs)
            {
                pos = new Vec3<float>();
                t = new Vec2<float>();

                this.pos.x = rhs.pos.x;
                this.pos.y = rhs.pos.y;
                this.pos.z = rhs.pos.z;
                
                this.t.x = rhs.t.x;
                this.t.y = rhs.t.y;
            }
            public static Vertex operator +(Vertex lhs, Vertex rhs)
            {
                return new Vertex(lhs.pos + rhs.pos, lhs.t + rhs.t);
            }
            public static Vertex operator -(Vertex lhs, Vertex rhs)
            {
                return new Vertex(lhs.pos - rhs.pos, lhs.t - rhs.t);
            }
            public static Vertex operator *(Vertex lhs, float rhs)
            {
                return new Vertex(lhs.pos * rhs, lhs.t * rhs);
            }
            public static Vertex operator /(Vertex lhs, float rhs)
            {
                return new Vertex(lhs.pos / rhs, lhs.t / rhs);
            }

            public Vec3<float> pos;
            public Vec2<float> t;
        }

        public Pipeline(PixelGraphics gfx_in)
        {
            gfx = gfx_in;
        }

        public void Draw(IndexedTriangleList<Vertex> triList)
        {
            ProccessVertices(triList.vertices, triList.indices);
        }

        public void BindRotation(Mat3<float> roation_in)
        {
            rotation = roation_in;
        }

        public void BindTranslation(Vec3<float> translation_in)
        {
            translation = translation_in;
        }

        public void BindTexture(string filename)
        {
            Bitmap bmp = new Bitmap(Image.FromFile(filename));

            tex = new DirectBitmap(bmp.Width, bmp.Height);
            for (int y = 0; y < tex.Height; y++)
            {
                for (int x = 0; x < tex.Width; x++)
                {
                    tex.SetPixel(x, y, bmp.GetPixel(x, y));
                }
            }
        }

        private void ProccessVertices(List<Vertex> vertices, List<int> indices)
        {
            List<Vertex> verticesOut = new List<Vertex>();

            foreach(Vertex v in vertices)
            {
                verticesOut.Add(new Vertex(v.pos * rotation + translation,v.t));
            }

            AssembleTriangles(verticesOut, indices);
        }

        private void AssembleTriangles(List<Vertex> vertices, List<int> indices)
        {
            for (int i = 0; i < indices.Count / 3; i++)
            {
                Vertex v0 = vertices[indices[i * 3]];
                Vertex v1 = vertices[indices[i * 3 + 1]];
                Vertex v2 = vertices[indices[i * 3 + 2]];

                if((v1.pos - v0.pos) % (v2.pos - v0.pos) * v0.pos < 0.0f)
                {
                    ProcessTriangle(v0, v1, v2);
                }
            }
        }

        private void ProcessTriangle(Vertex v0_in, Vertex v1_in, Vertex v2_in)
        {
            Vertex v0 = new Vertex(v0_in);
            Vertex v1 = new Vertex(v1_in);
            Vertex v2 = new Vertex(v2_in);
            PostProcessTriangleVertices(new Triangle<Vertex>(v0, v1, v2));
        }

        private void PostProcessTriangleVertices(Triangle<Vertex> triangle)
        {
            pst.Transform(triangle.v0.pos);
            pst.Transform(triangle.v1.pos);
            pst.Transform(triangle.v2.pos);

            DrawTriangle(triangle);
        }

        private void DrawTriangle(Triangle<Vertex> triangle)
        {
            Vertex v0 = triangle.v0;
            Vertex v1 = triangle.v1;
            Vertex v2 = triangle.v2;
            
            //Sorting Vertices by y
            if (v1.pos.y < v0.pos.y)
            {
                Vertex temp = v0;
                v0 = v1;
                v1 = temp;
            }
            if (v2.pos.y < v1.pos.y)
            {
                Vertex temp = v1;
                v1 = v2;
                v2 = temp;
            }
            if (v1.pos.y < v0.pos.y)
            {
                Vertex temp = v0;
                v0 = v1;
                v1 = temp;
            }

            if (v0.pos.y == v1.pos.y) // Flat Top
            {
                if (v1.pos.x < v0.pos.x)
                {
                    Vertex temp = v0;
                    v0 = v1;
                    v1 = temp;
                }
                DrawFlatTopTriangle(v0, v1, v2);
            }
            else if (v1.pos.y == v2.pos.y) // Flat Bottom
            {
                if (v2.pos.x < v1.pos.x)
                {
                    Vertex temp = v1;
                    v1 = v2;
                    v2 = temp;
                }
                DrawFlatBottomTriangle(v0, v1, v2);
            }
            else // General Triangle
            {
                float alphaSplit = (v1.pos.y - v0.pos.y) / (v2.pos.y - v0.pos.y);
                Vertex vi = v0 + (v2 - v0) * alphaSplit;
                if (v1.pos.x < vi.pos.x) // Major Right Triangle
                {
                    DrawFlatBottomTriangle(v0, v1, vi);
                    DrawFlatTopTriangle(v1, vi, v2);
                }
                else // Major Left
                {
                    DrawFlatBottomTriangle(v0, vi, v1);
                    DrawFlatTopTriangle(vi, v1, v2);
                }
            }
        }

        private void DrawFlatTopTriangle(Vertex it0, Vertex it1, Vertex it2)
        {
            float delta_y = it2.pos.y - it0.pos.y;
            Vertex dit0 = (it2 - it0) / delta_y;
            Vertex dit1 = (it2 - it1) / delta_y;

            Vertex itEdge1 = it1;

            DrawFlatTriangle(it0, it1, it2, dit0, dit1, itEdge1);
        }

        private void DrawFlatBottomTriangle(Vertex it0, Vertex it1, Vertex it2)
        {
            float delta_y = it2.pos.y - it0.pos.y;
            Vertex dit0 = (it1 - it0) / delta_y;
            Vertex dit1 = (it2 - it0) / delta_y;

            Vertex itEdge1 = it0;

            DrawFlatTriangle(it0, it1, it2, dit0, dit1, itEdge1);
        }

        private void DrawFlatTriangle(Vertex it0, Vertex it1, Vertex it2,
                                      Vertex dv0, Vertex dv1, Vertex itEdge1)
        {
            Vertex itEdge0 = it0;

            int yStart = (int)Math.Ceiling(it0.pos.y - 0.5f);
            int yEnd = (int)Math.Ceiling(it2.pos.y - 0.5f);

            itEdge0 += dv0 * ((float)yStart + 0.5f - it0.pos.y);
            itEdge1 += dv1 * ((float)yStart + 0.5f - it0.pos.y);

            float tex_width = tex.Width;
            float tex_height = tex.Height;
            float tex_clamp_x = tex_width - 1.0f;
            float tex_clamp_y = tex_height - 1.0f;

            for (int y = yStart; y < yEnd; y++, itEdge0 += dv0, itEdge1 += dv1)
            {
                int xStart = (int)Math.Ceiling(itEdge0.pos.x - 0.5f);
                int xEnd = (int)Math.Ceiling(itEdge1.pos.x - 0.5f);

                Vertex iLine = itEdge0;

                float dx = itEdge1.pos.x - itEdge0.pos.x;
                Vertex diLine = (itEdge1 - iLine) / dx;

                iLine += diLine * (xStart + 0.5f - itEdge0.pos.x);

                for (int x = xStart; x < xEnd; x++, iLine += diLine)
                {
                    gfx.PutPixel(x, y, tex.GetPixel(
                        (int)Math.Max(Math.Min(iLine.t.x * tex_width + 0.5f, tex_clamp_x),0),
                        (int)Math.Max(Math.Min(iLine.t.y * tex_height + 0.5f, tex_clamp_y),0)
                    ));
                }
            }
        }

        PixelGraphics gfx;
	    PubeScreenTransformer pst = new PubeScreenTransformer();
        Mat3<float> rotation;
        Vec3<float> translation;
        DirectBitmap tex;
    }
}
