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
    class MapHider
    {
        int[,] c;
        int HiderWidth, HiderHeight; 
        Vector2 v;
        public MapHider()
        {
            c = new int[StaticClass.WIDTH * StaticClass.mapScale, StaticClass.HEIGHT * StaticClass.mapScale];
            HiderWidth = (int)((StaticClass.HiderAnimation.First().Value.FrameWidth / 2) * StaticClass.CharacterScale);
            HiderHeight = (int)((StaticClass.HiderAnimation.First().Value.FrameHeight / 2) * StaticClass.CharacterScale);

        }
        /// <summary>
        /// In
        /// </summary>
        /// <param name="LastPos"></param>
        public void initMap(Vector2 LastPos)
        {

            int x = (int)LastPos.X, y = (int)LastPos.Y;
            for (int j = -HiderWidth; j < HiderWidth; j++)
            {
                for (int i = -HiderHeight; i < HiderHeight; i++)
                {
                    if (0 < i + y && StaticClass.HEIGHT * StaticClass.mapScale > i + y && 0 < j + x && StaticClass.WIDTH * StaticClass.mapScale > j + x)
                    {
                        c[(j + x), (i + y)] = 0;
                    }
                }
            }

        }
        public void draw()
        {

            StaticClass.drawVec(0, new Vector2(v.X, v.Y + HiderHeight), Color.Red, HiderHeight * 2,
                HiderWidth * 2);

        }

        public void SetHiderOnMap(int x, int y, Vector2 LastPos)
        {
            initMap(LastPos);

            v = LastPos;
            //MainGame.DrawEvent += draw;

            for (int j = -HiderWidth; j < HiderWidth; j++)
            {
                for (int i = -HiderHeight; i < HiderHeight; i++)
                {
                    if (0 < i + y && StaticClass.HEIGHT * StaticClass.mapScale > i + y && 0 < j + x && StaticClass.WIDTH * StaticClass.mapScale > j + x)
                    {
                        c[(j + x), (i + y)] = 1;
                    }
                }
            }
        }

        public bool IsHiderFound(Vector2 v)
        {
            if (c[MathHelper.Clamp(Math.Abs((int)v.X), 1, StaticClass.WIDTH - 1), MathHelper.Clamp(Math.Abs((int)v.Y), 1, StaticClass.HEIGHT - 1)] == 1)
                return true;
            else
                return false;
        }
    }
}
