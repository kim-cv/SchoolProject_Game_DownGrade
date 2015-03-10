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
    internal class Level_Instructions : Level
    {
        private Game gameReference;

        private SoundEffect soundEffect;

        private KeyboardState _keyState;

        private InputController inputController1;
        private Texture2D Instructions;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SoundEffect titleSoundEffect;
        private SoundEffectInstance titleSoundEffectInstance;

        public Level_Instructions(Game gameRef)
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
            Instructions = gameReference.Content.Load<Texture2D>("Instructions.png");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public void UnloadContent()
        {
            titleSoundEffectInstance.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                    Keyboard.GetState().IsKeyDown(Keys.Escape))
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

            spriteBatch.Draw(Instructions, new Vector2(0, 0), null, Color.White, 0f,
                new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

            spriteBatch.End();
        }
    }
}