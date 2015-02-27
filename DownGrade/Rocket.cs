using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DownGrade.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DownGrade
{
    class Rocket : Sprite, IInputGamePadLeftStick, IInputGamePadAnalogTriggers, IInputGamePadButtons
    {
        private KeyboardState _keyState;

        public Rocket(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position)
        {
            CollisionHandler.Instance.register(this);
            Origin = new Vector2(SpriteTexture.Width / 2f, SpriteTexture.Height / 2f);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _keyState.IsKeyUp(Keys.Space)) Shoot();

            _keyState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.A)) Rotation -= 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) Rotation += 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                var deltaX = Math.Sin(Rotation);
                var deltaY = -Math.Cos(Rotation);
                Vector2 moveVector = new Vector2((float)deltaX, (float)deltaY);
                moveVector = moveVector * 2f;
                Position += moveVector;
            }
        }

        public override void Collide(Sprite s)
        {
            //collide
        }

        public void LeftStickMove(Vector2 moveVector)
        {
            if (moveVector.X > 0) { 
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
            var deltaX = Math.Sin(Rotation);
            var deltaY = -Math.Cos(Rotation);
            Vector2 moveVector = new Vector2((float)deltaX, (float)deltaY);
            moveVector = moveVector * pressure;
            Position += moveVector;
        }

        void Shoot()
        {
            Bullet bullet = (Bullet)Spawner.Instance.Spawn("Bullet");
            bullet.Scale = 0.5f;
            bullet.speed = 5f;
            bullet.Position = Position;
            bullet.Rotation = Rotation;
        }

        public void ButtonXDown(InputController.ButtonStates buttonStates)
        {
            Shoot();
        }

        public void ButtonADown(InputController.ButtonStates buttonStates)
        {
            throw new NotImplementedException();
        }

        public void ButtonBDown(InputController.ButtonStates buttonStates)
        {
            throw new NotImplementedException();
        }

        public void ButtonYDown(InputController.ButtonStates buttonStates)
        {
            throw new NotImplementedException();
        }

        public void ButtonLeftShoulderDown(InputController.ButtonStates buttonStates)
        {
            throw new NotImplementedException();
        }

        public void ButtonRightShoulderDown(InputController.ButtonStates buttonStates)
        {
            throw new NotImplementedException();
        }

        public void ButtonStartDown(InputController.ButtonStates buttonStates)
        {
            throw new NotImplementedException();
        }

        public void ButtonBackDown(InputController.ButtonStates buttonStates)
        {
            throw new NotImplementedException();
        }

        public void ButtonLeftStickDown(InputController.ButtonStates buttonStates)
        {
            throw new NotImplementedException();
        }

        public void ButtonRightStickDown(InputController.ButtonStates buttonStates)
        {
            throw new NotImplementedException();
        }
    }
}
