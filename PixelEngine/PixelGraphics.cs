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

        public void SetPixel(int x, int y, Color colour)
        {
            int index = x + (y * Width);
            int col = colour.ToArgb();

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
