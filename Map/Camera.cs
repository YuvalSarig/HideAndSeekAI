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
    class Camera
    {
        public Matrix Mat { get; private set; }
        List<IFocus> focus;
        Viewport vp;
        Vector2 pos;
        Vector2 Lastpos;
        float zoom;

        public Camera(List<IFocus> focus, Viewport vp, Vector2 pos, float zoom = 1)
        {
            this.focus = focus;
            this.vp = vp;
            this.pos = pos;
            this.zoom = zoom;
            MainGame.UpdateEvent += update;
        }


        void update()
        {
            //    Vector2 v = Vector2.Lerp(Lastpos, focus[0].Position, 0.03f);
            //    if (StaticClass.LiveTopSeeker != null)
            //    {
            //        v = Vector2.Lerp(Lastpos ,StaticClass.LiveTopSeeker.Position, 0.03f);
            //    }

            //    Vector2 sum = new Vector2();
            //    float Dis = 0;

            //    sum = v + focus[0].Position;
            //    sum /= 2;

            //    Dis = Vector2.Distance(v, focus[0].Position);

            //    pos = Vector2.Lerp(pos, sum, 0.03f);

            //    zoom = MathHelper.Clamp((StaticClass.HEIGHT - 100f) / Dis, 0.7f, 1.2f);

            Mat = Matrix.CreateTranslation(-pos.X, -pos.Y, 0);
                //Matrix.CreateScale(zoom) *
                //Matrix.CreateTranslation(vp.Width / 2, vp.Height / 2, 0);
            //Lastpos = v;
        }
    }
}
