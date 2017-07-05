using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDITest
{
    class StickManSprite : ImageSprite
    {

        public StickManSprite(float x, float y, float vx, float vy)
            :base(".\\Graphics\\stick.png")
        {
            position.X = x;
            position.Y = y;

            velocity.X = vx;
            velocity.Y = vy;
            size.X = frame.Width;
            size.Y = frame.Height;
        }
        /// <summary>
        /// basic movement with wrapping
        /// </summary>
        public override void Update()
        {

            base.Update();

            Wrap();
        }
    }
}
