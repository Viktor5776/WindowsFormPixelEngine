using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PixelEngine.Scenes
{
    internal class SolidCubeScene : Scene
    {
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
            IndexedTriangleList<Vec3<float>> triangles = cube.GetTriangles().DeepCopy();

            Mat3<float> rot =
                Mat3<float>.RotationX(theta_x) *
                Mat3<float>.RotationY(theta_y) *
                Mat3<float>.RotationZ(theta_z);

            for (int i = 0; i < triangles.vertices.Count; i++)
            {
                triangles.vertices[i] *= rot;
                triangles.vertices[i] += new Vec3<float>(0.0f, 0.0f, offset_z);

                lines.vertices[i] *= rot;
                lines.vertices[i] += new Vec3<float>(0.0f, 0.0f, offset_z);
            }

            for (int i = 0, end = triangles.indices.Count / 3; i < end; i++)
            {
                Vec3<float> v0 = triangles.vertices[triangles.indices[i * 3]];
                Vec3<float> v1 = triangles.vertices[triangles.indices[i * 3 + 1]];
                Vec3<float> v2 = triangles.vertices[triangles.indices[i * 3 + 2]];
                triangles.cullFlags[i] = (v1 - v0) % (v2 - v0) * v0 >= 0.0f;
            }

            for (int i = 0; i < triangles.vertices.Count; i++)
            {
                pst.Transform(triangles.vertices[i]);
                pst.Transform(lines.vertices[i]);
            }

            for (int i = 0, end = triangles.indices.Count / 3; i < end; i++)
            {
                if (!triangles.cullFlags[i])
                {
                    gfx.DrawTriangle(
                        triangles.vertices[triangles.indices[i * 3]],
                        triangles.vertices[triangles.indices[i * 3 + 1]],
                        triangles.vertices[triangles.indices[i * 3 + 2]],
                        colors[i]);
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
        Cube cube = new Cube(1.0f);
        Color[] colors = new Color[12]{
		    Color.White,
		    Color.White,
		    Color.LimeGreen,
		    Color.LimeGreen,
		    Color.Red,
		    Color.Red,
		    Color.Yellow,
		    Color.Yellow,
		    Color.Blue,
		    Color.Blue,
		    Color.Orange,
		    Color.Orange
        };
        float dTheta = (float)Math.PI;
        float offset_z = 2.0f;
        float theta_x = 0.0f;
        float theta_y = 0.0f;
        float theta_z = 0.0f;
        bool drawWireFrame = false;
    }
}
