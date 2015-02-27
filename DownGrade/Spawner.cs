using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DownGrade
{
    public class Spawner
    {
        private Game GameReference = null;
        List<Sprite> listOfObjects = null;
        private Random rnd = new Random();

        private static readonly Spawner _instance = new Spawner();
        public static Spawner Instance
        {
            get { return _instance; }
        }


        public void SetGameReference(Game reference, List<Sprite> listOfSprites)
        {
            GameReference = reference;
            listOfObjects = listOfSprites;
        }


        public Sprite Spawn(String typeOfObject)
        {
            float xPosition = rnd.Next(100, 500);
            float yPosition = 0;

            Sprite gameObject = InstanciateGameObject(typeOfObject);

            if (gameObject != null)
            {
                gameObject.Position = new Vector2(xPosition, yPosition);
                listOfObjects.Add(gameObject);
            }
            else
            {
                //Error log?
            }

            return gameObject;
        }

        private Sprite InstanciateGameObject(string typeOfObject)
        {
            Texture2D texture;
            switch (typeOfObject)
            {
                case "Asteroid":
                    {
                        texture = GameReference.Content.Load<Texture2D>("meteorGrey_big2.png");
                        return new Asteroid(texture, new Vector2());
                    }
                case "Bullet":
                    {
                        texture = GameReference.Content.Load<Texture2D>("laserGreen10.png");
                        return new Bullet(texture, new Vector2());
                    }
                case "Robot":
                    {
                        texture = GameReference.Content.Load<Texture2D>("robot.png");
                        return new Robot(texture, new Vector2());
                    }
                case "Rocket":
                    {
                        texture = GameReference.Content.Load<Texture2D>("playerShip1_red.png");
                        return new Rocket(texture, new Vector2());
                    }
                case "AnimatedRocket":
                    {
                        texture = GameReference.Content.Load<Texture2D>("SpaceShip_SpriteSheet_standIn.png");
                        return new AnimatedRocket(texture, new Vector2());
                    }
                case "Ufo":
                    {
                        texture = GameReference.Content.Load<Texture2D>("UFO.png");
                        return new Ufo(texture, new Vector2());
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
