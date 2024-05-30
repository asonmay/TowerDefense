using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class Tilemap
    {
        public Point Size;
        public Point TileSize;
        public Tile[,] Tiles;
        private Vector2 mapPosition;
        private Texture2D spriteSheet;
        private Dictionary<PathTileType, Rectangle> sourceRectangles;
        public Point MouseHoverPos;

        public Tilemap(Point size, Point tileSize, PathTileType[,] TileTypes, Vector2 mapPosition, Dictionary<PathTileType, Rectangle> sourceRectangles, Texture2D spriteSheet)
        {
            Size = size;
            TileSize = tileSize;
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
                    if (Tiles[x, y] is PathTile)
                    {
                        PathTile temp = (PathTile)Tiles[x, y];
                        temp.neighbors = getNeighbors(new Point(x, y), TileTypes);
                    }
                }
            }
        }

        private PathTile[] getNeighbors(Point pos, PathTileType[,] TileTypes)
        {
            List<PathTile> neighbors = new List<PathTile>();

            if(pos.X + 1 < Size.X && TileTypes[pos.X + 1, pos.Y] != PathTileType.None)
            {
                neighbors.Add((PathTile)Tiles[pos.X + 1, pos.Y]);
            }
            if (pos.X - 1 > 0 && TileTypes[pos.X - 1, pos.Y] != PathTileType.None)
            {
                neighbors.Add((PathTile)Tiles[pos.X - 1, pos.Y]);
            }
            if (pos.Y + 1 < Size.Y && TileTypes[pos.X, pos.Y + 1] != PathTileType.None)
            {
                neighbors.Add((PathTile)Tiles[pos.X, pos.Y + 1]);
            }
            if (pos.Y - 1 > 0 && TileTypes[pos.X, pos.Y - 1] != PathTileType.None)
            {
                neighbors.Add((PathTile)Tiles[pos.X, pos.Y - 1]);
            }

            return neighbors.ToArray();         
        }

        private void setTileMap(PathTileType[,] TileTypes)
        {
            Tiles = new Tile[Size.X, Size.Y];
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    setTile(new Point(x,y), TileTypes);
                }
            }
        }

        private void setTile(Point pos, PathTileType[,] TileTypes)
        {
            Vector2 position = new Vector2(mapPosition.X + pos.X * TileSize.X, mapPosition.Y + pos.Y * TileSize.Y);
            float scale = (float)TileSize.X / sourceRectangles[PathTileType.None].Width;
            if (TileTypes[pos.X, pos.Y] == PathTileType.None)
            {
                Tiles[pos.X, pos.Y] = new Tile(position, Color.White, scale, 0, sourceRectangles[PathTileType.None], Vector2.Zero, spriteSheet, new Point(pos.X, pos.Y));
            }
            else
            {
                Tiles[pos.X, pos.Y] = new PathTile(position, Color.White, scale, 0, sourceRectangles[TileTypes[pos.X, pos.Y]], Vector2.Zero, spriteSheet, new Point(pos.X, pos.Y), TileTypes[pos.X, pos.Y]);
            }
        }

        public void UpdateMouse()
        {
            Point mousePos = Mouse.GetState().Position;
            Rectangle mouseHitbox = new Rectangle(mousePos, new Point(1, 1));

            MouseHoverPos = new Point(200, 200);
            for (int x = 0; x < Size.X; x++)
            {
                for(int y = 0; y < Size.Y; y++)
                {
                    Rectangle tileHitbox = new Rectangle(Tiles[x, y].position.ToPoint(), TileSize);
                    if (mouseHitbox.Intersects(tileHitbox))
                    {
                        MouseHoverPos = new Point(x, y);
                    }
                }
            }
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int x = 0; x < Size.X; x++)
            {
                for(int y = 0; y < Size.Y; y++)
                {
                    Tiles[x, y].Draw(spriteBatch);
                    if (new Point(x, y) == MouseHoverPos)
                    {
                        spriteBatch.DrawRectangle(Tiles[x, y].Hitbox, Color.Red, 4);
                    }
                }
            }
        }
    }
}
