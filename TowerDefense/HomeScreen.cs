using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class HomeScreen : Screen
    {
        private Button playButton;
        private Button mapEditorButton;
        private Texture2D backgroundTexture;

        public HomeScreen(Texture2D backgroundTexture, Point playButtonPos, Point mapEditorPos, Color playButtonColor, Color mapEditorColor, SpriteFont buttonFont)
            :base()
        {
            this.backgroundTexture = backgroundTexture;
            playButton = new Button(playButtonColor,"play", playButtonPos, buttonFont, Color.Black);
            playButton = new Button(mapEditorColor, "map editor", mapEditorPos, buttonFont, Color.Black);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            playButton.Draw(spriteBatch);
            mapEditorButton.Draw(spriteBatch);
        }

        public override ScreenTypes Update()
        {
            return GetScreenToSwitch();
        }

        public ScreenTypes GetScreenToSwitch()
        {
            if (playButton.isClicked())
            {
                return ScreenTypes.PlayMenu;
            }
            else if(mapEditorButton.isClicked())
            {
                return ScreenTypes.MapEditorMenu;
            }
            return ScreenTypes.Home;
        }
    }
}
