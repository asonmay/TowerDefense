using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

//https://free-game-assets.itch.io/free-field-enemies-pixel-art-for-tower-defense
//https://free-game-assets.itch.io/free-archer-towers-pixel-art-for-tower-defense

namespace TowerDefense
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ScreenManager screen;
        private List<TileMapProfile> savedMaps;
        private List<MapEditorMenuItem> items;
        private SpriteFont buttonFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            graphics.PreferredBackBufferWidth = 796;
            graphics.PreferredBackBufferHeight = 660;
            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            screen = ScreenManager.Instance;
            buttonFont = Content.Load<SpriteFont>("ButtonFont");

            Texture2D spriteSheet = Content.Load<Texture2D>("FieldsTileset");

            Dictionary<PathTileType, Rectangle> sourceRectangles = new Dictionary<PathTileType, Rectangle>
            {
                [PathTileType.None] = new Rectangle(160, 128, 32, 32),
                [PathTileType.Center] = new Rectangle(64, 64, 32, 32),
                [PathTileType.Right] = new Rectangle(0, 32, 32, 32),
                [PathTileType.Left] = new Rectangle(128, 32, 32, 32),
                [PathTileType.LeftUp] = new Rectangle(32, 32, 32, 32),
                [PathTileType.RightUp] = new Rectangle(96, 32, 32, 32),
                [PathTileType.LeftDown] = new Rectangle(32, 96, 32, 32),
                [PathTileType.RightDown] = new Rectangle(96, 96, 32, 32),
                [PathTileType.Up] = new Rectangle(64, 128, 32, 32),
                [PathTileType.Down] = new Rectangle(64, 0, 32, 32),
            };

            Texture2D background = Content.Load<Texture2D>("another zanlin");
            SpriteFont titleFont = Content.Load<SpriteFont>("TitleFont");

            savedMaps = new List<TileMapProfile>((TileMapProfile[])JsonSerializer.Deserialize(File.ReadAllText("Z://TowerDefense//TowerDefense//bin//Debug//net6.0//SavedMaps.Json"), typeof(TileMapProfile[])));
            UpdateSavedMaps(savedMaps);

            Dictionary<ScreenTypes, Screen> screens = new Dictionary<ScreenTypes, Screen>
            {
                [ScreenTypes.HomeScreen] = new HomeScreen(background, new Point(100, 500), new Point(200, 500), Color.Red, Color.Red, buttonFont, background, titleFont, GraphicsDevice.Viewport),
                [ScreenTypes.MapEditorMenu] = new MapEditorMenu(spriteSheet, sourceRectangles, new Rectangle(100, 100, 596, 460), Color.WhiteSmoke, new Point(20, 20), new Point(200, 10), buttonFont, new Point(470, 10), background),
                [ScreenTypes.MapEditor] = new MapEditor(new Vector2(704, 64), buttonFont, new Point(32,10), spriteSheet, sourceRectangles, background)
            };

            screen.Initilize(screens);
        }

        public List<MapEditorMenuItem> UpdateSavedMaps(List<TileMapProfile> profiles)
        {
            List<MapEditorMenuItem> temp = new List<MapEditorMenuItem>();
            for (int i = 0; i < temp.Count; i++)
            {
                temp.Add(new MapEditorMenuItem(profiles[i], new Point(380, 50), Color.Black, buttonFont, new Point(10, 10), new Point(110, 110 + i * 60), new Point(110, 110 + i * 60)));
            }
            savedMaps = profiles;
            return temp;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            screen.Update(savedMaps);

            base.Update(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            string maps = JsonSerializer.Serialize(savedMaps.ToArray());
            File.WriteAllText("SavedMaps.Json",maps);

            base.OnExiting(sender, args);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            screen.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
