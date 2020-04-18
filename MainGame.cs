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


        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            StaticClass.init(base.GraphicsDevice, Content);
            map = new Drawer(Content.Load<Texture2D>("map"),
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
            MoveDeadSeekers();
            UpdateEvent?.Invoke();
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            base.GraphicsDevice.Clear(Color.Black);
            StaticClass.sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, cam.Mat);

            DrawEvent?.Invoke();
            StaticClass.sb.DrawString(StaticClass.font, "Iteration number: " + StaticClass.PopulationNumber, new Vector2(0, 0), Color.Black);
            StaticClass.sb.DrawString(StaticClass.font, "Last Fitness: " + StaticClass.Stats["LastFitness"], new Vector2(0, 20), Color.Black);
            StaticClass.sb.DrawString(StaticClass.font, "Max Fitness: " + StaticClass.Stats["MaxFitness"], new Vector2(0, 40), Color.Black);
            StaticClass.sb.DrawString(StaticClass.font, "Num of founds: " + StaticClass.Stats["NumOfFounds"], new Vector2(0, 60), Color.Black);

            StaticClass.sb.End();

            base.Draw(gameTime);
        }

        // Initialize Seekers List
        private void InitSeekers()
        {
            StaticClass.StartSeekerRot = (float)StaticClass.rnd.NextDouble() * MathHelper.TwoPi;
            for (int i = 0; i < StaticClass.SeekerNum; i++)
            {
                NeuralNetwork.NeuralNetwork Neuralnetwork = new NeuralNetwork.NeuralNetwork(
                    StaticClass.SeekerInputs,
                    StaticClass.hiddenLayersConfig[StaticClass.rnd.Next(StaticClass.hiddenLayersConfig.Count)],
                    StaticClass.SeekerOutputs);
                SeekerPop.Add(new Seeker(Neuralnetwork, new BotKeys(),
                    Content.Load<Texture2D>("seeker"), StaticClass.StartSeekerPos,
                       null, Color.White, StaticClass.StartSeekerRot, new Vector2(96, 106),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
        }

        // Check if seeker was dead and then moving to the deead seekers list
        // if all seekers is dead create population
        private void MoveDeadSeekers()
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

        // Create seeker population by the best seekers
        private void CreateSeekerPopulation()
        {
            // sort dead seekers by the best fitness they achive
            DeadSeekers.Sort(new SeekerComper());
            DeadSeekers.Reverse();
            // set the best seeker by the most fitness points 
            if (StaticClass.topSeeker == null || DeadSeekers[0].Fitness >
                StaticClass.topSeeker.Fitness)
            {
                StaticClass.topSeeker2 = StaticClass.topSeeker;
                StaticClass.topSeeker = DeadSeekers[0];
            }
            // Change the dictionary
            if (StaticClass.topSeeker2 == null) StaticClass.topSeeker2 = DeadSeekers[1];
            StaticClass.Stats["MaxFitness"] = Math.Max(StaticClass.Stats["MaxFitness"],
                DeadSeekers[0].Fitness);
            StaticClass.Stats["LastFitness"] = DeadSeekers[0].Fitness;
            if (DeadSeekers[0].Fitness == 0) throw new Exception();
            // create black seekers that Their Neural Network Similar the best seeker
            for (int i = 0; i < StaticClass.SeekerNum * 0.35; i++)
            {
                NeuralNetwork.NeuralNetwork Neuralnetwork = 
                    StaticClass.topSeeker.Neuralnetwork.Copy();
                Neuralnetwork.Cross(DeadSeekers[0].Neuralnetwork);
                Neuralnetwork.ChangeNeuronWeights(StaticClass.shakeRate);
                SeekerPop.Add(new Seeker(Neuralnetwork, new BotKeys(),
                    Content.Load<Texture2D>("seeker"), StaticClass.StartSeekerPos,
                       null, Color.Black, StaticClass.StartSeekerRot, new Vector2(96, 106),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
            // create blue seekers that Their Neural Network With a little change of the best seeker
            for (int i = 0; i < StaticClass.SeekerNum * 0.35; i++)
            {
                NeuralNetwork.NeuralNetwork Neuralnetwork = 
                    StaticClass.topSeeker.Neuralnetwork.Copy();
                Neuralnetwork.ChangeNeuronWeights(StaticClass.shakeRate);
                SeekerPop.Add(new Seeker(Neuralnetwork, new BotKeys(),
                    Content.Load<Texture2D>("seeker"), StaticClass.StartSeekerPos,
                       null, Color.Blue, StaticClass.StartSeekerRot, new Vector2(96, 106),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
            // create red seekers that Their Neural Network With a little change of the second best seeker
            for (int i = 0; i < StaticClass.SeekerNum * 0.2; i++)
            {
                NeuralNetwork.NeuralNetwork Neuralnetwork = 
                    StaticClass.topSeeker2.Neuralnetwork.Copy();
                Neuralnetwork.ChangeNeuronWeights(StaticClass.shakeRate);
                SeekerPop.Add(new Seeker(Neuralnetwork, new BotKeys(),
                    Content.Load<Texture2D>("seeker"), StaticClass.StartSeekerPos,
                       null, Color.Red, StaticClass.StartSeekerRot, new Vector2(96, 106),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
            // create regular seekers that Their Neural Network is Random
            for (int i = 0; i < StaticClass.SeekerNum * 0.1; i++)
            {
                NeuralNetwork.NeuralNetwork Neuralnetwork = new NeuralNetwork.NeuralNetwork(
                    StaticClass.SeekerInputs, StaticClass.hiddenLayersConfig[
                        StaticClass.rnd.Next(StaticClass.hiddenLayersConfig.Count)],
                    StaticClass.SeekerOutputs);
                SeekerPop.Add(new Seeker(Neuralnetwork, new BotKeys(),
                    Content.Load<Texture2D>("seeker"), StaticClass.StartSeekerPos,
                       null, Color.White, StaticClass.StartSeekerRot, new Vector2(96, 106),
                       new Vector2(StaticClass.CharacterScale), 0, 0));
            }
            DeadSeekers.Clear();
            StaticClass.PopulationNumber++;
            StaticClass.Stats["NumOfFounds"] = StaticClass.NumOfFounds;
            StaticClass.NumOfFounds = 0;
        }
    }
}
