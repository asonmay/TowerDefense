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
        private TileMapProfile profile;
        private Point mapHovorPos;

        private PathTileType[,] palletTileTypes;
        private TileMapSpecs palletSpecs;
        private Point palletHovorPos;

        private PathTileType selectedType;
        private Button exitButton;
        private TextBox nameTextBox;

        public MapEditor(Vector2 canvasPos, SpriteFont font, Point saveButtonPos, Texture2D spriteSheet, Dictionary<PathTileType, Rectangle> sourceRectangles, TileMapProfile[] savedMaps, Texture2D background)
        {
            palletTileTypes = new PathTileType[,]
            {
                { PathTileType.LeftUp, PathTileType.Left, PathTileType.LeftDown, PathTileType.None },
                { PathTileType.Up, PathTileType.Center, PathTileType.Down , PathTileType.None},
                { PathTileType.RightUp, PathTileType.Right, PathTileType.RightDown, PathTileType.None },
            };

            palletSpecs = new TileMapSpecs(new Point(palletTileTypes.GetLength(0), palletTileTypes.GetLength(1)), profile.specs.TileSize, canvasPos, profile.specs.SpriteSheet, profile.specs.SourceRectangles);
            exitButton = new Button(Color.Red, "exit", saveButtonPos, font, Color.Black);

            this.spriteSheet = spriteSheet;
            this.sourceRectangles = sourceRectangles;
            this.savedMaps = savedMaps;
            this.background = background;
            //nameTextBox = new TextBox(new Rectangle(100, 10, 100, 50));
        }

        public void Initialize(TileMapProfile profile)
        {
            this.profile = profile;
        }

        private Point getHoveredTile(TileMapSpecs map)
        {
            for(int x = 0; x < profile.specs.Size.X; x++)
            {
                for(int y = 0; y < profile.specs.Size.Y; y++)
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
            mapHovorPos = getHoveredTile(profile.specs);
            palletHovorPos = getHoveredTile(palletSpecs);
            nameTextBox.FocusOnTextbox();
            profile.name = nameTextBox.text;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (palletHovorPos.X >= 0)
                {
                    selectedType = palletTileTypes[palletHovorPos.X, palletHovorPos.Y];
                }

                if (mapHovorPos.X >= 0)
                {
                    profile.tileTypes[mapHovorPos.X, mapHovorPos.Y] = selectedType;
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
            DrawMap(profile.specs, profile.tileTypes, spriteBatch, mapHovorPos);
            DrawMap(palletSpecs, palletTileTypes, spriteBatch, palletHovorPos);

            exitButton.Draw(spriteBatch);
        }
    }
}
