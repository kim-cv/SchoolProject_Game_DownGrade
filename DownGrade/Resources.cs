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
    class Resources : Sprite
    {
        public static int maxhealth;
        public static int currenthealth;
        public static int maxshield;
        public static int currentshield;

        private Texture2D texture;


        public Resources(Texture2D spriteTexture, Vector2 position)
            : base(spriteTexture, position)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public static void Hit(int damage)
        {

        }
    }
}
