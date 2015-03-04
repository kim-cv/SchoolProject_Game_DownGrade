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
        public enum TypeOfGameObject
        {
            AsteroidBig_Explosion_64,
            AsteroidBig_64,
            AsteroidBig1,
            AsteroidBig2,
            AsteroidMedium1,
            AsteroidMedium2,
            AsteroidSmall1,
            AsteroidSmall2,
            Bullet,
            Robot,
            Rocket,
            Ufo,
            UI,
            Healthbar,
            Shieldbar
        }

        private Game GameReference = null;
        private Random rnd = new Random();

        private static readonly Spawner _instance = new Spawner();
        public static Spawner Instance
        {
            get { return _instance; }
        }


        public void SetGameReference(Game reference, List<Sprite> listOfSprites)
        {
            GameReference = reference;
        }

        public void SetGameReference(Game reference)
        {
            GameReference = reference;
        }


        public Sprite Spawn(TypeOfGameObject typeOfObject)
        {
            float xPosition = rnd.Next(100, 500);
            float yPosition = 0;

            Sprite gameObject = InstanciateGameObject(typeOfObject);

            if (gameObject != null)
            {
                gameObject.Position = new Vector2(xPosition, yPosition);
                GameObjectHandler.Instance.AddGameObject(gameObject);
            }
            else
            {
                //Error log?
            }

            return gameObject;
        }


        private Sprite InstanciateGameObject(TypeOfGameObject typeOfObject)
        {
            Texture2D texture;
            switch (typeOfObject)
            {
                case TypeOfGameObject.AsteroidBig_64:
                    {
                        texture = GameReference.Content.Load<Texture2D>("Asteroid_64x64.png");
                        return new Asteroid(texture, new Vector2());
                    }
                case TypeOfGameObject.AsteroidBig_Explosion_64:
                    {
                        texture = GameReference.Content.Load<Texture2D>("Asteroid_Explosion_64x64.png");
                        return new Asteroid_Explosion(texture, new Vector2());
                    }
                case TypeOfGameObject.AsteroidBig1:
                    {
                        texture = GameReference.Content.Load<Texture2D>("meteorGrey_big1.png");
                        return new Asteroid(texture, new Vector2());
                    }
                case TypeOfGameObject.AsteroidBig2:
                    {
                        texture = GameReference.Content.Load<Texture2D>("meteorGrey_big2.png");
                        return new Asteroid(texture, new Vector2());
                    }
                case TypeOfGameObject.AsteroidMedium1:
                    {
                        texture = GameReference.Content.Load<Texture2D>("meteorGrey_med1.png");
                        return new Asteroid(texture, new Vector2());
                    }
                case TypeOfGameObject.AsteroidMedium2:
                    {
                        texture = GameReference.Content.Load<Texture2D>("meteorGrey_med2.png");
                        return new Asteroid(texture, new Vector2());
                    }
                case TypeOfGameObject.AsteroidSmall1:
                    {
                        texture = GameReference.Content.Load<Texture2D>("meteorGrey_small1.png");
                        return new Asteroid(texture, new Vector2());
                    }
                case TypeOfGameObject.AsteroidSmall2:
                    {
                        texture = GameReference.Content.Load<Texture2D>("meteorGrey_small2.png");
                        return new Asteroid(texture, new Vector2());
                    }
                case TypeOfGameObject.Bullet:
                    {
                        texture = GameReference.Content.Load<Texture2D>("laserGreen10.png");
                        return new Bullet(texture, new Vector2());
                    }
                case TypeOfGameObject.Robot:
                    {
                        texture = GameReference.Content.Load<Texture2D>("robot.png");
                        return new Robot(texture, new Vector2());
                    }
                case TypeOfGameObject.Rocket:
                    {
                        texture = GameReference.Content.Load<Texture2D>("playerShip1_red.png");
                        return new Rocket(texture, new Vector2());
                    }
                case TypeOfGameObject.Ufo:
                    {
                        texture = GameReference.Content.Load<Texture2D>("UFO.png");
                        return new Ufo(texture, new Vector2());
                    }
                case TypeOfGameObject.UI:
                    {
                        texture = GameReference.Content.Load<Texture2D>("UI.png");
                        return new UI(texture, new Vector2());
                    }
                case TypeOfGameObject.Healthbar:
                    {
                        texture = GameReference.Content.Load<Texture2D>("HealthFill.png");
                        return new Healthbar(texture, new Vector2());
                    }
                case TypeOfGameObject.Shieldbar:
                    {
                        texture = GameReference.Content.Load<Texture2D>("ShieldFill.png");
                        return new Shieldbar(texture, new Vector2());
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
