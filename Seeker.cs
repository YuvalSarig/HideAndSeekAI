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
    class Seeker : MovingObj
    {
        public Seeker(BaseKeys keys, Texture2D texture, Vector2 position,
          Rectangle? sourceRectangle, Color color,
          float rotation, Vector2 origin, Vector2 scale,
          SpriteEffects effects, float layerDepth) :
           base(keys, texture, position,
           sourceRectangle, color,
           rotation, origin, scale,
           effects, layerDepth)
        {

            Game1.Event_Update += update;

        }
        public override void update()
        {
            if ((DateTime.Now - Game1.StartTime).TotalSeconds > 5)
            {
                base.update();
            }
        }

        public override void draw()
        {
            if ((DateTime.Now - Game1.StartTime).TotalSeconds > 5)
            {
                float angele = Rotation + (float)Math.PI / 6;
                Matrix mat = Matrix.CreateRotationZ(angele);
                Vector2 step = Vector2.Transform(Vector2.UnitY, mat);
                Vector2 temp = Position;
                while (G.map[temp].ToString() != "Obstacle" && (Position - temp).Length() <= 300)
                {
                    temp += step * 2;
                }
                G.drawVec(Rotation + (float)Math.PI + (float)Math.PI / 6, Position, Color.Blue, (Position - temp).Length(), 3);

                angele = Rotation - (float)Math.PI / 6;
                mat = Matrix.CreateRotationZ(angele);
                step = Vector2.Transform(Vector2.UnitY, mat);
                temp = Position;
                while (G.map[temp].ToString() != "Obstacle" && (Position - temp).Length() <= 300)
                {
                    temp += step * 2;
                }
                G.drawVec(Rotation + (float)Math.PI - (float)Math.PI / 6, Position, Color.Blue, (Position - temp).Length(), 3);
            }
            base.draw();
        }
    }
}
