using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDITest
{
    class HelloWorld : MovingSprite
    {
        private Font f = new Font("Arial", 16);
        private Brush b = Brushes.White;
        private string text;

        
        public HelloWorld(float xs, float ys, float xv, float yv)
        {
            text = "Hello World";
            SizeF dimensions = Engine.GDI.spriteBuffer.Graphics.MeasureString(text, f);
            size.X = dimensions.Width;
            size.Y = dimensions.Height;

            velocity = new Vector2(xv, yv);
            position.X = xs;
            position.Y = ys;
        }

        public override void Update()
        {
            base.Update();

            Wrap();


        }

        public override void Draw()
        {
            base.Draw();

            Engine.GDI.spriteBuffer.Graphics.DrawString(text, f, b, position.X, position.Y);
        }
    }
}
