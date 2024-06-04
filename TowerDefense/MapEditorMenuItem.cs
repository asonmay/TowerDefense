using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class MapEditorMenuItem
    {
        public TileMapProfile profile;
        private Button editButton;
        private Rectangle rect;
        private Color backgroundColor;
        private SpriteFont nameFont;
        private Point namePos;
    }
}
