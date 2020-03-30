using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HNS
{
    static class G
    {
        public static Map map;
        public static MapHider maph;
        public static int mapScale;
        public const float STEERSPEED = 0.005f;
        public static Random rnd;
        public static SpriteBatch sb;
        public static KeyboardState ks;
        public static Texture2D pixel;
        public static View v;

        public static void init(GraphicsDevice gd, ContentManager Content)
        {
            map = new Map(Content.Load<Texture2D>("mask"));
            mapScale = 2;
            rnd = new Random(0);
            sb = new SpriteBatch(gd);
            pixel = new Texture2D(gd, 1, 1);
            pixel.SetData<Color>(new Color[1] { Color.White });
            v = new View();
            maph = new MapHider();
            Game1.Event_Update += update;
        }
        public static Vector2 rotate_vectorX(float rot)
        {
            return Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(rot));
        }
        public static Vector2 rotate_vectorY(float rot)
        {
            return Vector2.Transform(Vector2.UnitY, Matrix.CreateRotationZ(rot));
        }
        public static Vector2 rotate_vector(Vector2 vec, float rot, float scale)
        {
            return Vector2.Transform(vec, Matrix.CreateRotationZ(rot) * 
                Matrix.CreateScale(scale));
        }

        static void update()
        {
            ks = Keyboard.GetState();
        }
        public static void drawVec(float dir, Vector2 pos, Color c, float l, int thicc = 10)
        {
            sb.Draw(pixel, pos, null, c, dir, new Vector2(0.5f, 1f),
                new Vector2(thicc, l), 0, 0);
        }
        //Make Random Position Vectors while the position not on obstacle
        public static Vector2 GetRandPos()
        {
            Random rand = new Random();
            Vector2 v = new Vector2(rand.Next(Game1.W * mapScale), rand.Next(Game1.H * mapScale));
            while (true)
                if (G.map[v].ToString() == "Obstacle")
                {
                    v.X = rand.Next(Game1.W * mapScale);
                    v.Y = rand.Next(Game1.H * mapScale);
                }
                else
                    return v;
        }
    }
}
