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
    class Rocket : AnimatedSprite, IInputGamePadLeftStick, IInputGamePadAnalogTriggers
    {
        //Animation State
        private State _lastState;
        private State _state = State.Idle;
        public enum State
        {
            Flying,
            Idle
        }

        private KeyboardState _keyState;
        private GamePadState _padState;

        private float acceleration = 5f;
        private float de_acceleration = 10f;
        private float velocity = 0f;
        private float maxSpeed = 5f;
        private float delta;
        private float fireOffset = 11f;

        private Healthbar health;
        public int maxHealth;
        public int currentHealth = -1;

        private Shieldbar shield;
        public int maxShield;
        public int currentShield = -1;

        public Rocket(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0.2f)
        {
            CollisionHandler.Instance.register(this);
            Origin = new Vector2(32, 32);
            drawResources();


            SourceRectangle = new Rectangle(0, 0, 64, 64);
        }

        public override void Update(GameTime gameTime)
        {
            //Run base Update
            base.Update(gameTime);

            //Get ship rotation
            var deltaX = Math.Sin(Rotation);
            var deltaY = -Math.Cos(Rotation);
            Vector2 moveVector = new Vector2((float)deltaX, (float)deltaY);

            //If pressing X - shoot
            if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed && _padState.Buttons.X == ButtonState.Released) { 
                Shoot(moveVector);
            }

            //If pressing Space - shoot             
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _keyState.IsKeyUp(Keys.Space))
            {
                Shoot(moveVector);
            }

            //MACHINE GUN MADNESS!!!
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                Shoot(moveVector);
            }

            //Calculate delta time
            delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            StayInsideSCreen();


            //What is keyboard and gamepad state?
            _keyState = Keyboard.GetState();
            _padState = GamePad.GetState(PlayerIndex.One);


            //Move & Rotate
            if (Keyboard.GetState().IsKeyDown(Keys.A)) Rotation -= 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) Rotation += 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.W) || GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.1f)
            {
                if (velocity < maxSpeed)
                {
                    velocity += acceleration * delta;
                    _state = State.Flying;
                }
            }
            else
            {
                _state = State.Idle;
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


            //Which animation should play? based on state
            if (_lastState != _state)
            {
                switch (_state)
                {
                    case State.Idle:
                        {
                            List<Rectangle> tempFrames = new List<Rectangle>();
                            tempFrames.Add(new Rectangle(128, 0, 64, 64));
                            tempFrames.Add(new Rectangle(192, 0, 64, 64));
                            Frames = tempFrames;

                            Delay = 75;
                            Loop = true;
                            break;
                        }
                    case State.Flying:
                        {
                            List<Rectangle> tempFrames = new List<Rectangle>();
                            tempFrames.Add(new Rectangle(0, 0, 64, 64));
                            tempFrames.Add(new Rectangle(64, 0, 64, 64));
                            Frames = tempFrames;

                            Delay = 75;
                            Loop = true;
                            break;
                        }
                }

                _lastState = _state;
            }
        }

        void Shoot(Vector2 shipMove)
        {
            Vector2 meh = new Vector2((float)Math.Cos(Rotation - MathHelper.PiOver2), (float)Math.Sin(Rotation - MathHelper.PiOver2)) * 4f + shipMove;

            Bullet bullet = (Bullet)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.Bullet, (Position + meh * fireOffset));
            bullet.Scale = 0.5f;
            bullet.speed = 8f;
            //bullet.Position = Position + meh * fireOffset;
            bullet.Rotation = Rotation;

        }

        public override void Collide(Sprite s)
        {
                Hit(1);

                if (currentHealth <= 0)
                {
                    Asteroid_Explosion _asteroid =
                        (Asteroid_Explosion)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.AsteroidBig_Explosion_64, new Vector2(PositionX - 60f, PositionY - 60f));
                    _asteroid.Scale = 2f;
                    CollisionHandler.Instance.unregister(this);
                    GameObjectHandler.Instance.RemoveGameObject(this);
                }
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
            if (pressure > 0.1f)
            {
                var deltaX = Math.Sin(Rotation);
                var deltaY = -Math.Cos(Rotation);
                Vector2 newVector = new Vector2((float)deltaX, (float)deltaY);
                newVector = newVector * pressure;
                Position += newVector;

                _state = State.Flying;
            }
            else
            {
                _state = State.Idle;
            }
        }

        private void Hit(int damage)
        {
            if (currentShield == -1 && currentHealth == -1)
            {
                currentShield = maxShield;
                Debug.Print(Convert.ToString("Start shield: " + currentShield));
                currentHealth = maxHealth;            
            }

            if (currentHealth > 0 && currentShield > 0)
            {
                currentShield -= damage;
                if (currentShield == 0)
                {
                    shield.Scale = 0f;
                }else{
                    shield.Scale -= (maxShield - damage) / 90f;
                }
            }
            else
            {
                currentHealth -= damage;
                if (currentHealth == 0)
                {
                    health.Scale = 0f;
                }
                else
                {
                    health.Scale -= (maxHealth - damage) / 90f;
                }
            }
            
        }

        private void drawResources()
        {
            health = (Healthbar)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.Healthbar, new Vector2(225, 702));
            health.Scale = 1f;
            shield = (Shieldbar)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.Shieldbar, new Vector2(8, 702));
            shield.Scale = 1f;
        }

        //Checks if object is at the edge of screen, and makes it stay inside!
        private void StayInsideSCreen()
        {
            var width = 1280;
            var height = 800;

            //Maybe (SpriteTexture.Height * 'sprite scale') for more exact check
            var spriteSize = SpriteTexture.Height;

            if (PositionX < 0 + ((spriteSize) / 2))
                Position = new Vector2(0 + ((spriteSize) / 2), PositionY);

            if (PositionY < 0 + ((spriteSize) / 2))
                Position = new Vector2(PositionX, 0 + ((spriteSize) / 2));

            if (PositionX > width - ((spriteSize) / 2))
                Position = new Vector2(width - ((spriteSize) / 2), PositionY);

            //if (PositionY > height - ((spriteSize) / 2))
            //    Position = new Vector2(PositionX, height - ((spriteSize) / 2));

            //Get GUI height -> 'height - guiHeight' = YES SIR!
            if (PositionY > 605)
                Position = new Vector2(PositionX, 605);
        }
    }
}
