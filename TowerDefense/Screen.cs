﻿using Microsoft.Xna.Framework;
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
        protected SpriteFont buttonFont;
        protected Texture2D spriteSheet;
        protected Dictionary<PathTileType, Rectangle> sourceRectangles;
        protected TileMapProfile[] savedMaps;
        protected Texture2D background;

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

        public abstract Type Update();

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
