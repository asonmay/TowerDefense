using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private SpriteFont font;
        private Tower selectedTower;
        private List<Tower> currentTowers;
        private Point hoverPos;

        public GameScreen(TileMapSpecs specs, Enemy startingEnemy, TimeSpan enemySpawnRate, Tower[] towers, SpriteFont font)
        {
            this.specs = specs;
            money = 100;
            faze = 1;
            enemies = new List<Enemy>();
            StartingEnemy = startingEnemy;
            this.enemySpawnRate = enemySpawnRate;
            this.font = font;
            this.towers = towers;
            currentTowers = new List<Tower>();
        }

        public void Initialize(TileMapProfile profile)
        {
            map = new Tilemap(profile.Size, specs.TileSize, profile.TileTypes, profile.MapPosition, specs.SourceRectangles, specs.SpriteSheet);
            StartingEnemy.Map = map;
            StartingEnemy.GridPos = map.StartingPoint;
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

            towerShop.Draw(spriteBatch);
        }

        public override ScreenTypes ReturnType()
        {
            return ScreenTypes.Game;
        }

        public override ScreenTypes Update(GameTime gameTime)
        {
            hoverPos = getHovorPos();
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
                if (enemies[i].HasReachedEnd || enemies[i].Health <= 0)
                {
                    enemies.Remove(enemies[i]);
                }
            }

            for(int i = 0; i < currentTowers.Count; i++)
            {
                currentTowers[i].Update(enemies.ToArray(), gameTime);
            }

            for(int i = 0; i < towerShop.items.Length; i++)
            {
                if (towerShop.items[i].BuyButton.isClicked())
                {
                    selectedTower = towerShop.items[i].DefaultTower;
                }
            }
            
            if(Mouse.GetState().LeftButton == ButtonState.Pressed && hoverPos.X < int.MaxValue && selectedTower != null)
            {
                currentTowers.Add(selectedTower.GetTower(hoverPos, map.Specs.TileSize, map.mapPosition));
                selectedTower = null;
            }

            return ReturnType();
        }

        public Point getHovorPos()
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
