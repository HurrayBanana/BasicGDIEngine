using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDITest
{
    /// <summary>
    /// a really basic engine for rendering graphics in GDI environment
    /// Implementated as a singleton for easy referencing and to take advantage of class instancing
    /// </summary>
    public class Engine
    {
        private static Engine instance;
        /// <summary>
        /// reference through Engine.GDI
        /// if first time used the private constructor is instantiated and a static reference created
        /// if already instantiated then the previous reference is used
        /// </summary>
        public static Engine GDI
        {
            get
            {
                if (instance == null)
                    instance = new Engine();
                return instance;
            }
        }
        /// <summary>
        /// list of active sprites to update and render
        /// </summary>
        private List<Sprite> spritelist;
        /// <summary>
        /// list of active backgrounds (if transparent can be overlaid0
        /// </summary>
        private List<Background> backgroundList;
        /// <summary>
        /// list of sprites that need removing after the next update cycle
        /// </summary>
        private List<Sprite> removedSprites;
        /// <summary>
        /// list of backgrounds that need removing after the next update cycle
        /// </summary>
        private List<Background> removedBackgrounds;

        /// <summary>
        /// construct lists
        /// </summary>
        private Engine()
        {
            spritelist = new List<Sprite>();
            removedSprites = new List<Sprite>();
            backgroundList = new List<Background>();
            removedBackgrounds = new List<Background>();
        }
        /// <summary>
        /// handle to the surface of the form
        /// </summary>
        private Graphics gameSurface;

        /// <summary>
        /// a writeable reference to the forms gamesurface buffer
        /// </summary>
        public  BufferedGraphics spriteBuffer;
        /// <summary>
        /// size of the screen area
        /// </summary>
        public RectangleF Area;
        /// <summary>
        /// fraction of time for game updates
        /// </summary>
        public float Delta;
        /// <summary>
        /// initialises buffers and game area for given form
        /// </summary>
        /// <param name="gameForm">form we are rendering on</param>
        public void Start(GameForm gameForm)
        {
            Area = gameForm.DisplayRectangle;

            //currentContext = BufferedGraphicsManager.Current;
            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;

            gameSurface = gameForm.CreateGraphics();

            spriteBuffer = currentContext.Allocate(gameSurface, gameForm.DisplayRectangle);
            spriteBuffer.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
        }
        /// <summary>
        /// performs engine loop each time interval
        /// </summary>
        /// <param name="delta">fraction of time for this update</param>
        public void Ticker(float delta)
        {
            Delta = delta;
            //updates
            GameLoop();
            //remove resources before attempting to render
            ManageRemovedBackgrounds();
            ManageRemovedSprites();
            //render
            GameRender();
        }
        /// <summary>
        /// remove backgrounds no longer wanted
        /// </summary>
        private void ManageRemovedBackgrounds()
        {
            int i = backgroundList.Count - 1;
            while (i >= 0)
            {
                if (removedBackgrounds.Contains(backgroundList[i]))
                {
                    backgroundList[i].Cleanup();
                    backgroundList.RemoveAt(i);
                }
                i--;
            }
            removedSprites.Clear();
        }
        /// <summary>
        /// remove sprites no longer wanted
        /// </summary>
        private void ManageRemovedSprites()
        {
            int i = spritelist.Count - 1;
            while (i >= 0)
            {
                if (removedSprites.Contains(spritelist[i]))
                {
                    spritelist[i].Cleanup();
                    spritelist.RemoveAt(i);
                }
                i--;
            }
            removedSprites.Clear();
        }
        /// <summary>
        /// perform game object updates
        /// </summary>
        private void GameLoop()
        {
            BackgroundUpdate();
            SpriteUpdate();
        }
        /// <summary>
        /// update backgrounds
        /// </summary>
        private void BackgroundUpdate()
        {
            foreach (Background b in backgroundList)
                b.Update();
        }

        /// <summary>
        /// update any sprites
        /// </summary>
        private void SpriteUpdate()
        {
            foreach (Sprite s in spritelist)
                s.Update();
        }
        /// <summary>
        /// render scene
        /// </summary>
        private void GameRender()
        {
            spriteBuffer.Graphics.Clear(Color.Black);
            BackGroundDraw();
            SpriteDraw();
            Show();
        }
        /// <summary>
        /// render all backgrounds
        /// </summary>
        private void BackGroundDraw()
        {
            foreach (Background b in backgroundList)
                b.Draw();
        }
        /// <summary>
        /// render all sprites
        /// </summary>
        private void SpriteDraw()
        {
            foreach (Sprite s in spritelist)
                s.Draw();
        }

        /// <summary>
        /// show image on game surface
        /// </summary>
        public void Show()
        {
            spriteBuffer.Render(gameSurface);
        }
        /// <summary>
        /// add a managed sprite
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Sprite AddSprite(Sprite s)
        {
            spritelist.Add(s);
            return s;
        }
        /// <summary>
        /// remove an unwanted sprites
        /// </summary>
        /// <param name="s"></param>
        public void RemoveSprite(Sprite s)
        {
            removedSprites.Add(s);
        }
        /// <summary>
        /// add managed background
        /// </summary>
        /// <param name="b"></param>
        internal void AddBackground(Background b)
        {
            backgroundList.Add(b);
        }
        /// <summary>
        /// remove a managed background
        /// </summary>
        /// <param name="b"></param>
        public void RemoveBackground(Background b)
        {
            removedBackgrounds.Add(b);
        }
    }
}
