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

        public TowerShop(Tower[] towers, Point position, SpriteFont font, Color backColor)
        {
            items = new TowerShopItem[towers.Length];
            for(int i= 0; i < towers.Length; i++)
            {
                Point itemPos = new Point(position.X, (65 * i) + position.Y);
                items[i] = new TowerShopItem(towers[i], towers[i].Cost, itemPos, backColor, font);
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
