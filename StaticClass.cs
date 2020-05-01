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
    static class StaticClass
    {
        public static GameTime gameTime;
        public static Map map;
        public static MapHider maph;
        public static int mapScale;
        public static float CharacterScale = 0.3f;
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
        public static double shakeRate = 0.2f;
        public static Seeker LiveTopSeeker;
        public static Seeker TopSeeker;
        public static Seeker TopSeeker2;
        public static List<List<int>> hiddenLayersConfig = new List<List<int>>
        {
            new List<int> { 5 }
        };

        // Dictionary
        public static Dictionary<string, float> Stats = new Dictionary<string, float>();
        public static Dictionary<string, Animation.Animation> SeekerAnimation = new Dictionary<string, Animation.Animation>();
        public static Dictionary<string, Animation.Animation> HiderAnimation;
        
        /// <summary>
        /// Initialize all the values of the variables
        /// </summary>
        /// <param name="gd">GraphicsDevice of game</param>
        /// <param name="Content">ContentManager of game</param>
        public static void init(GraphicsDevice gd, ContentManager Content)
        {
            SeekerAnimation["WalkDown"] = new Animation.Animation(Content.Load<Texture2D>("Seeker/WalkinDown"), 4);
            HiderAnimation = new Dictionary<string, Animation.Animation>()
        {
          { "WalkUp", new Animation.Animation(Content.Load<Texture2D>("Hider/WalkingUp"), 4) },
          { "WalkDown", new Animation.Animation(Content.Load<Texture2D>("Hider/WalkingDown"), 4) },
          { "WalkLeft", new Animation.Animation(Content.Load<Texture2D>("Hider/WalkingLeft"), 4) },
          { "WalkRight", new Animation.Animation(Content.Load<Texture2D>("Hider/WalkingRight"), 4) },
        };
            font = Content.Load<SpriteFont>("Arial");
            START_TIME = DateTime.Now;
            map = new Map(Content.Load<Texture2D>("mask1"));
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

        /// <summary>
        /// Rotate vector
        /// </summary>
        /// <param name="vec">vector of position</param>
        /// <param name="rot">rotate to object</param>
        /// <param name="scale">scale of vector</param>
        /// <returns>The rotated vector</returns>
        public static Vector2 rotate_vector(Vector2 vec, float rot, float scale)
        {
            return Vector2.Transform(vec, Matrix.CreateRotationZ(rot) * 
                Matrix.CreateScale(scale));
        }

        /// <summary>
        /// Update the keys depending on what is pressed on kseyboard
        /// </summary>
        static void update()
        {
            ks = Keyboard.GetState();
        }
        /// <summary>
        /// Draw a line on the screen
        /// </summary>
        /// <param name="dir">Direction of the vector</param>
        /// <param name="pos">Position of the vector</param>
        /// <param name="c">Color of the line</param>
        /// <param name="l">length of vector</param>
        /// <param name="thick">thickness of line</param> 
        public static void drawVec(float dir, Vector2 pos, Color c, float l, int thick = 10)
        {
            sb.Draw(pixel, pos, null, c, dir, new Vector2(0.5f, 1f),
                new Vector2(thick, l), 0, 0);
        }
 
        /// <summary>
        /// Make Random Position Vectors while the position not on obstacle
        /// </summary>
        /// <returns>Vector of position</returns>
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
