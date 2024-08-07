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
        private bool didWin;
        private GameStats stats;
        private SpriteFont statsFont;
        private int stringOffset;

        public GameOverScreen(SpriteFont gameOverFont, Viewport viewport, SpriteFont statsFont, int stringOffset)
        {
            this.gameOverFont = gameOverFont;
            viewPort = viewport;
            this.statsFont = statsFont;
            homeButton = new Button(Color.Green, "Go Home", Point.Zero, statsFont, Color.Black);
            this.stringOffset = stringOffset;
        }

        public void Init(GameStats stats, bool didWin)
        {
            this.stats = stats;
            this.didWin = didWin;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            string text = didWin ? "You Won" : "You Lost";
            var stringSize = gameOverFont.MeasureString(text);
            Vector2 pos = new Vector2((viewPort.Width / 2) - (stringSize.X / 2), (viewPort.Height / 2) - (stringSize.Y / 2));
            spriteBatch.DrawString(gameOverFont, text, pos, Color.White);

            text = $"Enemies Killed: {stats.EnemiesKilled}";
            stringSize = statsFont.MeasureString($"Enemies Killed: {stats.EnemiesKilled}");
            pos = new Vector2((viewPort.Width / 2) - (stringSize.X / 2), (viewPort.Height / 2) - (stringSize.Y / 2) + (stringSize.Y + stringOffset));
            spriteBatch.DrawString(statsFont, text, pos, Color.White);

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
