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

            Dictionary<PathTileType, Rectangle>  sourceRectangles = new Dictionary<PathTileType, Rectangle>
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
            screen = new HomeScreen(background, new Point(100, 100), new Point(200, 100), Color.Red, Color.Red, buttonFont);

            TileMapProfile savedMaps = new TileMapProfile[0];

            screen.Initilize(screen, spriteFont, spriteSheet, sourceRectangles, savedMaps, background);
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
