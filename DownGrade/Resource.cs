﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DownGrade.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DownGrade
{
    class Resource : Sprite
    {

        public Resource(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 1)
        {
            
        }

    }
}
