using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using MonoGame.Extended; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    public class TextBox
    {
        private Rectangle box;
        private Color color;
        public string text;
        private SpriteFont font;
        
        public TextBox(Rectangle box, SpriteFont font)
        {
            this.box = box;
            this.font = font;
            text = "";
 
        }

        public void Update(bool isFocused)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
           // spriteBatch.FillRectangle(box, "text", color);
        }
    }
}
