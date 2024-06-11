using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

//https://free-game-assets.itch.io/free-field-enemies-pixel-art-for-tower-defense
//https://free-game-assets.itch.io/free-archer-towers-pixel-art-for-tower-defense

namespace TowerDefense
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private TileMapProfile selectedMap;
        private ScreenManager screen;

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

            SpriteFont buttonFont = Content.Load<SpriteFont>("ButtonFont");
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

            TileMapProfile[] savedMaps = new TileMapProfile[0];
            MapEditorMenuItem[] items = new MapEditorMenuItem[savedMaps.Length];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new MapEditorMenuItem(savedMaps[i], new Point(380, 50), Color.Black, buttonFont, new Point(10, 20), new Point(100, 20));
            }

            Dictionary<ScreenTypes, Screen> screens = new Dictionary<ScreenTypes, Screen>
            {
                [ScreenTypes.HomeScreen] = new HomeScreen(background, new Point(100, 100), new Point(200, 100), Color.Red, Color.Red, buttonFont, spriteSheet, sourceRectangles, savedMaps, background),
                [ScreenTypes.MapEditorMenu] = new MapEditorMenu(spriteSheet, sourceRectangles, new Rectangle(100, 100, 596, 460), Color.WhiteSmoke, items, new Point(20, 20), new Point(200, 10), buttonFont, new Point(470, 10), background),
                [ScreenTypes.MapEditor] = new MapEditor()
            };

            screen.Initilize(screens);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            base.Update(gameTime);
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
