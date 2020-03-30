using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HNS
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        Seeker seeker;
        Hider hider;
        Drawer map;
        public static event DlgDraw Event_Draw;
        public static event DlgUpdate Event_Update;
        Camera cam;
        public static int W = 1920, H = 1080;
        public static DateTime StartTime = DateTime.Now;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = H;
            graphics.PreferredBackBufferWidth = W;
            Window.AllowUserResizing = true;

            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {

            IsMouseVisible = true;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            G.init(GraphicsDevice, Content);
            map = new Drawer(Content.Load<Texture2D>("map2"),
                       new Vector2(0, 0),
                       null, Color.White, 0, new Vector2(0),
                       new Vector2(G.mapScale), SpriteEffects.None, 0);
            seeker = new Seeker(
                new UserKeys(Keys.A, Keys.D, Keys.W, Keys.S),
                Content.Load<Texture2D>("seeker"),
                       G.GetRandPos(),
                       null, Color.White, 0, new Vector2(96, 106),
                       new Vector2(0.3f), 0, 0);
            hider = new Hider(
                new UserKeys(Keys.Left, Keys.Right, Keys.Up, Keys.Down),
                Content.Load<Texture2D>("hider"),
                       G.GetRandPos(),
                       null, Color.White, 0, new Vector2(104, 124),
                       new Vector2(0.3f), 0, 0);
            while((seeker.Position - hider.Position).Length() < 400)
            {
                hider.Position = G.GetRandPos();
            }
            IFocous[] focous = new IFocous[2];
            focous[0] = seeker;
            focous[1] = hider;
            cam = new Camera(focous, new Viewport(0, 0, Game1.W, Game1.H), Vector2.Zero);
        }


        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            string s;         
            if (G.v.IsView(seeker.Position, hider.Position))
            {
                s = "Hider View The Seeker: True,";
            }
            else
            {
                s = "Hider View The Seeker: false,";
            }
            if (G.v.FindHider(seeker.Position, seeker.Rotation - (float)Math.PI / 6, hider.Position))
            {
                s += " Hider Found: true";
            }
            else
                s += " Hider Found: false";
            Window.Title = s;
            Event_Update?.Invoke();


            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            G.sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, cam.Mat);

            Event_Draw?.Invoke();
            G.sb.End();

            base.Draw(gameTime);
        }
    }
}
