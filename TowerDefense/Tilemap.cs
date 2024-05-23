using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class Tilemap
    {
        private Point size;
        private Point tileSize;
        private Tile[,] tiles;
        private Vector2 mapPosition;
        private Texture2D spriteSheet;
        private Dictionary<PathTileType, Rectangle> sourceRectangles;
        private Point mouseHoverPos;

        public Tilemap(Point size, Point tileSize, PathTileType[,] TileTypes, Vector2 mapPosition, Dictionary<PathTileType, Rectangle> sourceRectangles, Texture2D spriteSheet)
        {
            this.size = size;
            this.tileSize = tileSize;
            this.mapPosition = mapPosition;
            this.sourceRectangles = sourceRectangles;
            this.spriteSheet = spriteSheet;

            setTileMap(TileTypes);
            setNeighbors(TileTypes);
        }

        private void setNeighbors(PathTileType[,] TileTypes)
        {
            for (int x = 0; x < TileTypes.GetLength(0); x++)
            {
                for (int y = 0; y < TileTypes.GetLength(1); y++)
                {
                    if (tiles[x, y] is PathTile)
                    {
                        PathTile temp = (PathTile)tiles[x, y];
                        temp.neighbors = getNeighbors(new Point(x, y), TileTypes);
                    }
                }
            }
        }

        private PathTile[] getNeighbors(Point pos, PathTileType[,] TileTypes)
        {
            List<PathTile> neighbors = new List<PathTile>();

            if(pos.X + 1 < size.X && TileTypes[pos.X + 1, pos.Y] != PathTileType.None)
            {
                neighbors.Add((PathTile)tiles[pos.X + 1, pos.Y]);
            }
            if (pos.X - 1 > 0 && TileTypes[pos.X - 1, pos.Y] != PathTileType.None)
            {
                neighbors.Add((PathTile)tiles[pos.X - 1, pos.Y]);
            }
            if (pos.Y + 1 < size.Y && TileTypes[pos.X, pos.Y + 1] != PathTileType.None)
            {
                neighbors.Add((PathTile)tiles[pos.X, pos.Y + 1]);
            }
            if (pos.Y - 1 > 0 && TileTypes[pos.X, pos.Y - 1] != PathTileType.None)
            {
                neighbors.Add((PathTile)tiles[pos.X, pos.Y - 1]);
            }

            return neighbors.ToArray();         
        }

        private void setTileMap(PathTileType[,] TileTypes)
        {
            tiles = new Tile[size.X, size.Y];
            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    setTile(new Point(x,y), TileTypes);
                }
            }
        }

        private void setTile(Point pos, PathTileType[,] TileTypes)
        {
            Vector2 position = new Vector2(mapPosition.X + pos.X * tileSize.X, mapPosition.Y + pos.Y * tileSize.Y);
            float scale = (float)tileSize.X / sourceRectangles[PathTileType.None].Width;
            if (TileTypes[pos.X, pos.Y] == PathTileType.None)
            {
                tiles[pos.X, pos.Y] = new Tile(position, Color.White, scale, 0, sourceRectangles[PathTileType.None], Vector2.Zero, spriteSheet, new Point(pos.X, pos.Y));
            }
            else
            {
                tiles[pos.X, pos.Y] = new PathTile(position, Color.White, scale, 0, sourceRectangles[TileTypes[pos.X, pos.Y]], Vector2.Zero, spriteSheet, new Point(pos.X, pos.Y), TileTypes[pos.X, pos.Y]);
            }
        }

        public void UpdateMouse()
        {
            Point mousePos = Mouse.GetState().Position;
            Rectangle mouseHitbox = new Rectangle(mousePos, new Point(1, 1));

            mouseHoverPos = new Point(200, 200);
            for (int x = 0; x < size.X; x++)
            {
                for(int y = 0; y < size.Y; y++)
                {
                    Rectangle tileHitbox = new Rectangle(tiles[x, y].position.ToPoint(), tileSize);
                    if (mouseHitbox.Intersects(tileHitbox))
                    {
                        mouseHoverPos = new Point(x, y);
                    }
                }
            }
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int x = 0; x < size.X; x++)
            {
                for(int y = 0; y < size.Y; y++)
                {
                    if(new Point(x,y) == mouseHoverPos)
                    {

                    }
                    tiles[x, y].Draw(spriteBatch);
                }
            }
        }
    }
}
