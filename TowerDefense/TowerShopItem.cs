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
        private Button buyButton;
        private int cost;
        private Point position;
        private Color backgroundColor;
        private SpriteFont font;

        public TowerShopItem(Tower defaultTower, Button buyButton, int cost, Point position, Color backgroundColor, SpriteFont font)
        {
            DefaultTower = defaultTower;
            this.buyButton = buyButton;
            this.cost = cost;
            this.position = position;
            this.backgroundColor = backgroundColor;
            this.font = font;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float scale = 0 / DefaultTower.Texture.Width;
            spriteBatch.DrawRectangle(new Rectangle(position, new Point(100,50)), backgroundColor);
            spriteBatch.Draw(DefaultTower.Texture, new Vector2(position.X + 5, position.Y + 5), DefaultTower.SourceRectangle, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
            float startingPos = position.X + 10 + DefaultTower.Texture.Width;
            spriteBatch.DrawString(font, $"Cost: {cost}", new Vector2(startingPos, position.Y + 5), Color.Black);
            spriteBatch.DrawString(font, $"Damage: {DefaultTower.Damage}", new Vector2(startingPos, position.Y + 5 + font.MeasureString("L").Y), Color.Black);
            buyButton.Draw(spriteBatch);
        }
    }
}
