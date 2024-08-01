using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class ShooterTower : Tower
    {
        private int range;
        public int Damage;
        private List<Projectile> projectiles;
        private Texture2D projectileTexture;
        private float projectileScale;
        private int projectileSpeed;

        public ShooterTower(int range, int damage, Texture2D projectileTexture, float projectileScale, int projectileSpeed, Point gridPos, TimeSpan actionRate, Texture2D texture, Point tileSize, float scale, Vector2 mapPostion, int cost)
            : base(gridPos, scale, texture, tileSize.ToVector2(), mapPostion, actionRate, cost)
        {
            this.range = range;
            Damage = damage;
            this.projectileTexture = projectileTexture;
            this.projectileScale = projectileScale;
            this.projectileSpeed = projectileSpeed;
            projectiles = new List<Projectile>();
        }

        private void LaunchProjectile(Enemy enemy)
        {
            projectiles.Add(new Projectile(projectileSpeed, Damage, Position, projectileScale, projectileTexture, enemy));
        }

        public override void Update(Enemy[] enemies, GameTime gametime)
        {
            base.Update(enemies, gametime);

            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i].Update())
                {
                    projectiles.RemoveAt(i);
                }
            }
        }

        protected override void Activate(Enemy[] enemies)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (Math.Abs(enemies[i].GridPos.X - GridPos.X) + Math.Abs(enemies[i].GridPos.Y - GridPos.Y) < range)
                {
                    LaunchProjectile(enemies[i]);
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

        public override Tower GetTower(Point gridPos, Point tileSize, Vector2 mapPosition)
        {
            return new ShooterTower(range, Damage, projectileTexture, projectileScale, projectileSpeed, gridPos, ActionRate, Texture, tileSize, Scale, mapPosition, Cost);
        }
    }
}
