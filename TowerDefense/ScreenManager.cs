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
                Screen temp = currentScreen;
                currentScreen = screens[type];
                currentType = type;
                if(type == ScreenTypes.MapEditor)
                {
                   ((MapEditor)currentScreen).Initialize(((MapEditorMenu)temp).SelectedMap);
                }
                else if(type == ScreenTypes.MapEditorMenu)
                {
                    if(lastScreen.ReturnType() == ScreenTypes.MapEditor)
                    {
                        profiles.Add(((MapEditor)lastScreen).profile);
                    }

                    List<MapEditorMenuItem> items = new List<MapEditorMenuItem>();
                    for (int i = 0; i < profiles.Count; i++)
                    {
                        items.Add(new MapEditorMenuItem(profiles[i], new Point(380, 50), Color.Black, currentScreen.buttonFont, new Point(10, 20), new Point(100, 20)));
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
