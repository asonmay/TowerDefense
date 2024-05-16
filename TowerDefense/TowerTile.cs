using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class TowerTile : Tile
    {
        private Tower tower;

        public TowerTile(Vector2 position, Color color, float scale, int rotation, Rectangle sourceRectangle, Vector2 origin, Texture2D texture, Point gridPos, Tower tower)
           : base(position, color, scale, rotation, sourceRectangle, origin, texture, gridPos)
        {
            this.tower = tower;
        }
    }
}
