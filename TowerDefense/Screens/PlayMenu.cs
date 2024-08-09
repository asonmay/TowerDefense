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
    public class PlayMenu : Screen
    {
        public Rectangle listRect;
        private Color listColor;
        private List<MapEditorMenuItem> mapFiles;
        private Button homeButton;
        public Point mapFileOffests;
        public TileMapProfile[] savedMaps;
        public TileMapProfile SelectedMap;
        

        public PlayMenu(Texture2D spriteSheet, Dictionary<TileTypes, Rectangle> sourceRectangles, Rectangle listRect, Color listColor, Point mapFileOffests, Point homeButtonPos, SpriteFont buttonFont, Texture2D background)
        {
            this.listColor = listColor;
            this.listRect = listRect;
            this.mapFileOffests = mapFileOffests;
            this.buttonFont = buttonFont;
            homeButton = new Button(Color.Red, "Home", homeButtonPos, buttonFont, Color.Black);
            this.background = background;

            this.spriteSheet = spriteSheet;
            this.sourceRectangles = sourceRectangles;
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
        }

        public override ScreenTypes Update(GameTime gameTime)
        {

            for(int i = 0; i < mapFiles.Count; i++)
            {
                if (mapFiles[i].editButton.isClicked())
                {
                    SelectedMap = mapFiles[i].profile;
                    return ScreenTypes.Game;
                }
            }
            if (homeButton.isClicked())
            {
                return ScreenTypes.HomeScreen;
            }
            return ScreenTypes.PlayMenu;
        }
    }
}
