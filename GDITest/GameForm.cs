using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDITest
{
    public partial class GameForm : Form
    {

        public GameForm()
        {
            InitializeComponent();

            //set the drawing surface size of the form
            ClientSize = new Size(720, 576);

            //initialse game engine and setup scenee
            Engine.GDI.Start(this);
            
            CreateScene();
            timer1.Start();
        }

        private void CreateScene()
        {
            //pass reference to game form so we can add our click event
            Engine.GDI.AddBackground(new SpaceBack(this));

            //specific text sprites
            Engine.GDI.AddSprite(new HelloWorld(200, 100, -50, -20));
            Engine.GDI.AddSprite(new HelloWorld(200, 200, -100, 20));
            Engine.GDI.AddSprite(new HelloWorld(200, 300, -150, 0));

            //image sprites
            Engine.GDI.AddSprite(new StickManSprite(100,100, -100,100));
            Engine.GDI.AddSprite(new StickManSprite(200,100, 50, -50));

            //line driven sprites
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                Engine.GDI.AddSprite(new MyShape(100 + r.Next(200), 100 + r.Next(200), r.Next(-150,150), r.Next(-150,150)));
            }
        }

        /// <summary>
        /// control gameloop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_loop(object sender, EventArgs e)
        {
            Engine.GDI.Ticker(timer1.Interval * 0.001f);
        }

    }
}
