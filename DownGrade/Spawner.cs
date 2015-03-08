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
            BulletRed,
            Robot,
            Rocket,
            Ufo,
            UI,
            Healthbar,
            Shieldbar,
            Laser,
            Player_Explosion,
            Mine
        }

        private Game GameReference = null;
        public Random rnd = new Random();

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

        public Sprite Spawn(TypeOfGameObject typeOfObject, Vector2 pos)
        {
            Sprite gameObject = InstanciateGameObject(typeOfObject, pos);

            if (gameObject != null)
            {
                GameObjectHandler.Instance.AddGameObject(gameObject);
            }
            else
            {
                //Error log?
            }

            return gameObject;
        }


        private Sprite InstanciateGameObject(TypeOfGameObject typeOfObject, Vector2 pos=default(Vector2))
        {
            Texture2D texture;
            switch (typeOfObject)
            {
                case TypeOfGameObject.AsteroidBig_64:
                    {
                        texture = GameReference.Content.Load<Texture2D>("Asteroid_64x64.png");
                        return new Asteroid(texture, pos);
                    }
                case TypeOfGameObject.AsteroidBig_Explosion_64:
                    {
                        texture = GameReference.Content.Load<Texture2D>("Asteroid_Explosion_64x64.png");
                        return new Asteroid_Explosion(texture, pos);
                    }
                case TypeOfGameObject.AsteroidBig1:
                    {
                        texture = GameReference.Content.Load<Texture2D>("meteorGrey_big1.png");
                        return new Asteroid(texture, pos);
                    }
                case TypeOfGameObject.AsteroidBig2:
                    {
                        texture = GameReference.Content.Load<Texture2D>("meteorGrey_big2.png");
                        return new Asteroid(texture, pos);
                    }
                case TypeOfGameObject.AsteroidMedium1:
                    {
                        texture = GameReference.Content.Load<Texture2D>("meteorGrey_med1.png");
                        return new Asteroid(texture, pos);
                    }
                case TypeOfGameObject.AsteroidMedium2:
                    {
                        texture = GameReference.Content.Load<Texture2D>("meteorGrey_med2.png");
                        return new Asteroid(texture, pos);
                    }
                case TypeOfGameObject.AsteroidSmall1:
                    {
                        texture = GameReference.Content.Load<Texture2D>("meteorGrey_small1.png");
                        return new Asteroid(texture, pos);
                    }
                case TypeOfGameObject.AsteroidSmall2:
                    {
                        texture = GameReference.Content.Load<Texture2D>("meteorGrey_small2.png");
                        return new Asteroid(texture, pos);
                    }
                case TypeOfGameObject.Bullet:
                    {
                        texture = GameReference.Content.Load<Texture2D>("Bullet_green.png");
                        return new Bullet(texture, pos);
                    }
                case TypeOfGameObject.BulletRed:
                    {
                        texture = GameReference.Content.Load<Texture2D>("Bullet_red.png");
                        return new Bullet(texture, pos);
                    }
                case TypeOfGameObject.Laser:
                    {
                        texture = GameReference.Content.Load<Texture2D>("Laser.png");
                        return new Laser(texture, pos);
                    }
                case TypeOfGameObject.Robot:
                    {
                        texture = GameReference.Content.Load<Texture2D>("robot.png");
                        return new Robot(texture, pos);
                    }
                case TypeOfGameObject.Rocket:
                    {
                        texture = GameReference.Content.Load<Texture2D>("Player_1_64x64.png");
                        return new Rocket(texture, pos);
                    }
                case TypeOfGameObject.Ufo:
                    {
                        texture = GameReference.Content.Load<Texture2D>("UFO.png");
                        return new Ufo(texture, pos);
                    }
                case TypeOfGameObject.UI:
                    {
                        texture = GameReference.Content.Load<Texture2D>("UI_1280x720.png");
                        return new UI(texture, pos);
                    }
                case TypeOfGameObject.Healthbar:
                    {
                        texture = GameReference.Content.Load<Texture2D>("HealthFill.png");
                        return new Healthbar(texture, pos);
                    }
                case TypeOfGameObject.Shieldbar:
                    {
                        texture = GameReference.Content.Load<Texture2D>("ShieldFill.png");
                        return new Shieldbar(texture, pos);
                    }
                case TypeOfGameObject.Player_Explosion:
                    {
                        texture = GameReference.Content.Load<Texture2D>("Ship_Explosion_96x96.png");
                        return new Player_Explosion(texture, pos);
                    }   
                case TypeOfGameObject.Mine:
                    {
                        texture = GameReference.Content.Load<Texture2D>("Mine.png");
                        return new Mine(texture, pos);
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
