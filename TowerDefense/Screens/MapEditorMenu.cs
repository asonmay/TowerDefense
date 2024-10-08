﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class MapEditorMenu : Screen
    {
        public Rectangle listRect;
        private Color listColor;
        private List<MapEditorMenuItem> mapFiles;
        private Button homeButton;
        public Point mapFileOffests;
        public TileMapProfile[] savedMaps;
        private Button newFileButton;
        public TileMapProfile SelectedMap;
        public bool shouldMakeNew;

        public MapEditorMenu(Texture2D spriteSheet, Dictionary<TileTypes, Rectangle> sourceRectangles, Rectangle listRect, Color listColor, Point mapFileOffests, Point homeButtonPos, SpriteFont buttonFont, Point newButtonPos, Texture2D background)
        {
            TileMapSpecs defultSpecs = new TileMapSpecs(new Point(32,32), spriteSheet, sourceRectangles);
            TileTypes[,] defaultTileTypes = new TileTypes[20,18];
            SelectedMap = new TileMapProfile(defaultTileTypes, "Map1", new Point(20, 18), new Vector2(20, 64));

            this.listColor = listColor;
            this.listRect = listRect;
            this.mapFileOffests = mapFileOffests;
            this.buttonFont = buttonFont;
            homeButton = new Button(Color.Red, "Home", homeButtonPos, buttonFont, Color.Black);
            newFileButton = new Button(Color.Red, "New Map", newButtonPos, buttonFont, Color.Black);
            this.background = background;

            this.spriteSheet = spriteSheet;
            this.sourceRectangles = sourceRectangles;
            shouldMakeNew = false;
        }

        public void Initalize(MapEditorMenuItem[] profiles)
        {
            mapFiles = profiles.ToList();
        }       

        public override ScreenTypes ReturnType()
        {
            return ScreenTypes.MapEditorMenu;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, new Rectangle(0, 3, background.Width, background.Height), Color.White, 0, Vector2.Zero, new Vector2(0.8f, 1.2f), SpriteEffects.None, 1);
            spriteBatch.FillRectangle(listRect, listColor);
        
            for(int i = 0; i < mapFiles.Count; i++)
            {
                mapFiles[i].Draw(spriteBatch);
            }

            homeButton.Draw(spriteBatch);
            newFileButton.Draw(spriteBatch);
        }

        public override ScreenTypes Update(GameTime gameTime)
        {
            for(int i = 0; i < mapFiles.Count; i++)
            {
                if (mapFiles[i].editButton.isClicked())
                {
                    SelectedMap = mapFiles[i].profile;
                    shouldMakeNew = false;
                    return ScreenTypes.MapEditor;
                }
            }
            if(newFileButton.isClicked())
            {
                shouldMakeNew = true;
                return ScreenTypes.MapEditor;
            }
            if (homeButton.isClicked())
            {
                return ScreenTypes.HomeScreen;
            }
            return ScreenTypes.MapEditorMenu;
        }
    }
}
