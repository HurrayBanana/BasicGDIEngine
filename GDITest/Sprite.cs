using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDITest
{

    /// <summary>
    /// defines the basic structure of a game sprite
    /// </summary>
    public abstract class Sprite
    {
        /// <summary>
        /// render position
        /// </summary>
        protected Vector2 position = new Vector2(0, 0);
        /// <summary>
        /// scaling
        /// </summary>
        protected Vector2 size = new Vector2(1, 1);
        public Sprite()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {
        }
        /// <summary>
        /// perform any resource deallocation when item removed
        /// </summary>
        public virtual void Cleanup()
        {

        }
    }
}
