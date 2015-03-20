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
    class EnemyBullet : Sprite
    {
        public float speed;

        //Sounds
        private SoundEffect bulletSoundEffect;
        private SoundEffectInstance bulletSoundEffectInstance;

        public EnemyBullet(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0.2f)
        {
            CollisionHandler.Instance.register(this);
            bulletSoundEffect = AudioHandler.Instance.LoadSoundEffect(AudioHandler.TypeOfSound.Laser_Shoot2);
            bulletSoundEffectInstance = bulletSoundEffect.CreateInstance();
            bulletSoundEffectInstance.Volume = 0.1f;

            bulletSoundEffectInstance.Play();
        }

        public override void Update(GameTime gameTime)
        {
            var deltaX = Math.Sin(Rotation);
            var deltaY = -Math.Cos(Rotation);
            Vector2 meh = new Vector2((float)deltaX, (float)deltaY);
            meh = meh * speed;
            Position += meh;


            DestroyOnExit();
        }

        public override void Collide(Sprite s)
        {
            if (s.GetType() != typeof(EnemyShip) && s.GetType() != typeof(Bullet) && s.GetType() != typeof(Asteroid) && s.GetType() != typeof(Mine) && s.GetType() != typeof(EnemyBullet))
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
