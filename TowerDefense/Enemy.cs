using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class Enemy : Sprite
    {
        public int health { get; set; }
        public Point gridPos { get; set; }
        public int speed;

        public Enemy(int speed, int health, Point gridPos, Vector2 position, Color color, float scale, int rotation, Rectangle sourceRectangle, Vector2 origin, Texture2D texture)
            : base(position, color, scale, rotation, sourceRectangle, origin, texture)
        {
            this.speed = speed;
            this.health = health;
            this.gridPos = gridPos;
        }

        public void Move()
        {

        }

        public void Update()
        {

        }

        public void Draw()
        {

        }
    }
}
