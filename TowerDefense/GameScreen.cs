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
        private Enemy[] enemies;
        private int faze;
        private TileMapSpecs specs;

        public GameScreen(TileMapSpecs specs)
        {
            this.specs = specs;
            money = 100;
            faze = 1;
            towers = new Tower[0];
            enemies = new Enemy[0];
        }

        public void Initialize(TileMapProfile profile)
        {
            map = new Tilemap(profile.Size, specs.TileSize, profile.TileTypes, profile.MapPosition, specs.SourceRectangles, specs.SpriteSheet);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);

            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Draw(spriteBatch);
            }
        }

        public override ScreenTypes ReturnType()
        {
            return ScreenTypes.Game;
        }

        public override ScreenTypes Update()
        {
            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Update();
            }

            return ReturnType();
        }
    }
}
