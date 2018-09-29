using System;
using System.Collections.Generic;
using System.Linq;
/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;*/
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GDITest
{
    
    /// <summary>
    /// specifies a particular tile for use in a Tilemap
    /// </summary>
    public class Tile//was Tile
    {
        /// <summary>
        /// returns information about the tile
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Size:{0}x{1} Wash:{2} offsets L:{3} R:{4} Rect:{5}~",
                            portion.Width,
                            portion.Height,
                            wash.ToString(),
                            LeftOffset,
                            RightOffset,
                            portion.ToString());
            return sb.ToString();
        }
        /// <summary>
        /// holds slope offsets for height map of tile, defaults to 2 elements to be used for sloping tiles
        /// but can be changed to any number of elements if required
        /// </summary>
        public int[] offsets = new int[2];
        /// <summary>
        /// quick reference to the left hand slope offset of a tile
        /// </summary>
        public int LeftOffset {  get { return offsets[0]; } set { offsets[0] = value; } }
        /// <summary>
        /// quick reference to the right hand slope offsets of a tile
        /// </summary>
        public int RightOffset { get { return offsets[1]; } set { offsets[1] = value; } }
        /// <summary>
        /// works out the slope offset given a position along a tile
        /// </summary>
        /// <param name="distanceFromLeft"></param>
        /// <returns>the effective slope offset at this position</returns>
        public float LerpHeight(float distanceFromLeft)
        {
            if (distanceFromLeft < 0) distanceFromLeft = -distanceFromLeft;
            return MathHelper.Lerp(offsets[0], offsets[1], distanceFromLeft / portion.Width);
        }
        /// <summary>
        /// linear interpolates the offset along a tile returning the actual tile y offset from the top of the tile
        /// </summary>
        /// <param name="xpos">the x position to check inside the tile</param>
        /// <param name="tileX">left hand position of the tile</param>
        /// <returns>The actual offset from the top of tile based on the left and right depth positions of the tile</returns>
        public float LerpHeight(float xpos, float tileX)
        {
            float off = (xpos - tileX) / portion.Width;
            if (off < 0) off = -off;
            return MathHelper.Lerp(LeftOffset, RightOffset, off);
        }
        /// <summary>
        /// Calculates the physical y top position of the slope at the xpos offset along the tile given
        /// </summary>
        /// <param name="xpos">position to check for offset of</param>
        /// <param name="tilepos">current tile topleft TilePosition() will give you this</param>
        /// <returns>y value for top of slope at the xpos given</returns>
        public float LerpTopPosition(float xpos, Vector2 tilepos)
        {
            return LerpHeight(xpos, tilepos.X) + tilepos.Y;
        }
        /// <summary>
        /// determines if a slope is facing towards the players direction of movement and therefore can be climbed or not
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>true if we can move up slope, false if we can't</returns>
        public bool PassableSlope(int direction)
        {
            if (direction == TileMap.LEFT)
                return (offsets[0] - offsets[1]) < 0;
            else
                return (offsets[1] - offsets[0]) < 0;
        }
        /// <summary>
        /// returns true if the tile represents a slope
        /// </summary>
        public bool IsSlope { get { return offsets[0] != offsets[1]; } }
        /// <summary>
        /// returns true if the tile represents a standard flat tile object (not sloped)
        /// </summary>
        public bool IsFlat { get { return offsets[0] == offsets[1]; } }
        /// <summary>
        /// holds the rectangle portion from the given texture
        /// </summary>
        public Rectangle portion;
        /// <summary>
        /// texture this tile refers to
        /// </summary>
        public Image /*Texture2D*/ texture;
        /// <summary>
        /// holds a wash to apply to the tile when rendered (defaults to white)
        /// </summary>
        public Color wash = Color.White;
        /// <summary>
        /// Creates a new Tile with the given parameters
        /// </summary>
        /// <param name="texture">texture containing graphic</param>
        /// <param name="rectangle">portion of texture with image</param>
        public Tile(Image /*Texture2D*/ texture, Rectangle rectangle)
        {
            this.texture = texture;
            this.portion = rectangle;
        }
        /// <summary>
        /// Creates a new Tile using the entire texture given with height map values
        /// </summary>
        /// <param name="texture">texture containing graphic</param>
        /// <param name="left">the height offset at the left hand edge of the tile</param>
        /// <param name="right">the height offset at the right hand edge of the tile</param>
        public Tile(Image /*Texture2D*/ texture, int left, int right)
            : this(texture, texture.GetBounds())
        {
            rec
            offsets[0] = left;
            offsets[1] = right;
        }
        /// <summary>
        /// Creates a new Tile with using the texture portion and with height map values
        /// </summary>
        /// <param name="texture">texture containing graphic</param>
        /// <param name="rectangle">portion of texture with image</param>
        /// <param name="left">the height offset at the left hand edge of the tile</param>
        /// <param name="right">the height offset at the right hand edge of the tile</param>
        public Tile(Image /*Texture2D*/ texture, Rectangle rectangle, int left, int right)
            : this(texture, rectangle)
        {
            offsets[0] = left;
            offsets[1] = right;
        }
        /// <summary>
        /// Creates a new Tile with the given parameters including a wash colour and height map values
        /// </summary>
        /// <param name="texture">texture containing graphic</param>
        /// <param name="rectangle">portion of texture with image</param>
        /// <param name="wash">The tint to apply to the tile when rendered</param>
        /// <param name="left">the height offset at the left hand edge of the tile</param>
        /// <param name="right">the height offset at the right hand edge of the tile</param>
        public Tile(Image /*Texture2D*/ texture, Rectangle rectangle, Color wash, int left, int right)
            : this(texture, rectangle, wash)
        {
            offsets[0] = left;
            offsets[1] = right;
        }
        /// <summary>
        /// Creates a new Tile with the which uses the entire texture specified
        /// </summary>
        /// <param name="texture">texture containing graphic</param>
        public Tile(Image /*Texture2D*/ texture)
            :this(texture, texture.GetBounds())
        {}
        /// <summary>
        /// constructs an empty tile
        /// </summary>
        public Tile()
        {

        }
        /// <summary>
        /// Creates a new Tile with the given parameters including a wash colour
        /// </summary>
        /// <param name="texture">texture containing graphic</param>
        /// <param name="rectangle">portion of texture with image</param>
        /// <param name="wash">The tint to apply to the tile when rendered</param>
        public Tile(Image /*Texture2D*/ texture, Rectangle rectangle, Color wash)
           : this(texture, rectangle)
        {
            this.wash = wash;
        }
    }
}
