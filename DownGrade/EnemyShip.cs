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
    class EnemyShip : AnimatedSprite
    {
        private double rotationdifference;
        private float speed = 100f;

        private float machinegunFireOffset = 0f;
        private float _bulletDelay = 500;
        private double _msSinceLastBullet;

        private Vector2 direction;

        private SoundEffect explosionSoundEffect;
        private SoundEffectInstance explosionSoundEffectInstance;

        public EnemyShip(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0.2f)
        {
            CollisionHandler.Instance.register(this);

            SourceRectangle = new Rectangle(0, 0, 64, 64);

            Origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height / 2);

            //Explosion Sound
            explosionSoundEffect = AudioHandler.Instance.LoadSoundEffect(AudioHandler.TypeOfSound.Explosion);
            explosionSoundEffectInstance = explosionSoundEffect.CreateInstance();
            explosionSoundEffectInstance.Volume = 0.1f;

            

            

        }

        public void Direction()
        {
            if (GameObjectHandler.Instance.FindGameObject("Rocket") != null) { 
                direction = GameObjectHandler.Instance.FindGameObject("Rocket").Position - Position;
                direction.Normalize();

                float angle = (float)Math.Atan2(direction.X, -direction.Y);

                Rotation = angle;
            }
            
            
           

        }

        public override void Update(GameTime gameTime)
        {
            Animation();
            Direction();
            var deltaX = Math.Sin(Rotation);
            var deltaY = -Math.Cos(Rotation);
            Vector2 moveVector = new Vector2((float)deltaX, (float)deltaY);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            Shoot(moveVector, gameTime);


            
            //if (rotationdifference < 0)
            //{
            //    Rotation++;
            //}
            //else if(rotationdifference > 0)
            //{
            //    Rotation--;
            //}


            Position += direction * speed * deltaTime;

            DestroyOnExit();
        }

        private void DestroyOnExit()
        {
            var temp = 1280;

            if (PositionX < -100 || PositionY < -100 || PositionX > temp + 100 || PositionY > temp + 100)
            {
                GameObjectHandler.Instance.RemoveGameObject(this);
                CollisionHandler.Instance.unregister(this);
            }
        }

        public override void Collide(Sprite s)
        {
            //base.Collide(s);
            if (s.GetType() != typeof(Asteroid) && s.GetType() != typeof(Mine) && s.GetType() != typeof(EnemyBullet))
            {
                CollisionHandler.Instance.unregister(this);

                Asteroid_Explosion _asteroid = (Asteroid_Explosion)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.AsteroidBig_Explosion_64, new Vector2(PositionX - 32f, PositionY - 32f));
                _asteroid.Scale = Scale;
                
                explosionSoundEffectInstance.Play();

                GameObjectHandler.Instance.RemoveGameObject(this);

                int experience = GameObjectHandler.Instance.FindGameObjectProperty("DownGrade.Rocket");
                if (s.GetType() == typeof (Bullet))
                {
                    GameObjectHandler.Instance.SetGameObjectProperty("DownGrade.Rocket", experience - 20);
                }
            }
        }

        private void Shoot(Vector2 shipMove, GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds > _msSinceLastBullet + _bulletDelay)
            {
                Vector2 meh = new Vector2((float)Math.Cos(Rotation - MathHelper.PiOver2), (float)Math.Sin(Rotation - MathHelper.PiOver2)) * 4f + shipMove;

                EnemyBullet enemyBullet = (EnemyBullet)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.EnemyBullet, (Position + meh * machinegunFireOffset));
                enemyBullet.Scale = 0.5f;
                enemyBullet.speed = 8f;
                enemyBullet.Rotation = Rotation;

                _msSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }
        private void Animation()
        {
                List<Rectangle> tempFrames = new List<Rectangle>();
                tempFrames.Add(new Rectangle(0, 0, 64, 64));
                tempFrames.Add(new Rectangle(64, 0, 64, 64));
                Frames = tempFrames;

                Delay = 75;
                Loop = true;
            
        }
    }
}
