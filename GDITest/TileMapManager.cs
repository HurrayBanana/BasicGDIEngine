using System;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GDITest
{
    // - incorporates TileSheetHelper methods
    /// <summary>
    /// Overseas the updating, adding and removal of layers
    /// and tiling of layers using static methods
    /// </summary>
    public class TileMapManager : IUpdatable
    {
        //needs to go back to internal
        internal List<TileMap> Active = new List<TileMap>();
        /// <summary>
        /// cap for tilemaps for batching purposes may re-write that part
        /// </summary>
        internal const int MaximumTileMaps = 20;
        EngineManager em;
        /// <summary>
        /// used for interating during update
        /// </summary>
        int sheet = 0;
        /// <summary>
        /// holds update period
        /// </summary>
        float period;
        /// <summary>
        /// constructs the tilemap manager
        /// </summary>
        /// <param name="game"></param>
        public TileMapManager(Game game)
            : base(game)
        {
        }
        /// <summary>
        /// initialises the tilemap manager
        /// </summary>
        public override void Initialize()
        {
            //pick up the engine manager
            em = (EngineManager)Game.Services.GetService(typeof(IEngineManager));
            base.Initialize();
        }
        /// <summary>
        /// updates all the tilemaps if the system is not paused
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(float gameTime)
        {
            //check to see if engine paused
            if (!em.Paused)
            {
                //get update period
                period = (float)gameTime.ElapsedGameTime.TotalSeconds;
                for (sheet = 0; sheet < Active.Count; sheet++)
                {
                    //check for render changes
                    Active[sheet].CheckDirty();
                    //only update those that are moving
                    if (Active[sheet]._scroll != Vector2.Zero)
                    {
                        //work out a per frame value for scroll distance
                        Active[sheet].MoveBy(ref period);
                    }
                }
            }
        }

        /// <summary>
        /// adds a tilemap to the active list
        /// </summary>
        /// <param name="tilemap">The tilemap to draw</param>
        public void Add(TileMap tilemap)
        {
#if DEBUG
            if (tilemap == null) throw new Exception("the tilemap you attempted to Add is null or you have not created it yet");
#endif
            if (Active.Count < MaximumTileMaps)
            {
                Active.Add(tilemap);
                //store ref to engine manager
                tilemap.engineManager = em;
                //perform auto tile operation
                if (tilemap.autotile)
                    Tile(tilemap, tilemap.autotileTexture);
            }
        }
        /// <summary>
        /// removes all active tilemaps
        /// </summary>
        public void Clear()
        {
            foreach (TileMap item in Active) item.CleanUp();
            Active.Clear();
        }
        /// <summary>
        /// removes the specified tilemap
        /// </summary>
        /// <param name="tilemap">The tilemap</param>
        public void Remove(TileMap tilemap)
        {
            if (tilemap != null)
            {
                for (int i = 0; i < Active.Count; i++)
                {
                    if (Active[i] == tilemap)
                    {
                        Active[i].CleanUp();
                        Active.RemoveAt(i);
                        break;
                    }
                }
            }
        }//end sub remove

        /// <summary>
        /// Builds a collision map from the tilemap and a list of mappings to convert normal tiles into collidable types
        /// You must have already provided the 
        /// </summary>
        /// <param name="collisionMappings">the list of collision tiles</param>
        /// <param name="tileMap">The tile map to create collision map for</param>
        public static void CreateCollisionMap(TileMap tileMap, int[] collisionMappings)
        {
            //create a map same size as tiles
            tileMap.cTiles = new int[tileMap.Rows, tileMap.Columns];
            //iterate through making assignments based on collisionMappings
            for (int row = 0; row < tileMap.Rows; row++)
            {
                for (int col = 0; col < tileMap.Columns; col++)
                {
                    //if we don't find a mapping from a tilemap index then place a zero at that location
                    try { tileMap.cTiles[row, col] = collisionMappings[tileMap.dTiles[row, col]]; }
                    catch { tileMap.cTiles[row, col] = 0; }
                }
            }
        }

        /// <summary>
        /// NOT IMPLEMENTED YET
        /// tiles a portion of a texture to a tilemap, use this if you have a background graphic that is on a spritesheet
        /// </summary>
        /// <param name="tileMap">The tilemap to tile the texture to</param>
        /// <param name="position">The start position for this tile, this must start on a tile boundary.
        /// If your tilewidth is 50, your x position must be a factor of 50 and similar for the y value</param>
        /// <param name="texture">The texture to tile</param>
        /// <param name="portion"></param>
        private void Tile(TileMap tileMap, Vector2 position, Texture2D texture, Rectangle portion)
        { }

        /// <summary>
        /// Fills a tilemap where each tile is at least 1 by 1 pixel. The data for this comes from a texture using it's alpha channel.
        /// Transparent areas generate a tile number of 0 and non transparent areas generate a tile number of 1
        /// These are not intended to be drawn (and do not need adding to the tilemap manager), but rather used for pixel level collision detection. As Such the tilemap will be disabled.
        /// You need to make sure you have set the size of tilemap using SetSamplingMap() first.
        /// </summary>
        /// <param name="tileMap">The tilemap to fill the map in for</param>
        /// <param name="texture">The texture whose alpha channel will be used</param>
        public void FillMapFromAlphaChannel(TileMap tileMap, Texture2D texture)
        {
            FillMapFromAlphaChannel(tileMap, Point.Zero, texture, texture.Bounds);
        }
        /// <summary>
        /// Fills a portion of a tilemap where each tile is at least 1 by 1 pixel. The data for this comes from a texture using it's alpha channel.
        /// Transparent areas generate a tile number of 0 and non transparent areas generate a tile number of 1
        /// These are not intended to be drawn (and do not need adding to the tilemap manager), but rather used for pixel level collision detection. As Such the tilemap will be disabled.
        /// You need to make sure you have set the size of tilemap using SetSamplingMap() first.
        /// </summary>
        /// <param name="tileMap">The tilemap to fill the map in for</param>
        /// <param name="cornerOffset">The start (pixel position) co-ordinates of the tilemap where this alpha data should be placed</param>
        /// <param name="texture">The texture whose alpha channel will be used</param>
        /// <param name="portion">The area of the texture to place in the tilemap</param>
        public void FillMapFromAlphaChannel(TileMap tileMap, Point cornerOffset,Texture2D texture, Rectangle portion)
        {
            //use tile size as map resolution
            int dx = tileMap.TileWidth;
            int dy = tileMap.TileHeight;


            //force tilemap invisible in case user added to manager
            tileMap.Visible = false;
            //offsets for alignment purposes
            int basex, basey;

            //basex = cornerOffset.X;
            //basey = cornerOffset.Y;

            //set base position in map but factor in tile size to reduce displacement and offset
            basex = cornerOffset.X / dx;
            basey = cornerOffset.Y / dy;

            //THIS IS THE FUNCTION TO ADD TO THE GAME ENGINE + SOME EXTRA TILEING ACTION
            //grab the alpha map for the entire texture
            byte[,] alphamap = TexAnalyser.GetRegionAlphaGrid(texture, portion);

            //map location access values
            int mapX = 0;
            int mapY = 0;

            //interate over all cells using tile size as offset
            // r and c refer to pixel sampling point
            for (int r = 0; r < portion.Height; r += dy)
            {
                mapX = 0;
                for (int c = 0; c < portion.Width; c += dx)
                {
                    //set map tile to 0 if alpha is zero at this pixel pos, otherwise set it to 1
                    tileMap.Map[basey + r, basex + c] = alphamap[r, c] == 0 ? 0 : 1;
                    mapX++;
                }
                mapY++;
            }
            ////interate over all cells
            //for (int r = 0; r < portion.Height; r++)
            //    for (int c = 0; c < portion.Width; c++)
            //        //set map tile to 0 if alpha is zero at this pixel pos, otherwise set it to 1
            //        tileMap.Map[basey + r, basex + c] = alphamap[r, c] == 0 ? 0 : 1;
        }

        /// <summary>
        /// Fills a portion of a tilemap where each tile is at least 1 by 1 pixel. The data for this comes from a texture using each pixels colour.
        /// For larger tile sizes (2x2) and (4x4) the sampling resolution is changed which gives 
        /// Each pixels colour is converted into a packed value (unsigned integer) which is then converted into a signed int to be stored in the tilemap
        /// These are not intended to be drawn (and do not need adding to the tilemap manager), but rather used for pixel level collision detection. As Such the tilemap will be disabled.
        /// You need to make sure you have set the size of tilemap using SetSamplingMap() first.
        /// You should use this with special graphic maps using colour regions for different collision purposes.
        /// </summary>
        /// <param name="tileMap">The tilemap to fill the map in for</param>
        /// <param name="texture">The texture whose pixels will be used</param>
        public void FillMapFromPixelData(TileMap tileMap, Texture2D texture)
        {
            FillMapFromPixelData(tileMap, Point.Zero, texture, texture.Bounds);
        }
        /// <summary>
        /// Fills a portion of a tilemap where each tile is at least 1 by 1 pixel. The data for this comes from a texture using each pixels colour.
        /// For larger tile sizes (2x2) and (4x4) the sampling resolution is changed which gives 
        /// Each pixels colour is converted into a packed value (unsigned integer) which is then converted into a signed int to be stored in the tilemap
        /// These are not intended to be drawn (and do not need adding to the tilemap manager), but rather used for pixel level collision detection. As Such the tilemap will be disabled.
        /// You need to make sure you have set the size of tilemap using SetSamplingMap() first.
        /// You should use this with special graphic maps using colour regions for different collision purposes.
        /// 
        /// These colour maps can be used for a large variety of tasks, collision detection, height maps for scaling etc...
        /// </summary>
        /// <param name="tileMap">The tilemap to fill the map in for</param>
        /// <param name="cornerOffset">The start (pixel position) co-ordinates of the tilemap where this alpha data should be placed</param>
        /// <param name="texture">The texture whose pixels will be used</param>
        /// <param name="portion">The area of the texture to place in the tilemap</param>
        public void FillMapFromPixelData(TileMap tileMap, Point cornerOffset, Texture2D texture, Rectangle portion)
        {
            //use tile size as map resolution
            int dx = tileMap.TileWidth;
            int dy = tileMap.TileHeight;

            //force tilemap invisible in case user added to manager
            tileMap.Visible = false;
            //offsets for alignment purposes
            int basex, basey;

            //set base position in map but factor in tile size to reduce displacement and offset
            basex = cornerOffset.X / dx;
            basey = cornerOffset.Y / dy;
            //THIS IS THE FUNCTION TO ADD TO THE GAME ENGINE + SOME EXTRA TILEING ACTION
            //grab the alpha map for the entire texture
            Color[,] pixels = TexAnalyser.GetRegionPixelGrid(texture, portion);
            
            //map location access values
            int mapX = 0;
            int mapY = 0;

            //int HeightCollapse

            //interate over all cells using tile size as offset
            // r and c refer to pixel sampling point
            for (int r = 0; r < portion.Height; r += dy)
            {
                mapX = 0;
                for (int c = 0; c < portion.Width; c += dx)
                {
                    //sample pixel at this point
                    tileMap.Map[basey + mapY, basex + mapX] = (int)pixels[r, c].PackedValue;
                    mapX++;
                }
                mapY++;
            }
            //for (int r = 0; r < portion.Height; r++)
            //    for (int c = 0; c < portion.Width; c++)
                    //set map tile to 0 if alpha is zero at this pixel pos, otherwise set it to 1
                    //tileMap.Map[basey + r, basex + c] = (int)pixels[r, c].PackedValue;
        }

        /// <summary>
        /// tiles an entire texture to a tilemap setting it to the top left corner
        /// </summary>
        /// <param name="tileMap">The tilemap to tile the texture to</param>
        /// <param name="texture">The texture to tile</param>
        public void Tile(TileMap tileMap, Texture2D texture)
        {
            Tile(tileMap, Vector2.Zero, texture);
        }

        /// <summary>
        /// tiles an entire texture to a tilemap
        /// </summary>
        /// <param name="tileMap">The tilemap to tile the texture to</param>
        /// <param name="position">The start position for this tile, this must start on a tile boundary.
        /// If your tilewidth is 50, your x position must be a factor of 50 and similar for the y value</param>
        /// <param name="texture">The texture to tile</param>
        public void Tile(TileMap tileMap, Vector2 position, Texture2D texture)
        {
            //create starting tile
            Point startTile = new Point((int)(position.X / tileMap.tileWidth), 
                                    (int)(position.Y / tileMap.tileHeight));
            Point activeTile = startTile;

            //used to pick the rectangle from the texture for a particular tile
            Rectangle rect = new Rectangle(0,0,tileMap.tileWidth, tileMap.tileHeight);

            try
            {
                for (int row = 0; row < texture.Height; row += tileMap.tileHeight)
                {
                    activeTile.X = startTile.X;
                    rect.Y = row;

                    for (int col = 0; col < texture.Width; col += tileMap.tileWidth)
                    {
                        rect.X = col;
                        //tileMap.myTileList[tileMap.tileCount] = new Tile(texture, rect);
                        tileMap.dTiles[activeTile.Y, activeTile.X] = tileMap.AddTile(texture, rect);//tileMap.tileCount;
                        //tileMap.tileCount++;
                        activeTile.X = activeTile.X + 1;
                    }
                    activeTile.Y = activeTile.Y + 1;

                }
            }
            catch (IndexOutOfRangeException)
            {
                
                //if (tileMap == null) 
                //    e.Data.Add("tileMap", "The given tileMap has not been created.\tYou need to have defined it previously with the new keyword");
                //if (texture == null) e.Data.Add("texture", "The given texture does not contain a graphic image");
                //if (texture.Width % tileMap.tileWidth != 0 || texture.Height % tileMap.tileHeight != 0)
                throw new IndexOutOfRangeException("texture dimension " + texture.Width + "," + texture.Height +
                                               " is not evenly divisible by tile dimensions " + tileMap.tileWidth + ", " + tileMap.tileHeight + "\nchange the dimensions of the tiles");
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("tileMap", "The given tileMap has not been created.\tYou need to have defined it previously with the new keyword");
            }
        }
        /// <summary>
        /// tiles port of a texture to a tilemap.
        /// You must ensure that the size of your tilemap has been defined properly to accomodate your portion
        /// </summary>
        /// <param name="tileMap">The tilemap to tile the texture to</param>
        /// <param name="portion">the rectangular area of the texture to tile</param>
        /// <param name="position">The start position for this tile, this must start on a tile boundary.
        /// If your tilewidth is 50, your x position must be a factor of 50 and similar for the y value</param>
        /// <param name="texture">The texture to tile</param>
        public void TilePortion(TileMap tileMap, Rectangle portion, Vector2 position, Texture2D texture)
        {
#if DEBUG
            if (tileMap == null) throw new ArgumentNullException("tileMap", "The given tileMap has not been created.\tYou need to have defined it previously with the new keyword");
#endif
#if DEBUG
            if (texture == null) throw new ArgumentNullException("texture", "The given texture does not contain a graphic image");
#endif

            //TileMap001 needs fixing
            //create starting tile
            Point startTile = new Point((int)(position.X / tileMap.tileWidth),
                                    (int)(position.Y / tileMap.tileHeight));
            Point activeTile = startTile;

            //used to pick the rectangle from the texture for a particular tile
            Rectangle rect = new Rectangle(0, 0, tileMap.tileWidth, tileMap.tileHeight);

            for (int row = 0; row < portion.Height; row += tileMap.tileHeight)
            {
                activeTile.X = startTile.X;
                rect.Y = row + portion.Y;

                for (int col = 0; col < portion.Width; col += tileMap.tileWidth)
                {
                    rect.X = col + portion.X;
                    //tileMap.myTileList[tileMap.tileCount] = new Tile(texture, rect);
                    tileMap.dTiles[activeTile.Y, activeTile.X] = tileMap.AddTile(texture, rect);//tileMap.tileCount;
                    //tileMap.tileCount++;
                    activeTile.X = activeTile.X + 1;
                }
                activeTile.Y = activeTile.Y + 1;

            }
        }
        /// <summary>
        /// Attempts to fill the extents of the tilemap with the given texture
        /// repeating it as many times as necessary, better to create a smaller tilemap
        /// the same size as your texture and set it to repeat the drawing to fill viewport
        /// </summary>
        /// <param name="tileMap">The tilemap to tile the texture to</param>
        /// <param name="texture">The texture to tile</param>
        /// <remarks>This will work best if your texture divides exactly into the size of your tilemap. tilemap 800 by 600
        /// with a texture 200,200 this would be tiled 8 times across the tilemap (4 across 2 down)</remarks>
        public void TileToExtents(TileMap tileMap, Texture2D texture)
        {
#if DEBUG
            if (tileMap == null) throw new ArgumentNullException("tileMap", "The given tileMap has not been created.\tYou need to have defined it previously with the new keyword");
#endif
#if DEBUG
            if (texture == null) throw new ArgumentNullException("texture", "The given texture does not contain a graphic image");
#endif
            //used to pick the rectangle from the texture for a particular tile
            Rectangle rect = new Rectangle(0, 0, tileMap.tileWidth, tileMap.tileHeight);

            for (int row = 0; row < tileMap.Rows; row ++)
            {
                //reset at start of each row
                rect.X = 0;
                for (int col = 0; col < tileMap.Columns; col ++)
                {
                    tileMap.dTiles[row, col] = tileMap.AddTile(texture, rect);
                    rect.X = rect.X + tileMap.tileWidth;
                    //wrap in x direction
                    if (rect.X >= texture.Width)
                        rect.X = 0;
                }
                rect.Y = rect.Y + tileMap.tileHeight;
                //wrap in y direction
                if (rect.Y >= texture.Height)
                    rect.Y = 0;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool BackTileMaps { get { return true; } }
    }//end class tilemap
}
