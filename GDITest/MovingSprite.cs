using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDITest
{
    class MovingSprite : Sprite
    {
        public Vector2 velocity = new Vector2(0, 0);

        public override void Update()
        {
            base.Update();
            position += velocity * Engine.GDI.Delta;
        }

        public void Wrap()
        {
            if (position.X + size.X < Engine.GDI.Area.Left)
                position.X = Engine.GDI.Area.Right;
            else if (position.X > Engine.GDI.Area.Right)
                position.X = Engine.GDI.Area.Left - size.X;

            if (position.Y + size.Y < Engine.GDI.Area.Top)
                position.Y = Engine.GDI.Area.Bottom;
            else if (position.Y > Engine.GDI.Area.Bottom)
                position.Y = Engine.GDI.Area.Top - size.Y;
        }
    }
}
