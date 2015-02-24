using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DownGrade.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DownGrade
{
    class AnimatedRocket : Sprite, IInputGamePadLeftStick, IInputGamePadAnalogTriggers, IInputGamePadButtons
    {
        Animation animation;
        private State _state;
        public enum State
        {
            Waiting,
            walking,
            Jumping
        }

        public AnimatedRocket(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position)
        {
            // set sourcerectangle
            SourceRectangle = new Rectangle(0, 114, 72, 78);

            CollisionHandler.Instance.register(this);
            Origin = new Vector2(SpriteTexture.Width / 2f, SpriteTexture.Height / 2f);
        }

        public override void Update(GameTime gameTime)
        {
            if (animation != null)
            {
                animation.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Do we have a texture? If not then there is nothing to draw...
            if (SpriteTexture != null)
            {
                // Has a source rectangle been set?
                if (SourceRectangle.IsEmpty)
                {
                    // No, so draw the entire sprite texture
                    spriteBatch.Draw(SpriteTexture, Position, null, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0f);
                    //spriteBatch.Draw(SpriteTexture, Position, null, Color.White, Rotation, Origin, new Vector2(Scale, Scale), SpriteEffect, 0f);
                }
                else
                {
                    // Yes, so just draw the specified SourceRect
                    spriteBatch.Draw(SpriteTexture, Position, SourceRectangle, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0f);
                }
            }
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

            if (_state != State.walking)
            {
                //instansiate animation and set frames
                animation = new Animation(this);
                animation.Frames.Add(new Rectangle(0, 96, 72, 96));
                animation.Frames.Add(new Rectangle(72, 96, 72, 96));
                animation.Frames.Add(new Rectangle(144, 96, 72, 96));
                _state = State.walking;
            }
        }


        public void LeftTriggerPressed(float pressure)
        {

        }

        public void RightTriggerPressed(float pressure)
        {
            var deltaX = Math.Sin(Rotation);
            var deltaY = -Math.Cos(Rotation);
            Vector2 meh = new Vector2((float)deltaX, (float)deltaY);
            meh = meh * pressure;
            Position += meh;
        }

        void Shoot()
        {

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

    class Animation : IUpdateable
    {
        private double _milisecondsSinceLastFrameUpdate = 0;
        private Sprite _sprite;
        private int _currentFrame = 0;
        private List<Rectangle> _frames = new List<Rectangle>();
        private bool _loop;
        private int _delay;

        public bool Loop
        {
            get { return _loop; }
            set { _loop = value; }
        }

        public int Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        public List<Rectangle> Frames
        {
            get { return _frames; }
            set { _frames = value; }
        }

        public Animation(Sprite sprite)
        {
            _sprite = sprite;
            _delay = 200;
            _loop = true;
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds > _milisecondsSinceLastFrameUpdate + Delay)
            {
                _sprite.SourceRectangle = NextFrame();
                _milisecondsSinceLastFrameUpdate = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        private Rectangle NextFrame()
        {
            if (_currentFrame == _frames.Count - 1 && _loop) _currentFrame = 0;
            else if (_currentFrame < _frames.Count - 1) _currentFrame++;
            return Frames[_currentFrame];
        }

        public bool Enabled { get; private set; }
        public int UpdateOrder { get; private set; }
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
    }

}
