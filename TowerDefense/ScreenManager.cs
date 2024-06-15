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

namespace TowerDefense
{
    public enum ScreenTypes
    {
        HomeScreen,
        MapEditor,
        MapEditorMenu,
    }
    public class ScreenManager
    {
        public Screen currentScreen;
        private ScreenTypes currentType;
        private Dictionary<ScreenTypes, Screen> screens;

        private ScreenManager()
        {
            
        }

        public static ScreenManager Instance { get; private set; } = new ScreenManager();

        public void Initilize(Dictionary<ScreenTypes, Screen> screens)
        {
            this.screens = screens;
            currentScreen = screens[ScreenTypes.HomeScreen];
        }

        public void Update(List<TileMapProfile> profiles)
        {
            Screen lastScreen = currentScreen;
            ScreenTypes type = currentScreen.Update();

            if (type != currentType)
            {
                currentScreen = screens[type];
                currentType = type;
                if(type == ScreenTypes.MapEditor)
                {
                    ((MapEditorMenu)lastScreen).SelectedMap.name = $"Map {profiles.Count}";
                    ((MapEditor)currentScreen).Initialize(((MapEditorMenu)lastScreen).SelectedMap, ((MapEditorMenu)lastScreen).shouldMakeNew);
                }
                else if(type == ScreenTypes.MapEditorMenu)
                {
                    if (lastScreen.ReturnType() == ScreenTypes.MapEditor)
                    {
                        if(((MapEditor)lastScreen).isNew)
                        {
                            profiles.Add(((MapEditor)lastScreen).profile);
                        }
                        else
                        {
                            profiles.Remove(((MapEditor)lastScreen).original);
                            profiles.Add(((MapEditor)lastScreen).original);
                        }
                       
                    }
                    
                    List<MapEditorMenuItem> items = new List<MapEditorMenuItem>();
                    for (int i = 0; i < profiles.Count; i++)
                    {
                        Rectangle rect = ((MapEditorMenu)currentScreen).listRect;
                        Point offset = ((MapEditorMenu)currentScreen).mapFileOffests;
                        items.Add(new MapEditorMenuItem(profiles[i], new Point(566, 50), Color.Black, currentScreen.buttonFont, new Point(10, 20), new Point(rect.X + offset.X, rect.Y + i * 60 + offset.Y), new Point(rect.X + offset.X, rect.Y + i * 60 + offset.Y)));
                    }

                    ((MapEditorMenu)currentScreen).Initalize(items.ToArray());
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
        }
    }
}
