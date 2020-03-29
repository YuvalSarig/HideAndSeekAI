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
        int[] c;
        public MapHider()
        {
            c = new int[G.W * G.H * G.mapScale * G.mapScale];

        }

        public void initMap(Vector2 LastPos)
        {
            int x = (int)LastPos.X, y = (int)LastPos.Y;
            for (int i = (int)(-124 * 0.3); i < 121 * 0.3; i++)
            {
                for (int j = (int)(-104 * 0.3); j < 113 * 0.3; j++)
                {
                    if (0 < i + y && G.H * G.mapScale > i + y && 0 < j + x && G.W * G.mapScale > j + x)
                    {
                        c[(i + y) * G.W + (j + x)] = 0;
                    }
                }
            }
        }

        public void SetHiderOnMap(int x, int y, Vector2 LastPos)
        {
            initMap(LastPos);
            for (int i = (int)(-124 * 0.3); i < 121 * 0.3; i++)
            {
                for (int j = (int)(-104 * 0.3); j < 113 * 0.3; j++)
                {
                    if ( 0 < i + y && G.H * G.mapScale > i + y && 0 < j + x && G.W * G.mapScale > j + x)
                    {
                        c[(i + y)* G.W + (j + x)] = 1;
                    }
                }
            }
        }

        public bool IsHiderFound(Vector2 v)
        {
            if (c[Math.Abs((int)v.Y) * G.W + Math.Abs((int)v.X)] == 1)
                return true;
            else
                return false;
        }
    }
}
