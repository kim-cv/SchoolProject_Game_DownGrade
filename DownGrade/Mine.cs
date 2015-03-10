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
    class Mine : AnimatedSprite
    {

        private SoundEffect explosionSoundEffect;
        private SoundEffectInstance explosionSoundEffectInstance;

        public Mine(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0.2f)
        {
            // set sourcerectangle
            SourceRectangle = new Rectangle(0, 0, 16, 16);

            Frames.Add(new Rectangle(0, 0, 16, 16));
            Frames.Add(new Rectangle(16, 0, 16, 16));


            Delay = 500;
            Loop = true;

            Origin = new Vector2(16, 16);

            explosionSoundEffect = AudioHandler.Instance.LoadSoundEffect(AudioHandler.TypeOfSound.Explosion);
            explosionSoundEffectInstance = explosionSoundEffect.CreateInstance();
            explosionSoundEffectInstance.Volume = 0.4f;

            CollisionHandler.Instance.register(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }

        public override void Collide(Sprite s)
        {
            if(s.GetType() != typeof(Mine))
            {
                CollisionHandler.Instance.unregister(this);

                Asteroid_Explosion _asteroid = (Asteroid_Explosion)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.AsteroidBig_Explosion_64, new Vector2(PositionX - 40f, PositionY - 40f));

                explosionSoundEffectInstance.Play();

                GameObjectHandler.Instance.RemoveGameObject(this);
            }
        }
    }
}
