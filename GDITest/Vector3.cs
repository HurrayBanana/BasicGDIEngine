using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDITest
{
    public class Vector3

    {
        public float x;
        public float y;
        public float z;

        public Vector3(float v1, float v2, float v3)
        {
            this.x = v1;
            this.y = v2;
            this.z = v3;
        }

        public static Vector3 Zero { get { return new Vector3(0, 0, 0); } }
    }
}
