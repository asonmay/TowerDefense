using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Screens
{
    public struct GameStats
    {
        public int EnemiesKilled { get; set; }
        public int DamageGiven { get; set; }
        public int Points { get; set; }
        public int TowersPlaced { get; set; }

        public GameStats(int enemiesKilled, int damageGiven, int points, int towersPlaced)
        {
            EnemiesKilled = enemiesKilled;
            DamageGiven = damageGiven;
            Points = points;
            TowersPlaced = towersPlaced;
        }
    }
    public class GameOverScreen : Screen
    {
        private Button homeButton;
        private SpriteFont gameOverFont;
        private GameStats stats;
        private SpriteFont statsFont;

        public GameOverScreen(SpriteFont gameOverFont, Viewport viewport, SpriteFont statsFont)
        {
            this.gameOverFont = gameOverFont;
            viewPort = viewport;
            this.statsFont = statsFont;
            homeButton = new Button(Color.Green, "Go Home", Point.Zero, statsFont, Color.Black);
        }

        public void Init(GameStats stats)
        {
            this.stats = stats;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            var stringSize = gameOverFont.MeasureString("GAME OVER");
            spriteBatch.DrawString(gameOverFont, "GAME OVER", new Vector2((viewPort.Width / 2) - (stringSize.X / 2), (viewPort.Height / 2) - (stringSize.Y / 2)), Color.White);
            homeButton.Draw(spriteBatch);
        }

        public override ScreenTypes ReturnType()
        {
            if(homeButton.isClicked())
            {
                return ScreenTypes.HomeScreen;
            }
            return ScreenTypes.GameOver;
        }

        public override ScreenTypes Update(GameTime gameTime)
        {
            return ReturnType();
        }
    }
}
