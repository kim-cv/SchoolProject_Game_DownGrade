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
    class Laser : Sprite
    {
        public float speed;

        public Laser(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0.2f)
        {
            CollisionHandler.Instance.register(this);
        }

        public override void Update(GameTime gameTime)
        {
            var deltaX = Math.Sin(Rotation);
            var deltaY = -Math.Cos(Rotation);
            Vector2 meh = new Vector2((float)deltaX, (float)deltaY);
            //meh = meh * speed;
            //Position += meh;
            GameObjectHandler.Instance.RemoveGameObject(this);
            CollisionHandler.Instance.unregister(this);

            DestroyOnExit();
        }

        public override void Collide(Sprite s)
        {
            if (s.GetType() != typeof(Rocket) && s.GetType() != typeof(Laser))
            {
                CollisionHandler.Instance.unregister(this);
                GameObjectHandler.Instance.RemoveGameObject(this);
            }
        }

        private void DestroyOnExit()
        {
            //var width = 800;
            //var height = 600;
            var width = 1280;
            var height = 720;

            if (PositionX < -100 || PositionY < -100 || PositionX > width + 100 || PositionY > height + 100)
            {
                GameObjectHandler.Instance.RemoveGameObject(this);
                CollisionHandler.Instance.unregister(this);
            }
        }
    }
}
