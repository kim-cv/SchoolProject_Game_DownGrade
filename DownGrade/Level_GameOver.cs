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
    internal class Level_GameOver : Level
    {
        private Game gameReference;

        private SoundEffect soundEffect;

        private KeyboardState _keyState;

        private InputController inputController1;
        private Texture2D background;
        private Texture2D gameOver;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SoundEffect gameOverSoundEffect;
        private SoundEffectInstance gameOverSoundEffectInstance;

        public Level_GameOver(Game gameRef)
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


            gameOverSoundEffect = AudioHandler.Instance.LoadSoundEffect(AudioHandler.TypeOfSound.GameOver_Music);
            gameOverSoundEffectInstance = gameOverSoundEffect.CreateInstance();
            gameOverSoundEffectInstance.Volume = 0.2f;
            gameOverSoundEffectInstance.IsLooped = true;
            gameOverSoundEffectInstance.Play();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(gameReference.GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Load textures
            background = gameReference.Content.Load<Texture2D>("Background_1280x720.png");
            gameOver = gameReference.Content.Load<Texture2D>("Gameover.png");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public void UnloadContent()
        {
            gameOverSoundEffectInstance.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed ||
                    Keyboard.GetState().IsKeyDown(Keys.Enter))
                LevelHandler.Instance.LoadLevel(LevelHandler.TypeOfLevel.MainScreen);

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

            spriteBatch.Draw(background, new Vector2(0, 0), null, Color.White, 0f,
                new Vector2(0, 0), 1f, SpriteEffects.None, 0.9f);
            spriteBatch.Draw(gameOver, new Vector2((1280 / 2) - (gameOver.Width / 2), 300), null, Color.White, 0f,
                new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

            spriteBatch.End();
        }
    }
}