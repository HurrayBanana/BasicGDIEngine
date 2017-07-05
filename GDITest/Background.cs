using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDITest
{
    /// <summary>
    /// basic background class with provision for update and draw calls
    /// </summary>
    public class Background 
    {
        protected Image image;
        protected Vector2 position = new Vector2(0, 0);
        public Background(string backImage)
        {
            image = Image.FromFile(backImage);
        }
        public virtual void Update()
        {

        }
        public virtual void Draw()
        {
            Engine.GDI.spriteBuffer.Graphics.DrawImage(image, position.X, position.Y);
        }
        /// <summary>
        /// perform any resource deallocation when item removed
        /// </summary>
        public virtual void Cleanup()
        {
            image.Dispose();
        }
    }
}
