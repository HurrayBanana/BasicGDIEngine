using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDITest
{
    public class Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator *(Vector2 v1, float scaler)
        {
            return new Vector2(v1.X * scaler, v1.Y * scaler);
        }

        public static Vector2 Zero { get { return new Vector2(0, 0); } }
    }
}
