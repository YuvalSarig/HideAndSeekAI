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
    public class Hider : Animation.AnimationManagement
    {
        private float Speed = 5;
        public Vector2 Velocity;
        public Dictionary<string, Animation.Animation> animations;
        Vector2 LastPos;
        protected BaseKeys keys;
        public Hider(Dictionary<string, Animation.Animation> animations, BaseKeys keys, Vector2 position,
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
            MainGame.UpdateEvent += update;
        }

        /// <summary>
        /// Update hider objects on screen
        /// </summary>
        public override void update()
        {
            StaticClass.maph.SetHiderOnMap((int)Position.X, (int)Position.Y, LastPos);
            LastPos = Position;
            if (keys.Up())
                Velocity.Y = -Speed;
            if (keys.Down())
                Velocity.Y = Speed;
            if (keys.Left())
                Velocity.X = -Speed;
            if (keys.Right())
                Velocity.X = Speed;

            if (StaticClass.map[Position + Velocity].ToString() != "Obstacle" && Position.X > 0 && Position.X < StaticClass.WIDTH && Position.Y > 0 && Position.Y < StaticClass.HEIGHT)
            {
                Position += Velocity;
            }

            SetAnimations();

            base.update();
            Velocity = Vector2.Zero;
        }

        /// <summary>
        /// Set the animations base the direction walk
        /// </summary>
        protected virtual void SetAnimations()
        {
            if (Position.X > LastPos.X)
                base.Play(animations["WalkRight"]);
            else if (Position.X < LastPos.X)
                base.Play(animations["WalkLeft"]);
            else if (Position.Y > LastPos.Y)
                base.Play(animations["WalkDown"]);
            else if (Position.Y < LastPos.Y)
                base.Play(animations["WalkUp"]);
            else
                base.Stop();

        }
    }
}
