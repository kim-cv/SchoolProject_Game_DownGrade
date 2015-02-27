using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using DownGrade.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DownGrade
{
    class Rocket : Sprite, IInputGamePadLeftStick, IInputGamePadAnalogTriggers
    {
        private KeyboardState _keyState;
<<<<<<< HEAD
        private GamePadState _padState;
=======
        private float acceleration = 5f;
        private float de_acceleration = 10f;
        private float velocity = 0f;
        private float maxSpeed = 5f;
        private float delta;
>>>>>>> 59c6b3d78965d9b5bf04e015bb2086b34f029838

        public Rocket(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position)
        {
            CollisionHandler.Instance.register(this);
            Origin = new Vector2(SpriteTexture.Width / 2f, SpriteTexture.Height / 2f);
        }

        public override void Update(GameTime gameTime)
        {
<<<<<<< HEAD
            if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed && _padState.Buttons.X == ButtonState.Released)
                Shoot();

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _keyState.IsKeyUp(Keys.Space))
                Shoot();
=======
            delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _keyState.IsKeyUp(Keys.Space)) Shoot();
>>>>>>> 59c6b3d78965d9b5bf04e015bb2086b34f029838

            _keyState = Keyboard.GetState();
            _padState = GamePad.GetState(PlayerIndex.One);

            var deltaX = Math.Sin(Rotation);
            var deltaY = -Math.Cos(Rotation);
            Vector2 moveVector = new Vector2((float)deltaX, (float)deltaY);

            if (Keyboard.GetState().IsKeyDown(Keys.A)) Rotation -= 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) Rotation += 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.W) || GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.1f)
            {
                if (velocity < maxSpeed)
                {
                    velocity += acceleration * delta;
                }
            }
            else
            {
                if (velocity > 0)
                {
                    velocity -= de_acceleration * delta;
                }
                else if (velocity < 0)
                {
                    velocity = 0;
                }
            }
            Position += moveVector * velocity;
        }

        void Shoot()
        {
            Bullet bullet = (Bullet)Spawner.Instance.Spawn("Bullet");
            bullet.Scale = 0.5f;
            bullet.speed = 5f;
            bullet.Position = Position;
            bullet.Rotation = Rotation;
        }

        public override void Collide(Sprite s)
        {
            //collide
        }

        public void LeftStickMove(Vector2 moveVector)
        {
            if (moveVector.X > 0)
            {
                Rotation += 0.05f;
            }
            if (moveVector.X < 0)
            {
                Rotation -= 0.05f;
            }
        }


        public void LeftTriggerPressed(float pressure)
        {

        }

        public void RightTriggerPressed(float pressure)
        {
        }
    }
}
