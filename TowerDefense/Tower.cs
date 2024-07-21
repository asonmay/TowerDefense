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
        private Point gridPos;
        private int range;
        public int Cost;
        public int Damage;
        private List<Projectile> projectiles;

        public Tower(Vector2 position, Color color, float scale, int rotation, Rectangle sourceRectangle, Vector2 origin, Texture2D texture, int cost)
            : base(position, color, scale, rotation, sourceRectangle, origin, texture)
        {
            this.Cost = cost;
        }

        protected void Attack(Enemy[] enemies)
        {
            for(int i = 0; i < enemies.Length; i++)
            {
                if (Math.Abs(enemies[i].GridPos.X - gridPos.X) + Math.Abs(enemies[i].GridPos.Y - gridPos.Y) <= range)
                {
                    enemies[i].Health -= Damage;
                    LaunchProjectile();
                }
            }
        }

        private void LaunchProjectile()
        {
            projectiles.Add(new Projectile());
        }
    }
}
