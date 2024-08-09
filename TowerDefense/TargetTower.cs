using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended;

namespace TowerDefense
{
    public class TargetTower : Tower
    {
        private int range;
        public int Damage;
        private List<Projectile> projectiles;
        private Texture2D projectileTexture;
        private float projectileScale;
        private int projectileSpeed;
        private Enemy target;

        public TargetTower(int range, int damage, Texture2D projectileTexture, float projectileScale, int projectileSpeed, Point gridPos, TimeSpan actionRate, Texture2D texture, Point tileSize, float scale, Vector2 mapPostion, int cost)
            : base(gridPos, scale, texture, tileSize.ToVector2(), mapPostion, actionRate, cost)
        {
            this.range = range;
            Damage = damage;
            this.projectileTexture = projectileTexture;
            this.projectileScale = projectileScale;
            this.projectileSpeed = projectileSpeed;
            projectiles = new List<Projectile>();
        }

        private void LaunchProjectile()
        {
            projectiles.Add(new Projectile(projectileSpeed, Damage, Position, projectileScale, projectileTexture, target));
        }

        public override void Update(Enemy[] enemies, GameTime gametime)
        {
            if (target == null || !enemies.Contains(target) || Math.Abs(target.GridPos.X - GridPos.X) + Math.Abs(target.GridPos.Y - GridPos.Y) > range)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (Math.Abs(enemies[i].GridPos.X - GridPos.X) + Math.Abs(enemies[i].GridPos.Y - GridPos.Y) < range)
                    {
                        target = enemies[i];
                        break;
                    }
                }
            }

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
            LaunchProjectile();
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
            return new TargetTower(range, Damage, projectileTexture, projectileScale, projectileSpeed, gridPos, ActionRate, Texture, tileSize, Scale, mapPosition, Cost);
        }
    }
}
