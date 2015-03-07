using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DownGrade
{
    public interface Level
    {
        void Initialize();
        void LoadContent();
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
