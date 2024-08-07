using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Screens;

namespace TowerDefense
{
    public enum ScreenTypes
    {
        HomeScreen,
        PlayMenu,
        Game,
        MapEditor,
        MapEditorMenu,
        GameScreen,
        GameOver,
    }
    public class ScreenManager
    {
        public Screen currentScreen;
        private ScreenTypes currentType;
        private Dictionary<ScreenTypes, Screen> screens;

        private ScreenManager() { }

        public static ScreenManager Instance { get; private set; } = new ScreenManager();

        public void Initilize(Dictionary<ScreenTypes, Screen> screens)
        {
            this.screens = screens;
            currentScreen = screens[ScreenTypes.HomeScreen];
        }

        public void Update(List<TileMapProfile> profiles, GameTime gameTime)
        {
            Screen lastScreen = currentScreen;
            ScreenTypes type = currentScreen.Update(gameTime);

            if (type != currentType)
            {
                currentScreen = screens[type];
                currentType = type;
                if(type == ScreenTypes.MapEditor)
                {
                    ((MapEditor)currentScreen).Initialize(((MapEditorMenu)lastScreen).SelectedMap, ((MapEditorMenu)lastScreen).shouldMakeNew);
                }
                else if (type == ScreenTypes.Game)
                {
                    ((GameScreen)currentScreen).Initialize(((PlayMenu)lastScreen).SelectedMap);
                }
                else if (type == ScreenTypes.PlayMenu)
                {
                    List<MapEditorMenuItem> items = new List<MapEditorMenuItem>();
                    for (int i = 0; i < profiles.Count; i++)
                    {
                        Rectangle rect = ((PlayMenu)currentScreen).listRect;
                        Point offset = ((PlayMenu)currentScreen).mapFileOffests;
                        Point buttonPos = new Point(rect.X + offset.X + 556 - (int)currentScreen.buttonFont.MeasureString("Select").X, rect.Y + i * 60 + offset.Y);
                        items.Add(new MapEditorMenuItem(profiles[i], new Point(566, 50), Color.Black, currentScreen.buttonFont, new Point(10, 10), buttonPos, new Point(rect.X + offset.X, rect.Y + i * 60 + offset.Y)));
                    }

                    ((PlayMenu)currentScreen).Initalize(items.ToArray());
                }
                else if(type == ScreenTypes.MapEditorMenu)
                {
                    if (lastScreen.ReturnType() == ScreenTypes.MapEditor)
                    {
                        if(((MapEditor)lastScreen).isNew)
                        {
                            TileMapProfile orignalProfile = ((MapEditor)lastScreen).profile;
                            TileMapProfile profile = new TileMapProfile(orignalProfile.TileTypes, $"Map {profiles.Count}", orignalProfile.Size, orignalProfile.MapPosition);
                            profiles.Add(profile);
                        }
                        else
                        {
                            profiles.Remove(((MapEditor)lastScreen).original);
                            profiles.Add(((MapEditor)lastScreen).profile);
                        }
                       
                    }
                    
                    List<MapEditorMenuItem> items = new List<MapEditorMenuItem>();
                    for (int i = 0; i < profiles.Count; i++)
                    {
                        Rectangle rect = ((MapEditorMenu)currentScreen).listRect;
                        Point offset = ((MapEditorMenu)currentScreen).mapFileOffests;
                        Point buttonPos = new Point(rect.X + offset.X + 556 - (int)currentScreen.buttonFont.MeasureString("Select").X, rect.Y + i * 60 + offset.Y);
                        items.Add(new MapEditorMenuItem(profiles[i], new Point(566, 50), Color.Black, currentScreen.buttonFont, new Point(10, 10), buttonPos, new Point(rect.X + offset.X, rect.Y + i * 60 + offset.Y)));
                    }

                    ((MapEditorMenu)currentScreen).Initalize(items.ToArray());
                }
                else if(type == ScreenTypes.GameOver)
                {
                    ((GameOverScreen)currentScreen).Init(((GameScreen)lastScreen).stats, ((GameScreen)lastScreen).DidWin);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
        }
    }
}
