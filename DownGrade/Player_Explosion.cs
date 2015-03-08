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
    class Player_Explosion : AnimatedSprite
    {
        public Player_Explosion(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0.2f)
        {
            // set sourcerectangle
            SourceRectangle = new Rectangle(0, 0, 64, 64);

            Frames.Add(new Rectangle(0, 0, 96, 96));
            Frames.Add(new Rectangle(96, 0, 96, 96));
            Frames.Add(new Rectangle(192, 0, 96, 96));
            Frames.Add(new Rectangle(0, 96, 96, 96));
            Frames.Add(new Rectangle(96, 96, 96, 96));
            Frames.Add(new Rectangle(192, 96, 96, 96));
            Frames.Add(new Rectangle(0, 192, 96, 96));
            Frames.Add(new Rectangle(96, 192, 96, 96));

            //Delay = 200;
            Delay = 75;
            Loop = true;

            Origin = new Vector2(48, 48);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (CurrentFrame == Frames.Count - 1)
            {
                GameObjectHandler.Instance.RemoveGameObject(this);
            }
        }
    }
}
