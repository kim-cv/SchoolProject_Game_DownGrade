using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Xna.Framework;

namespace DownGrade
{
    class LevelHandler
    {
        private Game1 GameReference;

        public List<Level> ListOfLevels = new List<Level>();
        public enum TypeOfLevel
        {
            MainScreen,
            Game,
            Instructions
        }


        private static readonly LevelHandler _instance = new LevelHandler();
        public static LevelHandler Instance
        {
            get { return _instance; }
        }

        private LevelHandler() { }

        public void SetGameReference(Game1 gameref)
        {
            GameReference = gameref;
        }

        public void LoadLevel(TypeOfLevel typeOflevel)
        {
            foreach (Level level in ListOfLevels)
            {
                if (level.GetType().ToString() == "DownGrade.Level_"+typeOflevel)
                {
                    GameReference.LoadLevel(level);
                    break;
                }
            }
        }
    }
}
