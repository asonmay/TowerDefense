using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class Button
    {
        private Point size;
        private Color color;
        private string text;
        private Point position;
        private SpriteFont font;
        private Color textColor;

        public Button( Color color, string text, Point position, SpriteFont font, Color textColor)
        {
            size = font.MeasureString(text).ToPoint();
            this.color = color;
            this.text = text;
            this.position = position;
            this.font = font;
            this.textColor = textColor;
        }

        public bool isClicked()
        {
            Rectangle mouseHitbox = new Rectangle(Mouse.GetState().Position, new Point(1, 1));
            bool isClicked = mouseHitbox.Intersects(new Rectangle(position, size));
            return isClicked;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(new Rectangle(position, size), color);
            spriteBatch.DrawString(font, text, position.ToVector2(), textColor);
        }
    }
}
