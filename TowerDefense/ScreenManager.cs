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
        public Screen screen;
        private ScreenTypes currentType;
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

            if (type != currentType)
            {
                screen = screens[type];
                currentType = type;
                if(type == ScreenTypes.MapEditor)
                {
                    
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
