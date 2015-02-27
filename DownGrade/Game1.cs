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
            //inputController2 = new InputController(PlayerIndex.Two);

            Spawner.Instance.SetGameReference(this, sprites);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Load textures
            Texture2D animatedRocketTexture = Content.Load<Texture2D>("SpaceShip_SpriteSheet_standIn.png");

            //Make gameobjects
            Asteroid _asteroid = (Asteroid)Spawner.Instance.Spawn("Asteroid");
            _asteroid.Scale = 0.3f;

            AnimatedRocket _rocket = (AnimatedRocket)Spawner.Instance.Spawn("AnimatedRocket");
            _rocket.Scale = 0.2f;
            _rocket.Position = new Vector2(100, 250);
            
            //Controllers
            inputController1.InputGamePadLeftStickListeners.Add(_rocket);
            inputController1.InputGamePadAnalogTriggerListeners.Add(_rocket);
            inputController1.InputGamePadButtonListeners.Add(_rocket);
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
            foreach (Sprite sprite in sprites)
            {
                tempSprites.Add(sprite);
            }

            foreach (Sprite sprite in tempSprites)
            {
                sprite.Update(gameTime);
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