﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using TowerDefense.Screens;

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
        private List<TileMapProfile> levels;

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

            string serializedData = File.ReadAllText("SavedMaps.Json");
            savedMaps = new List<TileMapProfile>((TileMapProfile[])JsonSerializer.Deserialize(serializedData, typeof(TileMapProfile[])));

            #region SourceRectangles
            Dictionary<TileTypes, Rectangle> sourceRectangles = new Dictionary<TileTypes, Rectangle>
            {
                [TileTypes.Grass] = new Rectangle(160, 128, 32, 32),
                [TileTypes.Spawner] = new Rectangle(64, 64, 32, 32),
                [TileTypes.End] = new Rectangle(64, 64, 32, 32),
                [TileTypes.Center] = new Rectangle(64, 64, 32, 32),
                [TileTypes.Right] = new Rectangle(0, 32, 32, 32),
                [TileTypes.Left] = new Rectangle(128, 32, 32, 32),
                [TileTypes.LeftUp] = new Rectangle(32, 32, 32, 32),
                [TileTypes.RightUp] = new Rectangle(96, 32, 32, 32),
                [TileTypes.LeftDown] = new Rectangle(32, 96, 32, 32),
                [TileTypes.RightDown] = new Rectangle(96, 96, 32, 32),
                [TileTypes.Up] = new Rectangle(64, 128, 32, 32),
                [TileTypes.Down] = new Rectangle(64, 0, 32, 32),
            };
            #endregion
            #region Content
            Texture2D background = Content.Load<Texture2D>("zanlindefense");
            Texture2D spriteSheet = Content.Load<Texture2D>("FieldsTileset");

            //Fonts
            SpriteFont titleFont = Content.Load<SpriteFont>("TitleFont");
            SpriteFont editorFont = Content.Load<SpriteFont>("DescriptionFont");
            SpriteFont statsFont = Content.Load<SpriteFont>("StatsFont");
            buttonFont = Content.Load<SpriteFont>("ButtonFont");

            //Enemy Images
            Texture2D enemyTexture = Content.Load<Texture2D>("another zanlin");
            Texture2D baldZanlin = Content.Load<Texture2D>("happierZanlin");

            //Tower Images
            Texture2D wailin = Content.Load<Texture2D>("wallin");
            Texture2D ariana = Content.Load<Texture2D>("ariana");
            Texture2D liz = Content.Load<Texture2D>("liz");
            Texture2D baldHakop = Content.Load<Texture2D>("BaldHakop");
            Texture2D hakopCat = Content.Load<Texture2D>("HakopCat");
            Texture2D niketaBall = Content.Load<Texture2D>("NikitaBall");
            Texture2D nekita = Content.Load<Texture2D>("NikitaPhillistine");
            Texture2D edin = Content.Load<Texture2D>("Edden");
            Texture2D gimp = Content.Load<Texture2D>("gimp");
            #endregion
            #region Towers
            Tower[] towers = 
            {
                new ShooterTower(10, 3, wailin, 0.1f, 20, Point.Zero, TimeSpan.FromMilliseconds(200), wailin, new Point(32,32), 0.15f, new Vector2(20,20), 50),
                new TargetTower(50, 10, liz, 0.05f, 30, Point.Zero, TimeSpan.FromMilliseconds(100), liz, new Point(32,32), 0.1f, new Vector2(20,20),50),
                new ShooterTower(50, 10000, ariana, 0.05f, 40, Point.Zero, TimeSpan.FromMilliseconds(10000), ariana, new Point(32,32), 0.1f, new Vector2(20,20),200),
                new ShooterTower(10, 45, baldHakop, 0.08f, 20, Point.Zero, TimeSpan.FromMilliseconds(1500), hakopCat, new Point(32,32), 0.08f, new Vector2(20,20), 50),
                new ShooterTower(10, 30, niketaBall, 0.05f, 20, Point.Zero, TimeSpan.FromMilliseconds(800), nekita, new Point(32,32), 0.025f, new Vector2(20,20), 75),
                new TargetTower(20, 100, gimp, 0.2f, 20, Point.Zero, TimeSpan.FromMilliseconds(1000), edin, new Point(32,32), 0.06f, new Vector2(20,20), 30),
            };
            #endregion
            #region Enemies
            Enemy[] enemys =
            {
                new Enemy(500, 200, 0.2f, new Rectangle(0, 0, enemyTexture.Width, enemyTexture.Height), enemyTexture, 2, "Zanlin"),
                new Enemy(300, 400, 0.2f, new Rectangle(0, 0, enemyTexture.Width, enemyTexture.Height), baldZanlin, 5, "Bald Zanlin"),
                new Enemy(400, 400, 0.4f, new Rectangle(0, 0, enemyTexture.Width, enemyTexture.Height), enemyTexture, 5, "Big Zanlin"),
                new Enemy(300, 600, Vector2.Zero, Color.Red, 0.6f, 0, new Rectangle(0,0, enemyTexture.Width, enemyTexture.Height), enemyTexture, 20, "Red Zanlin"),
                new Enemy(300, 600, 0.6f, new Rectangle(0, 0, enemyTexture.Width, enemyTexture.Height), baldZanlin, 20, "Big Bald Zanlin"),
                new Enemy(500, 700, 0.6f, new Rectangle(0, 0, enemyTexture.Width, enemyTexture.Height), baldZanlin, 25, "Big Bald Zanlin"),
            };
            #endregion;
            #region Screens
            Dictionary<ScreenTypes, Screen> screens = new Dictionary<ScreenTypes, Screen>
            {
                [ScreenTypes.HomeScreen] = new HomeScreen(background, new Point(300, 20), new Point(300, 100), Color.Red, Color.Red, buttonFont, background, titleFont, GraphicsDevice.Viewport),
                [ScreenTypes.MapEditorMenu] = new MapEditorMenu(spriteSheet, sourceRectangles, new Rectangle(100, 100, 596, 460), Color.WhiteSmoke, new Point(20, 20), new Point(200, 10), buttonFont, new Point(470, 10), background),
                [ScreenTypes.MapEditor] = new MapEditor(new Vector2(704, 64), buttonFont, new Point(32, 10), new TileMapSpecs(new Point(32, 32), spriteSheet, sourceRectangles), spriteSheet, sourceRectangles, background),
                [ScreenTypes.PlayMenu] = new PlayMenu(spriteSheet, sourceRectangles, new Rectangle(100, 100, 596, 460), Color.WhiteSmoke, new Point(20, 20), new Point(375, 10), buttonFont, background),                
                [ScreenTypes.Game] = new GameScreen(new TileMapSpecs(new Point(32, 32), spriteSheet, sourceRectangles), enemys, TimeSpan.FromMilliseconds(1250), towers, editorFont, 100, TimeSpan.FromSeconds(20)),
                [ScreenTypes.GameOver] = new GameOverScreen(titleFont, GraphicsDevice.Viewport, statsFont, 25)
            };
            #endregion;

            screen.Initilize(screens);
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            screen.Update(savedMaps, gameTime);

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
