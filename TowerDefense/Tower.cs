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
        public Point GridPos;
        public int Range;
        public int Cost;
        public int Damage;
        public TimeSpan FireRate;
        public List<Projectile> Projectiles;
        public Texture2D ProjectileTexture;
        public float ProjectileScale;
        public TimeSpan AttackTimer;

        public Tower(Point gridPos, float scale, Texture2D texture, int cost, int damage, int range, Texture2D projectileTexture, float projectileScale, Vector2 tileSize, Vector2 MapPosition, TimeSpan fireRate)
            : base(new Vector2(MapPosition.X + tileSize.X * gridPos.X, MapPosition.Y + tileSize.Y * gridPos.Y), Color.White, scale, 0, new Rectangle(0,0,texture.Width,texture.Height), Vector2.Zero, texture)
        {
            Cost = cost;
            GridPos = gridPos;
            Damage = damage;
            Range = range;
            AttackTimer = TimeSpan.Zero;
            ProjectileScale = projectileScale;
            ProjectileTexture = projectileTexture;
            Projectiles = new List<Projectile>();
            FireRate = fireRate;
        }

        protected void Attack(Enemy[] enemies)
        {
            for(int i = 0; i < enemies.Length; i++)
            {
                if (Math.Abs(enemies[i].GridPos.X - GridPos.X) + Math.Abs(enemies[i].GridPos.Y - GridPos.Y) <= Range)
                {
                    LaunchProjectile(enemies[i]);
                }
            }
        }

        private void LaunchProjectile(Enemy enemy)
        {
            Projectiles.Add(new Projectile(12, Damage, Position, ProjectileScale, ProjectileTexture, enemy));
        }

        public void Update(Enemy[] enemies, GameTime gametime)
        {
            AttackTimer += gametime.ElapsedGameTime;
            if(AttackTimer > FireRate)
            {
                Attack(enemies);
                AttackTimer = TimeSpan.Zero;
            }

            for(int i = 0; i < Projectiles.Count; i++)
            {
                if (Projectiles[i].Update())
                {
                    Projectiles.RemoveAt(i);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectiles[i].Draw(spriteBatch);
            }
        }
    }
}
