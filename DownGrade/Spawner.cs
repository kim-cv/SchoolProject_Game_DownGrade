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
        private static readonly Spawner _instance = new Spawner();
        public static Spawner _Instance
        {
            get { return _instance; }
        }


        Random rnd = new Random();

        public void Spawn(String typeOfObject, Texture2D texture, List<Sprite> listOfObjects)
        {
            float xPosition = rnd.Next(100, 500);
            float yPosition = 0;

            Sprite gameObject = InstanciateGameObject(typeOfObject, texture);

            if (gameObject != null)
            {
                gameObject.Position = new Vector2(xPosition, yPosition);
                listOfObjects.Add(gameObject);
            }
            else
            {
                //Error log?
            }
        }

        private Sprite InstanciateGameObject(string typeOfObject, Texture2D texture)
        {
            switch (typeOfObject)
            {
                case "Asteroid":
                    {
                        return new Asteroid(texture, new Vector2());
                    }
                case "Bullet":
                    {
                        return new Bullet(texture, new Vector2());
                    }
                case "Robot":
                    {
                        return new Robot(texture, new Vector2());
                    }
                case "Rocket":
                    {
                        return new Rocket(texture, new Vector2());
                    }
                case "AnimatedRocket":
                    {
                        return new AnimatedRocket(texture, new Vector2());
                    }
                case "Ufo":
                    {
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
