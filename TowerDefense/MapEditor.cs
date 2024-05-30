using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class MapEditor : Screen
    {
        private Point screenSize;

        private Tilemap map;
        private PathTileType[,] tileTypes;
  
        private PathTileType[,] palletTileTypes;
        private Tilemap palletMap;

        private PathTileType selectedType;

        public MapEditor(Vector2 mapPos, Point size, Point tileSize, Dictionary<PathTileType, Rectangle> sourceRectangles, Texture2D spriteSheet, Point screenSize, Vector2 canvasPos)
        {
            tileTypes = new PathTileType[size.X, size.Y];
            map = new Tilemap(size, tileSize, tileTypes, mapPos, sourceRectangles, spriteSheet);
            this.screenSize = screenSize;

            palletTileTypes = new PathTileType[,]
            {
                { PathTileType.LeftUp, PathTileType.Left, PathTileType.LeftDown, PathTileType.None },
                { PathTileType.Up, PathTileType.Center, PathTileType.Down , PathTileType.None},
                { PathTileType.RightUp, PathTileType.Right, PathTileType.RightDown, PathTileType.None },
            };
            palletMap = new Tilemap(new Point(palletTileTypes.GetLength(0), palletTileTypes.GetLength(1)), tileSize, palletTileTypes, canvasPos, sourceRectangles, spriteSheet);
        }

        private Point getHoveredTile(Tilemap map)
        {
            for(int x = 0; x < map.Size.X; x++)
            {
                for(int y = 0; y < map.Size.Y; y++)
                {
                    if (DoesHovorRect(map.Tiles[x,y].Hitbox))
                    {
                        return new Point(x, y);
                    }
                }
            }
            return new Point(-1,-1);
        }

        public override void Update()
        {
            map.MouseHoverPos = getHoveredTile(map);
            palletMap.MouseHoverPos = getHoveredTile(palletMap);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);
            palletMap.Draw(spriteBatch);
        }
    }
}
