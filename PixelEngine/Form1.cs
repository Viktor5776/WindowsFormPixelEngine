using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

        float dt = 0.0f;

        public Form1()
        {
            InitializeComponent();
            //Path for project folder
            string filePath = @"C:\Users\vikto\OneDrive\Skrivbord\WindowsFromPixelEngine\";
            scenes.Add(new CubeSkinScene(gfx, filePath + @"PixelEngine\Images\office_skin.jpg"));


            Timer tmr = new Timer
            {
                Interval = 2   // milliseconds
            };
            tmr.Tick += Frame;  // set handler
            tmr.Start();
        }

        private void Frame(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            gfx.ResetScreen();
            UpdateModel();
            ComposeFrame();
            gfx.Draw(this.CreateGraphics());

            sw.Stop();

            dt = sw.ElapsedMilliseconds / 1000.0f;
        }

        private void UpdateModel()
        {
            
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
            scenes[curScene].Draw();
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
