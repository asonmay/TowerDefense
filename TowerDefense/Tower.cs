using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class Tower : Sprite
    {
        private Point GridPos;
        private int range;
        public int Cost;
        public int Damage;
        private List<Projectile> projectiles;
        private Texture2D projectileTexture;
        private float projectileScale;

        public Tower(Point gridPos, float scale, Rectangle sourceRectangle, Texture2D texture, int cost, int damage, int range, Texture2D projectileTexture, float projectileScale, Vector2 tileSize, Vector2 MapPosition)
            : base(new Vector2(MapPosition.X + tileSize.X * gridPos.X, MapPosition.Y + tileSize.Y * gridPos.Y), Color.White, scale, 0, sourceRectangle, Vector2.Zero, texture)
        {
            Cost = cost;
            GridPos = gridPos;
            Damage = damage;
            this.range = range;
            this.projectileScale = projectileScale;
            this.projectileTexture = projectileTexture;
            projectiles = new List<Projectile>();
        }

        protected void Attack(Enemy[] enemies)
        {
            for(int i = 0; i < enemies.Length; i++)
            {
                if (Math.Abs(enemies[i].GridPos.X - GridPos.X) + Math.Abs(enemies[i].GridPos.Y - GridPos.Y) <= range)
                {
                    LaunchProjectile(enemies[i]);
                }
            }
        }

        private void LaunchProjectile(Enemy enemy)
        {
            projectiles.Add(new Projectile(12, 10, Position, projectileScale, projectileTexture, enemy));
        }

        public void Update(Enemy[] enemies)
        {
            Attack(enemies);

            for(int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i].Update())
                {
                    projectiles.RemoveAt(i);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw(spriteBatch);
            }
        }
    }
}
