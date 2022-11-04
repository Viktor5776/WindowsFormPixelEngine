using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PixelEngine
{
    public class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public void SetPixel(int x, int y, Color color)
        {
            int index = x + (y * Width);
            int col = color.ToArgb();

            Bits[index] = col;
        }

        public Color GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            int col = Bits[index];
            Color result = Color.FromArgb(col);

            return result;
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }


    internal class PixelGraphics
    {
        DirectBitmap bitmap = new DirectBitmap(640, 640);
        public void PutPixel(int x, int y, Color c)
        {
            bitmap.SetPixel(x, y, c);
        }

        public void DrawLine(Vec2<float> p0, Vec2<float> p1, Color c)
        {
            float m = 0.0f;
            if (p1.x != p0.x)
            {
                m = (p1.y - p0.y) / (p1.x - p0.x);
            }

            if (p1.x != p0.x && Math.Abs(m) <= 1.0f)
            {
                if (p0.x > p1.x)
                {
                    (p0, p1) = (p1, p0);
                }

                float b = p0.y - m * p0.x;

                for (int x = (int)p0.x; x <= (int)p1.x; x++)
                {
                    float y = m * (float)x + b;

                    int yi = (int)y;
                    if (x >= 0 && x < 640 && yi >= 0 && yi < 640)
                    {
                        PutPixel(x, yi, c);
                    }
                }
            }
            else
            {
                if (p0.y > p1.y)
                {
                    (p0, p1) = (p1, p0);
                }

                float w = (p1.x - p0.x) / (p1.y - p0.y);
                float p = p0.x - w * p0.y;

                for (int y = (int)p0.y; y <= (int)p1.y; y++)
                {
                    float x = w * (float)y + p;

                    int xi = (int)x;
                    if (xi >= 0 && xi < 640 && y >= 0 && y < 640)
                    {
                        PutPixel(xi, y, c);
                    }
                }
            }
        }

        public void DrawClosedPolyline(List<Vec2<float>> verts, Color c)
        {
            for (int i = 0; i < verts.Count - 1; i++)
            {
                DrawLine(verts[i], verts[i + 1], c);
            }
            DrawLine(verts[verts.Count - 1], verts[0], c);
        }

        public void DrawTriangle(Vec2<float> v0, Vec2<float> v1, Vec2<float> v2, Color c)
        {

            if (v1.y < v0.y) 
            {
                Vec2<float> temp = v0;
                v0 = v1;
                v1 = temp;
            }
	        if (v2.y < v1.y) 
            {
                Vec2<float> temp = v1;
                v1 = v2;
                v2 = temp;
            }
	        if (v1.y < v0.y) 
            {
                Vec2<float> temp = v0;
                v0 = v1;
                v1 = temp;
            }

	        if (v0.y == v1.y) // Flat Top
	        {
		        if (v1.x < v0.x)
                {
                    Vec2<float> temp = v0;
                    v0 = v1;
                    v1 = temp;
                }
                DrawFlatTopTriangle(v0, v1, v2, c);
            }
	        else if (v1.y == v2.y) // Flat Bottom
	        {
		        if (v2.x < v1.x)
                {
                    Vec2<float> temp = v1;
                    v1 = v2;
                    v2 = temp;
                }
                DrawFlatBottomTriangle(v0, v1, v2, c);
            }
            else // General Triangle
            {
                float alphaSplit = (v1.y - v0.y) / (v2.y - v0.y);
                Vec2<float> vi = v0 + (v2 - v0) * alphaSplit;
                if (v1.x < vi.x) // Major Right Triangle
                {
                    DrawFlatBottomTriangle(v0, v1, vi, c);
                    DrawFlatTopTriangle(v1, vi, v2, c);
                }
                else // Major Left
                {   
                    DrawFlatBottomTriangle(v0, vi, v1, c);
                    DrawFlatTopTriangle(vi, v1, v2, c);
                }
            }
        }

        private void DrawFlatTopTriangle(Vec2<float> v0, Vec2<float> v1, Vec2<float> v2, Color c)
        {   

            float k0 = (v2.x - v0.x) / (v2.y - v0.y);
            float k1 = (v2.x - v1.x) / (v2.y - v1.y);

            int yStart = (int)Math.Ceiling(v0.y - 0.5f);
            int yEnd = (int)Math.Ceiling(v2.y - 0.5f);

	        for (int y = yStart; y<yEnd; y++)
	        {
		        float px0 = k0 * ((float)(y) + 0.5f - v0.y) + v0.x;
                float px1 = k1 * ((float)(y) + 0.5f - v1.y) + v1.x;

                int xStart = (int)Math.Ceiling(px0 - 0.5f);
                int xEnd = (int)Math.Ceiling(px1 - 0.5f);

		        for (int x = xStart; x<xEnd; x++)
		        {
			        PutPixel(x, y, c);
                }
            }
        }

        private void DrawFlatBottomTriangle(Vec2<float> v0, Vec2<float> v1, Vec2<float> v2, Color c)
        {

            float k0 = (v1.x - v0.x) / (v1.y - v0.y);
            float k1 = (v2.x - v0.x) / (v2.y - v0.y);

            int yStart = (int)Math.Ceiling(v0.y - 0.5f);
            int yEnd = (int)Math.Ceiling(v2.y - 0.5f);

            for (int y = yStart; y < yEnd; y++)
            {
                float px0 = k0 * ((float)(y) + 0.5f - v0.y) + v0.x;
                float px1 = k1 * ((float)(y) + 0.5f - v0.y) + v0.x;

                int xStart = (int)Math.Ceiling(px0 - 0.5f);
                int xEnd = (int)Math.Ceiling(px1 - 0.5f);

                for (int x = xStart; x < xEnd; x++)
                {
                    PutPixel(x, y, c);
                }
            }
        }

        public void DrawTriangleTex(TexVertex v0, TexVertex v1, TexVertex v2, DirectBitmap tex)
        {

            if (v1.pos.y < v0.pos.y)
            {
                TexVertex temp = v0;
                v0 = v1;
                v1 = temp;
            }
            if (v2.pos.y < v1.pos.y)
            {
                TexVertex temp = v1;
                v1 = v2;
                v2 = temp;
            }
            if (v1.pos.y < v0.pos.y)
            {
                TexVertex temp = v0;
                v0 = v1;
                v1 = temp;
            }

            if (v0.pos.y == v1.pos.y) // Flat Top
            {
                if (v1.pos.x < v0.pos.x)
                {
                    TexVertex temp = v0;
                    v0 = v1;
                    v1 = temp;
                }
                DrawFlatTopTriangleTex(v0, v1, v2, tex);
            }
            else if (v1.pos.y == v2.pos.y) // Flat Bottom
            {
                if (v2.pos.x < v1.pos.x)
                {
                    TexVertex temp = v1;
                    v1 = v2;
                    v2 = temp;
                }
                DrawFlatBottomTriangleTex(v0, v1, v2, tex);
            }
            else // General Triangle
            {
                float alphaSplit = (v1.pos.y - v0.pos.y) / (v2.pos.y - v0.pos.y);
                TexVertex vi = v0.InterpolateTo(v2,alphaSplit);
                if (v1.pos.x < vi.pos.x) // Major Right Triangle
                {
                    DrawFlatBottomTriangleTex(v0, v1, vi, tex);
                    DrawFlatTopTriangleTex(v1, vi, v2, tex);
                }
                else // Major Left
                {
                    DrawFlatBottomTriangleTex(v0, vi, v1, tex);
                    DrawFlatTopTriangleTex(vi, v1, v2, tex);
                }
            }
        }

        private void DrawFlatTopTriangleTex(TexVertex v0, TexVertex v1, TexVertex v2, DirectBitmap tex)
        {
            float delta_y = v2.pos.y - v0.pos.y;
            TexVertex dv0 = (v2 - v0) / delta_y;
            TexVertex dv1 = (v2 - v1) / delta_y;

            TexVertex itEdge1 = v1;

            DrawFlatTriangleTex(v0, v1, v2, tex, dv0, dv1, itEdge1);
        }

        private void DrawFlatBottomTriangleTex(TexVertex v0, TexVertex v1, TexVertex v2, DirectBitmap tex)
        {

            float delta_y = v2.pos.y - v0.pos.y;
            TexVertex dv0 = (v1 - v0) / delta_y;
            TexVertex dv1 = (v2 - v0) / delta_y;

            TexVertex itEdge1 = v0;

            DrawFlatTriangleTex(v0, v1, v2, tex, dv0, dv1, itEdge1);
        }

        private void DrawFlatTriangleTex( TexVertex v0, TexVertex v1, TexVertex v2, DirectBitmap tex,
							  TexVertex dv0, TexVertex dv1, TexVertex itEdge1 )
        {
            TexVertex itEdge0 = v0;

            int yStart = (int)Math.Ceiling(v0.pos.y - 0.5f);
            int yEnd = (int)Math.Ceiling(v2.pos.y - 0.5f);

            itEdge0 += dv0 * ((float)yStart + 0.5f - v0.pos.y);
            itEdge1 += dv1 * ((float)yStart + 0.5f - v0.pos.y);

            float tex_width = tex.Width;
            float tex_height = tex.Height;
            float tex_clamp_x = tex_width - 1.0f;
            float tex_clamp_y = tex_height - 1.0f;

            for (int y = yStart; y < yEnd; y++, itEdge0 += dv0, itEdge1 += dv1)
            {
                int xStart = (int)Math.Ceiling(itEdge0.pos.x - 0.5f);
                int xEnd = (int)Math.Ceiling(itEdge1.pos.x - 0.5f);

                Vec2<float> dtcLine = (itEdge1.tc - itEdge0.tc) / (itEdge1.pos.x - itEdge0.pos.x);

                Vec2<float> itcLine = itEdge0.tc + dtcLine * ((float)xStart + 0.5f - itEdge0.pos.x);

                for (int x = xStart; x < xEnd; x++, itcLine += dtcLine)
                {
                    PutPixel(x, y, tex.GetPixel(
                        (int)Math.Min(itcLine.x * tex_width, tex_clamp_x),
                        (int)Math.Min(itcLine.y * tex_height, tex_clamp_y)
                    ));
                }
            }
        }

        public void DrawTriangleTexWrap(TexVertex v0, TexVertex v1, TexVertex v2, DirectBitmap tex)
        {

            if (v1.pos.y < v0.pos.y)
            {
                TexVertex temp = v0;
                v0 = v1;
                v1 = temp;
            }
            if (v2.pos.y < v1.pos.y)
            {
                TexVertex temp = v1;
                v1 = v2;
                v2 = temp;
            }
            if (v1.pos.y < v0.pos.y)
            {
                TexVertex temp = v0;
                v0 = v1;
                v1 = temp;
            }

            if (v0.pos.y == v1.pos.y) // Flat Top
            {
                if (v1.pos.x < v0.pos.x)
                {
                    TexVertex temp = v0;
                    v0 = v1;
                    v1 = temp;
                }
                DrawFlatTopTriangleTexWrap(v0, v1, v2, tex);
            }
            else if (v1.pos.y == v2.pos.y) // Flat Bottom
            {
                if (v2.pos.x < v1.pos.x)
                {
                    TexVertex temp = v1;
                    v1 = v2;
                    v2 = temp;
                }
                DrawFlatBottomTriangleTexWrap(v0, v1, v2, tex);
            }
            else // General Triangle
            {
                float alphaSplit = (v1.pos.y - v0.pos.y) / (v2.pos.y - v0.pos.y);
                TexVertex vi = v0.InterpolateTo(v2, alphaSplit);
                if (v1.pos.x < vi.pos.x) // Major Right Triangle
                {
                    DrawFlatBottomTriangleTexWrap(v0, v1, vi, tex);
                    DrawFlatTopTriangleTexWrap(v1, vi, v2, tex);
                }
                else // Major Left
                {
                    DrawFlatBottomTriangleTexWrap(v0, vi, v1, tex);
                    DrawFlatTopTriangleTexWrap(vi, v1, v2, tex);
                }
            }
        }

        private void DrawFlatTopTriangleTexWrap(TexVertex v0, TexVertex v1, TexVertex v2, DirectBitmap tex)
        {
            float delta_y = v2.pos.y - v0.pos.y;
            TexVertex dv0 = (v2 - v0) / delta_y;
            TexVertex dv1 = (v2 - v1) / delta_y;

            TexVertex itEdge1 = v1;

            DrawFlatTriangleTexWrap(v0, v1, v2, tex, dv0, dv1, itEdge1);
        }

        private void DrawFlatBottomTriangleTexWrap(TexVertex v0, TexVertex v1, TexVertex v2, DirectBitmap tex)
        {

            float delta_y = v2.pos.y - v0.pos.y;
            TexVertex dv0 = (v1 - v0) / delta_y;
            TexVertex dv1 = (v2 - v0) / delta_y;

            TexVertex itEdge1 = v0;

            DrawFlatTriangleTexWrap(v0, v1, v2, tex, dv0, dv1, itEdge1);
        }

        private void DrawFlatTriangleTexWrap(TexVertex v0, TexVertex v1, TexVertex v2, DirectBitmap tex,
                              TexVertex dv0, TexVertex dv1, TexVertex itEdge1)
        {
            TexVertex itEdge0 = v0;

            int yStart = (int)Math.Ceiling(v0.pos.y - 0.5f);
            int yEnd = (int)Math.Ceiling(v2.pos.y - 0.5f);

            itEdge0 += dv0 * ((float)yStart + 0.5f - v0.pos.y);
            itEdge1 += dv1 * ((float)yStart + 0.5f - v0.pos.y);

            float tex_width = tex.Width;
            float tex_height = tex.Height;
            float tex_clamp_x = tex_width - 1.0f;
            float tex_clamp_y = tex_height - 1.0f;

            for (int y = yStart; y < yEnd; y++, itEdge0 += dv0, itEdge1 += dv1)
            {
                int xStart = (int)Math.Ceiling(itEdge0.pos.x - 0.5f);
                int xEnd = (int)Math.Ceiling(itEdge1.pos.x - 0.5f);

                Vec2<float> dtcLine = (itEdge1.tc - itEdge0.tc) / (itEdge1.pos.x - itEdge0.pos.x);

                Vec2<float> itcLine = itEdge0.tc + dtcLine * ((float)xStart + 0.5f - itEdge0.pos.x);

                for (int x = xStart; x < xEnd; x++, itcLine += dtcLine)
                {
                    PutPixel(x, y, tex.GetPixel(
                        (int)((itcLine.x * tex_width) % tex_clamp_x),
                        (int)((itcLine.y * tex_height) % tex_clamp_y)
                    ));
                }
            }
        }

        public void ResetScreen()
        {
            for (int x = 0; x < 640; x++)
            {
                for (int y = 0; y < 640; y++)
                {
                    PutPixel(x, y, Color.Black);
                }
            }
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(bitmap.Bitmap, 0, 0);
        }
    }
}
