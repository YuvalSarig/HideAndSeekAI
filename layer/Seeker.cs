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
    internal class Seeker : Characters
    {
        public int Score { get; set; }
        public int Found { get; set; }
        public int LifeTime { get; set; }
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
        public int Fitness => Score * Score  + LifeTime;
        float LeftVec, RightVec;
        public Seeker(NeuralNetwork.NeuralNetwork Neuralnetwork, BaseKeys keys, Texture2D texture, Vector2 position,
          Rectangle? sourceRectangle, Color color,
          float rotation, Vector2 origin, Vector2 scale,
          SpriteEffects effects, float layerDepth) :
           base(keys, texture, position,
           sourceRectangle, color,
           rotation, origin, scale,
           effects, layerDepth)
        {
            LifeTime = 0;
            Score = 1000;
            this.Neuralnetwork = Neuralnetwork;
            Neuralnetwork.Seeker = this;
            Energy = 1000;
            MainGame.UpdateEvent += update;

        }
        public override void update()
        {
            if (Dead) return;
            Neuralnetwork.SetAllLayerBias(GetInputs());
            double rotRight = Neuralnetwork.OutputsLayer[0].NBias;
            double rotLeft = Neuralnetwork.OutputsLayer[1].NBias;

            if (rotRight > 0.5)
            {
                Rotation += 0.1f;
            }
            if (rotLeft > 0.5)
            {
                Rotation -= 0.1f;
            }
            Move();

            base.update();

        }

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
            RightVec = (Position - temp).Length();
            angele = Rotation - (float)Math.PI / 6;
            mat = Matrix.CreateRotationZ(angele);
            step = Vector2.Transform(Vector2.UnitY, mat);
            temp = Position;
            while (StaticClass.map[temp].ToString() != "Obstacle" && (Position - temp).Length() <= 300)
            {
                temp += step * 2;
            }
            StaticClass.drawVec(Rotation + (float)Math.PI - (float)Math.PI / 6, Position, Color.Blue, (Position - temp).Length(), 3);
            LeftVec = (Position - temp).Length();
            base.draw();
        }
        private void Move()
        {
            if (Dead)
                return;

            Matrix mat = Matrix.CreateRotationZ(Rotation);
            Vector2 step = Vector2.Transform(Vector2.UnitY, mat);

            Position += step * 4;
            if (StaticClass.map[Position].ToString() == "Obstacle" || Position.X < 0 || Position.X > StaticClass.WIDTH || Position.Y < 0 || Position.Y > StaticClass.HEIGHT)
            {
                Score = 0;
                Dead = true;
                return;
            }
            else if (!StaticClass.v.FindHider(Position, Rotation - (float)Math.PI / 6))
            {
                Score = Math.Max(--Score, 0);
            }
            else if (StaticClass.v.FindHider(Position, Rotation - (float)Math.PI / 6))
            {
                Dead = true;
                Score += 100;
                return;
            }
            LifeTime++;
            Energy--;
        }
        private List<double> GetInputs()
        {
            List<double> inputs = new List<double>();
            float angle;
            angle = (float)Math.Atan2(MainGame.hider.Position.Y - Position.Y, MainGame.hider.Position.X - Position.X) - (float)Math.PI / 2;
            inputs.Add(angle);
            Matrix mat;
            Vector2 step;
            angle = Rotation;
            mat = Matrix.CreateRotationZ(angle);
            step = Vector2.Transform(Vector2.UnitY, mat);
            Vector2 temp = Position;
            float add = MathHelper.TwoPi / 12;

            for (int i = 0; i < 12; i++)
            {
                while (StaticClass.map[temp].ToString() != "Obstacle" && temp.X > 0 && temp.X < StaticClass.WIDTH && temp.Y > 0 && temp.Y < StaticClass.HEIGHT)
                {
                    temp += step * 10;
                }
                inputs.Add((Position - temp).Length() / 100);
                angle += add;
                mat = Matrix.CreateRotationZ(angle);
                step = Vector2.Transform(Vector2.UnitY, mat);
                temp = Position;
            }
            return inputs;
        }
    }
    internal class SeekerComper : IComparer<Seeker>
    {
        public int Compare(Seeker Seeker1, Seeker Seeker2)
        {
            int Seeker1Score = Seeker1.Fitness;
            if (Seeker1.Energy == 0) Seeker1Score -= 100;
            int Seeker2Score = Seeker2.Fitness;
            if (Seeker2.Energy == 0) Seeker2Score -= 100;
            return Seeker1Score.CompareTo(Seeker2Score);
        }
    }
}
