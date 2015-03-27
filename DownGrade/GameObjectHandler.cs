using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DownGrade
{
    public class GameObjectHandler
    {
        private List<Sprite> ListOfGameObjects = new List<Sprite>();

        private static readonly GameObjectHandler _instance = new GameObjectHandler();
        public static GameObjectHandler Instance
        {
            get { return _instance; }
        }

        public void AddGameObject(Sprite gameObject)
        {
            ListOfGameObjects.Add(gameObject);
        }

        public bool RemoveGameObject(Sprite gameObject)
        {
            return ListOfGameObjects.Remove(gameObject);
        }

        public List<Sprite> GetListOfGameObjects()
        {
            return ListOfGameObjects;
        }

        public Sprite FindGameObject(string gameObjectTag)
        {
            foreach (Sprite o in ListOfGameObjects)
            {
                if (o.GetType().ToString() == "DownGrade."+gameObjectTag)
                {
                    return o;
                }
            }
            return null;
        }

        public int FindGameObjectProperty(string gameObjectTag)
        {
            foreach (Sprite o in ListOfGameObjects)
            {
                if (o.GetType().ToString() == "DownGrade.Rocket")
                {
                    return (int) o.GetType().GetProperty("Experience").GetValue(o, null);
                }
            }
            return 0;
        }

        public void SetGameObjectProperty(string gameObjectTag, int value)
        {
            foreach (Sprite o in ListOfGameObjects)
            {
                if (o.GetType().ToString() == "DownGrade.Rocket")
                {
                    o.GetType().GetProperty("Experience").SetValue(o, value, null);
                }
            }
        }

        public void UnloadContent()
        {
            ListOfGameObjects.Clear();
        }
    }
}
