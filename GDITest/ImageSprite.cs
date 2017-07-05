using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDITest
{
    /// <summary>
    /// implements a sprite with an image file
    /// </summary>
    class ImageSprite :MovingSprite
    {
        protected Image frame;
        protected Vector2 scale = new Vector2(1, 1);
        public ImageSprite(string imagefile)
        {
            frame = Image.FromFile(imagefile);
            scale = new Vector2(1, 1);
        }

        public override void Draw()
        {
            base.Draw();
            Engine.GDI.spriteBuffer.Graphics.DrawImage(frame, position.X, position.Y, scale.X * frame.Width, scale.Y * frame.Width);
        }
        public override void Cleanup()
        {
            base.Cleanup();
            frame.Dispose();
        }
    }
}
