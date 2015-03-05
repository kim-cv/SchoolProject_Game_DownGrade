using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DownGrade.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DownGrade
{
    class UI : Sprite
    {
        public UI(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0.99f)
        {

        }
    }
}
