using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public abstract class Sprite
    {
        public Vector2 Position;
        private Color color;
        public float Scale;
        private int rotation;
        public Rectangle SourceRectangle;
        private Vector2 origin;
        public Texture2D Texture;
        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(SourceRectangle.Width * Scale), (int)(SourceRectangle.Height * Scale));
            }
        }

        public Sprite(Vector2 position, Color color, float scale, int rotation, Rectangle sourceRectangle, Vector2 origin, Texture2D texture)
        {
            Position = position;
            this.color = color;
            Scale = scale;
            this.rotation = rotation;
            SourceRectangle = sourceRectangle;
            this.origin = origin;
            Texture = texture;
        }

        public Sprite(Vector2 position, float scale, Texture2D texture)
            : this(position, Color.White, 1, 0, new Rectangle(0, 0, texture.Width, texture.Height), Vector2.Zero, texture) { }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRectangle, color, rotation, origin, Scale, SpriteEffects.None, 1);
        }
    }
}
