using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace GDITest
{
    /// <summary>
    /// a call back to handle briefly highlighting node neighbours
    /// </summary>
    /// <param name="node"></param>
    public delegate void BriefHighlightNode(NetworkNode node);

    /// <summary>
    /// tile call back which accepts a tile number and location then must return either a replacement tile or the same tile
    /// this happens before a tile is placed into the scene
    /// </summary>
    /// <param name="tilenumber">the tile number ready to be placed</param>
    /// <param name="location">the location the tile is to be placed</param>
    /// <returns>tile to set a given location</returns>
    public delegate int TileCallBack(int tilenumber, Point location);
    ///// <summary>
    ///// defines a method that will accept a message that on object has subscribed to
    ///// </summary>
    ///// <param name="message">the message information that was subscribed to</param>
    //public delegate void MessageHandler(Message message);
    /// <summary>
    /// a method that will cast the given object to its correct object type
    /// </summary>
    /// <param name="objectData">the data to cast</param>
    public delegate void XMLAutoReader(Object objectData);
    ///// <summary>
    ///// delegate definition for code to supervise the final act of loading an xml file, or dealing with broken non-existant file
    ///// </summary>
    ///// <param name="r">Contains properties and info about the file attempted to be loaded</param>
    //public delegate void ReadRoutine(ReadHelper r);
    /// <summary>
    /// delegate definition for code to respond to two sprites meeting, it is up to you how to handle the collision after detection
    /// </summary>
    /// <param name="hit">the sprite collided with</param>
    public delegate void CollisionCallBack(Sprite hit);
    /// <summary>
    /// delegate definition for code to respond to sprites meeting, where pixel collision detection is needed
    /// </summary>
    /// <param name="hit">The sprite hit</param>
    /// <param name="Overlap">The portion of render space where sprites overlapped</param>
    /// <param name="myOffset">overlap rectangles offset from top left corner of the main sprite rectangle</param>
    /// <param name="hitOffset">overlap rectangles offset from top left corner of sprite hit's rectangle</param>
    /// <return>True if you want to continue processing collisions for this sprite
    ///  (do this if you don't hit a sprite you want pixel collision detection for), return false if you processed pixel collisions</return>
    public delegate bool CollisionPixelCallBack(Sprite hit, Rectangle Overlap, Point myOffset, Point hitOffset);
    ///// <summary>
    ///// delegate definition for code to respond to sprites meeting, where pixel collision detection is needed
    ///// </summary>
    ///// <param name="hit">The sprite hit</param>
    ///// <param name="myOverlap">The portion of my texture rectangle for the current frame that </param>
    ///// <param name="hitOverlap">The rectangle overlap of the hit sprite</param>
    ///// <param name="offset"></param>
    //public delegate void CollisionPixelCallBack(Sprite hit, Rectangle myOverlap, Rectangle hitOverlap, Point offset);

    /// <summary>
    /// delegate definition for code to respond to two sprites meeting, this allows you to set things before collision response is conducted
    /// </summary>
    /// <param name="hit">the sprite collided with</param>
    public delegate void PrologueCallBack(Sprite hit);
    /// <summary>
    /// delegate function called after collision has been handeld and response calculated
    /// </summary>
    /// <param name="hit">the sprite collided with</param>
    public delegate void EpilogueCallBack(Sprite hit);
    /// <summary>
    /// a delegate for a custom collision response,  this may change if requests need this
    /// </summary>
    /// <param name="hit">the sprite collided with</param>
    /// <param name="collisionNormal">the normal of the collision</param>
    /// <param name="impactVelocity">the impact velocity at the time of collision</param>
    public delegate void ResponseCallBack(Sprite hit, Vector2 collisionNormal, float impactVelocity);
    /// <summary>
    /// delegate definition for code to respond to two sprites meeting, it is up to you how to handle the collision after detection
    /// </summary>
    /// <param name="collisionOwner">Sprite which attached this</param>
    /// <param name="collidedWith">the sprite collided with</param>
    public delegate void CollisionUserHandler(Sprite collisionOwner, Sprite collidedWith);
    /// <summary>
    /// delegate definition for code to respond to two sprites meeting, this allows you to set things before collision response is conducted
    /// </summary>
    /// <param name="collisionOwner">Sprite which attached this</param>
    /// <param name="collidedWith">the sprite collided with</param>
    public delegate void CollisionPrologue(Sprite collisionOwner, Sprite collidedWith);
    /// <summary>
    /// a delegate for a custom collision response,  this may change if requests need this
    /// </summary>
    /// <param name="collisionOwner">Sprite which attached this epilogue</param>
    /// <param name="collidedWith">the sprite collided with</param>
    /// <param name="collisionNormal">the normal of the collision</param>
    /// <param name="impactVelocity">the impact velocity at the time of collision</param>
    public delegate void CollisionResponse(Sprite collisionOwner, Sprite collidedWith, Vector2 collisionNormal, float impactVelocity);
    /// <summary>
    /// delegate function called after collision has been handeld and response calculated
    /// </summary>
    /// <param name="collisionOwner">Sprite which attached this epilogue</param>
    /// <param name="collidedWith">the sprite collided with</param>
    public delegate void CollisionEpilogue(Sprite collisionOwner, Sprite collidedWith);
    /// <summary>
    /// Delegate function to allow user controlled text effects, implemented but not activated yet
    /// </summary>
    /// <param name="ts">The text data for the text you are going to manipulate</param>
    /// <param name="dx">The x position of the next character will be placed</param>
    /// <param name="dy">The y position of the next character will be placed</param>
    /// <remarks>This has been deactivated at the moment there is no way to access it</remarks>
    public delegate void TextHandler(TextStore ts, ref float dx, ref float dy);
    /// <summary>
    /// delegate definition for text handlers
    /// </summary>
    /// <param name="ts"></param>
    public delegate void TextMenuHandler(TextStore ts);
    /// <summary>
    /// Delegate definition for an Event.
    /// </summary>
    public delegate void EventHandler(/*Event e*/);
    /// <summary>
    /// defines a delegate sig for audio effects
    /// </summary>
    public delegate void AudioCallBack();
    ///// <summary>
    ///// Defines a delegate sig for a standard sprite handler
    ///// </summary>
    ///// <param name="handleMe">This is the sprite that you have asked to manipulate</param>
    //public delegate void SpriteHandler(Sprite handleMe);
    ///// <summary>
    ///// Defines a delegate which will be called when the engine has been asked to locate
    ///// a sprite with this point within it
    ///// </summary>
    ///// <param name="foundMe">The sprite found in contact with the location</param>
    ///// <param name="location">The location checked</param>
    //public delegate void PointHandler(Sprite foundMe, Vector2 location);
    /// <summary>
    /// Defines a delegate which will be called when the engine has been asked to locate
    /// a sprite with this point within it
    /// </summary>
    /// <param name="location">The location checked</param>
    public delegate void PointCallBack(Vector2 location);
    /// <summary>
    /// defines a delegate which will be called at the end of the update routines 
    /// to enable cleaner state changes
    /// </summary>
    public delegate void StateChanger();
    /// <summary>
    /// call back for sprite functions used for inherited sprites that don't
    /// need to have the sprite reference passed to them
    /// </summary>
    public delegate void SpriteCallBack();
    /// <summary>
    /// call back for asynchronus path finding routines. setup your code to do something else while waiting for the call back
    /// </summary>
    /// <param name="route">the route found, if null then a route is not possible</param>
    public delegate void PathCallBack(List<Point> route);
}
