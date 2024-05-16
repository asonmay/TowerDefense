using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class Tower : Sprite
    {
        private int health;
        private int maxHealth;

        public Tower(Vector2 position, Color color, float scale, int rotation, Rectangle sourceRectangle, Vector2 origin, Texture2D texture)
            : base(position,color,scale,rotation,sourceRectangle,origin,texture)
        {

        }
    }
}
