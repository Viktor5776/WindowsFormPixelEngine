using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PixelEngine
{
    internal class PixelGraphics
    {
        readonly Color[] PixelArray = new Color[640 * 640];

        public void PutPixel(int x, int y, Color c)
        {
            PixelArray[y * 640 + x] = c;
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

        public void DrawClosedPolyline(List<Vec2<float>> verts,Color c )
        {
            for (int i = 0; i < verts.Count - 1; i++)
            {
                DrawLine(verts[i], verts[i + 1], c);
            }
            DrawLine(verts[verts.Count - 1], verts[0], c );
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
            Bitmap bitmap = new Bitmap(640, 640, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int y = 0; y < 640; y++)
            {
                for (int x = 0; x < 640; x++)
                {
                    bitmap.SetPixel(x, y, PixelArray[y * 640 + x]);
                }
            }
            g.DrawImage(bitmap, 0, 0);
        }
    }
}
