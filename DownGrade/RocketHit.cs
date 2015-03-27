using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Security.Policy;
using System.Text;
using DownGrade.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DownGrade
{
    class RocketHit : Sprite
    {
        public string tag = "RocketHit";

        private Rocket _rocket = (Rocket)GameObjectHandler.Instance.FindGameObject("Rocket");

        
        

        public RocketHit(Texture2D spriteTexture, Vector2 position) : base(spriteTexture, position, 0f)
        {
            Origin = new Vector2(32, 32);
        }

        public override void Update(GameTime gameTime)
        {
            Position = _rocket.Position;
            Rotation = _rocket.Rotation;
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
