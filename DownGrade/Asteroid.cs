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
    class Asteroid : Sprite
    {

        public int brokenState = 1;
        private int maxBrokenState = 3;

        public Asteroid(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position)
        {
            CollisionHandler.Instance.register(this);
        }

        public override void Update(GameTime gameTime)
        {
            PositionY++;
        }

        public override void Collide(Sprite s)
        {
            //base.Collide(s);
            if (s.GetType() != typeof(Asteroid))
            {
                CollisionHandler.Instance.unregister(this);
                GameObjectHandler.Instance.RemoveGameObject(this);

                if (brokenState < maxBrokenState)
                {
                    Asteroid a = (Asteroid)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.AsteroidBig1);
                    a.brokenState = (brokenState+1);
                    a.Scale = 0.2f;
                }
            }
        }
    }
}
