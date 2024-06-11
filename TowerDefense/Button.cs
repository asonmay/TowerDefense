using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
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
            size = new Point((int)font.MeasureString(text).X + 10, (int)font.MeasureString(text).Y + 10);
            this.color = color;
            this.text = text;
            this.position = position;
            this.font = font;
            this.textColor = textColor;
        }

        public bool isClicked()
        {
            Rectangle mouseHitbox = new Rectangle(Mouse.GetState().Position, new Point(1, 1));
            bool isClicked = mouseHitbox.Intersects(new Rectangle(position, size)) && Mouse.GetState().LeftButton == ButtonState.Pressed;
            return isClicked;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(new Rectangle(position, size), color);
            spriteBatch.DrawString(font, text, new Vector2(position.X + 5, position.Y + 5), textColor);
        }
    }
}
