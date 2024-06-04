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
    public struct TileMapProfile
    {
        private PathTileType[,] tileTypes;
        private TileMapSpecs specs;
        public string name;
        
        public TileMapProfile(PathTileType[,] tileTypes, TileMapSpecs specs, string name)
        {
            this.tileTypes = tileTypes;
            this.specs = specs;
            this.name = name;
        }
    }

    public struct TileMapSpecs
    {
        public Point Size;
        public Point TileSize;
        public Vector2 MapPosition;
        public Texture2D SpriteSheet;
        public Dictionary<PathTileType, Rectangle> SourceRectangles;

        public TileMapSpecs(Point size, Point tileSize, Vector2 mapPosition, Texture2D spriteSheet, Dictionary<PathTileType, Rectangle> sourceRectangle)
        {
            Size = size;
            TileSize = tileSize;
            MapPosition = mapPosition;
            SpriteSheet = spriteSheet;
            SourceRectangles = sourceRectangle;
        }
    }


    public class Tilemap
    {
        public TileMapSpecs Specs;
        public Tile[,] Tiles;

        public Tilemap(Point size, Point tileSize, PathTileType[,] TileTypes, Vector2 mapPosition, Dictionary<PathTileType, Rectangle> sourceRectangles, Texture2D spriteSheet)
        {
            Specs = new TileMapSpecs(size, tileSize, mapPosition, spriteSheet, sourceRectangles);
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

            if(pos.X + 1 < Specs.Size.X && TileTypes[pos.X + 1, pos.Y] != PathTileType.None)
            {
                neighbors.Add((PathTile)Tiles[pos.X + 1, pos.Y]);
            }
            if (pos.X - 1 > 0 && TileTypes[pos.X - 1, pos.Y] != PathTileType.None)
            {
                neighbors.Add((PathTile)Tiles[pos.X - 1, pos.Y]);
            }
            if (pos.Y + 1 < Specs.Size.Y && TileTypes[pos.X, pos.Y + 1] != PathTileType.None)
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
            Tiles = new Tile[Specs.Size.X, Specs.Size.Y];
            for (int x = 0; x < Specs.Size.X; x++)
            {
                for (int y = 0; y < Specs.Size.Y; y++)
                {
                    setTile(new Point(x,y), TileTypes);
                }
            }
        }

        private void setTile(Point pos, PathTileType[,] TileTypes)
        {
            Vector2 position = new Vector2(Specs.MapPosition.X + pos.X * Specs.TileSize.X, Specs.MapPosition.Y + pos.Y * Specs.TileSize.Y);
            float scale = (float)Specs.TileSize.X / Specs.SourceRectangles[PathTileType.None].Width;

            if (TileTypes[pos.X, pos.Y] == PathTileType.None)
            {
                Tiles[pos.X, pos.Y] = new Tile(position, Color.White, scale, 0, Specs.SourceRectangles[PathTileType.None], Vector2.Zero, Specs.SpriteSheet, new Point(pos.X, pos.Y));
            }
            else
            {
                Tiles[pos.X, pos.Y] = new PathTile(position, Color.White, scale, 0, Specs.SourceRectangles[TileTypes[pos.X, pos.Y]], Vector2.Zero, Specs.SpriteSheet, new Point(pos.X, pos.Y), TileTypes[pos.X, pos.Y]);
            }
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int x = 0; x < Specs.Size.X; x++)
            {
                for(int y = 0; y < Specs.Size.Y; y++)
                {
                    Tiles[x, y].Draw(spriteBatch);
                }
            }
        }
    }
}
