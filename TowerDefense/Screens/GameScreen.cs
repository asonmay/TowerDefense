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
        public Enemy StartingEnemy;
        private TowerShop towerShop;

        public GameScreen(TileMapSpecs specs, Enemy startingEnemy, TimeSpan enemySpawnRate, Tower[] towers)
        {
            this.specs = specs;
            money = 100;
            faze = 1;
            enemies = new List<Enemy>();
            StartingEnemy = startingEnemy;
            this.enemySpawnRate = enemySpawnRate;

            this.towers = towers;
        }

        public void Initialize(TileMapProfile profile)
        {
            map = new Tilemap(profile.Size, specs.TileSize, profile.TileTypes, profile.MapPosition, specs.SourceRectangles, specs.SpriteSheet);
            StartingEnemy.Map = map;
            StartingEnemy.GridPos = map.StartingPoint;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);

            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }

            for (int i = 0; i < towers.Length; i++)
            {
                towers[i].Draw(spriteBatch);
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
                enemies.Add(new Enemy(StartingEnemy.Speed, StartingEnemy.Health, StartingEnemy.Scale, StartingEnemy.SourceRectangle, StartingEnemy.Texture));
                enemies[enemies.Count - 1].Map = map;
                enemies[enemies.Count - 1].GridPos = map.StartingPoint;
                enemySpawnTimer = TimeSpan.Zero;
                enemies[enemies.Count - 1].GenerateRougt();
            }

            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(gameTime);
                if (enemies[i].HasReachedEnd)
                {
                    enemies.Remove(enemies[i]);
                }
            }

            for(int i = 0; i < towers.Length; i++)
            {
                towers[i].Update(enemies.ToArray());
            }

            return ReturnType();
        }
    }
}
