using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TowerDefense.Screens;

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
        public Enemy[] EnemyQueue;
        private TowerShop towerShop;
        private SpriteFont font;
        private Tower selectedTower;
        private List<Tower> currentTowers;
        private Point hoverPos;
        private int enemyindex;
        private TimeSpan nextWaveRate;
        private TimeSpan waveTimer;
        public int EndHealth { get; private set; }

        public GameStats stats;

        public GameScreen(TileMapSpecs specs, Enemy[] startingEnemy, TimeSpan enemySpawnRate, Tower[] towers, SpriteFont font, int endHealth, TimeSpan nextWaveRate)
        {
            this.specs = specs;
            money = 100;
            faze = 1;
            enemies = new List<Enemy>();
            EnemyQueue = startingEnemy;
            this.enemySpawnRate = enemySpawnRate;
            this.font = font;
            this.towers = towers;
            currentTowers = new List<Tower>();
            enemyindex = 0;
            EndHealth = endHealth;
            stats = new GameStats(0, 0, 0, 0);
            this.nextWaveRate = nextWaveRate;
            waveTimer = TimeSpan.Zero;
        }

        public void Initialize(TileMapProfile profile)
        {
            map = new Tilemap(profile.Size, specs.TileSize, profile.TileTypes, profile.MapPosition, specs.SourceRectangles, specs.SpriteSheet);

            for(int i = 0; i < EnemyQueue.Length; i++)
            {
                EnemyQueue[i].Map = map;
                EnemyQueue[i].GridPos = map.StartingPoint;
            }
            
            towerShop = new TowerShop(towers.ToArray(), new Point((int)(map.mapPosition.X + (specs.TileSize.X * map.size.X) + 5), (int)map.mapPosition.Y), font, Color.Green);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch, hoverPos);

            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }

            for (int i = 0; i < currentTowers.Count; i++)
            {
                currentTowers[i].Draw(spriteBatch);
            }

            spriteBatch.DrawString(towerShop.items[0].BuyButton.Font, $"Money: {money}", new Vector2(map.mapPosition.X, 0), Color.White);

            towerShop.Draw(spriteBatch);
        }

        public override ScreenTypes ReturnType()
        {
            return ScreenTypes.Game;
        }

        public override ScreenTypes Update(GameTime gameTime)
        {
            waveTimer += gameTime.ElapsedGameTime;
            if(waveTimer >= nextWaveRate)
            {
                waveTimer = TimeSpan.Zero;
                if(enemyindex + 1 < EnemyQueue.Length)
                {
                    enemyindex++;
                }
                enemySpawnRate -= TimeSpan.FromMilliseconds(150);
            }

            hoverPos = GetHovorPos();
            enemySpawnTimer += gameTime.ElapsedGameTime;

            if(EndHealth <= 0)
            {
                return ScreenTypes.GameOver;
            }

            SpawnEnemies();

            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(gameTime);
                if (enemies[i].Health <= 0)
                {
                    money += enemies[i].Reward;
                    enemies.Remove(enemies[i]);
                }
                else if (enemies[i].HasReachedEnd)
                {
                    EndHealth -= 20;
                    enemies.Remove(enemies[i]);
                }
            }

            for(int i = 0; i < currentTowers.Count; i++)
            {
                currentTowers[i].Update(enemies.ToArray(), gameTime);
            }

            if(selectedTower == null)
            {
                for (int i = 0; i < towerShop.items.Length; i++)
                {
                    if (towerShop.items[i].BuyButton.isClicked() && money - towerShop.items[i].DefaultTower.Cost >= 0)
                    {
                        selectedTower = towerShop.items[i].DefaultTower;
                        money -= selectedTower.Cost;
                    }
                }
            }
            
            if(Mouse.GetState().LeftButton == ButtonState.Pressed && hoverPos.X < int.MaxValue && selectedTower != null)
            {
                currentTowers.Add(selectedTower.GetTower(hoverPos, map.Specs.TileSize, map.mapPosition));
                selectedTower = null;
            }

            return ReturnType();
        }

        private void SpawnEnemies()
        {
            if (enemySpawnTimer >= enemySpawnRate)
            {
                enemies.Add(new Enemy(EnemyQueue[enemyindex].Speed, EnemyQueue[enemyindex].Health, EnemyQueue[enemyindex].Scale, EnemyQueue[enemyindex].SourceRectangle, EnemyQueue[enemyindex].Texture, EnemyQueue[enemyindex].Reward));
                enemies[enemies.Count - 1].Map = map;
                enemies[enemies.Count - 1].GridPos = map.StartingPoint;
                enemySpawnTimer = TimeSpan.Zero;
                enemies[enemies.Count - 1].GenerateRougt();
                if (enemyindex >= EnemyQueue.Length)
                {
                    enemyindex = 0;
                }
            }
        }
        public Point GetHovorPos()
        {
            for(int x = 0; x < map.Tiles.GetLength(0); x++)
            {
                for(int y = 0; y < map.Tiles.GetLength(1); y++)
                {
                    Rectangle hitbox = new Rectangle(map.Tiles[x, y].Position.ToPoint(), map.Specs.TileSize);
                    Point mousePos = Mouse.GetState().Position;
                    if (hitbox.Intersects(new Rectangle(mousePos, new Point(1, 1))))
                    {
                        return new Point(x, y);
                    }
                }
            }
            return new Point(int.MaxValue, int.MaxValue);
        }
    }
}
