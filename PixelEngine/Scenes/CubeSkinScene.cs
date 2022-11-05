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
using static PixelEngine.Pipeline;

namespace PixelEngine.Scenes
{
    internal class CubeSkinScene : Scene
    {
        public CubeSkinScene(PixelGraphics gfx, string filename)
        {
            itList = Cube.GetSkinned().DeepCopy();
            pipeline = new Pipeline(gfx);
            pipeline.BindTexture(filename);
        }

        public override void Update(Keyboard keyboard, float dt)
        {
            if (keyboard.GetKeyPressed(Keys.Q))
            {
                theta_x = Maths<float>.wrap_angle(theta_x + dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.W))
            {
                theta_y = Maths<float>.wrap_angle(theta_y + dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.E))
            {
                theta_z = Maths<float>.wrap_angle(theta_z + dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.A))
            {
                theta_x = Maths<float>.wrap_angle(theta_x - dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.S))
            {
                theta_y = Maths<float>.wrap_angle(theta_y - dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.D))
            {
                theta_z = Maths<float>.wrap_angle(theta_z - dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.R))
            {
                offset_z += 2.0f * dt;
            }
            if (keyboard.GetKeyPressed(Keys.F))
            {
                offset_z -= 2.0f * dt;
            }
        }

        public override void Draw()
        {
            Mat3<float> rot =
                Mat3<float>.RotationX(theta_x) *
                Mat3<float>.RotationY(theta_y) *
                Mat3<float>.RotationZ(theta_z);

            Vec3<float> translation = new Vec3<float>(0.0f,0.0f,offset_z);

            pipeline.BindRotation(rot);
            pipeline.BindTranslation(translation);

            pipeline.Draw(itList.DeepCopy());
        }

        Pipeline pipeline;
        IndexedTriangleList<Vertex> itList;
        float dTheta = (float)Math.PI;
        float offset_z = 2.0f;
        float theta_x = 0.0f;
        float theta_y = 0.0f;
        float theta_z = 0.0f;
    }
}

