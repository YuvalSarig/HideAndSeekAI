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
    public abstract class BaseKeys
    {
        public abstract bool Left();
        public abstract bool Right();
        public abstract bool Up();
        public abstract bool Down();



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
            return StaticClass.ks.IsKeyDown(left);
        }
        public override bool Up()
        {
            return StaticClass.ks.IsKeyDown(up);
        }
        public override bool Down()
        {
            return StaticClass.ks.IsKeyDown(down);
        }
        public override bool Right()
        {
            return StaticClass.ks.IsKeyDown(right);
        }
    }
    #endregion
    public class BotKeys : BaseKeys
    {
        #region DATA
        bool left, right, up, down;


        #endregion
        #region Ctors
        public BotKeys()
        {

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
        void update()
        {
          
        }
    }
}
