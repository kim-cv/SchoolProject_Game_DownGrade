using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Security.Policy;
using System.Text;
using DownGrade.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DownGrade
{
    class Rocket : AnimatedSprite, IInputGamePadLeftStick
    {
        public int Experience { get; set; }

        public string tag = "Rocket";
        private bool _spawned = false;
        private RocketHit _rocketHit;
        private ShieldRegenLost _shieldRegenLost;
        private ShieldLost _shieldLost;
        private AutoLaserLost _autoLaserLost;
        private MissileLost _missileLost;

        //Animation State
        private State _lastState;
        private State _state = State.Idle;
        public enum State
        {
            Flying,
            Idle
        }

        public enum Weapons
        {
            SemiLaser,
            AutoLaser,
            Missile
        }

        private Weapons _currentWeapon;
        private int _weaponPointer = 0;
        private List<Weapons> _weaponList = new List<Weapons>(); 


        private KeyboardState _keyState;
        private GamePadState _padState;

        private bool _hit;
        private double _msSinceLastHit;

        private double _msSinceLastFeedback;

        //Movement
        private float acceleration = 5f;
        private float de_acceleration = 10f;
        private float velocity = 0f;
        private float maxSpeed = 5f;
        private float delta;
        private float machinegunFireOffset = 5f;
        private float laserFireOffset = 40f;
        private float _bulletDelay = 100;
        private double _msSinceLastBullet;

        //Health
        private Healthbar health;
        public double maxHealth;
        public double currentHealth = -1;

        //Shield
        private Shieldbar shield;
        public double maxShield;
        public double currentShield = -1;
        private float _shieldRegainCooldown = 5000;
        private double _msSinceLastDamage;
        private double _msSinceLastShield;

        // Abilities
        private List<string> abilitiesList = new List<string>();

        //Experience
        private int level = 5;
        private int maxExperience = 40;
        private double _msSinceDead;
        private float gameoverDelay = 1000;
        private GameTime gameref;

        public Rocket(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0.2f)
        {
            CollisionHandler.Instance.register(this);
            Origin = new Vector2(32, 32);

            Experience = maxExperience;

            SourceRectangle = new Rectangle(0, 0, 64, 64);

            foreach (Weapons w in Enum.GetValues(typeof(Weapons)))
            {
                _weaponList.Add(w);
            }

            _currentWeapon = _weaponList[0];

            abilitiesList.Add("Shield");
            abilitiesList.Add("ShieldRegain");
        }

        public override void Update(GameTime gameTime)
        {
            getGametime(gameTime);
            //Get ship rotation
            var deltaX = Math.Sin(Rotation);
            var deltaY = -Math.Cos(Rotation);
            Vector2 moveVector = new Vector2((float)deltaX, (float)deltaY);

            if (currentShield == -1 && currentHealth == -1)
            {
                currentShield = maxShield;
                currentHealth = maxHealth;
                drawResources();
            }

            if (!_spawned)
            {
                _rocketHit = (RocketHit) GameObjectHandler.Instance.FindGameObject("RocketHit");
                _shieldRegenLost = (ShieldRegenLost)GameObjectHandler.Instance.FindGameObject("ShieldRegenLost");
                _shieldLost = (ShieldLost)GameObjectHandler.Instance.FindGameObject("ShieldLost");
                _autoLaserLost = (AutoLaserLost)GameObjectHandler.Instance.FindGameObject("AutoLaserLost");
                _missileLost = (MissileLost)GameObjectHandler.Instance.FindGameObject("MissileLost");
                _spawned = true;
            }

            //All sorts of methods controlling the player
            Movement(moveVector);
            Animation();
            StayInsideSCreen();
            xpCalculator();
            ShootButton(moveVector, gameTime, Weapons.SemiLaser);
            ChangeWeapon();
            regainShield();
            shieldAbility();
            CheckDeath();


            if (gameTime.TotalGameTime.TotalMilliseconds > _msSinceLastHit + 200)
            {
                _rocketHit.Hide();

                _msSinceLastHit = gameTime.TotalGameTime.TotalMilliseconds;
            }

            if (gameTime.TotalGameTime.TotalMilliseconds > _msSinceLastFeedback + 2000)
            {
                _shieldRegenLost.Hide();
                _shieldLost.Hide();
                _autoLaserLost.Hide();
                _missileLost.Hide();
            }


            //What is keyboard and gamepad state?
            _keyState = Keyboard.GetState();
            _padState = GamePad.GetState(PlayerIndex.One);

            //Calculate delta time
            delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Experience
            

            //Run base Update
            base.Update(gameTime);

            

        }

        private void ShootButton(Vector2 moveVector, GameTime gameTime, Weapons selectedWeapon)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                switch (_currentWeapon)
                {
                    case Weapons.SemiLaser:
                    {
                        if (_padState.Buttons.X == ButtonState.Released && _keyState.IsKeyUp(Keys.Space))
                        {
                            Vector2 meh = new Vector2((float)Math.Cos(Rotation - MathHelper.PiOver2), (float)Math.Sin(Rotation - MathHelper.PiOver2)) * 4f + moveVector;

                            Bullet bullet = (Bullet)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.Bullet, (Position + meh * machinegunFireOffset));
                            bullet.Scale = 0.5f;
                            bullet.speed = 8f;
                            //bullet.Position = Position + meh * fireOffset;
                            bullet.Rotation = Rotation;
                        }

                        break;
                    }

                    case Weapons.Missile:
                    {
                        if (_padState.Buttons.X == ButtonState.Released && _keyState.IsKeyUp(Keys.Space))
                        {
                            Vector2 meh = new Vector2((float)Math.Cos(Rotation - MathHelper.PiOver2), (float)Math.Sin(Rotation - MathHelper.PiOver2)) * 10f + moveVector;

                            Missile missile = (Missile)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.Missile, (Position + meh * machinegunFireOffset));
                            missile.Scale = 1.5f;
                            missile.Rotation = Rotation;
                        }

                        break;
                    }

                    case Weapons.AutoLaser:
                    {
                        if (gameTime.TotalGameTime.TotalMilliseconds > _msSinceLastBullet + _bulletDelay)
                        {
                            Vector2 meh = new Vector2((float)Math.Cos(Rotation - MathHelper.PiOver2), (float)Math.Sin(Rotation - MathHelper.PiOver2)) * 4f + moveVector;

                            Bullet bullet = (Bullet)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.Bullet, (Position + meh * machinegunFireOffset));
                            bullet.Scale = 0.5f;
                            bullet.speed = 8f;
                            //bullet.Position = Position + meh * fireOffset;
                            bullet.Rotation = Rotation;

                            _msSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
                        }

                        break;
                    }
                }
            }
        }

        public void ChangeWeapon()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed &&
                _padState.Buttons.A == ButtonState.Released || Keyboard.GetState().IsKeyDown(Keys.Tab) && _keyState.IsKeyUp(Keys.Tab))
            {
                _weaponPointer++;
                if (_weaponPointer > _weaponList.Count -1)
                {
                    _weaponPointer = 0;
                }
                _currentWeapon = _weaponList[_weaponPointer];
            }
        }

        public override void Collide(Sprite s)
        {
            if (s.GetType() != typeof(Rocket) && s.GetType() != typeof(Bullet) && s.GetType() != typeof(Missile))
            {

                Hit(1);

                if (currentHealth <= 0)
                {
                    Player_Explosion _playerExplosion =
                        (Player_Explosion)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.Player_Explosion, new Vector2(PositionX, PositionY));
                    _playerExplosion.Scale = 0.7f;
                    _playerExplosion.Rotation = Rotation;
                    _playerExplosion.Position = Position;

                     _msSinceDead = gameref.TotalGameTime.TotalMilliseconds;
                    this.Layer = 0f;
                    CollisionHandler.Instance.unregister(this);
                    
                }
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

        private void Hit(double damage)
        {
            _rocketHit.Show();

            if (currentHealth > 0 && currentShield > 0)
            {
                currentShield -= damage;
                if (currentShield == 0)
                {
                    shield.Position = new Vector2(shield.PositionX - 190, shield.PositionY);
                }else{
                    shield.Position = new Vector2(shield.PositionX - (float)((190 * damage) / maxShield), shield.PositionY);
                }
            }
            else
            {
                if(currentHealth > 0){
                        currentHealth -= damage;
                    if (currentHealth == 0)
                    {
                        health.Position = new Vector2(health.PositionX - 190, health.PositionY);
                    }
                    else
                    {
                        health.Position = new Vector2(health.PositionX - (float)((190 * damage) / maxHealth), health.PositionY);
                    }
                }
            }
            _msSinceLastDamage = gameref.TotalGameTime.TotalMilliseconds;
        }

        private void regainShield()
        {
            if(abilitiesList.Contains("ShieldRegain")){
                if (currentShield != maxShield)
                {
                    if (gameref.TotalGameTime.TotalMilliseconds > _msSinceLastShield + _shieldRegainCooldown && gameref.TotalGameTime.TotalMilliseconds > _msSinceLastDamage + _shieldRegainCooldown)
                    {
                        shield.Position = new Vector2(shield.PositionX + (float)(190 / maxShield), shield.PositionY);
                        if (shield.PositionX > 9f)
                        {
                            shield.PositionX = 9f;
                        }
                        _msSinceLastShield = gameref.TotalGameTime.TotalMilliseconds;
                        currentShield += 1;
                    }
                }
            }
        }

        private void shieldAbility()
        {
            if (!abilitiesList.Contains("Shield"))
            {
                currentShield = 0;
                maxShield = 0;
                shield.PositionX = -190f;
                abilitiesList.Remove("ShieldRegain");
            }
        }

        private void drawResources()
        {
            health = (Healthbar)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.Healthbar, new Vector2(9, 702));
            health.Scale = 1f;
            shield = (Shieldbar)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.Shieldbar, new Vector2(9, 662));
            shield.Scale = 1f;

            if (currentHealth == 0)
            {
                health.Position = new Vector2(health.PositionX - 190, health.PositionY);
            }

            if (currentShield == 0)
            {
                shield.Position = new Vector2(shield.PositionX - 190, shield.PositionY);
            }
            
        }

        private void getGametime(GameTime gameTime)
        {
            gameref = gameTime;
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

        private void Movement(Vector2 moveVector)
        {
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
        }

        private void Animation()
        {
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

        public void xpCalculator()
        {
            if (Experience < 1 && level > 1)
            {
                level -= 1;
                if (abilitiesList.Count > 0)
                {
                    if (abilitiesList.Contains("ShieldRegain"))
                    {
                        abilitiesList.Remove("ShieldRegain");
                        _msSinceLastFeedback = gameref.TotalGameTime.TotalMilliseconds;
                        _shieldRegenLost.Show();
                        Debug.WriteLine("Shield Regain LOST");
                    }
                    else
                    {
                        if (abilitiesList.Contains("Shield"))
                        {
                            abilitiesList.Remove("Shield");
                            _msSinceLastFeedback = gameref.TotalGameTime.TotalMilliseconds;
                            _shieldLost.Show();
                            Debug.WriteLine("Shield LOST");
                        }
                    }
                }
                else
                {
                    if (_weaponList.Count > 1)
                    {
                        if (_weaponList.Contains(Weapons.Missile))
                        {
                            _weaponList.Remove(Weapons.Missile);
                            _msSinceLastFeedback = gameref.TotalGameTime.TotalMilliseconds;
                            _missileLost.Show();
                            Debug.WriteLine("Missile LOST");
                            
                        }
                        else
                        {
                            if (_weaponList.Contains(Weapons.AutoLaser))
                            {
                                _weaponList.Remove(Weapons.AutoLaser);
                                _msSinceLastFeedback = gameref.TotalGameTime.TotalMilliseconds;
                                _autoLaserLost.Show();
                                Debug.WriteLine("Auto Laser LOST");
                                _weaponPointer++;
                                if (_weaponPointer > _weaponList.Count - 1)
                                {
                                    _weaponPointer = 0;
                                }
                                _currentWeapon = _weaponList[_weaponPointer];
                            }
                        }
                    }
                }
                maxExperience *= 2;
                Experience = maxExperience;
            }
        }

        private void CheckDeath()
        {
            if(currentHealth <= 0){
                if (gameref.TotalGameTime.TotalMilliseconds > _msSinceDead + gameoverDelay)
                {
                    Debug.Print("Shipdead check: " + _msSinceDead);
                    GameObjectHandler.Instance.RemoveGameObject(this);
                    _currentWeapon = Weapons.SemiLaser;
                    LevelHandler.Instance.LoadLevel(LevelHandler.TypeOfLevel.GameOver);
                }
            }
        }
    }
}
