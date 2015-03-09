using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DownGrade
{
    internal class Level_Game : Level
    {
        private Game gameReference;

        private SoundEffect soundEffect;

        private InputController inputController1;
        private InputController inputController2;
        private Texture2D backgroundTexture;
        private Texture2D machinegun;
        private Texture2D laser;
        private Texture2D weapons;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private double _msSinceLastAsteroid;
        private float _asteroidDelay = 1000;
        private Random rnd = new Random();

        public Level_Game(Game gameRef)
            : base()
        {
            gameReference = gameRef;

            //graphics = new GraphicsDeviceManager(gameReference);
            gameReference.Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public void Initialize()
        {
            //base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent()
        {
            inputController1 = new InputController(PlayerIndex.One);


            AudioHandler.Instance.SetGameReference(gameReference);
            Spawner.Instance.SetGameReference(gameReference);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(gameReference.GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Load textures
            backgroundTexture = gameReference.Content.Load<Texture2D>("Background_1280x720.png");

            //Temp GUI Weapon show
            machinegun = gameReference.Content.Load<Texture2D>("UI_Machinegun_Marked.png");
            laser = gameReference.Content.Load<Texture2D>("UI_Laser.png");
            weapons = gameReference.Content.Load<Texture2D>("GUI_Weapons.png");


            //Make gameobjects
            Rocket _rocket = (Rocket)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.Rocket, new Vector2(608, 328));
            _rocket.maxHealth = 10;
            _rocket.maxShield = 5;

            _rocket.Scale = 0.7f;
            //_rocket.Position = new Vector2(100, 250);

            Mine _mine = (Mine) Spawner.Instance.Spawn(Spawner.TypeOfGameObject.Mine, new Vector2(800, 300));
            _mine.Position = new Vector2(800, 300);

            UI ui = (UI)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.UI, new Vector2(0, 640));


            //Controllers
            inputController1.InputGamePadLeftStickListeners.Add(_rocket);
            inputController1.InputGamePadAnalogTriggerListeners.Add(_rocket);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            soundEffect = null;
            backgroundTexture = null;
            machinegun = null;
            laser = null;
            weapons = null;

            _msSinceLastAsteroid = 0;

            CollisionHandler.Instance.UnloadContent();
            GameObjectHandler.Instance.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                LevelHandler.Instance.LoadLevel(LevelHandler.TypeOfLevel.MainScreen);
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                gameReference.Exit();

            // TODO: Add your update logic here

            List<Sprite> tempSprites = new List<Sprite>();

            foreach (Sprite sprite in GameObjectHandler.Instance.GetListOfGameObjects())
            {
                tempSprites.Add(sprite);
            }

            foreach (Sprite sprite in tempSprites)
            {
                sprite.Update(gameTime);
            }


            //Random asteroids 1 every second
            if (gameTime.TotalGameTime.TotalMilliseconds > _msSinceLastAsteroid + _asteroidDelay)
            {
                //MediaPlayer.Volume = 1.0f;
                //MediaPlayer.Play(soundEffect);
                //soundEffect.Play();

                int side = rnd.Next(1, 4);
                int amount = rnd.Next(1280);
                Vector2 v = new Vector2();
                switch (side)
                {
                    case 1:
                        {
                            //top
                            v.Y = -100;
                            v.X = amount;
                            break;
                        }
                    case 2:
                        {
                            //bund
                            v.Y = 1280 + 100;
                            v.X = amount;
                            break;
                        }
                    case 3:
                        {
                            //Venstre
                            v.Y = amount;
                            v.X = -100;
                            break;
                        }
                    case 4:
                        {
                            //Hojre
                            v.Y = amount;
                            v.X = 1280 + 100;
                            break;
                        }
                }

                Asteroid a = (Asteroid)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.AsteroidBig_64, v);
                a.Direction("normal");
                _msSinceLastAsteroid = gameTime.TotalGameTime.TotalMilliseconds;
            }


            CollisionHandler.Instance.Update(gameTime);
            inputController1.Update(gameTime);
            //inputController2.Update(gameTime);
            //base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            //gameReference.GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearWrap,
                DepthStencilState.Default, RasterizerState.CullNone);
            foreach (Sprite sprite in GameObjectHandler.Instance.GetListOfGameObjects())
            {
                sprite.Draw(gameTime, spriteBatch);
            }

            spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), new Rectangle(0, 0, 1280, 720), Color.White, 0f,
                new Vector2(0, 0), 1f, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(weapons, new Vector2(230, 682), null, Color.White, 0f, new Vector2(0, 0), 1f,
                SpriteEffects.None, 1f);
            spriteBatch.Draw(machinegun, new Vector2(370, 675), null, Color.White, 0f, new Vector2(0, 0), 1f,
                SpriteEffects.None, 1f);
            spriteBatch.Draw(laser, new Vector2(450, 675), null, Color.White, 0f, new Vector2(0, 0), 1f,
                SpriteEffects.None, 1f);

            spriteBatch.End();

            //base.Draw(gameTime);
        }
    }
}
