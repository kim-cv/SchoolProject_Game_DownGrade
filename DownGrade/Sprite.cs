using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DownGrade
{
    public class Sprite
    {
        public Sprite(Texture2D spriteTexture, Vector2 position)
        {
            this.SpriteTexture = spriteTexture;
            this.Position = position;
            Scale = 1;

        }
        public virtual float Scale { get; set; }
        public Texture2D SpriteTexture { get; set; }
        public virtual Rectangle SourceRectangle { get; set; }

        public virtual float Rotation { get; set; }

        public Vector2 Position
        {
            get { return new Vector2(PositionX, PositionY); }
            set
            {
                PositionX = value.X;
                PositionY = value.Y;
            }
        }
        public float PositionY { get; set; }

        public float PositionX { get; set; }

        public virtual Vector2 Origin { get; set; }

        public virtual SpriteEffects SpriteEffect { get; set; }


        public virtual Rectangle BoundingBox
        {
            get
            {
                Rectangle result;
                Vector2 spritesize;

                if (SourceRectangle.IsEmpty)
                {
                    spritesize = new Vector2(SpriteTexture.Width, SpriteTexture.Height);
                }
                else
                {
                    spritesize = new Vector2(SourceRectangle.Width, SourceRectangle.Height);
                }
                result = new Rectangle((int)PositionX, (int)PositionY, (int)(spritesize.X * Scale), (int)(spritesize.Y * Scale));
                result.Offset((int)(-Origin.X * Scale), (int)(-Origin.Y * Scale));
                return result;
            }
        }

        public virtual void Collide(Sprite s)
        {
            // collision logic
        }

        public virtual void Update(GameTime gameTime)
        {
            // sprite logic goes here...
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteTexture, Position, null, Color.White, Rotation, Origin, new Vector2(Scale, Scale), SpriteEffect, 0f);
        }
    }
}
