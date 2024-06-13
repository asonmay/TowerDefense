using Microsoft.Xna.Framework;
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
        private Rectangle listRect;
        private Color listColor;
        private MapEditorMenuItem[] mapFiles;
        private Button homeButton;
        private Point mapFileOffests;
        public TileMapProfile[] savedMaps;
        private Button newFileButton;
        public TileMapProfile SelectedMap;
        private Texture2D background;

        public MapEditorMenu(Texture2D spriteSheet, Dictionary<PathTileType, Rectangle> sourceRectangles, Rectangle listRect, Color listColor, Point mapFileOffests, Point homeButtonPos, SpriteFont buttonFont, Point newButtonPos, Texture2D background)
        {
            TileMapSpecs defultSpecs = new TileMapSpecs(new Point(32,32), spriteSheet, sourceRectangles);
            PathTileType[,] defaultTileTypes = new PathTileType[20,18];
            SelectedMap = new TileMapProfile(defaultTileTypes, defultSpecs, "Map1", new Point(20, 18), new Vector2(20, 64));

            this.listColor = listColor;
            this.listRect = listRect;
            this.mapFileOffests = mapFileOffests;
            this.buttonFont = buttonFont;
            homeButton = new Button(Color.Red, "Home", homeButtonPos, buttonFont, Color.Black);
            newFileButton = new Button(Color.Red, "New Map", newButtonPos, buttonFont, Color.Black);
            this.background = background;

            this.spriteSheet = spriteSheet;
            this.sourceRectangles = sourceRectangles;
        }

        public void Initalize(MapEditorMenuItem[] profiles)
        {
            mapFiles = profiles;
        }

        public override ScreenTypes ReturnType()
        {
            return ScreenTypes.MapEditorMenu;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, new Rectangle(0, 0, background.Width, background.Height), Color.White, 0, Vector2.Zero, new Vector2(1.1f, 1), SpriteEffects.None, 1);
            spriteBatch.FillRectangle(listRect, listColor);
        
            for(int i = 0; i < mapFiles.Length; i++)
            {
                mapFiles[i].Draw(spriteBatch, listRect.Location + new Point(i * mapFileOffests.X, i * mapFileOffests.Y));
            }

            homeButton.Draw(spriteBatch);
            newFileButton.Draw(spriteBatch);
        }

        public override ScreenTypes Update()
        {
            for(int i = 0; i < mapFiles.Length; i++)
            {
                if (mapFiles[i].editButton.isClicked())
                {
                    SelectedMap = mapFiles[i].profile;
                    return ScreenTypes.MapEditor;
                }
            }
            if(newFileButton.isClicked())
            {
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
