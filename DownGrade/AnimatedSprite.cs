using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DownGrade
{
    class AnimatedSprite : Sprite
    {
        private double _milisecondsSinceLastFrameUpdate = 0;
        private int _currentFrame = 0;
        private List<Rectangle> _frames = new List<Rectangle>();
        private bool _loop;
        private int _delay;

        public bool Loop
        {
            get { return _loop; }
            set { _loop = value; }
        }

        public int Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        public List<Rectangle> Frames
        {
            get { return _frames; }
            set { _frames = value; }
        }

        public int CurrentFrame
        {
            get { return _currentFrame; }
            set { _currentFrame = value; }
        }

        public AnimatedSprite(Texture2D spriteTexture, Vector2 position, float layer)
            : base(spriteTexture, position, 0)
        {
            this.SpriteTexture = spriteTexture;
            this.Position = position;
            Scale = 1;
            Layer = layer;
        }


        public override void Update(GameTime gameTime)
        {
            if (_frames.Count > 0)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > _milisecondsSinceLastFrameUpdate + Delay)
                {
                    SourceRectangle = NextFrame();
                    _milisecondsSinceLastFrameUpdate = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Do we have a texture? If not then there is nothing to draw...
            if (SpriteTexture != null)
            {
                // Has a source rectangle been set?
                if (SourceRectangle.IsEmpty)
                {
                    // No, so draw the entire sprite texture
                    spriteBatch.Draw(SpriteTexture, Position, null, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0f);
                }
                else
                {
                    // Yes, so just draw the specified SourceRect
                    spriteBatch.Draw(SpriteTexture, Position, SourceRectangle, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0f);
                }
            }
        }
        private Rectangle NextFrame()
        {
            if (_currentFrame == _frames.Count - 1 && _loop) _currentFrame = 0;
            else if (_currentFrame < _frames.Count - 1) _currentFrame++;
            return Frames[_currentFrame];
        }
    }
}
