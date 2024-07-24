using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class Projectile : Sprite
    {
        private int speed;
        private int damage;
        private Enemy target;
        private int currentLerpIndex;
        Vector2[] lerp;

        public Projectile(int speed, int damage, Vector2 postion, float scale, Texture2D texture, Enemy target)
            : base(postion, Color.White, scale, 0, new Rectangle(0, 0, texture.Width, texture.Height), Vector2.Zero, texture)
        {
            this.speed = speed;
            this.damage = damage;
            this.target = target;
            lerp = Lerp(Position, target.Position, speed);
            currentLerpIndex = 0;
        }

        private Vector2[] Lerp(Vector2 start, Vector2 end, int amount)
        {
            Vector2[] lerp = new Vector2[amount];

            Vector2 section = new Vector2((end.X - start.X) / amount, (end.Y - start.Y) / amount);
            for(int i = 0; i < amount; i++)
            {
                float x = start.X + section.X * i;
                float y = start.Y + section.Y * i;
                lerp[i] = new Vector2(x, y);
            }

            return lerp;
        }

        public bool Update()
        {
            if(currentLerpIndex >= lerp.Length)
            {
                target.Health -= damage;
                return true;
            }

            Position = lerp[currentLerpIndex];
            currentLerpIndex++;

            return false;
        }
    }
}
