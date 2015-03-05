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
        private float speed = 95f;

        public int brokenState = 1;
        private int maxBrokenState = 2;

        private Vector2 direction;

        public Asteroid(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0.2f)
        {
            CollisionHandler.Instance.register(this);

            //Position = new Vector2(200, 20);

            direction = new Vector2(0, 400) - Position;
            direction.Normalize();
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Fly asteroid FLY!
            PositionX += direction.X * speed * deltaTime;
            PositionY += direction.Y * speed * deltaTime;
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
                    a.Position = Position;
                    a.Scale = 0.5f;
                }
            }
        }
    }
}
