using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class GameScreen : Screen
    {
        private Tilemap map;
        private int money;
        private Tower[] towers;
        private List<Enemy> enemies;
        private int faze;
        private TileMapSpecs specs;
        private TimeSpan enemySpawnTimer;
        private TimeSpan enemySpawnRate;
        private Enemy startingEnemy;

        public GameScreen(TileMapSpecs specs, Enemy startingEnemy)
        {
            this.specs = specs;
            money = 100;
            faze = 1;
            towers = new Tower[0];
            enemies = new List<Enemy>();
            this.startingEnemy = startingEnemy;
        }

        public void Initialize(TileMapProfile profile)
        {
            map = new Tilemap(profile.Size, specs.TileSize, profile.TileTypes, profile.MapPosition, specs.SourceRectangles, specs.SpriteSheet);
            startingEnemy.GridPos = map.StartingPoint;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);

            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }
        }

        public override ScreenTypes ReturnType()
        {
            return ScreenTypes.Game;
        }

        public override ScreenTypes Update(GameTime gameTime)
        {
            enemySpawnTimer += gameTime.ElapsedGameTime;
            if(enemySpawnTimer >= enemySpawnRate)
            {
                enemies.Add(startingEnemy);
            }

            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(gameTime, map);
                if (enemies[i].HasReachedEnd)
                {
                    enemies.Remove(enemies[i]);
                }
            }

            return ReturnType();
        }
    }
}
