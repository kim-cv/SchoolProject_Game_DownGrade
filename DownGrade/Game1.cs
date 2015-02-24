#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace DownGrade
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private Rocket _rocket;
        private Asteroid _asteroid;
        private Bullet _bullet;
        private Robot _robot;
        private Ufo _ufo;

        private Spawner _spawner = new Spawner();


        private List<Sprite> sprites = new List<Sprite>();
        private InputController inputController1;
        private InputController inputController2;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

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
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //Set game size
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
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
            inputController2 = new InputController(PlayerIndex.Two);
            

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Load textures
            Texture2D asteroidTexture = Content.Load<Texture2D>("asteroid.png");
            Texture2D rocketTexture = Content.Load<Texture2D>("rocket.png");
            Texture2D bulletTexture = Content.Load<Texture2D>("Bullet.png");
            Texture2D robotTexture = Content.Load<Texture2D>("robot.png");
            Texture2D UFOTexture = Content.Load<Texture2D>("UFO.png");

            //Make gameobjects
            _asteroid = new Asteroid(asteroidTexture, new Vector2(0, 0));
            _asteroid.Scale = 0.3f;
            _rocket = new Rocket(rocketTexture, new Vector2(100, 300));
            _bullet = new Bullet(bulletTexture, new Vector2(50, 300));
            _robot = new Robot(robotTexture, new Vector2(50, 300));
            _ufo = new Ufo(UFOTexture, new Vector2(50, 300));

            //Controllers
            inputController1.InputGamePadLeftStickListeners.Add(_rocket);
            inputController1.InputGamePadAnalogTriggerListeners.Add(_rocket);

            _spawner.Spawn(_asteroid, sprites);

            //Add sprites
            //_asteroid.Scale = 0.3f;
            //sprites.Add(_asteroid);
            //sprites.Add(_bullet);
            //sprites.Add(_robot);
            //sprites.Add(_ufo);
            _rocket.Scale = 0.2f;
            sprites.Add(_rocket);
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
            if (Keyboard.GetState().IsKeyDown(Keys.A)) _rocket.Rotation -= 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) _rocket.Rotation += 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                var deltaX = Math.Sin(_rocket.Rotation);
                var deltaY = -Math.Cos(_rocket.Rotation);
                Vector2 meh = new Vector2((float)deltaX, (float)deltaY);
                meh = meh * 2f;
                _rocket.Position += meh;
            }

            //if (Keyboard.GetState().IsKeyDown(Keys.Right)) dikkiDinosaurPosition.X++;
            //if (Keyboard.GetState().IsKeyDown(Keys.Up)) dikkiDinosaurPosition.Y--;
            //if (Keyboard.GetState().IsKeyDown(Keys.Down)) dikkiDinosaurPosition.Y++;

            foreach (Sprite sprite in sprites)
            {
                sprite.Update(gameTime);
            }

            CollisionHandler.Instance.Update(gameTime);
            inputController1.Update(gameTime);
            inputController2.Update(gameTime);
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
            spriteBatch.Begin();
            foreach (Sprite sprite in sprites)
            {
                sprite.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}