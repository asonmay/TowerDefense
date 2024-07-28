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
        private TileTypes[,] palletTileTypes;
        private TileMapSpecs specs;
        private TileMapProfile pallet;
        private Point palletHovorPos;
        private TileTypes selectedType;
        private Button exitButton;
        private TextBox nameTextBox;
        private Vector2 canvasPos;
        public bool isNew;

        public MapEditor(Vector2 canvasPos, SpriteFont font, Point saveButtonPos, TileMapSpecs specs, Texture2D spriteSheet, Dictionary<TileTypes, Rectangle> sourceRectangles, Texture2D background)
        {
            palletTileTypes = new TileTypes[,]
            {
                { TileTypes.LeftUp, TileTypes.Left, TileTypes.LeftDown, TileTypes.Grass, TileTypes.Spawner},
                { TileTypes.Up, TileTypes.Center, TileTypes.Down , TileTypes.Grass, TileTypes.End},
                { TileTypes.RightUp, TileTypes.Right, TileTypes.RightDown, TileTypes.Grass, TileTypes.Grass},
            };

            exitButton = new Button(Color.Red, "exit", saveButtonPos, font, Color.Black);
            this.specs = specs;
            this.spriteSheet = spriteSheet;
            this.sourceRectangles = sourceRectangles;
            this.background = background;
            this.canvasPos = canvasPos;
        }

        public void Initialize(TileMapProfile profile, bool isNew)
        {
            this.profile = profile;      
            pallet = new TileMapProfile(palletTileTypes, "pallet", new Point(palletTileTypes.GetLength(0), palletTileTypes.GetLength(1)), canvasPos);
            this.isNew = isNew;
            original = profile;
            if(isNew)
            {
                clear();
            }
        }

        private Point GetHoveredTile(TileMapProfile map)
        {
            for(int x = 0; x < profile.Size.X; x++)
            {
                for(int y = 0; y < profile.Size.Y; y++)
                {
                    Rectangle hitbox = new Rectangle((int)(x * specs.TileSize.X + map.MapPosition.X), (int)(y * specs.TileSize.Y + map.MapPosition.Y), specs.TileSize.X, specs.TileSize.Y);
                    if (DoesHovorRect(hitbox))
                    {
                        return new Point(x, y);
                    }
                }
            }
            return new Point(int.MaxValue / 32,int.MaxValue / 32);
        }

        public override ScreenTypes Update(GameTime gameTime)
        {
            mapHovorPos = GetHoveredTile(profile);
            palletHovorPos = GetHoveredTile(pallet);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (palletHovorPos.X != int.MaxValue / 32)
                {
                    selectedType = palletTileTypes[palletHovorPos.X, palletHovorPos.Y];
                }

                if (mapHovorPos.X != int.MaxValue / 32)
                {
                    profile.TileTypes[mapHovorPos.X, mapHovorPos.Y] = selectedType;
                }
            }

            return GetScreenToSwitch();
        }

        private void clear()
        {
            profile.TileTypes = new TileTypes[profile.Size.X, profile.Size.Y];
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

       

        public override void Draw(SpriteBatch spriteBatch)
        {
            profile.DrawMap(spriteBatch, mapHovorPos, specs);
            pallet.DrawMap(spriteBatch, palletHovorPos, specs);

            exitButton.Draw(spriteBatch);
        }
    }
}
