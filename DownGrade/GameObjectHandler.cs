using System;
using System.Collections.Generic;
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

        //public void FindGameObject(string gameObjectTag)
        //{
        //    //ListOfGameObjects.Add(gameObject);
        //}

    }
}
