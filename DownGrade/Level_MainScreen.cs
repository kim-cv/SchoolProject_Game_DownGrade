using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DownGrade
{
    internal class Level_MainScreen : Level
    {
        private Game gameReference;

        private SoundEffect soundEffect;

        private KeyboardState _keyState;

        private InputController inputController1;
        private Texture2D Start;
        private Texture2D Start_Marked;
        private bool isStartSelected = true;
        private Texture2D Settings;
        private Texture2D Settings_Marked;
        private bool isSettingsSelected;
        private Texture2D Exit;
        private Texture2D Exit_Marked;
        private bool isExitSelected;
        private Texture2D backgroundTexture;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SoundEffect titleSoundEffect;
        private SoundEffectInstance titleSoundEffectInstance;

        public Level_MainScreen(Game gameRef)
            : base()
        {
            gameReference = gameRef;

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


            titleSoundEffect = AudioHandler.Instance.LoadSoundEffect(AudioHandler.TypeOfSound.Title_Music);
            titleSoundEffectInstance = titleSoundEffect.CreateInstance();
            titleSoundEffectInstance.Volume = 0.2f;
            titleSoundEffectInstance.IsLooped = true;
            titleSoundEffectInstance.Play();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(gameReference.GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Load textures
            backgroundTexture = gameReference.Content.Load<Texture2D>("TitleScreen.png");
            Exit = gameReference.Content.Load<Texture2D>("Exit.png");
            Exit_Marked = gameReference.Content.Load<Texture2D>("Exit - Marked.png");
            Settings = gameReference.Content.Load<Texture2D>("Settings.png");
            Settings_Marked = gameReference.Content.Load<Texture2D>("Settings - Marked.png");
            Start = gameReference.Content.Load<Texture2D>("Startgame.png");
            Start_Marked = gameReference.Content.Load<Texture2D>("Startgame - Marked.png");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            isStartSelected = true;
            isSettingsSelected = false;
            isExitSelected = false;

            titleSoundEffectInstance.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            //    Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    gameReference.Exit();


            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.S) && _keyState.IsKeyUp(Keys.S))
            {
                if (isStartSelected)
                {
                    isStartSelected = false;
                    isSettingsSelected = true;
                    isExitSelected = false;
                }
                else if (isSettingsSelected)
                {
                    isStartSelected = false;
                    isSettingsSelected = false;
                    isExitSelected = true;
                }
                else if (isExitSelected)
                {
                    isStartSelected = true;
                    isSettingsSelected = false;
                    isExitSelected = false;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W) && _keyState.IsKeyUp(Keys.W))
            {
                if (isStartSelected)
                {
                    isStartSelected = false;
                    isSettingsSelected = false;
                    isExitSelected = true;
                }
                else if (isSettingsSelected)
                {
                    isStartSelected = true;
                    isSettingsSelected = false;
                    isExitSelected = false;
                }
                else if (isExitSelected)
                {
                    isStartSelected = false;
                    isSettingsSelected = true;
                    isExitSelected = false;
                }                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _keyState.IsKeyUp(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
            {
                if (isStartSelected)
                {
                    LevelHandler.Instance.LoadLevel(LevelHandler.TypeOfLevel.Game);
                }
                else if (isSettingsSelected)
                {
                    
                }
                else if (isExitSelected)
                {
                    gameReference.Exit();
                }                
            }

            _keyState = Keyboard.GetState();
            inputController1.Update(gameTime);
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

            spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), new Rectangle(0, 0, 1280, 720), Color.White, 0f,
                new Vector2(0, 0), 1f, SpriteEffects.None, 0.1f);


            if (isStartSelected)
            {
                spriteBatch.Draw(Start_Marked, new Vector2((1280 / 2) - (Start_Marked.Width / 2), 300), null, Color.White, 0f,
                    new Vector2(0, 0), 1f, SpriteEffects.None, 0.2f);
            }
            else
            {
                spriteBatch.Draw(Start, new Vector2((1280 / 2) - (Start.Width / 2), 300), null, Color.White, 0f,
                    new Vector2(0, 0), 1f, SpriteEffects.None, 0.2f);
            }

            if (isSettingsSelected)
            {
                spriteBatch.Draw(Settings_Marked, new Vector2((1280 / 2) - (Settings_Marked.Width / 2), 400), null, Color.White, 0f,
                    new Vector2(0, 0), 1f, SpriteEffects.None, 0.2f);
            }
            else
            {
                spriteBatch.Draw(Settings, new Vector2((1280 / 2) - (Settings.Width / 2), 400), null, Color.White, 0f,
                    new Vector2(0, 0), 1f, SpriteEffects.None, 0.2f);
            }
            if (isExitSelected)
            {
                spriteBatch.Draw(Exit_Marked, new Vector2((1280 / 2) - (Exit_Marked.Width / 2), 500), null, Color.White, 0f,
                    new Vector2(0, 0), 1f, SpriteEffects.None, 0.2f);
            }
            else
            {
                spriteBatch.Draw(Exit, new Vector2((1280 / 2) - (Exit.Width / 2), 500), null, Color.White, 0f,
                    new Vector2(0, 0), 1f, SpriteEffects.None, 0.2f);
            }
            spriteBatch.End();
        }
    }
}