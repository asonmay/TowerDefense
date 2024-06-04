using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class MapEditor : Screen
    {
        private Point screenSize;
        private PathTileType[,] tileTypes;
        private TileMapSpecs specs;
        private Point mapHovorPos;
        private PathTileType[,] palletTileTypes;
        private TileMapSpecs palletSpecs;
        private Point palletHovorPos;
        private PathTileType selectedType;
        private Button exitButton;

        public MapEditor(Vector2 mapPos, Point size, Point tileSize, Dictionary<PathTileType, Rectangle> sourceRectangles, Texture2D spriteSheet, Point screenSize, Vector2 canvasPos, SpriteFont font, Point saveButtonPos)
        {
            tileTypes = new PathTileType[size.X, size.Y];
            specs = new TileMapSpecs(size, tileSize, mapPos, spriteSheet, sourceRectangles);
            this.screenSize = screenSize;

            palletTileTypes = new PathTileType[,]
            {
                { PathTileType.LeftUp, PathTileType.Left, PathTileType.LeftDown, PathTileType.None },
                { PathTileType.Up, PathTileType.Center, PathTileType.Down , PathTileType.None},
                { PathTileType.RightUp, PathTileType.Right, PathTileType.RightDown, PathTileType.None },
            };

            palletSpecs = new TileMapSpecs(new Point(palletTileTypes.GetLength(0), palletTileTypes.GetLength(1)), tileSize, canvasPos, spriteSheet, sourceRectangles);
            exitButton = new Button(Color.Red, "exit", saveButtonPos, font, Color.Black);
        }

        private Point getHoveredTile(TileMapSpecs map)
        {
            for(int x = 0; x < specs.Size.X; x++)
            {
                for(int y = 0; y < specs.Size.Y; y++)
                {
                    Rectangle hitbox = new Rectangle((int)(x * map.TileSize.X + map.MapPosition.X), (int)(y * map.TileSize.Y + map.MapPosition.Y), map.TileSize.X, map.TileSize.Y);
                    if (DoesHovorRect(hitbox))
                    {
                        return new Point(x, y);
                    }
                }
            }
            return new Point(-1,-1);
        }

        public override ScreenTypes Update()
        {
            mapHovorPos = getHoveredTile(specs);
            palletHovorPos = getHoveredTile(palletSpecs);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (palletHovorPos.X >= 0)
                {
                    selectedType = palletTileTypes[palletHovorPos.X, palletHovorPos.Y];
                }

                if (mapHovorPos.X >= 0)
                {
                    tileTypes[mapHovorPos.X, mapHovorPos.Y] = selectedType;
                }
            }
            return GetScreenToSwitch();
        }

        public ScreenTypes GetScreenToSwitch()
        {
            if(exitButton.isClicked())
            {
                return ScreenTypes.MapEditorMenu;
            }
            return ScreenTypes.MapEditor;
        }

        private void DrawMap(TileMapSpecs specs, PathTileType[,] types, SpriteBatch sp, Point hovorPos)
        {
            for (int x = 0; x < specs.Size.X; x++)
            {
                for (int y = 0; y < specs.Size.Y; y++)
                {
                    Vector2 pos = new Vector2(x * specs.TileSize.X + specs.MapPosition.X, y * specs.TileSize.Y + specs.MapPosition.Y);
                    float scale = (float)specs.TileSize.X / specs.SourceRectangles[PathTileType.None].Width;

                    sp.Draw(specs.SpriteSheet, pos, specs.SourceRectangles[types[x,y]], Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);

                    if (hovorPos == new Point(x, y))
                    {
                        sp.DrawRectangle(new Rectangle(pos.ToPoint(), specs.TileSize), Color.Red, 4);
                    }

                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawMap(specs, tileTypes, spriteBatch, mapHovorPos);
            DrawMap(palletSpecs, palletTileTypes, spriteBatch, palletHovorPos);
        }
    }
}
