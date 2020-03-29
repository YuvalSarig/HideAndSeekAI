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
    class Hider : MovingObj
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
            Game1.Event_Update += update;
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
                cordPos[i] = Position + G.rotate_vector(cord[i] - Position, Rotation, scale.X);
            }
        }

        private void update()
        {
            //fillCordPos();
            G.maph.SetHiderOnMap((int)Position.X, (int)Position.Y, LastPos);
            LastPos = Position;
        }

    }
}
