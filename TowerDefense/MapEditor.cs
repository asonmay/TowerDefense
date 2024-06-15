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
        public TileMapProfile profile;
        private Point mapHovorPos;

        public TileMapProfile original;

        private PathTileType[,] palletTileTypes;
        private TileMapSpecs palletSpecs;
        private TileMapProfile pallet;
        private Point palletHovorPos;

        private PathTileType selectedType;
        private Button exitButton;
        private TextBox nameTextBox;
        private Vector2 canvasPos;
        public bool isNew;

        public MapEditor(Vector2 canvasPos, SpriteFont font, Point saveButtonPos, Texture2D spriteSheet, Dictionary<PathTileType, Rectangle> sourceRectangles, Texture2D background)
        {
            palletTileTypes = new PathTileType[,]
            {
                { PathTileType.LeftUp, PathTileType.Left, PathTileType.LeftDown, PathTileType.None },
                { PathTileType.Up, PathTileType.Center, PathTileType.Down , PathTileType.None},
                { PathTileType.RightUp, PathTileType.Right, PathTileType.RightDown, PathTileType.None },
            };           
            exitButton = new Button(Color.Red, "exit", saveButtonPos, font, Color.Black);

            this.spriteSheet = spriteSheet;
            this.sourceRectangles = sourceRectangles;
            this.background = background;
            this.canvasPos = canvasPos;
        }

        public void Initialize(TileMapProfile profile, bool isNew)
        {
            this.profile = profile;
            palletSpecs = new TileMapSpecs(profile.specs.TileSize, profile.specs.SpriteSheet, profile.specs.SourceRectangles);
            pallet = new TileMapProfile(palletTileTypes, palletSpecs, "pallet", new Point(palletTileTypes.GetLength(0), palletTileTypes.GetLength(1)), canvasPos);
            this.isNew = isNew;
            original = profile;
            if(isNew)
            {
                clear();
            }
        }

        private Point getHoveredTile(TileMapProfile map)
        {
            for(int x = 0; x < profile.Size.X; x++)
            {
                for(int y = 0; y < profile.Size.Y; y++)
                {
                    Rectangle hitbox = new Rectangle((int)(x * map.specs.TileSize.X + map.MapPosition.X), (int)(y * map.specs.TileSize.Y + map.MapPosition.Y), map.specs.TileSize.X, map.specs.TileSize.Y);
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
            mapHovorPos = getHoveredTile(profile);
            palletHovorPos = getHoveredTile(pallet);

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

        private void clear()
        {
            profile.tileTypes = new PathTileType[profile.Size.X, profile.Size.Y];
        }

        public override ScreenTypes ReturnType()
        {
            return ScreenTypes.MapEditor;
        }

        public ScreenTypes GetScreenToSwitch()
        {
            if(exitButton.isClicked())
            {               
                return ScreenTypes.MapEditorMenu;
            }
            return ScreenTypes.MapEditor;
        }

        private void DrawMap(TileMapProfile profile, PathTileType[,] types, SpriteBatch sp, Point hovorPos)
        {
            for (int x = 0; x < profile.Size.X; x++)
            {
                for (int y = 0; y < profile.Size.Y; y++)
                {
                    Vector2 pos = new Vector2(x * profile.specs.TileSize.X + profile.MapPosition.X, y * profile.specs.TileSize.Y + profile.MapPosition.Y);
                    float scale = (float)profile.specs.TileSize.X / profile.specs.SourceRectangles[PathTileType.None].Width;

                    sp.Draw(profile.specs.SpriteSheet, pos, profile.specs.SourceRectangles[types[x,y]], Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);

                    if (hovorPos == new Point(x, y))
                    {
                        sp.DrawRectangle(new Rectangle(pos.ToPoint(), profile.specs.TileSize), Color.Red, 4);
                    }

                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawMap(profile, profile.tileTypes, spriteBatch, mapHovorPos);
            DrawMap(pallet, palletTileTypes, spriteBatch, palletHovorPos);

            exitButton.Draw(spriteBatch);
        }
    }
}
