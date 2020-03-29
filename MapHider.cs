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
        int HiderSize = 139;
        public MapHider()
        {
            c = new int[G.W * G.H * G.mapScale * G.mapScale];

        }

        public void initMap(List<int> LastPos)
        {
            if (LastPos != null)
                foreach (int item in LastPos)
                {
                    c[item] = 0;
                }


        }

        public List<int> SetHiderOnMap(int x, int y, List<int> LastPos)
        {
            List<int> a = new List<int>();
            Console.WriteLine(LastPos);
            Console.WriteLine((x) + " " + (y));
            initMap(LastPos);

            for (int i = 0; i < G.H * G.mapScale; i++)
            {
                for (int j = 0; j < G.W * G.mapScale; j++)
                {
                    if (i < y + 121 * 0.3 && i > y - 124 * 0.3 && j < x + 113 * 0.3 && j > x - 104 * 0.3)
                    {
                        c[i * G.W + j] = 1;
                        a.Add(i * G.W + j);
                    }
                }
            }
            return a;
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
