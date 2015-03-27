﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DownGrade
{
    class MissileLost : Sprite
    {
        public string tag = "MissileLost";



        public MissileLost(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position, 0f)
        {
            
        }

        public void Show()
        {
            Layer = 1f;
        }

        public void Hide()
        {
            Layer = 0f;
        }
    }
}