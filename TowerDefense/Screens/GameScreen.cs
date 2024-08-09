using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TowerDefense.Screens;

namespace TowerDefense
{
    public class GameScreen : Screen
    {
        private Tilemap map;
        private TileMapSpecs specs;
        private Point hoverPos;
        private TowerShop towerShop;
        private Tower[] towers;     
        private Tower selectedTower;
        private List<Tower> currentTowers;
        private int enemyindex;
        private Timer enemySpawnTimer;
        private List<Enemy> enemies;
        public Enemy[] EnemyQueue;
        private Timer waveTimer;
        private SpriteFont font;
        public int EndHealth { get; private set; }
        public GameStats stats;
        private int money;
        public bool DidWin { get; private set; }

        public GameScreen(TileMapSpecs specs, Enemy[] startingEnemy, TimeSpan enemySpawnRate, Tower[] towers, SpriteFont font, int endHealth, TimeSpan nextWaveRate)
        {
            this.specs = specs;         
            EnemyQueue = startingEnemy;
            this.font = font;
            this.towers = towers;
            waveTimer = new Timer(nextWaveRate);
            enemySpawnTimer = new Timer(enemySpawnRate);
            DidWin = false;
        }

        public void Initialize(TileMapProfile profile)
        {
            map = new Tilemap(profile.Size, specs.TileSize, profile.TileTypes, profile.MapPosition, specs.SourceRectangles, specs.SpriteSheet);
            for(int i = 0; i < EnemyQueue.Length; i++)
            {
                EnemyQueue[i].Map = map;
                EnemyQueue[i].GridPos = map.StartingPoint;
            }
            currentTowers = new List<Tower>();
            money = 100;
            enemies = new List<Enemy>();
            enemyindex = 0;
            EndHealth = 100;
            stats = new GameStats(0, 0, 0, 0);
            DidWin = false;
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

        private bool UpdateWave(GameTime gameTime)
        {
            if (waveTimer.Update(gameTime))
            {
                if (enemyindex + 1 < EnemyQueue.Length)
                {
                    enemyindex++;
                    money += 150;
                }
                else
                {
                    DidWin = true;
                    return true;
                }
            }
            return false;
        }

        public override ScreenTypes Update(GameTime gameTime)
        {
            if(UpdateWave(gameTime))
            {
                return ScreenTypes.GameOver;
            }

            hoverPos = GetHovorPos();

            if(EndHealth <= 0)
            {
                DidWin = false;
                return ScreenTypes.GameOver;
            }

            SpawnEnemies(gameTime);
            UpdateEnemies(gameTime);
            SetSelectedTower();

            for (int i = 0; i < currentTowers.Count; i++)
            {
                currentTowers[i].Update(enemies.ToArray(), gameTime);
            }

            if(Mouse.GetState().LeftButton == ButtonState.Pressed && hoverPos.X < int.MaxValue && selectedTower != null && map.Tiles[hoverPos.X, hoverPos.Y].Type == TileTypes.Grass && !DoesHaveTile(hoverPos))
            {
                currentTowers.Add(selectedTower.GetTower(hoverPos, map.Specs.TileSize, map.mapPosition));
                selectedTower = null;
            }

            return ReturnType();
        }

        private void SetSelectedTower()
        {
            if (selectedTower == null)
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
        }
        
        private void UpdateEnemies(GameTime gameTime)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(gameTime);
                if (enemies[i].Health <= 0)
                {
                    money += enemies[i].Reward;
                    enemies.Remove(enemies[i]);
                    stats.EnemiesKilled++;
                }
                else if (enemies[i].HasReachedEnd)
                {
                    EndHealth -= 20;
                    enemies.Remove(enemies[i]);
                }
            }
        }

        private bool DoesHaveTile(Point pos)
        {
            for(int i = 0; i < currentTowers.Count; i++)
            {
                if (currentTowers[i].GridPos == pos)
                {
                    return true;
                }
            }
            return false;
        }

        private void SpawnEnemies(GameTime gameTime)
        {
            if (enemySpawnTimer.Update(gameTime))
            {
                enemies.Add(EnemyQueue[enemyindex].Clone());
                enemies[enemies.Count - 1].Map = map;
                enemies[enemies.Count - 1].GridPos = map.StartingPoint;
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
