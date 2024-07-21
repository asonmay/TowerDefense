using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class TowerShop
    {
        public TowerShopItem[] items;

        private Point position;

        public TowerShop(Tower[] towers, Point position, Button buyButton, SpriteFont font, Color backColor, Point buffer)
        {
            items = new TowerShopItem[towers.Length];
            for(int i= 0; i < towers.Length; i++)
            {
                items[i] = new TowerShopItem(towers[i], buyButton, towers[i].Cost, new Point(100 * i + buffer.X + position.X, 50 * i + buffer.Y + position.Y), backColor, font);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < items.Length; i++)
            {
                items[i].Draw(spriteBatch);
            }
        }
    }
}
