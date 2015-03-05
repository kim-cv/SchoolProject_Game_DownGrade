#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion
using System.Runtime.InteropServices;

namespace DownGrade
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private InputController inputController1;
        private InputController inputController2;
        private Texture2D backgroundTexture;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private double _msSinceLastAsteroid;
        private float _asteroidDelay = 1000;
        Random rnd = new Random();

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndIntertAfter,
            int X, int Y, int cx, int cy, int uFlags);
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int Which);

        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;
        private IntPtr HWND_TOP = IntPtr.Zero;
        private const int SWP_SHOWWINDOW = 64;

        public int ScreenWidth
        {
            get { return GetSystemMetrics(SM_CXSCREEN); }
        }
        public int ScreenHeight
        {
            get { return GetSystemMetrics(SM_CYSCREEN); }
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //Set game size
            int w = 1280;
            int h = 720;
            graphics.IsFullScreen = false;
            Window.IsBorderless = false;

            Window.Position = new Point((ScreenWidth / 2) - (w / 2), (ScreenHeight / 2) - (h / 2));
            
            graphics.PreferredBackBufferWidth = w;
            graphics.PreferredBackBufferHeight = h;

            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            inputController1 = new InputController(PlayerIndex.One);
            //inputController2 = new InputController(PlayerIndex.Two);

            Spawner.Instance.SetGameReference(this);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Load textures
            backgroundTexture = Content.Load<Texture2D>("Background_1280x720.png");
            

            //Make gameobjects
            Rocket _rocket = (Rocket)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.Rocket, new Vector2(608, 328));
            _rocket.maxHealth = 10;
            _rocket.maxShield = 10;

            _rocket.Scale = 0.7f;
            //_rocket.Position = new Vector2(100, 250);

            UI ui = (UI)Spawner.Instance.Spawn(Spawner.TypeOfGameObject.UI, new Vector2(0, 640));


            //Controllers
            inputController1.InputGamePadLeftStickListeners.Add(_rocket);
            inputController1.InputGamePadAnalogTriggerListeners.Add(_rocket);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone);
            foreach (Sprite sprite in GameObjectHandler.Instance.GetListOfGameObjects())
            {
                sprite.Draw(gameTime, spriteBatch);
            }

            spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), new Rectangle(0, 0, 1280, 720), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.1f);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}