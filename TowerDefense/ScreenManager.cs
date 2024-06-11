using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private Screen screen;
        private Dictionary<ScreenTypes, Screen> screens;

        private ScreenManager()
        {
            
        }

        public static ScreenManager Instance { get; private set; } = new ScreenManager();

        public void Initilize(Dictionary<ScreenTypes, Screen> screens)
        {
            this.screens = screens;
        }

        public void Update(Screen screen)
        {
            ScreenTypes type = screen.Update();

            if (type != screen.GetType())
            {
                if (type == typeof(HomeScreen))
                {
                    screen = new HomeScreen(background, new Point(100, 100), new Point(200, 100), Color.Red, Color.Red, buttonFont);
                }
                else if (type == typeof(MapEditor))
                {
                    screen = new MapEditor(((MapEditorMenu)screen).SelectedMap, new Vector2(680, 64), buttonFont, new Point(650, 600));
                }
                else if (type == typeof(MapEditorMenu))
                {
                    MapEditorMenuItem[] items = new MapEditorMenuItem[savedMaps.Length];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new MapEditorMenuItem(savedMaps[i], new Point(380, 50), Color.Black, buttonFont, new Point(10, 20), new Point(100, 20));
                    }
                    screen = new MapEditorMenu(spriteSheet, sourceRectangles, new Rectangle(100, 100, 596, 460), Color.WhiteSmoke, items, new Point(20, 20), new Point(200, 10), buttonFont, new Point(470, 10), background);
                }
            }

        }
    }
}
