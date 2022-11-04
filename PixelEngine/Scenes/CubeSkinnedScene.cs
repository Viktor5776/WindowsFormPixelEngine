using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Reflection;
using PixelEngine.Models;

namespace PixelEngine.Scenes
{
    internal class CubeSkinnedScene : Scene
    {
        public CubeSkinnedScene(string filename)
        {
            Bitmap bmp = new Bitmap(Image.FromFile(filename));

            texture = new DirectBitmap(bmp.Width, bmp.Height);
            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    texture.SetPixel(x, y, bmp.GetPixel(x, y));
                }
            }
        }

        public override void Update(Keyboard keyboard, float dt)
        {
            if (keyboard.GetKeyPressed(Keys.Q))
            {
                theta_x = wrap_angle(theta_x + dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.W))
            {
                theta_y = wrap_angle(theta_y + dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.E))
            {
                theta_z = wrap_angle(theta_z + dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.A))
            {
                theta_x = wrap_angle(theta_x - dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.S))
            {
                theta_y = wrap_angle(theta_y - dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.D))
            {
                theta_z = wrap_angle(theta_z - dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.R))
            {
                offset_z += 2.0f * dt;
            }
            if (keyboard.GetKeyPressed(Keys.F))
            {
                offset_z -= 2.0f * dt;
            }
            drawWireFrame = keyboard.GetKeyPressed(Keys.ControlKey);
        }

        public override void Draw(PixelGraphics gfx)
        {
            IndexedLineList lines = cube.GetLines().DeepCopy();
            IndexedTriangleList<TexVertex> triangles = cube.GetTrianglesTex().DeepCopy();

            Mat3<float> rot =
                Mat3<float>.RotationX(theta_x) *
                Mat3<float>.RotationY(theta_y) *
                Mat3<float>.RotationZ(theta_z);

            for (int i = 0; i < triangles.vertices.Count; i++)
            {
                triangles.vertices[i].pos *= rot;
                triangles.vertices[i].pos += new Vec3<float>(0.0f, 0.0f, offset_z);

                lines.vertices[i] *= rot;
                lines.vertices[i] += new Vec3<float>(0.0f, 0.0f, offset_z);
            }

            for (int i = 0, end = triangles.indices.Count / 3; i < end; i++)
            {
                Vec3<float> v0 = triangles.vertices[triangles.indices[i * 3]].pos;
                Vec3<float> v1 = triangles.vertices[triangles.indices[i * 3 + 1]].pos;
                Vec3<float> v2 = triangles.vertices[triangles.indices[i * 3 + 2]].pos;
                triangles.cullFlags[i] = (v1 - v0) % (v2 - v0) * v0 >= 0.0f;
            }

            for (int i = 0; i < triangles.vertices.Count; i++)
            {
                pst.Transform(triangles.vertices[i].pos);
                pst.Transform(lines.vertices[i]);
            }

            for (int i = 0, end = triangles.indices.Count / 3; i < end; i++)
            {
                if (!triangles.cullFlags[i])
                {
                    gfx.DrawTriangleTex(
                        triangles.vertices[triangles.indices[i * 3]],
                        triangles.vertices[triangles.indices[i * 3 + 1]],
                        triangles.vertices[triangles.indices[i * 3 + 2]],
                        texture);
                }
            }

            if (drawWireFrame)
            {
                for (int i = 0; i != lines.indices.Count; i += 2)
                {
                    gfx.DrawLine(lines.vertices[lines.indices[i]], lines.vertices[lines.indices[i + 1]], Color.White);
                }
            }
        }

        PubeScreenTransformer pst = new PubeScreenTransformer();
        CubeSkinned cube = new CubeSkinned(1.0f);
        float dTheta = (float)Math.PI;
        float offset_z = 2.0f;
        float theta_x = 0.0f;
        float theta_y = 0.0f;
        float theta_z = 0.0f;
        bool drawWireFrame = false;
        DirectBitmap texture;
    }
}

