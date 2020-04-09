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
    class View 
    {
        public View()
        {
            
        }

        Vector2 GetVector(Vector2 SeekerPos, Vector2 HiderPos)
        {
            return SeekerPos - HiderPos;
        }

        public bool IsView(Vector2 SeekerPos, Vector2 HiderPos)
        {
            Vector2 temp = new Vector2();
            Vector2 v = GetVector(SeekerPos, HiderPos);
            int k = (int)v.Length();
            v /= v.Length();
            temp = HiderPos;
            int i = 1;
            while (i < k)
            {
                temp += v;
                if (StaticClass.map[temp].ToString() == "Obstacle")
                    return false;
                i++;
            }
            return true;
        }

        public bool FindHider(Vector2 SeekerPos, float rot)
        {
            if ((SeekerPos - MainGame.hider.Position).Length() < 500)
            {
                float angele = rot;
                Matrix mat = Matrix.CreateRotationZ(angele);
                Vector2 step = Vector2.Transform(Vector2.UnitY, mat);
                Vector2 temp = SeekerPos;

                int see = 60;
                for (int i = 0; i < see; i++)
                {
                    while (StaticClass.map[temp].ToString() != "Obstacle" && (SeekerPos - temp).Length() <= 300)
                    {
                        temp += step * 10;


                        if (StaticClass.maph.IsHiderFound(temp))
                        {
                            return true;
                        }
                    }
                    angele += (float)Math.PI / 180;
                    mat = Matrix.CreateRotationZ(angele);
                    step = Vector2.Transform(Vector2.UnitY, mat);
                    temp = SeekerPos;
                }
            }
            return false;
        }
    }
}
