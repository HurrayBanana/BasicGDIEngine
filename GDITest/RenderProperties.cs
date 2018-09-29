using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;


namespace GDITest
{
    /// <summary>
    /// holds rendering settings for sprite and text layers and tilesheets
    /// </summary>
    public class RenderProperties//was SpriteLayerProperties
    {
        //int myLayer = 0;
        /// <summary>
        /// determines whether we are rendering this layer
        /// </summary>
        public bool Active = true;
        ///// <summary>
        ///// if set specifies the complex effect renderer to use
        ///// </summary>
        //public PostProcessFramework PostProcessEffect = null;
        ///// <summary>
        ///// for back compatability gets or sets post process framework operation
        ///// currently only blur/glow availble
        ///// </summary>
        //private PostProcessFramework ActiveEffect{
        //    get{return PostProcessEffect;}
        //    set {PostProcessEffect=value;}
        //}
        //public PostProcessFramework ActivePostProcessEffect
        //{
        //    get { return PostProcessEffect; }
        //    set
        //    {
        //        //allowable types
        //        if (value == typeof(BlurNGlow))
        //        {

        //        }
        //    }
        //}
        ///// <summary>
        ///// sets a simple spritebatch available pass through effect (such as grayscale, sepia)
        ///// you can add your own to this
        ///// </summary>
        //public Effect SimpleEffect = null;
        ///// <summary>
        ///// if true tells renderer that it needs to draw a rendertarget to backbuffer
        ///// </summary>
        //internal bool renderedWithEffects = false;
        ///// <summary>
        ///// the rendering sample state the default is linear clamp (smooth blending)
        ///// </summary>
        //public SamplerState samplerState = SamplerState.LinearClamp;
        ///// <summary>
        ///// the blending state the default is AlphaBlend
        ///// </summary>
        //public BlendState blendState = BlendState.AlphaBlend;
        /// <summary>
        /// Creates the renderProperties for this layer or tilesheet with a default view association
        /// with the default PlayerView
        /// </summary>
        public RenderProperties(/*int layer*/)
        {
            //this.myLayer = layer;
            //ViewAssociation = new int[] { 0 };
            //associate with default fullscreen view
            viewAssociations = new List<int>();
            viewAssociations.Add(0);
        }

        /// <summary>
        /// gets or set the PlayerViews to render this layer with
        /// </summary>
        public List<int> ViewAssociation
        {
            get { return this.viewAssociations; }
            set { this.viewAssociations = value; }
        }
        //public int[] ViewAssociation
        //{
        //    get { return this.viewAssociations; }
        //    set { this.viewAssociations = value; }
        //}

        /// <summary>
        /// holds a list of the PlayerViews this should be rendered with
        /// </summary>
        //internal int[] viewAssociations;
        internal List<int> viewAssociations;
        /// <summary>
        /// holds number of items (sprites/tiles) rendered this loop for this layer
        /// </summary>
        public int itemsRendered = 0;//was sprites rendered
        //NOT USING THIS ANYMORE
        ///// <summary>
        ///// states whether layer is being used for drawing only used by engine manager and
        ///// its subsystems
        ///// </summary>
        //internal Boolean active = false;
        /// <summary>
        /// Specifies the scaling effect for the tilemap
        /// </summary>
        internal float _scaleFactor = 1.0f;
        //internal Boolean active = false;
        /// <summary>
        /// Specifies the scaling effect for the tilemap
        /// </summary>
        public float ScaleFactor
        {
            get { return _scaleFactor; }
            set { _scaleFactor = Math.Max(value, 0.01f); dirty = true; }
        }
        /// <summary>
        /// specifies the rotation angle for the tilemap
        /// </summary>
        private float rotationAngle = 0;
        /// <summary>
        /// gets or sets the rotation angle in degrees for the tilemap
        /// CreateMatrix must be set to true for this to have an effect
        /// </summary>
        public float RotationAngle
        {
            get { return MathHelper.ToDegrees(rotationAngle); }
            set { rotationAngle = MathHelper.ToRadians(value); }
        }
        /// <summary>
        /// Specifies the centre of rotation for the tilemap
        /// </summary>
        public Vector3 RotationCentre = Vector3.Zero;
        /// <summary>
        /// The displacement factor for the layer
        /// </summary>
        public Vector2 Displacement = Vector2.Zero;
        /// <summary>
        /// default matrix which does nothing
        /// </summary>
        public Matrix DrawMatrix = Matrix.Identity;
        /// <summary>
        /// if true creates a transformation matrix for the Sprite/text layer
        /// based on the scaleFactor, rotationAngle, rotationCentre values,
        /// otherwise the DrawMatrix is used (which defaults to Identity - which does nothing)
        /// </summary>
        public Boolean CreateMatrix = false;

        /// <summary>
        /// holds the scaled engine viewport for the sprite layer
        /// </summary>
        internal Rectangle scaledRenderArea = new Rectangle();//was ViewPort -- DO NOT WANT TO USE THIS FIND A BETTER WAY??
        /// <summary>
        /// set to true if any of settings have been changed
        /// </summary>
        public bool IsDirty { get { return dirty; } }
        /// <summary>
        /// if a settings has been changed set to true
        /// </summary>
        private bool dirty = false;

        internal virtual Matrix GetMatrix
        {
            get
            {
                //generate a matrix using RotationOrigin, ScaleFactor and RotationAngle 
                if (CreateMatrix)
                {
                    Vector3 offs = RotationCentre;
                    //offset origin based on offset amount
                    // offs += new Vector3(Displacement.X, Displacement.Y, 0);

                    return Matrix.CreateTranslation(-offs) *
                            Matrix.CreateScale(_scaleFactor) *
                            Matrix.CreateRotationZ(rotationAngle) *
                            Matrix.CreateTranslation(offs);
                }
                else
                    return DrawMatrix;
            }
        }

        //public override string ToString()
        //{
        //    StringBuilder sb.AppendFormat("Sprite Layer:{0} drawn:{1} history:{2} offset:{3} scale:{4}~",
        //                    GetLayerText(i),
        //                    renderCount[i], renderCountHistory[i],
        //                    Displacement.ToString(),
        //                    _scaleFactor);
        //}
        /// <summary>
        /// marks renderproperties as cleaned (dealt with any changes)
        /// </summary>
        internal void Cleaned()
        {
            dirty = false;
        }
    }
}
