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
    class Missile : Sprite
    {
        public float speed;

        //Sounds
        private SoundEffect missileSoundEffect;
        private SoundEffectInstance missileSoundEffectInstance;

        public Missile(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0.2f)
        {
            CollisionHandler.Instance.register(this);
            //missileSoundEffect = AudioHandler.Instance.LoadSoundEffect(AudioHandler.TypeOfSound.?);
            //missileSoundEffectInstance = missileSoundEffect.CreateInstance();
            //missileSoundEffectInstance.Volume = 0.1f;

            //missileSoundEffectInstance.Play();
        }

        public override void Update(GameTime gameTime)
        {
            var deltaX = Math.Sin(Rotation);
            var deltaY = -Math.Cos(Rotation);
            Vector2 meh = new Vector2((float)deltaX, (float)deltaY);
            meh = meh * speed;
            Position += meh;

            
            DestroyOnExit();
            speed += 0.3f;
        }

        public override void Collide(Sprite s)
        {
            if (s.GetType() != typeof(Rocket) && s.GetType() != typeof(Bullet) && s.GetType() != typeof(Missile))
            {
                CollisionHandler.Instance.unregister(this);
                GameObjectHandler.Instance.RemoveGameObject(this);
            }
        }

        private void DestroyOnExit()
        {
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
