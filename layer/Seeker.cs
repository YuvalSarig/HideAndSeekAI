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
    internal class Seeker : Animation.AnimationManagement
    {
        public Dictionary<string, Animation.Animation> animations;
        public float Score { get; set; }
        public int LifeTime { get; set; }
        public bool Found { get; set; }
        public bool Dead { get; set; }
        private int energy;
        public int Energy
        {
            get { return energy; }
            set
            {
                energy = value;
                if (energy <= 0) Dead = true;
            }
        }
        public NeuralNetwork.NeuralNetwork Neuralnetwork { get; }
        // Fitness function
        public float Fitness => Score * Score;
        protected BaseKeys keys;

        public Seeker(Dictionary<string, Animation.Animation> animations, NeuralNetwork.NeuralNetwork Neuralnetwork, BaseKeys keys, Vector2 position,
          Rectangle? sourceRectangle, Color color,
          float rotation, Vector2 origin, Vector2 scale,
          SpriteEffects effects, float layerDepth) :
           base(animations.First().Value, position,
           sourceRectangle, color,
           rotation, origin, scale,
           effects, layerDepth)
        {
            this.animations = animations;
            this.keys = keys;
            LifeTime = 1;
            Score = 0;
            this.Neuralnetwork = Neuralnetwork;
            Energy = 501;
            MainGame.UpdateEvent += update;

        }
        /// <summary>
        /// Update seeker objects on screen
        /// </summary>
        public override void update()
        {
            if (Dead) return;

            if (StaticClass.LiveTopSeeker == null || StaticClass.LiveTopSeeker.Dead || this.Fitness > StaticClass.LiveTopSeeker.Fitness)
                StaticClass.LiveTopSeeker = this;

            Neuralnetwork.SetAllLayerBias(GetInputs());
            double RotateRight = Neuralnetwork.OutputsLayer[0].NBias;
            double RotateLeft = Neuralnetwork.OutputsLayer[1].NBias;

            if (RotateRight > RotateLeft)
            {
                Rotation += 0.2f;
            }
            else
            {
                Rotation -= 0.2f;
            }
            base.Play(animations["WalkDown"]);
            Move();

            base.update();

        }

        /// <summary>
        /// Draw lines on screen
        /// </summary>
        public override void draw()
        {
            float angele = Rotation + (float)Math.PI / 6;
            Matrix mat = Matrix.CreateRotationZ(angele);
            Vector2 step = Vector2.Transform(Vector2.UnitY, mat);
            Vector2 temp = Position;
            while (StaticClass.map[temp].ToString() != "Obstacle" && (Position - temp).Length() <= 300)
            {
                temp += step * 2;
            }
            StaticClass.drawVec(Rotation + (float)Math.PI + (float)Math.PI / 6, Position, Color.Blue, (Position - temp).Length(), 3);
            angele = Rotation - (float)Math.PI / 6;
            mat = Matrix.CreateRotationZ(angele);
            step = Vector2.Transform(Vector2.UnitY, mat);
            temp = Position;
            while (StaticClass.map[temp].ToString() != "Obstacle" && (Position - temp).Length() <= 300)
            {
                temp += step * 2;
            }
            StaticClass.drawVec(Rotation + (float)Math.PI - (float)Math.PI / 6, Position, Color.Blue, (Position - temp).Length(), 3);
            base.draw();
        }

        /// <summary>
        /// Move the Seeker forward and give score
        /// </summary>
        private void Move()
        {
            if (Dead)
                return;

            // Set the rotation to move 
            Matrix mat = Matrix.CreateRotationZ(Rotation);
            Vector2 step = Vector2.Transform(Vector2.UnitY, mat);

            // Move forward
            Position += step * 5;
            // Stuck in obstacle or went out of the map
            if (StaticClass.map[Position].ToString() == "Obstacle" ||
                    Position.X < 0 || Position.X > StaticClass.WIDTH ||
                    Position.Y < 0 || Position.Y > StaticClass.HEIGHT)
            {
                Dead = true;
                return;
            }
            // Seeker found the hider
            else if (StaticClass.v.FindHider(Position, Rotation - (float)Math.PI / 6))
            {
                Found = true;
                Dead = true;
                Score += 50;
                return;
            }
            // Seeker was not found
            else
            {
                Score += 3 / (MainGame.hider.Position - Position).Length();
            }
            LifeTime++;
            Energy--;
        }

        /// <summary>
        /// Get the inputs parameters to input layer
        /// </summary>
        /// <returns>list of inputs number for neural network</returns>
        private List<double> GetInputs()
        {
            List<double> inputs = new List<double>();
            float angle, DeltaAngle;
            DeltaAngle = (float)Math.Atan2(MainGame.hider.Position.Y - Position.Y, MainGame.hider.Position.X - Position.X) - (float)Math.PI / 2;
            DeltaAngle = this.Rotation - DeltaAngle;
            inputs.Add(DeltaAngle);

            Matrix mat;
            Vector2 step;
            angle = Rotation;
            mat = Matrix.CreateRotationZ(angle);
            step = Vector2.Transform(Vector2.UnitY, mat);
            Vector2 temp = Position;
            float add = (float)Math.PI / 6;
            angle -= add;

            for (int i = 0; i < 3; i++)
            {
                while (StaticClass.map[temp].ToString() != "Obstacle" && temp.X > 0 && temp.X < StaticClass.WIDTH && temp.Y > 0 && temp.Y < StaticClass.HEIGHT)
                {
                    temp += step * 10;
                }
                inputs.Add((Position - temp).Length() / 300);
                angle += add;
                mat = Matrix.CreateRotationZ(angle);
                step = Vector2.Transform(Vector2.UnitY, mat);
                temp = Position;
            }
            return inputs;

        }
    }

        /// <summary>
        /// This class was made for sort a list 
        /// </summary>
        internal class SeekerCompare : IComparer<Seeker>
        {
            // The compare function
            public int Compare(Seeker Seeker1, Seeker Seeker2)
            {
                float Seeker1Score = Seeker1.Fitness;
                if (Seeker1.Energy == 0) Seeker1Score -= 100;
                float Seeker2Score = Seeker2.Fitness;
                if (Seeker2.Energy == 0) Seeker2Score -= 100;
                return Seeker1Score.CompareTo(Seeker2Score);
            }
        }
    
}
