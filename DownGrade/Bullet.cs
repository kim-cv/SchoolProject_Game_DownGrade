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
        public float speed;

        public Bullet(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0.2f)
        {
            CollisionHandler.Instance.register(this);
        }

        public override void Update(GameTime gameTime)
        {
            var deltaX = Math.Sin(Rotation);
            var deltaY = -Math.Cos(Rotation);
            Vector2 meh = new Vector2((float)deltaX, (float)deltaY);
            meh = meh * speed;
            Position += meh;
        }

        public override void Collide(Sprite s)
        {
            if (s.GetType() != typeof (Rocket) && s.GetType() != typeof (Bullet))
            {
                CollisionHandler.Instance.unregister(this);
                GameObjectHandler.Instance.RemoveGameObject(this);
            }
        }
    }
}
