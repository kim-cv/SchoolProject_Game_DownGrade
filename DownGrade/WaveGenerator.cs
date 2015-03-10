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

namespace DownGrade
{
    class WaveGenerator
    {
        private GameTime gameRef;

        private Random rnd = new Random();

        // Asteroids
        private double _msSinceLastAsteroid;
        private float _asteroidDelay = 1000;

        // Mines
        private double _msSinceLastMine;
        private float _mineDelay = 10000;
        private int _currentMines = 0;
        private int _maxMines = 5;
        
        
        public void Update(GameTime gameTime)
        {
            getGametime(gameTime);

            Asteroids();
            Mines();
        }

        public void Mines()
        {
            if (gameRef.TotalGameTime.TotalMilliseconds > _msSinceLastMine + _mineDelay && _currentMines < _maxMines)
            {
                int x = rnd.Next(8, 1272);
                int y = rnd.Next(8, 712);
                Mine _mine = (Mine) Spawner.Instance.Spawn(Spawner.TypeOfGameObject.Mine, new Vector2(x, y));
                _msSinceLastMine = gameRef.TotalGameTime.TotalMilliseconds;
                _currentMines ++;
            }
        }


        public void Asteroids()
        {
            if (gameRef.TotalGameTime.TotalMilliseconds > _msSinceLastAsteroid + _asteroidDelay)
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

                Asteroid a = (Asteroid) Spawner.Instance.Spawn(Spawner.TypeOfGameObject.AsteroidBig_64, v);
                a.Direction("normal");
                _msSinceLastAsteroid = gameRef.TotalGameTime.TotalMilliseconds;
            }
        }



        private void getGametime(GameTime gameTime)
        {
            gameRef = gameTime;
        }
    }
}
