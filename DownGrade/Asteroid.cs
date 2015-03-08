using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DownGrade.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace DownGrade
{
    class Asteroid : Sprite
    {
        private float speed = 195f;

        public int brokenState = 2;

        private Vector2 direction;

        private SoundEffect explosionSoundEffect;
        private SoundEffectInstance explosionSoundEffectInstance;

        public Asteroid(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0.2f)
        {
            CollisionHandler.Instance.register(this);

            //Explosion Sound
            explosionSoundEffect = AudioHandler.Instance.LoadSoundEffect(AudioHandler.TypeOfSound.Explosion);
            explosionSoundEffectInstance = explosionSoundEffect.CreateInstance();
            explosionSoundEffectInstance.Volume = 0.4f;
        }

        public void Direction(string type = "")
        {
            if (type == "normal")
            {
                //Try get position of rocket
                Sprite temp = GameObjectHandler.Instance.FindGameObject("Rocket");
                if (temp != null)
                {
                    direction = GameObjectHandler.Instance.FindGameObject("").Position - Position;
                }
                else
                {
                    direction = new Vector2(200, 200) - Position; 
                }

                direction.Normalize();
            }
            else
            {
                direction = new Vector2(Spawner.Instance.rnd.Next(1000), Spawner.Instance.rnd.Next(100)) - Position;
                direction.Normalize();
            }
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Fly asteroid FLY!
            Position += direction * speed * deltaTime;
        }

        public override void Collide(Sprite s)
        {
            //base.Collide(s);
            if (s.GetType() != typeof(Asteroid))
            {
                CollisionHandler.Instance.unregister(this);

                Asteroid_Explosion _asteroid = (Asteroid_Explosion)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.AsteroidBig_Explosion_64, Position);
                _asteroid.Scale = Scale;
                
                explosionSoundEffectInstance.Play();

                GameObjectHandler.Instance.RemoveGameObject(this);


                if (brokenState > 1 && s.GetType() != typeof(Rocket))
                {
                    for (int i = 0; i < brokenState; i++)
                    {
                        Asteroid a = (Asteroid)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.AsteroidBig_64, Position);
                        a.Scale = 0.5f;
                        a.brokenState -= 1;
                        a.Direction();
                    }

                }
            }
        }
    }
}
