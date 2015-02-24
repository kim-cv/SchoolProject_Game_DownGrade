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
    class Bullet : Sprite
    {
        public Bullet(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position)
        {
            CollisionHandler.Instance.register(this);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Collide(Sprite s)
        {
            //base.Collide(s);
            //this.Scale = 0;
            //Debug.WriteLine("3");
        }
    }
}
