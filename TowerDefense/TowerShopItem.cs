using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class TowerShopItem
    {
        public Tower DefaultTower;
        public Button BuyButton;
        private int cost;
        private Point position;
        private Color backgroundColor;
        private SpriteFont font;

        public TowerShopItem(Tower defaultTower, int cost, Point position, Color backgroundColor, SpriteFont font)
        {
            DefaultTower = defaultTower;
            this.cost = cost;
            this.position = position;
            this.backgroundColor = backgroundColor;
            this.font = font;
            BuyButton = new Button(Color.Black, "buy", new Point((int)(position.X + DefaultTower.Texture.Width * DefaultTower.Scale + 20), (int)(position.Y + font.MeasureString("L").Y * 2 + 5)), font, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float scale = DefaultTower.Scale;
            spriteBatch.FillRectangle(new Rectangle(position, new Point(100,60)), backgroundColor);
            spriteBatch.Draw(DefaultTower.Texture, new Vector2(position.X + 5, position.Y + 5), DefaultTower.SourceRectangle, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
            float startingPos = position.X + 10 + (DefaultTower.Texture.Width * scale);
            spriteBatch.DrawString(font, $"Cost: {cost}", new Vector2(startingPos, position.Y + 5), Color.Black);
            BuyButton.Draw(spriteBatch);
        }
    }
}
