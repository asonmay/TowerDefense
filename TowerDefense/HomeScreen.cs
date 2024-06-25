﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
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
        private SpriteFont titleFont;
        private Texture2D backgroundTexture;

        public HomeScreen(Texture2D backgroundTexture, Point playButtonPos, Point mapEditorPos, Color playButtonColor, Color mapEditorColor, SpriteFont buttonFont, Texture2D background, SpriteFont titleFont, Viewport viewPort)
        {
            this.backgroundTexture = backgroundTexture;
            playButton = new Button(playButtonColor,"play", playButtonPos, buttonFont, Color.Black);
            mapEditorButton = new Button(mapEditorColor, "map editor", mapEditorPos, buttonFont, Color.Black);

            this.background = background;
            this.titleFont = titleFont;
            this.viewPort = viewPort;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, new Rectangle(0,0, backgroundTexture.Width, backgroundTexture.Height), Color.White, 0, Vector2.Zero, new Vector2(1.1f,1), SpriteEffects.None,1);
            playButton.Draw(spriteBatch);
            spriteBatch.DrawString(titleFont, "ZANLIN", new Vector2((viewPort.Width - titleFont.MeasureString("ZANLIN").X) / 2, 5), Color.DimGray);
            spriteBatch.DrawString(titleFont, "DEFENSE", new Vector2((viewPort.Width - titleFont.MeasureString("DEFENSE").X) / 2, 100 + titleFont.MeasureString("ZANLIN").Y), Color.DimGray);
            mapEditorButton.Draw(spriteBatch);
        }

        public override ScreenTypes Update()
        {
            return GetScreenToSwitch();
        }

        public override ScreenTypes ReturnType()
        {
            return ScreenTypes.HomeScreen;
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
            return ScreenTypes.HomeScreen;
        }
    }
}
