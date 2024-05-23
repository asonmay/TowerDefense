using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

//https://free-game-assets.itch.io/free-field-enemies-pixel-art-for-tower-defense
//https://free-game-assets.itch.io/free-archer-towers-pixel-art-for-tower-defense

namespace TowerDefense
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Tilemap mapEditor;
        private Dictionary<PathTileType, Rectangle> sourceRectangles;
        private Texture2D texture;
        private PathTileType[,] tileType;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            texture = Content.Load<Texture2D>("FieldsTileset");
            sourceRectangles = new Dictionary<PathTileType, Rectangle>
            {
                [PathTileType.None] = new Rectangle(160, 128, 32, 32),
                [PathTileType.Center] = new Rectangle(64, 64, 32, 32),
                [PathTileType.Right] = new Rectangle(0, 32, 32, 32),
                [PathTileType.Left] = new Rectangle(128, 32, 32, 32),
            };

            tileType = getBlankMap(new Point(8, 8));
            mapEditor = new Tilemap(new Point(8,8), new Point(48,48), tileType, Vector2.Zero, sourceRectangles,texture);
        }

        private PathTileType[,] getBlankMap(Point size)
        {
            PathTileType[,] map = new PathTileType[size.X, size.Y];

            for(int x = 0; x < size.X; x++)
            {
                for(int y = 0; y < size.Y; y++)
                {
                    map[x, y] = PathTileType.None;
                }
            }

            return map;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            test.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
