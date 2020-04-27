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
        public MapHider()
        {
            c = new int[StaticClass.WIDTH * StaticClass.mapScale, StaticClass.HEIGHT * StaticClass.mapScale];

        }

        public void initMap(Vector2 LastPos)
        {
            int x = (int)LastPos.X, y = (int)LastPos.Y;
            for (int i = (int)(-MainGame.hider.animations.First().Value.FrameHeight / 2 * StaticClass.CharacterScale); i < MainGame.hider.animations.First().Value.FrameHeight / 2 * StaticClass.CharacterScale; i++)
            {
                for (int j = (int)(-MainGame.hider.animations.First().Value.FrameWidth / 2 * StaticClass.CharacterScale); j < -MainGame.hider.animations.First().Value.FrameWidth / 2 * StaticClass.CharacterScale; j++)
                {
                    if (0 < i + y && StaticClass.HEIGHT * StaticClass.mapScale > i + y && 0 < j + x && StaticClass.WIDTH * StaticClass.mapScale > j + x)
                    {
                        c[(j + x), (i + y)] = 0;
                    }
                }
            }
        }

        public void SetHiderOnMap(int x, int y, Vector2 LastPos)
        {
            initMap(LastPos);
            for (int i = (int)(-MainGame.hider.animations.First().Value.FrameHeight / 2 * StaticClass.CharacterScale); i < MainGame.hider.animations.First().Value.FrameHeight / 2 * StaticClass.CharacterScale; i++)
            {
                for (int j = (int)(-MainGame.hider.animations.First().Value.FrameWidth / 2 * StaticClass.CharacterScale); j < MainGame.hider.animations.First().Value.FrameWidth / 2 * StaticClass.CharacterScale; j++)
                {
                    if ( 0 < i + y && StaticClass.HEIGHT * StaticClass.mapScale > i + y && 0 < j + x && StaticClass.WIDTH * StaticClass.mapScale > j + x)
                    {
                        c[(j + x), (i + y)] = 1;
                    }
                }
            }
        }

        public bool IsHiderFound(Vector2 v)
        {
            if (c[MathHelper.Clamp(Math.Abs((int)v.X ), 1, StaticClass.WIDTH - 1), MathHelper.Clamp(Math.Abs((int)v.Y), 1, StaticClass.HEIGHT - 1)] == 1)
                return true;
            else
                return false;
        }
    }
}
