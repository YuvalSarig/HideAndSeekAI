using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HNS
{

    public class MainGame : Game
    {
        GraphicsDeviceManager GraphicsDeviceM;
        public static event DelgtDraw DrawEvent;
        public static event DelgtUpdate UpdateEvent;
        private List<Seeker> SeekerPop = new List<Seeker>();
        private List<Seeker> DeadSeekers = new List<Seeker>();
        public static Hider hider;
        Drawer map;
        Camera cam;

        public MainGame()
        {
            GraphicsDeviceM = new GraphicsDeviceManager(this);
            GraphicsDeviceM.PreferredBackBufferHeight = StaticClass.HEIGHT;
            GraphicsDeviceM.PreferredBackBufferWidth = StaticClass.WIDTH;
            Window.AllowUserResizing = true;
            GraphicsDeviceM.ApplyChanges();
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            StaticClass.init(base.GraphicsDevice, Content);
            map = new Drawer(Content.Load<Texture2D>("map1"),
                       new Vector2(0, 0),
                       null, Color.White, 0, new Vector2(0),
                       new Vector2(StaticClass.mapScale), SpriteEffects.None, 0);

            InitSeekers();
            var Dic = new Dictionary<string, Animation.Animation>()
        {
          { "WalkUp", new Animation.Animation(Content.Load<Texture2D>("Hider/WalkingUp"), 4) },
          { "WalkDown", new Animation.Animation(Content.Load<Texture2D>("Hider/WalkingDown"), 4) },
          { "WalkLeft", new Animation.Animation(Content.Load<Texture2D>("Hider/WalkingLeft"), 4) },
          { "WalkRight", new Animation.Animation(Content.Load<Texture2D>("Hider/WalkingRight"), 4) },
        };
            hider = new Hider(Dic, new UserKeys(Keys.Left, Keys.Right, Keys.Up, Keys.Down),
                       StaticClass.GetRandPos(),
                       null, Color.White, 0, new Vector2(Dic.First().Value.FrameWidth / 2, Dic.First().Value.FrameHeight / 2),
                       new Vector2(StaticClass.CharacterScale), 0, 0);
            while ((SeekerPop[0].Position - hider.Position).Length() < 400)
            {
                hider.Position = StaticClass.GetRandPos();
            }
            List<IFocus> focus = new List<IFocus>();
            focus.Add(hider);
            //focus.AddRange(SeekerPop);
            cam = new Camera(focus, new Viewport(0, 0, StaticClass.WIDTH, StaticClass.HEIGHT), Vector2.Zero);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            StaticClass.gameTime = gameTime;
            RemoveDeadSeekers();
            UpdateEvent?.Invoke();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.GraphicsDevice.Clear(Color.Black);
            StaticClass.sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, cam.Mat);

            DrawEvent?.Invoke();
            StaticClass.sb.DrawString(StaticClass.font, "Iteration number: " + StaticClass.PopulationNumber, new Vector2(0, 0), Color.White);
            StaticClass.sb.DrawString(StaticClass.font, "Last Fitness: " + StaticClass.Stats["LastFitness"], new Vector2(0, 20), Color.White);
            StaticClass.sb.DrawString(StaticClass.font, "Max Fitness: " + StaticClass.Stats["MaxFitness"], new Vector2(0, 40), Color.White);
            StaticClass.sb.DrawString(StaticClass.font, "Num of founds: " + StaticClass.Stats["NumOfFounds"], new Vector2(0, 60), Color.White);

            StaticClass.sb.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Initialize Seekers List
        /// </summary>
        private void InitSeekers()
        {
            StaticClass.StartSeekerRot = (float)StaticClass.rnd.NextDouble() * MathHelper.TwoPi;
            
            for (int i = 0; i < StaticClass.SeekerNum; i++)
            {
                NeuralNetwork.NeuralNetwork Neuralnetwork = new NeuralNetwork.NeuralNetwork(
                    StaticClass.SeekerInputs,
                    StaticClass.hiddenLayersConfig[StaticClass.rnd.Next(StaticClass.hiddenLayersConfig.Count)],
                    StaticClass.SeekerOutputs);
                SeekerPop.Add(new Seeker(StaticClass.SeekerAnimation, Neuralnetwork, new BotKeys(), StaticClass.StartSeekerPos,
                       null, Color.White, StaticClass.StartSeekerRot, new Vector2(StaticClass.SeekerAnimation.First().Value.FrameWidth / 2, StaticClass.SeekerAnimation.First().Value.FrameHeight / 2),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
        }

        /// <summary>
        /// Check if seeker was dead and then moving to the deead seekers list
        /// if all seekers is dead create population
        /// </summary>
        private void RemoveDeadSeekers()
        {
            for (int i = SeekerPop.Count - 1; i >= 0; i--)
            {
                if (SeekerPop[i].Dead)
                {
                    if (SeekerPop[i].Found)
                        StaticClass.NumOfFounds++;
                    DeadSeekers.Add(SeekerPop[i]);
                    DrawEvent -= SeekerPop[i].draw;
                    SeekerPop.RemoveAt(i);
                }

            }
            if (SeekerPop.Count == 0)
            {
                CreateSeekerPopulation();
            }
        }

        /// <summary>
        /// Create seeker population by the best seekers
        /// </summary>
        private void CreateSeekerPopulation()
        {
            // sort dead seekers by the best fitness they achive
            DeadSeekers.Sort(new SeekerCompare());
            DeadSeekers.Reverse();
            // set the best seeker by the most fitness points 
            if (StaticClass.TopSeeker == null || DeadSeekers[0].Fitness >
                StaticClass.TopSeeker.Fitness)
            {
                StaticClass.TopSeeker2 = StaticClass.TopSeeker;
                StaticClass.TopSeeker = DeadSeekers[0];
            }
            // Change the dictionary
            if (StaticClass.TopSeeker2 == null) StaticClass.TopSeeker2 = DeadSeekers[1];
            StaticClass.Stats["MaxFitness"] = Math.Max(StaticClass.Stats["MaxFitness"],
                DeadSeekers[0].Fitness);
            StaticClass.Stats["LastFitness"] = DeadSeekers[0].Fitness;
            if (DeadSeekers[0].Fitness == 0) throw new Exception();
            // create green seekers that Their Neural Network Similar the best seeker
            for (int i = 0; i < StaticClass.SeekerNum * 0.35; i++)
            {
                NeuralNetwork.NeuralNetwork Neuralnetwork =
                    StaticClass.TopSeeker.Neuralnetwork.Copy();
                Neuralnetwork.Cross(DeadSeekers[0].Neuralnetwork);
                Neuralnetwork.ChangeNeuronWeights(StaticClass.shakeRate);
                SeekerPop.Add(new Seeker(StaticClass.SeekerAnimation, Neuralnetwork, new BotKeys(), StaticClass.StartSeekerPos,
                       null, Color.Green, StaticClass.StartSeekerRot, new Vector2(StaticClass.SeekerAnimation.First().Value.FrameWidth / 2, StaticClass.SeekerAnimation.First().Value.FrameHeight / 2),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
            // create blue seekers that Their Neural Network With a little change of the best seeker
            for (int i = 0; i < StaticClass.SeekerNum * 0.35; i++)
            {
                NeuralNetwork.NeuralNetwork Neuralnetwork =
                    StaticClass.TopSeeker.Neuralnetwork.Copy();
                Neuralnetwork.ChangeNeuronWeights(StaticClass.shakeRate);
                SeekerPop.Add(new Seeker(StaticClass.SeekerAnimation, Neuralnetwork, new BotKeys(), StaticClass.StartSeekerPos,
                       null, Color.Blue, StaticClass.StartSeekerRot, 
                       new Vector2(StaticClass.SeekerAnimation.First().Value.FrameWidth / 2, StaticClass.SeekerAnimation.First().Value.FrameHeight / 2),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
            // create red seekers that Their Neural Network With a little change of the second best seeker
            for (int i = 0; i < StaticClass.SeekerNum * 0.2; i++)
            {
                NeuralNetwork.NeuralNetwork Neuralnetwork =
                    StaticClass.TopSeeker2.Neuralnetwork.Copy();
                Neuralnetwork.ChangeNeuronWeights(StaticClass.shakeRate);
                SeekerPop.Add(new Seeker(StaticClass.SeekerAnimation, Neuralnetwork, new BotKeys(), StaticClass.StartSeekerPos,
                       null, Color.Red, StaticClass.StartSeekerRot, 
                       new Vector2(StaticClass.SeekerAnimation.First().Value.FrameWidth / 2, StaticClass.SeekerAnimation.First().Value.FrameHeight / 2),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
            // create regular seekers that Their Neural Network is Random
            for (int i = 0; i < StaticClass.SeekerNum * 0.1; i++)
            {
                NeuralNetwork.NeuralNetwork Neuralnetwork = new NeuralNetwork.NeuralNetwork(
                    StaticClass.SeekerInputs, StaticClass.hiddenLayersConfig[
                        StaticClass.rnd.Next(StaticClass.hiddenLayersConfig.Count)],
                    StaticClass.SeekerOutputs);
                SeekerPop.Add(new Seeker(StaticClass.SeekerAnimation, Neuralnetwork, new BotKeys(), StaticClass.StartSeekerPos,
                       null, Color.White, StaticClass.StartSeekerRot, 
                       new Vector2(StaticClass.SeekerAnimation.First().Value.FrameWidth / 2, StaticClass.SeekerAnimation.First().Value.FrameHeight / 2),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
            StaticClass.LiveTopSeeker = null;
            DeadSeekers.Clear();
            StaticClass.PopulationNumber++;
            StaticClass.Stats["NumOfFounds"] = StaticClass.NumOfFounds;
            StaticClass.NumOfFounds = 0;
        }
    }
}
