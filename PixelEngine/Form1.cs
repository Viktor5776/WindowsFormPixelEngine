using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelEngine
{
    public partial class Form1 : Form
    {
        readonly PixelGraphics gfx = new PixelGraphics();
        readonly Keyboard keyboard = new Keyboard();

        PubeScreenTransformer pst = new PubeScreenTransformer();
        Pyramid cube = new Pyramid(1.0f);

        float dTheta = (float)Math.PI;
        float offset_z = 2.0f;
        float theta_x = 0.0f;
        float theta_y = 0.0f;
        float theta_z = 0.0f;

        public Form1()
        {
            InitializeComponent();

            Timer tmr = new Timer
            {
                Interval = 2   // milliseconds
            };
            tmr.Tick += Frame;  // set handler
            tmr.Start();
        }

        private void Frame(object sender, EventArgs e)
        {
            gfx.ResetScreen();
            UpdateModel();
            ComposeFrame();
            gfx.Draw(this.CreateGraphics());
        }

        private void UpdateModel()
        {
            float dt = 1.0f / 60.0f;
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

        }

        private void ComposeFrame()
        {
            IndexedLineList lines = cube.GetLines().DeepCopy();
            
            Mat3<float> rot =
                Mat3<float>.RotationX(theta_x) *
                Mat3<float>.RotationY(theta_y) *
                Mat3<float>.RotationZ(theta_z);
            
            for (int i = 0; i < lines.vertices.Count; i++ )
            {
                lines.vertices[i] *= rot;
                lines.vertices[i] += new Vec3<float>( 0.0f,0.0f,offset_z );
                pst.Transform(lines.vertices[i]);
            }
            
            for (int i = 0; i != lines.indices.Count; i += 2)
            {
                gfx.DrawLine(lines.vertices[lines.indices[i]], lines.vertices[lines.indices[i + 1]], Color.White);
            }
            
        }
            


        //Keyboard Update
        private void FormKeyDown(object sender, KeyEventArgs e)
        {
            keyboard.SetKey(e.KeyCode, true);
        }
        private void FormKeyUp(object sender, KeyEventArgs e)
        {
            keyboard.SetKey(e.KeyCode, false);
        }

        public float wrap_angle(float theta)
        {
            float modded = (float)(theta % (2.0f * Math.PI));
            return (modded > Math.PI) ?
                (modded - 2.0f * (float)Math.PI) :
                modded;
        }
    }
}
