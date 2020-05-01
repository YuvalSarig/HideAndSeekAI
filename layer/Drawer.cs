using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HNS
{
    interface IFocus
    {
        Vector2 Position { get; }
        float Rotation { get; }
    }

    public class Drawer : IFocus
    {
        #region data
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        Texture2D texture;
        Rectangle? sourceRectangle;
        protected Color color;
        protected Vector2 origin;
        protected Vector2 scale;
        protected SpriteEffects effects;
        protected float layerDepth;
        public string message;
        #endregion
        #region ctor
        public Drawer()
        {

        }
        public Drawer(Texture2D texture, Vector2 position,
           Rectangle? sourceRectangle, Color color,
           float rotation, Vector2 origin, Vector2 scale,
           SpriteEffects effects, float layerDepth)
        {
            this.texture = texture;
            this.Position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.Rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.effects = effects;
            this.layerDepth = layerDepth;
            MainGame.DrawEvent += draw;

        }
        public Drawer( Vector2 position,
           Rectangle? sourceRectangle, Color color,
           float rotation, Vector2 origin, Vector2 scale,
           SpriteEffects effects, float layerDepth)
        {
            this.Position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.Rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.effects = effects;
            this.layerDepth = layerDepth;
            MainGame.DrawEvent += draw;

        }

        #endregion
        #region funcs

        /// <summary>
        /// Update objects on screen
        /// </summary>
        public virtual void update()
        {
        }

        /// <summary>
        /// Draw objects on screen
        /// </summary>
        public virtual void draw()
        {
            StaticClass.sb.Draw(texture, Position, sourceRectangle,
                      color, Rotation, origin, scale,
                      effects, layerDepth);
        }
        #endregion
    }
}
