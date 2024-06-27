using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public abstract class Screen
    {
        public SpriteFont buttonFont;
        public Texture2D spriteSheet;
        public Dictionary<PathTileType, Rectangle> sourceRectangles;
        public Texture2D background;
        public Viewport viewPort;

        public bool IsClicked(Rectangle rect)
        {
            bool isMouseClicked = Mouse.GetState().LeftButton == ButtonState.Pressed;

            if (DoesHovorRect(rect) && isMouseClicked)
            {
                return true;
            }
            return false;
        }

        public bool DoesHovorRect(Rectangle rect)
        {
            Point mousePos = Mouse.GetState().Position;
            Rectangle mouseHitbox = new Rectangle(mousePos, new Point(1, 1));

            if (rect.Intersects(mouseHitbox))
            {
                return true;
            }

            return false;
        }

        public abstract ScreenTypes ReturnType();

        public abstract ScreenTypes Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
