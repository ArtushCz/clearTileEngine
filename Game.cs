using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TestGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        private Map gameMap;
        private Camera camera;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;
        SpriteFont spriteFont;
        private SpriteFont spriteFontBig;


        public const int WINDOW_HEIGHT = 720;
        public const int WINDOW_WIDTH = 1280;
        private Texture2D consoleText;
        private bool debug = false;
        public GameMouse gameMouse;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferMultiSampling = true;
            IsMouseVisible = true;

            // graphics.ToggleFullScreen();

            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            gameMap = new Map(spriteBatch, graphics);
            camera = new Camera(WINDOW_HEIGHT, WINDOW_WIDTH);
            gameMouse = new GameMouse();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteFont = Content.Load<SpriteFont>("font");
            spriteFontBig = Content.Load<SpriteFont>("fontBig");

            consoleText = Content.Load<Texture2D>("console");
            gameMap.load(Content, spriteBatch, spriteFont, spriteFontBig);
            // TODO: use this.Content to load your game content here



        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            gameMouse.state = Mouse.GetState();

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

           
            camera.controls(Keyboard.GetState().GetPressedKeys());
            gameMap.handleMouse(gameMouse, camera);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            
           
            spriteBatch.Begin();

            gameMap.drawMap(camera);

            frameCounter++;
            
            if (camera.debug)
            {
                drawConsole(spriteBatch, spriteFont);
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawConsole(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Draw(consoleText, new Rectangle(0, 0, 300, 48), Color.White);

            string tilesdr = string.Format("TilesCount: {0} ", gameMap.tilesDrawed);

            spriteBatch.DrawString(spriteFontBig, tilesdr, new Vector2(6, 16), Color.Black);
            spriteBatch.DrawString(spriteFontBig, tilesdr, new Vector2(5, 15), Color.White);

            string fps = string.Format("fps: {0} mem : {1}", frameRate, GC.GetTotalMemory(false));

            spriteBatch.DrawString(spriteFontBig, fps, new Vector2(6, 1), Color.Black);
            spriteBatch.DrawString(spriteFontBig, fps, new Vector2(5, 0), Color.White);

            string totalTiles = string.Format("Total tiles: {0} ", gameMap.mapSize);

            spriteBatch.DrawString(spriteFontBig, totalTiles, new Vector2(6, 32), Color.Black);
            spriteBatch.DrawString(spriteFontBig, totalTiles, new Vector2(5, 31), Color.White);

        }
    }
}
