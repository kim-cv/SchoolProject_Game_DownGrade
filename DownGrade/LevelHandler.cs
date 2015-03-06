using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DownGrade
{
    class LevelHandler
    {
        public List<Level> ListOfLevels = new List<Level>();

        private static readonly LevelHandler _instance = new LevelHandler();
        public static LevelHandler Instance
        {
            get { return _instance; }
        }
    }
}
