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
    public abstract class Tower : Sprite
    {
        public Point GridPos;
        public int Cost;
        private TimeSpan actionTimer;
        protected TimeSpan ActionRate;

        public Tower(Point gridPos, float scale, Texture2D texture, Vector2 tileSize, Vector2 MapPosition, TimeSpan actionRate)
            : base(new Vector2(MapPosition.X + tileSize.X * gridPos.X, MapPosition.Y + tileSize.Y * gridPos.Y), Color.White, scale, 0, new Rectangle(0, 0, texture.Width, texture.Height), Vector2.Zero, texture)
        {
            actionTimer = TimeSpan.Zero;
            ActionRate = actionRate;
            GridPos = gridPos;
        }

        protected abstract void Activate(Enemy[] enemies);

        public virtual void Update(Enemy[] enemies, GameTime gametime)
        {
            actionTimer += gametime.ElapsedGameTime;
            if (actionTimer > ActionRate)
            {
                Activate(enemies);
                actionTimer = TimeSpan.Zero;
            }
        }

        public abstract Tower GetTower(Point gridPos, Point tileSize, Vector2 mapPosition);
    }
}
