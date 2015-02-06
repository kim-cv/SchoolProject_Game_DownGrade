using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DownGrade
{
    class Cloud : Sprite
    {
        public Cloud(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position)
        {
            CollisionHandler.Instance.register(this);
        }

        public override void Update(GameTime gameTime)
        {
            //move slowly be decreasing positionX
            PositionX++;

            // if sky moves out of the window move it back into position
            //if (PositionX < -200) PositionX = 900;
        }

        public override void Collide(Sprite s)
        {
            //base.Collide(s);
            this.Scale = 0;
            Debug.WriteLine("3");
        }
    }
}
