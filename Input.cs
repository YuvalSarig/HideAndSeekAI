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
    #region base
    abstract class BaseKeys
    {
        public abstract bool Left();
        public abstract bool Right();
        public abstract bool Up();
        public abstract bool Down();
        public abstract void WhoAmI(IFocous me);



    }
    #endregion
    #region user
    class UserKeys : BaseKeys
    {
        Keys up, down, left, right;
        public UserKeys(Keys left, Keys right, Keys up, Keys down)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
        }
        public override bool Left()
        {
            return G.ks.IsKeyDown(left);
        }
        public override bool Up()
        {
            return G.ks.IsKeyDown(up);
        }
        public override bool Down()
        {
            return G.ks.IsKeyDown(down);
        }
        public override bool Right()
        {
            return G.ks.IsKeyDown(right);
        }
        public override void WhoAmI(IFocous me)
        {

        }
    }
    #endregion
    class BotKeys : BaseKeys
    {
        #region DATA
        bool left, right, up, down;
        IFocous Target;
        float wheretoaim = 0;
        IFocous mycar;
        #endregion
        #region Ctors
        public BotKeys(IFocous target)
        {
            this.Target = target;
            wheretoaim = (G.rnd.Next(100) - 50) / 50f;
            Game1.Event_Update += update;
        }
        #endregion
        #region OVERIDE FUNCS
        public override bool Left()
        {
            return left;
        }
        public override bool Up()
        {
            return up;
        }
        public override bool Down()
        {
            return down;
        }
        public override bool Right()
        {
            return right;
        }
        #endregion
        public override void WhoAmI(IFocous mycar)
        {
            this.mycar = mycar;
        }
        void update()
        {
            up = true;
            down = false;
            Vector2 distance = Target.Position - mycar.Position;
            if (distance.Length() < 400)
            {
                left = false;
                right = false;
                up = false;
                down = true;
                return;
            }

            double angle = Math.Atan2(distance.X, -distance.Y);

            left = false;
            right = false;

            float reallywheretoaim = wheretoaim;
            if (distance.Length() < 500)
            {
                reallywheretoaim = 0;
            }

            float angleDif = MathHelper.WrapAngle(mycar.Rotation -
                MathHelper.Pi / 2 - (float)angle + reallywheretoaim);

            if (Math.Abs(angleDif) > G.STEERSPEED)
            {
                if (angleDif >= 0) left = true;
                else right = true;
            }
        }
    }
}
