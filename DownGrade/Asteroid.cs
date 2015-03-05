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
        private int maxBrokenState = 2;

        public Asteroid(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0.2f)
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

                Asteroid_Explosion _asteroid = (Asteroid_Explosion)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.AsteroidBig_Explosion_64);
                _asteroid.Position = Position;
                _asteroid.Scale = Scale;

                GameObjectHandler.Instance.RemoveGameObject(this);

                if (brokenState < maxBrokenState)
                {
                    Asteroid a = (Asteroid)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.AsteroidBig_64);
                    a.brokenState = (brokenState+1);
                    a.Scale = 0.5f;
                }
            }
        }
    }
}
