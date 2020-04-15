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
    public class Hider : Characters
    {
        List<Vector2> cord;
        List<Vector2> cordPos;
        Vector2 LastPos;
        public Hider(BaseKeys keys, Texture2D texture, Vector2 position,
          Rectangle? sourceRectangle, Color color,
          float rotation, Vector2 origin, Vector2 scale,
          SpriteEffects effects, float layerDepth) :
           base(keys, texture, position,
           sourceRectangle, color,
           rotation, origin, scale,
           effects, layerDepth)
        {
            CreateList();
            MainGame.UpdateEvent += update;
        }

        private void CreateList()
        {
            int[] x = { 193, 13, 37, 101, 173 };
            int[] y = { 228, 227, 57, 7, 63 };
            cord = new List<Vector2>();
            cordPos = new List<Vector2>();
            for (int i = 0; i < x.Length; i++)
            {
                cord.Add(new Vector2(x[i], y[i]));
                cordPos.Add(Vector2.Zero);
            }
        }

        private void fillCordPos()
        {
            for (int i = 0; i < cordPos.Count; i++)
            {
                cordPos[i] = Position + StaticClass.rotate_vector(cord[i] - Position, Rotation, scale.X);
            }
        }

        public override void update()
        {
            //fillCordPos();
            StaticClass.maph.SetHiderOnMap((int)Position.X, (int)Position.Y, LastPos);
            LastPos = Position;
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
                if (StaticClass.map[Position].ToString() == "Obstacle" || Position.X < 0 || Position.X > StaticClass.WIDTH || Position.Y < 0 || Position.Y > StaticClass.HEIGHT)
                {
                    Position -= step * 5;
                }
            }
            base.update();
        }

    }
}
