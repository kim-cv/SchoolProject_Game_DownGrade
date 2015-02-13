using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DownGrade.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DownGrade
{
    class Cloud_2 : Sprite, IInputGamePadLeftStick
    {
        public Cloud_2(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position)
        {
            CollisionHandler.Instance.register(this);
        }

        public override void Update(GameTime gameTime)
        {
            //move slowly be decreasing positionX
            //PositionX--;

            // if sky moves out of the window move it back into position
            //if (PositionX < -200) PositionX = 900;
        }

        public override void Collide(Sprite s)
        {
            //base.Collide(s);
            Debug.WriteLine("2");
        }

        public void LeftStickMove(Vector2 moveVector)
        {
            //Position += moveVector;
            Vector2 newVec = new Vector2(moveVector.X, -moveVector.Y);
            newVec = newVec * 20;
            Position += newVec;
        }
    }
}
