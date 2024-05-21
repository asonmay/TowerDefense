using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public enum PathTileType
    {
        None,
        Center,
        Left,
        Right,
        Up,
        Down,
        LeftUp,
        RightUp,
        LeftDown,
        RightDown,
        LeftRight,
        LeftRightUp,
        LeftRightDown
    }

    public class PathTile : Tile
    {
        public PathTile[] neighbors { get; set; }
        private PathTileType type;

        public PathTile(Vector2 position, Color color, float scale, int rotation, Rectangle sourceRectangle, Vector2 origin, Texture2D texture, Point gridPos, PathTileType type)
            : base(position, color, scale, rotation, sourceRectangle, origin, texture, gridPos)
        {
            this.type = type;
        }

        public PathTileType GetTileType()
        {
            return type;
        }
    }
}
