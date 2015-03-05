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
    class Asteroid_Explosion : AnimatedSprite
    {
        public Asteroid_Explosion(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0.2f)
        {
            // set sourcerectangle
            SourceRectangle = new Rectangle(0, 0, 64, 64);

            Frames.Add(new Rectangle(0, 0, 64, 64));
            Frames.Add(new Rectangle(64, 0, 64, 64));
            Frames.Add(new Rectangle(128, 0, 64, 64));
            Frames.Add(new Rectangle(0, 64, 64, 64));
            Frames.Add(new Rectangle(64, 64, 64, 64));
            Frames.Add(new Rectangle(128, 64, 64, 64));
            Frames.Add(new Rectangle(0, 128, 64, 64));
            Frames.Add(new Rectangle(64, 128, 64, 64));

            //Delay = 200;
            Delay = 75;
            Loop = true;
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
