using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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
        SoundEffect soundEffect;
        private Level level;
        private bool isLevelChanging;

        private InputController inputController1;
        private InputController inputController2;
        private Texture2D backgroundTexture;
        private Texture2D machinegun;
        private Texture2D laser;
        private Texture2D weapons;

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
            
            
            LevelHandler.Instance.SetGameReference(this);
            
            LevelHandler.Instance.ListOfLevels.Add(new Level_MainScreen(this));
            LevelHandler.Instance.ListOfLevels.Add(new Level_Game(this));

            level = LevelHandler.Instance.ListOfLevels[0];
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

            if (!isLevelChanging) { 
            level.Initialize();
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            if (!isLevelChanging)
            {
                level.LoadContent();
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            if (!isLevelChanging)
            {
                level.UnloadContent();
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!isLevelChanging)
            {
                level.Update(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (!isLevelChanging)
            {
                level.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public void LoadLevel(Level level)
        {
            //Unload current level thingies
            UnloadContent();

            //Stop things which are dependent on current level, like Update and Draw
            isLevelChanging = true;

            //Change level reference
            this.level = level;

            //Load new level content
            level.LoadContent();

            //Start things which are dependent on current level, like Update and Draw
            isLevelChanging = false;
        }
    }
}