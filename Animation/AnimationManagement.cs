using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HNS.Animation
{
    public class AnimationManagement : Drawer
    {
        private Animation animation;

        private float timer;

        public AnimationManagement(Animation animation, Vector2 position,
          Rectangle? sourceRectangle, Color color,
          float rotation, Vector2 origin, Vector2 scale,
          SpriteEffects effects, float layerDepth) : base(position,
           sourceRectangle, color,
           rotation, origin, scale,
           effects, layerDepth)
        {
            this.animation = animation;
        }

        /// <summary>
        /// Draw the animation base on what direction
        /// </summary>
        public override void draw()
        {
            StaticClass.sb.Draw(animation.Texture, Position,
                new Rectangle(animation.CurrentFrame * animation.FrameWidth,
                    0, animation.FrameWidth, animation.FrameHeight),
                      base.color, Rotation, origin, scale,
                      effects, layerDepth);
        }

        /// <summary>
        /// Play the animation
        /// </summary>
        /// <param name="animation">Animation to play</param>
        public void Play(Animation animation)
        {
            if (this.animation == animation)
                return;

            this.animation = animation;

            this.animation.CurrentFrame = 0;

            timer = 0;
        }

        /// <summary>
        /// Stop the animation, if none keys was pressed
        /// </summary>
        public void Stop()
        {
            timer = 0f;
            animation.CurrentFrame = 0;
        }

        /// <summary>
        /// Update the animation
        /// </summary>
        public override void update()
        {
            timer += (float)StaticClass.gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > animation.FrameSpeed)
            {
                timer = 0f;

                animation.CurrentFrame++;

                if (animation.CurrentFrame >= animation.FrameCount)
                    animation.CurrentFrame = 0;
            }
        }
    }
}

