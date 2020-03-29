﻿using System;
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
        IFocous[] focus;
        Viewport vp;
        Vector2 pos;
        float zoom;

        public Camera(IFocous[] focus, Viewport vp, Vector2 pos, float zoom = 1)
        {
            this.focus = focus;
            this.vp = vp;
            this.pos = pos;
            this.zoom = zoom;
            Game1.Event_Update += update;
        }
        void update()
        {
            Vector2 sum = new Vector2();
            float max = 0;
            for (int i = 0; i < focus.Length; i++)
            {
                sum += focus[i].Position;
            }
            sum = sum / focus.Length;
            for (int i = 0; i < focus.Length; i++)
            {
                if (Vector2.Distance(sum, focus[i].Position) > max)
                {
                    max = Vector2.Distance(sum, focus[i].Position);
                }
            }
            pos = sum;

            if (max > (vp.Height / 2) - 100f && zoom > 0.5f)
            {
                zoom = ((vp.Height / 2) - 100f) / max;
            }
            else if (max < vp.Height - 100f && zoom < 0.5f)
            {
                zoom = ((vp.Height / 2) - 100f) / max;
            }

            Mat = Matrix.CreateTranslation(-pos.X, -pos.Y, 0) *
                Matrix.CreateScale(zoom) *
                Matrix.CreateTranslation(vp.Width / 2, vp.Height / 2, 0);

        }
    }
}