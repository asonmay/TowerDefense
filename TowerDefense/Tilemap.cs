using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TowerDefense
{
    public struct TileMapProfile
    {
        [JsonIgnore]
        public TileTypes[,] TileTypes { get; set; }
        [JsonIgnore]
        public Point Size { get; set; }

        [JsonIgnore]
        public Vector2 MapPosition { get; set; }

        public string Name { get; set; }

        public int Width
        {
            get => Size.X;
            set => Size = new Point(value, Size.Y);
        }
        public int Height
        {
            get => Size.Y;
            set => Size = new Point(Size.X, value);
        }
        public float MapPositionX
        {
            get => MapPosition.X;
            set => MapPosition = new Vector2(value, MapPosition.Y);
        }

        public float MapPositionY
        {
            get => MapPosition.Y;
            set => MapPosition = new Vector2(MapPosition.X, value);
        }

        public TileTypes[] JsonTileTypes
        {
            get
            {
                TileTypes[] temp = new TileTypes[Size.X * Size.Y];
                for (int x = 0; x < Size.X; x++)
                {
                    for (int y = 0; y < Size.Y; y++)
                    {
                        temp[y * Size.Y + x] = TileTypes[x, y];
                    }
                }
                return temp;
            }
            set
            {
                TileTypes = new TileTypes[Size.X, Size.Y];
                for (int x = 0; x < Size.X; x++)
                {
                    for (int y = 0; y < Size.Y; y++)
                    {
                        TileTypes[x,y] = value[y * Size.Y + x];
                    }
                }
            }
        }

        public TileMapProfile(TileTypes[,] tileTypes, string name, Point size, Vector2 mapPosition)
        {
            TileTypes = tileTypes;
            Name = name;
            Size = size;
            MapPosition = mapPosition;
        }

        public void DrawMap(SpriteBatch sp, Point hovorPos, TileMapSpecs specs)
        {
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    Vector2 pos = new Vector2(x * specs.TileSize.X + MapPosition.X, y * specs.TileSize.Y + MapPosition.Y);
                    float scale = (float)specs.TileSize.X / specs.SourceRectangles[TowerDefense.TileTypes.Grass].Width;
                    Color color = Color.White;

                    if (TileTypes[x,y] == TowerDefense.TileTypes.Spawner)
                    {
                        color = Color.Red;
                    }

                    sp.Draw(specs.SpriteSheet, pos, specs.SourceRectangles[TileTypes[x, y]], color, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
                }
            }

            sp.DrawRectangle(new Rectangle(new Point((int)(hovorPos.X * specs.TileSize.X + MapPosition.X), (int)(hovorPos.Y * specs.TileSize.Y + MapPosition.Y)), specs.TileSize), Color.Red, 4);
          
        }
    }

    public struct TileMapSpecs
    {       
        public Point TileSize;
        public Texture2D SpriteSheet;
        public Dictionary<TileTypes, Rectangle> SourceRectangles;

        public TileMapSpecs(Point tileSize, Texture2D spriteSheet, Dictionary<TileTypes, Rectangle> sourceRectangle)
        {
            TileSize = tileSize;
            SpriteSheet = spriteSheet;
            SourceRectangles = sourceRectangle;
        }
    }


    public class Tilemap
    {
        public TileMapSpecs Specs;
        public Vector2 mapPosition;
        public Point size;
        public Tile[,] Tiles;
        public Point StartingPoint;
        public Point EndingPoint;

        public Tilemap(Point size, Point tileSize, TileTypes[,] tileTypes, Vector2 mapPosition, Dictionary<TileTypes, Rectangle> sourceRectangles, Texture2D spriteSheet)
        {
            this.size = size;
            this.mapPosition = mapPosition;
            Specs = new TileMapSpecs(tileSize, spriteSheet, sourceRectangles);
            SetTileMap(tileTypes);
            SetNeighbors(tileTypes);

            for(int x = 0; x < Tiles.GetLength(0); x++)
            {
                for(int y = 0; y < Tiles.GetLength(1); y++)
                {
                    if (Tiles[x,y].Type == TileTypes.Spawner)
                    {
                        StartingPoint = new Point(x, y);
                    }
                    if (Tiles[x, y].Type == TileTypes.End)
                    {
                        EndingPoint = new Point(x, y);
                    }
                }
            }
        }

        private void SetNeighbors(TileTypes[,] TileTypes)
        {
            for (int x = 0; x < TileTypes.GetLength(0); x++)
            {
                for (int y = 0; y < TileTypes.GetLength(1); y++)
                {
                    if (Tiles[x, y] is PathTile)
                    {
                        PathTile temp = (PathTile)Tiles[x, y];
                        temp.Neighbors = GetNeighbors(new Point(x, y), TileTypes);
                    }
                }
            }
        }

        private PathTile[] GetNeighbors(Point pos, TileTypes[,] TileTypes)
        {
            List<PathTile> neighbors = new List<PathTile>();

            if(pos.X + 1 < size.X && TileTypes[pos.X + 1, pos.Y] != TowerDefense.TileTypes.Grass)
            {
                neighbors.Add((PathTile)Tiles[pos.X + 1, pos.Y]);
            }
            if (pos.X - 1 > 0 && TileTypes[pos.X - 1, pos.Y] != TowerDefense.TileTypes.Grass)
            {
                neighbors.Add((PathTile)Tiles[pos.X - 1, pos.Y]);
            }
            if (pos.Y + 1 < size.Y && TileTypes[pos.X, pos.Y + 1] != TowerDefense.TileTypes.Grass)
            {
                neighbors.Add((PathTile)Tiles[pos.X, pos.Y + 1]);
            }
            if (pos.Y - 1 > 0 && TileTypes[pos.X, pos.Y - 1] != TowerDefense.TileTypes.Grass)
            {
                neighbors.Add((PathTile)Tiles[pos.X, pos.Y - 1]);
            }

            return neighbors.ToArray();         
        }

        private void SetTileMap(TileTypes[,] TileTypes)
        {
            Tiles = new Tile[size.X, size.Y];
            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    SetTile(new Point(x,y), TileTypes);
                }
            }
        }

        private void SetTile(Point pos, TileTypes[,] TileTypes)
        {
            Vector2 position = new Vector2(mapPosition.X + pos.X * Specs.TileSize.X, mapPosition.Y + pos.Y * Specs.TileSize.Y);
            float scale = (float)Specs.TileSize.X / Specs.SourceRectangles[TowerDefense.TileTypes.Grass].Width;

            if (TileTypes[pos.X, pos.Y] == TowerDefense.TileTypes.Grass)
            {
                Tiles[pos.X, pos.Y] = new Tile(position, Color.White, scale, 0, Specs.SourceRectangles[TowerDefense.TileTypes.Grass], Vector2.Zero, Specs.SpriteSheet, new Point(pos.X, pos.Y), TileTypes[pos.X, pos.Y]);
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
            for(int x = 0; x < size.X; x++)
            {
                for(int y = 0; y < size.Y; y++)
                {
                    Tiles[x, y].Draw(spriteBatch);
                }
            }
        }
    }
}
