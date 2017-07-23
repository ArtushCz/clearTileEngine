using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace TestGame
{
    public class Map
    {
        private SpriteBatch spriteBatch;
        private GraphicsDeviceManager graphics;
        private List<Tile> tiles;
        private SpriteFont spriteFont;
        private ContentManager content;
        private SpriteFont spriteFontBig;
        public int tilesDrawed;


        public int tileWidth = 128;
        public int tileHeight = 128;

        
        public int mapSize;
        private List<GameTextures2D> textureList;
        public Point mousePosition;

        private Tile[][] twoDTiles;
        private int tilesX;
        private int tilesY;
        internal static bool drawTileSelect;
        private int selectedTexture = 1;
        private int grassTexturesCount;
        private Texture2D pixel;

        public Map(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
            this.tiles = new List<Tile>();
            tileWidth = 128 ;
            tileHeight = 128 ;
        }
      

        public Point getZigZagPos(int x, int y)  //x,y map position (offset)
        {
            int sx = x / (tileWidth/2);
            int sy = y / tileHeight;

            int off = (sx % 2 == 1) ? (tileWidth / 2 ) : (32);
            

            int isoX = (2 * y) / (tileHeight / 2);
            int isoY = (x - off) / tileWidth;

            return new Point(isoX, isoY);
        }

        private void changeTileAt(Point mousePosition, Camera camera, int selectedTexture)
        {
            Point mouseTilePos = getZigZagPos(mousePosition.X + camera.xOffset, mousePosition.Y + camera.yOffset);
            if (tileExists(mouseTilePos.X - 2, mouseTilePos.Y))
                twoDTiles[mouseTilePos.X - 2][mouseTilePos.Y].textureId = selectedTexture;
        }

        private void selectTileAt(Point mousePosition, Camera camera)
        {
            Point startTilesPos = new Point(75, 60);
            Point endTilePosition = new Point(camera.width - 110, camera.height - 110);
            int i = 1;
            for (int y = startTilesPos.Y; y < endTilePosition.Y; y += 64)
            {
                for (int x = startTilesPos.X; x < endTilePosition.X; x += 75)
                {
                    if ((mousePosition.X > x && mousePosition.X < x + 64) && mousePosition.Y > y && mousePosition.Y < y + 64)
                    {
                        if(i <= grassTexturesCount)
                        selectedTexture = i;
                                       
                    }
                    i++;
                }
            }
        }




        public void drawMap(Camera camera)
        {

            tileWidth = 128 ;
            tileHeight = 128 ;

            if (spriteBatch != null)
            {

                tilesDrawed = 0;

                Point start = new Point(0, 0);
                Point end = new Point(0, 0);




                start = getZigZagPos(camera.xOffset, camera.yOffset);
                end = getZigZagPos(camera.xOffset + camera.width + 32, camera.yOffset + camera.height);

                // edge of map controll
                if (start.Y <= 0) camera.moveableXLeft = false;
                else camera.moveableXLeft = true;
                if (start.X <= 2) camera.moveableYTop = false;
                else camera.moveableYTop = true;
                if (end.Y >= tilesY - 1) camera.moveableXRight = false;
                else camera.moveableXRight = true;
                if (end.X >= tilesX - 1) camera.moveableYDown = false;
                else camera.moveableYDown = true;


                for (int i = start.X - 3; i < end.X + 1; i++)
                {
                    for (int j = start.Y - 1; j < end.Y + 1; j++)
                    {
                        if (tileExists(i, j))
                        {
                            Tile tile = twoDTiles[i][j];
                            Point drawPosition = new Point((int)tile.position.X, (int)tile.position.Y);

                            Point isoCoords = new Point(drawPosition.X, drawPosition.Y);
                            drawPosition.X -= camera.xOffset;
                            drawPosition.Y -= camera.yOffset;

                            Texture2D renderTexture = textureList.Find(texture => texture.type.Equals("grass") && texture.id == tile.textureId).getTexture2D();
                            tile.drawTile(drawPosition, renderTexture, tileWidth, tileHeight);
                            /*
                                                        if (camera.debug)
                                                        {

                                                           string coords = string.Format(" X: {0} Y: {1} ", tile.positionNumber.X, tile.positionNumber.Y);
                                                            spriteBatch.DrawString(spriteFont, coords, new Vector2(drawPosition.X + (tileWidth / 2) - 20, drawPosition.Y + (tileHeight / 2 + 8)), Color.White);
                                                        }
                                                        */
                            tilesDrawed++;

                        }
                    }
                }
                if (camera.debug)
                {
                    for (int i = start.X - 3; i < end.X + 1; i++)
                    {
                        for (int j = start.Y -1; j < end.Y + 1; j++)
                        {

                            if (tileExists(i, j))
                            {
                                Tile tile = twoDTiles[i][j];
                                Point drawPosition = new Point((int)tile.position.X, (int)tile.position.Y);

                                Point isoCoords = new Point(drawPosition.X, drawPosition.Y);
                                drawPosition.X -= camera.xOffset;
                                drawPosition.Y -= camera.yOffset;

                                string coords = string.Format(" X: {0} Y: {1} ", tile.positionNumber.X, tile.positionNumber.Y);
                                spriteBatch.DrawString(spriteFont, coords, new Vector2(drawPosition.X + (tileWidth / 2) - 20, drawPosition.Y + (tileHeight / 2 + 8)), Color.White);
                            }
                        }
                    }
                }
            

                //mouse
                if (!drawTileSelect)
                {
                    Tile selectedTile = null;
                    Point mouseTilePos = getZigZagPos(mousePosition.X + camera.xOffset, mousePosition.Y + camera.yOffset);
                    if (tileExists(mouseTilePos.X - 2, mouseTilePos.Y))
                        selectedTile = twoDTiles[mouseTilePos.X - 2][mouseTilePos.Y];

                    if (selectedTile != null)
                    {

                        Point drawPosition = new Point((int)selectedTile.position.X, (int)selectedTile.position.Y);

                        Point isoCoords = new Point(drawPosition.X, drawPosition.Y);
                        drawPosition.X -= camera.xOffset;
                        drawPosition.Y -= camera.yOffset;
                        drawPosition.Y += 48;

                        Game.Log("Tile Texture ID: " + selectedTile.textureId);
                        Texture2D renderTexture1 = textureList.Find(texture => texture.type.Equals("basic") && texture.id == 999).getTexture2D();
                        selectedTile.drawTile(drawPosition, renderTexture1, tileWidth, tileHeight / 2);
                        if (camera.debug)
                        {
                            string mousecoords = string.Format(" {0},{1} ", selectedTile.positionNumber.X, selectedTile.positionNumber.Y);
                            spriteBatch.DrawString(spriteFont, mousecoords, new Vector2(mousePosition.X + 16, mousePosition.Y - 14), Color.Black);
                            spriteBatch.DrawString(spriteFont, mousecoords, new Vector2(mousePosition.X + 15, mousePosition.Y - 15), Color.White);
                        }
                    }
                }
                //end mouse
               
                //tile select 

                if (drawTileSelect)
                {
                    Point startTilesPos = new Point(75, 60);
                    Point endTilePosition = new Point(camera.width - 110, camera.height - 110);
                    MyGraphics.drawBox(new Point(startTilesPos.X - 26, startTilesPos.Y - 11), new Point(endTilePosition.X + 11, endTilePosition.Y + 11), spriteBatch, pixel, Color.DarkGray );

                    Texture2D consoleTexture = content.Load<Texture2D>("Textures/consoleSee");
                    
                    spriteBatch.Draw(consoleTexture, new Rectangle(startTilesPos.X-25, startTilesPos.Y-10, endTilePosition.X +10, endTilePosition.Y +10), Color.White);
                    int i = 1;
                    for (int y = startTilesPos.Y; y < endTilePosition.Y; y += 64)
                    {
                        for (int x = startTilesPos.X; x < endTilePosition.X; x += 75)
                        { 
                            if (i == grassTexturesCount+1) break;

                            if (i == selectedTexture) {
                                spriteBatch.Draw(content.Load<Texture2D>("Textures/selectBlock"), new Rectangle(x, y, tileWidth / 2, tileHeight / 2), Color.White); 
                            }

                            Texture2D renderTexture = textureList.Find(texture => texture.type.Equals("grass") && texture.id == i).getTexture2D();
                            if ((mousePosition.X > x && mousePosition.X < x + 64) && (mousePosition.Y > y && mousePosition.Y < y + 64))
                            {
                                spriteBatch.Draw(content.Load<Texture2D>("Textures/selectBlock"), new Rectangle(x, y, tileWidth/2, tileHeight/2), Color.White);
                                
                            }
                           
                            spriteBatch.Draw(renderTexture, new Rectangle(x,y, tileWidth / 2, tileHeight / 2), Color.White);
                          
                            i++;
                        }
                    }

                    Game.Log("Selected texture ID: " + this.selectedTexture.ToString());
                }
                //tile select end
            }
        }

        public bool tileExists(int i, int j)
        {
                if (i>=0 && j>=0)
                {
                    if(i<=tilesX && j <= tilesY)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }   
        }



        public void load(ContentManager content, SpriteBatch spriteBatch, SpriteFont spriteFont, SpriteFont spriteFontBig)
        {
            this.spriteBatch = spriteBatch;

            this.spriteFont = spriteFont;
            this.spriteFontBig = spriteFontBig;
            this.content = content;
            this.textureList = new List<GameTextures2D>();
            int counter = 0;
            this.tilesX = 1000;
            this.tilesY = 500;

            this.mapSize = 0;
            this.twoDTiles = new Tile[tilesX+1][];
            Random randomNum = new Random();

            //zigzag
            int offset_x = 0;
            for (int i = 0; i <= tilesX; i++) {
                twoDTiles[i] = new Tile[tilesY+1];
                if (i%2==0)
                    offset_x = tileWidth / 2;
                else
                    offset_x = 0;

                for (int j = 0; j <= tilesY; j++) {
                    mapSize++;
                    counter++;

                    int x = (j * tileWidth) + offset_x;
                    int y = i * (tileHeight -64) / 2;
                    twoDTiles[i][j] = new Tile(counter, randomNum.Next(1, 4), new Vector2(x, y), new Vector2(j,i), spriteBatch, spriteFont, content);
                }
            }
            
            loadTextures();
        }

        private void loadTextures()
        {
            grassTexturesCount = 3;
            // grass textures
            for(int i = 1; i <= grassTexturesCount; i++)
            {
                textureList.Add(new GameTextures2D(content.Load<Texture2D>("Textures/Grass/"+i.ToString()), "grass", i));
            }
            textureList.Add(new GameTextures2D(content.Load<Texture2D>("Textures/selectTile"), "basic", 999));
            textureList.Add(new GameTextures2D(content.Load<Texture2D>("Textures/baseTile"), "basic", 1000));

            pixel = content.Load<Texture2D>("Textures/Pixel");
        }

        public void handleMouse(GameMouse mouse, Camera camera)
        {
            mousePosition = mouse.state.Position;

            if (mouse.state.LeftButton == ButtonState.Pressed)
            {
                if (!drawTileSelect)
                {
                    changeTileAt(mousePosition, camera, selectedTexture);
                }
                if (drawTileSelect)
                {
                    selectTileAt(mousePosition, camera);
                }
               
            }

        }

    }


}
