using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TestGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        private Map gameMap;
        private Camera camera;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;
        SpriteFont spriteFont;
        private SpriteFont spriteFontBig;
        public static List<string> consoleText = new List<string>();


        public static int WINDOW_HEIGHT = 720;
        public static int WINDOW_WIDTH = 1280;
        private Texture2D consoleTexture;
        public GameMouse gameMouse;

        public MyKeyboard myKeyboard;
        private Texture2D[] mouseTextures;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferMultiSampling = true;
            IsMouseVisible = false;
           // graphics.ToggleFullScreen();

            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        public static void graphicsChange()
        {
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
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
           
            myKeyboard = new MyKeyboard(camera,gameMap);

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

            consoleTexture = Content.Load<Texture2D>("Textures/consoleSee");
            gameMouse = new GameMouse(Content.Load<Texture2D>("Textures/mouseMovementMap" + WINDOW_WIDTH + WINDOW_HEIGHT), new Point(WINDOW_WIDTH,WINDOW_HEIGHT));
            gameMap.load(Content, spriteBatch, spriteFont, spriteFontBig);
            // TODO: use this.Content to load your game content here

            mouseTextures = new Texture2D[9];
            mouseTextures[0] = Content.Load<Texture2D>("Textures/Cursor/down");
            mouseTextures[1] = Content.Load<Texture2D>("Textures/Cursor/downleft");
            mouseTextures[2] = Content.Load<Texture2D>("Textures/Cursor/downright");
            mouseTextures[3] = Content.Load<Texture2D>("Textures/Cursor/left");
            mouseTextures[4] = Content.Load<Texture2D>("Textures/Cursor/right");
            mouseTextures[5] = Content.Load<Texture2D>("Textures/Cursor/up");
            mouseTextures[6] = Content.Load<Texture2D>("Textures/Cursor/upleft");
            mouseTextures[7] = Content.Load<Texture2D>("Textures/Cursor/upright");
            mouseTextures[8] = Content.Load<Texture2D>("Textures/Cursor/staticCursor");

        }

        internal static void Log(string text)
        {
            consoleText.Add(text);

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

            
            consoleText.Add(string.Format("FPS: {0}", frameRate));
            consoleText.Add(string.Format("TilesCount: {0} ", gameMap.tilesDrawed));
            consoleText.Add(string.Format("Total tiles: {0} ", gameMap.mapSize));
            consoleText.Add("------------------------------------");


            myKeyboard.controls();
            gameMouse.updateCursor();
            gameMouse.handleMouse(camera);

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

            drawMoveCursor();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawMoveCursor()
        {
            if (gameMouse.moving)
            {
               
                spriteBatch.Draw(mouseTextures[gameMouse.drawMouse], new Rectangle(gameMouse.state.Position.X - 6, gameMouse.state.Position.Y - 6, 16, 16), Color.White);
            }
            else
            {
                spriteBatch.Draw(mouseTextures[8], new Rectangle(gameMouse.state.Position.X - 6, gameMouse.state.Position.Y - 6, 16, 16), Color.White);
            }
            
        }

        private void drawConsole(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            Point consolePosition = new Point(0, 0);
            int consoleTextSpace = 16;
            
            spriteBatch.Draw(consoleTexture, new Rectangle(0, consolePosition.Y, WINDOW_WIDTH, 195), Color.White);

            int i = 0;
            Vector2 position = new Vector2(5, consolePosition.Y +10);
            foreach (string text in consoleText)
            {
                if (i != 0)
                {
                    position.Y += consoleTextSpace;
                    if (i % 11 == 0)
                    {
                        position.X += 300;
                        position.Y = consolePosition.Y+10;
                    }
                }
                spriteBatch.DrawString(spriteFontBig, text, position, Color.White);
                i++;
            }
            consoleText.Clear();
           

        }
    }
}
