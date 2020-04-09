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
        private List<Seeker> SeekerPop  = new List<Seeker>();
        private List<Seeker> DeadSeekers = new List<Seeker>();
        public static Hider hider;
        Drawer map;
        Camera cam;
        private bool SynchronousUpdateDone = false;

        public MainGame()
        {
            GraphicsDeviceM = new GraphicsDeviceManager(this);
            GraphicsDeviceM.PreferredBackBufferHeight = StaticClass.HEIGHT;
            GraphicsDeviceM.PreferredBackBufferWidth = StaticClass.WIDTH;
            Window.AllowUserResizing = true;
            GraphicsDeviceM.ApplyChanges();
            Content.RootDirectory = "Content";

        }


        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            StaticClass.init(base.GraphicsDevice, Content);
            map = new Drawer(Content.Load<Texture2D>("map2"),
                       new Vector2(0, 0),
                       null, Color.White, 0, new Vector2(0),
                       new Vector2(StaticClass.mapScale), SpriteEffects.None, 0);

            InitSeekers();
            hider = new Hider(
                new UserKeys(Keys.Left, Keys.Right, Keys.Up, Keys.Down),
                Content.Load<Texture2D>("hider"),
                       StaticClass.GetRandPos(),
                       null, Color.White, 0, new Vector2(104, 124),
                       new Vector2(StaticClass.CharacterScale), 0, 0);
            while ((SeekerPop[0].Position - hider.Position).Length() < 400)
            {
                hider.Position = StaticClass.GetRandPos();
            }
            List<IFocous> focous = new List<IFocous>();
            focous.Add(hider);
            focous.AddRange(SeekerPop);
            cam = new Camera(focous, new Viewport(0, 0, StaticClass.WIDTH, StaticClass.HEIGHT), Vector2.Zero);
        }


        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            Update();
            UpdateEvent?.Invoke();
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            base.GraphicsDevice.Clear(Color.Black);
            StaticClass.sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, cam.Mat);
            StaticClass.sb.DrawString(StaticClass.font, "Iteration number: " +StaticClass.populationNumber, new Vector2(400, 400), Color.Red);

            DrawEvent?.Invoke();
            StaticClass.sb.End();

            base.Draw(gameTime);
        }

        // Initialize Seekers List
        private void InitSeekers()
        {
            for (int i = 0; i < StaticClass.SeekerNum; i++)
            {
                NeuralNetwork.SeekerNeuroNetwork Neuralnetwork = new NeuralNetwork.SeekerNeuroNetwork(
                    StaticClass.SeekerInputs,
                    StaticClass.hiddenLayersConfig[StaticClass.rnd.Next(StaticClass.hiddenLayersConfig.Count)],
                    StaticClass.SeekerOutputs, StaticClass.SeekerMemory);
                SeekerPop.Add(new Seeker(Neuralnetwork, new BotKeys(),
                    Content.Load<Texture2D>("seeker"), StaticClass.StartSeekerPos,
                       null, Color.White, 0/*(float)StaticClass.rnd.NextDouble() * MathHelper.TwoPi*/, new Vector2(96, 106),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
        }
        private void Update()
        {
            SynchronousUpdateDone = false;
            for (int i = SeekerPop.Count - 1; i >= 0; i--)
            {
                if (SeekerPop[i].Dead)
                {
                    DeadSeekers.Add(SeekerPop[i]);
                    DrawEvent -= SeekerPop[i].draw;
                    SeekerPop.RemoveAt(i);
                }
                else
                {
                    SeekerPop[i].update();
                }
            }
            if (SeekerPop.Count == 0)
            {
                CreateSeekerPopulation();
                SynchronousUpdateDone = true;
            }
        }
        private void CreateSeekerPopulation()
        {
            DeadSeekers.Sort(new SeekerComper());
            DeadSeekers.Reverse();
            if (StaticClass.topSeeker == null || DeadSeekers[0].Fitness > StaticClass.topSeeker.Fitness)
            {
                StaticClass.topSeeker2 = StaticClass.topSeeker;
                StaticClass.topSeeker = DeadSeekers[0];
            }
            if (StaticClass.topSeeker2 == null) StaticClass.topSeeker2 = DeadSeekers[1];
            StaticClass.scores["maxFitness"] = Math.Max(StaticClass.scores["maxFitness"], DeadSeekers[0].Fitness);
            StaticClass.scores["lastFitness"] = DeadSeekers[0].Fitness;
            if (DeadSeekers[0].Fitness == 0) throw new Exception();

            for (int i = 0; i < StaticClass.SeekerNum * 0.3; i++)
            {
                NeuralNetwork.SeekerNeuroNetwork Neuralnetwork = StaticClass.topSeeker.Neuralnetwork.Copy();
                Neuralnetwork.Cross(DeadSeekers[0].Neuralnetwork);
                Neuralnetwork.ChangeNeuronWeights(StaticClass.shakeRate);
                SeekerPop.Add(new Seeker(Neuralnetwork, new BotKeys(),
                    Content.Load<Texture2D>("seeker"), StaticClass.StartSeekerPos,
                       null, Color.Black, 0/*(float)StaticClass.rnd.NextDouble() * MathHelper.TwoPi*/, new Vector2(96, 106),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
            for (int i = 0; i < StaticClass.SeekerNum * 0.3; i++)
            {
                NeuralNetwork.SeekerNeuroNetwork Neuralnetwork = StaticClass.topSeeker.Neuralnetwork.Copy();
                Neuralnetwork.ChangeNeuronWeights(StaticClass.shakeRate);
                SeekerPop.Add(new Seeker(Neuralnetwork, new BotKeys(),
                    Content.Load<Texture2D>("seeker"), StaticClass.StartSeekerPos,
                       null, Color.Blue, 0/*(float)StaticClass.rnd.NextDouble() * MathHelper.TwoPi*/, new Vector2(96, 106),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
            for (int i = 0; i < StaticClass.SeekerNum * 0.2; i++)
            {
                NeuralNetwork.SeekerNeuroNetwork Neuralnetwork = StaticClass.topSeeker2.Neuralnetwork.Copy();
                Neuralnetwork.ChangeNeuronWeights(StaticClass.shakeRate);
                SeekerPop.Add(new Seeker(Neuralnetwork, new BotKeys(),
                    Content.Load<Texture2D>("seeker"), StaticClass.StartSeekerPos,
                       null, Color.Red, 0/*(float)StaticClass.rnd.NextDouble() * MathHelper.TwoPi*/, new Vector2(96, 106),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
            for (int i = 0; i < StaticClass.SeekerNum * 0.2; i++)
            {
                NeuralNetwork.SeekerNeuroNetwork Neuralnetwork = new NeuralNetwork.SeekerNeuroNetwork(StaticClass.SeekerInputs, StaticClass.hiddenLayersConfig[StaticClass.rnd.Next(StaticClass.hiddenLayersConfig.Count)], StaticClass.SeekerOutputs, StaticClass.SeekerMemory);
                SeekerPop.Add(new Seeker(Neuralnetwork, new BotKeys(),
                    Content.Load<Texture2D>("seeker"), StaticClass.StartSeekerPos,
                       null, Color.White, 0/*(float)StaticClass.rnd.NextDouble() * MathHelper.TwoPi*/, new Vector2(96, 106),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
            DeadSeekers.Clear();
            StaticClass.populationNumber++;
        }
    }
}
