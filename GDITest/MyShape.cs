using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDITest
{
    /// <summary>
    /// implements a line drawn sprite
    /// </summary>
    class MyShape : MovingSprite
    {
        /// <summary>
        /// definition of shape
        /// </summary>
        PointF[] vertices =
        {
            new PointF(0,-10),
            new PointF(10,10),
            new PointF(-10,10),
        };
        /// <summary>
        /// space to hold transformed coordinates
        /// </summary>
        PointF[] renderVerts = new PointF[3];
        /// <summary>
        /// drawing pen
        /// </summary>
        private Pen pen = new Pen(Color.Red, 4);

        public MyShape(float x, float y, float vx, float vy)
        {
            size.Y = 20;
            size.X = 20;
            position.X = x;
            position.Y = y;

            velocity.X = vx;
            velocity.Y = vy;
        }

        /// <summary>
        /// transform shape
        /// </summary>
        public override void Update()
        {
            //peform base velocity stuff first
            base.Update();

            for (int i = 0; i < vertices.Length; i++)
            {
                renderVerts[i].X = vertices[i].X + position.X;
                renderVerts[i].Y = vertices[i].Y + position.Y;
            }

            Wrap();
        }

        /// <summary>
        /// render as polygon
        /// </summary>
        public override void Draw()
        {
            base.Draw();
            Engine.GDI.spriteBuffer.Graphics.DrawPolygon(pen, renderVerts);
        }
    }
}
