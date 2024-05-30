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
        public Vector2 position;
        private Color color;
        private float scale;
        private int rotation;
        private Rectangle sourceRectangle;
        private Vector2 origin;
        private Texture2D texture;
        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)(sourceRectangle.Width * scale), (int)(sourceRectangle.Height * scale));
            }
        }

        public Sprite(Vector2 position, Color color, float scale, int rotation, Rectangle sourceRectangle, Vector2 origin, Texture2D texture)
        {
            this.position = position;
            this.color = color;
            this.scale = scale;
            this.rotation = rotation;
            this.sourceRectangle = sourceRectangle;
            this.origin = origin;
            this.texture = texture;
        }

        public Sprite(Vector2 position, float scale, Texture2D texture)
            : this(position, Color.White, 1, 0, new Rectangle(0, 0, texture.Width, texture.Height), Vector2.Zero, texture) { }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, SpriteEffects.None, 1);
        }
    }
}
