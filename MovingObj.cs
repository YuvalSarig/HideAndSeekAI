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
    class MovingObj : Drawer
    {
        #region data
        protected BaseKeys keys;
        View v;
        #endregion
        #region ctor
        public MovingObj(BaseKeys keys, Texture2D texture, Vector2 position,
          Rectangle? sourceRectangle, Color color,
          float rotation, Vector2 origin, Vector2 scale,
          SpriteEffects effects, float layerDepth) :
           base(texture, position,
           sourceRectangle, color,
           rotation, origin, scale,
           effects, layerDepth)
        {
            v = new View();
            Game1.Event_Update += update;
            this.keys = keys;
            keys.WhoAmI(this);
        }
        #endregion

        private void update()
        {
            Matrix mat;
            if (keys.Left())
            {
                Rotation -= 0.1f;
            }
            if (keys.Right())
            {
                Rotation += 0.1f;
            }
            if (keys.Up())
            {
                mat = Matrix.CreateRotationZ(Rotation);
                Vector2 step = Vector2.Transform(Vector2.UnitY, mat);
                Position += step * 5;
                if (G.map[Position].ToString() == "Obstacle" || Position.X < 0 || Position.X > 1280 * 2 || Position.Y < 0 || Position.Y > 720 * 2)
                {
                    Position -= step * 5;
                }
            }

        }
    }
}
