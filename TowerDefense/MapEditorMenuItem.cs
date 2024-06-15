using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class MapEditorMenuItem
    {
        public TileMapProfile profile;
        public Button editButton;
        private Point size;
        private Color backgroundColor;
        private SpriteFont nameFont;
        private Point namePos;
        private Point pos;

        public MapEditorMenuItem(TileMapProfile profile, Point size, Color backgroundColor, SpriteFont nameFont, Point namePos, Point buttonPos, Point pos)
        {
            this.profile = profile;
            this.size = size;
            this.backgroundColor = backgroundColor;
            this.nameFont = nameFont;
            this.pos = pos;
            this.namePos = namePos;
            editButton = new Button(Color.Gray, "Edit", buttonPos, nameFont, Color.Black);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(new Rectangle(pos, size), backgroundColor);
            spriteBatch.DrawString(nameFont, profile.name, (pos + namePos).ToVector2(), Color.White);
            editButton.Draw(spriteBatch);
        }
    }
}
