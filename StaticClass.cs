﻿using System;
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
    static class StaticClass
    {
        public static Map map;
        public static MapHider maph;
        public static int mapScale;
        public static float CharacterScale = 0.3f;
        public const float STEERSPEED = 0.005f;
        public static Random rnd;
        public static SpriteBatch sb;
        public static KeyboardState ks;
        public static Texture2D pixel;
        public static View v;
        public static int WIDTH = 1920, HEIGHT = 1080;
        public static DateTime START_TIME;
        public static SpriteFont font;
        public static Vector2 StartSeekerPos;
        public static float StartSeekerRot;
        public static int NumOfFounds = 0;
        public static int PopulationNumber = 1;
        public static int SeekerNum = 50;
        public static int SeekerInputs = 4;
        public static int SeekerOutputs = 2;
        public static double shakeRate = 0.4;
        public static Seeker topSeeker;
        public static Seeker topSeeker2;
        public static List<List<int>> hiddenLayersConfig = new List<List<int>>
        {
            new List<int> { 5 }
        };
        public static Dictionary<string, float> Stats = new Dictionary<string, float>();

        public static void init(GraphicsDevice gd, ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Arial");
            START_TIME = DateTime.Now;
            map = new Map(Content.Load<Texture2D>("mask"));
            mapScale = 1;
            rnd = new Random();
            sb = new SpriteBatch(gd);
            pixel = new Texture2D(gd, 1, 1);
            pixel.SetData<Color>(new Color[1] { Color.White });
            v = new View();
            maph = new MapHider();
            Stats["MaxFitness"] = 0;
            Stats["LastFitness"] = 0;
            Stats["NumOfFounds"] = 0;
            StartSeekerPos = StaticClass.GetRandPos();
            MainGame.UpdateEvent += update;
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
            Vector2 v = new Vector2(rnd.Next(WIDTH * mapScale), rnd.Next(HEIGHT * mapScale));
            while (true)
                if (map[v].ToString() == "Obstacle")
                {
                    v.X = rnd.Next(WIDTH * mapScale);
                    v.Y = rnd.Next(HEIGHT * mapScale);
                }
                else
                    return v;
        }
    }
}
