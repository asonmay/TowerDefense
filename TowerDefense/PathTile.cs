using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    public enum TileTypes
    {
        Grass,
        Spawner,
        End,
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
        public PathTile[] Neighbors { get; set; }
        private TileTypes type;
        public bool hasBeenVisited;
        public float cumulativeDistance;
        public PathTile Founder { get; set; }

        public PathTile(Vector2 position, Color color, float scale, int rotation, Rectangle sourceRectangle, Vector2 origin, Texture2D texture, Point gridPos, TileTypes type)
            : base(position, color, scale, rotation, sourceRectangle, origin, texture, gridPos, type)
        {
            this.type = type;
        }

        public TileTypes GetTileType()
        {
            return type;
        }
    }
}
