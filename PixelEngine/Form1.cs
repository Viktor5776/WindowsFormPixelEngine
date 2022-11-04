using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelEngine.Scenes;

namespace PixelEngine
{
    public partial class Form1 : Form
    {
        readonly PixelGraphics gfx = new PixelGraphics();
        readonly Keyboard keyboard = new Keyboard();

        List<Scene> scenes = new List<Scene>();
        int curScene = 0;
        bool changeScene = false;

        public Form1()
        {
            InitializeComponent();

            // Add Path to PixelEngine Between C: and \PixelEngine
            scenes.Add(new SolidCubeScene());
            scenes.Add(new TexWrapScene(@"C:\PixelEngine\Images\sauron100x100.png",4.0f));
            scenes.Add(new CubeSkinnedScene(@"C:\PixelEngine\Images\dice_skin.png"));
            scenes.Add(new CubeSkinnedScene(@"C:\PixelEngine\Images\office_skin.jpg"));
            scenes.Add(new FoldedCubeScene(@"C:\PixelEngine\Images\wood.jpg"));


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
            
            if(keyboard.GetKeyPressed(Keys.Tab) && !changeScene)
            {
                if(keyboard.GetKeyPressed(Keys.ShiftKey))
                {
                    if (--curScene == -1)
                    {
                        curScene = scenes.Count - 1;
                    }
                    changeScene = true;
                }
                else
                {
                    if (++curScene == scenes.Count)
                    {
                        curScene = 0;
                    }
                    changeScene = true;
                }
            }
            else if(!keyboard.GetKeyPressed(Keys.Tab))
            {
                changeScene = false;
            }

            scenes[curScene].Update(keyboard, dt);
        }

        private void ComposeFrame()
        {
            scenes[curScene].Draw(gfx);
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
    }
}
