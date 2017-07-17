using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace TestGame
{
    internal class Map
    {
        private SpriteBatch spriteBatch;
        private GraphicsDeviceManager graphics;
        private List<Tile> tiles;
        private SpriteFont spriteFont;
        private ContentManager content;
        private SpriteFont spriteFontBig;
        public int tilesDrawed;

        public int zoom = 1;

        public int tileWidth = 128;
        public int tileHeight = 128;

        
        public int mapSize;
        private List<GameTextures2D> textureList;
        public Point mousePosition;
        private Point pointingAt;

        public Map(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
            this.tiles = new List<Tile>();
            tileWidth = 128 / zoom;
            tileHeight = 128 / zoom;
        }
        private int calcIsoX(int x, int y)
        {
            return x - y;
        }

        private int calcIsoY(int x, int y)
        {
            return (x + y) / 2;
        }


        public void drawMap(Camera camera)
        {
            this.zoom = camera.zoom;
            tileWidth = 128 /zoom;
            tileHeight = 128 / zoom;



            if (spriteBatch != null)
            {
              



                tilesDrawed = 0;
                List<Tile> visibleTiles = tiles.FindAll(r => (
                               calcIsoX((int)r.positionNumber.X * tileWidth / 2, (int)r.positionNumber.Y * tileHeight / 2) + tileWidth 
                                        > camera.width + camera.xOffset - camera.width &&
                               calcIsoX((int)r.positionNumber.X * tileWidth / 2, (int)r.positionNumber.Y * tileHeight / 2) - tileWidth 
                                                       < camera.width + camera.xOffset) &&
                              (calcIsoY((int)r.positionNumber.X * tileWidth / 2, (int)r.positionNumber.Y * tileHeight / 2) + tileHeight 
                                        > camera.height + camera.yOffset - camera.height &&
                               calcIsoY((int)r.positionNumber.X * tileWidth / 2, (int)r.positionNumber.Y * tileHeight / 2) - tileHeight 
                                                       < camera.height + camera.yOffset));

                foreach (Tile tile in visibleTiles)
                {
                    int x = (int)tile.positionNumber.X * tileWidth / 2;
                    int y = (int)tile.positionNumber.Y * tileHeight / 2;

                    int ix = x - y;
                    int iy = (x + y) / 2;
                    Point isoCoords = new Point(ix, iy);
                    isoCoords.X -= camera.xOffset;
                    isoCoords.Y -= camera.yOffset;

                    Texture2D renderTexture = textureList.Find(texture => texture.type.Equals("grass") && texture.id == tile.textureId).getTexture2D();
                    tile.drawTile(isoCoords, renderTexture, tileWidth, tileHeight);

                    if (camera.debug)
                    {
                        string coords = string.Format(" {0},{1} ", tile.positionNumber.X, tile.positionNumber.Y);
                        spriteBatch.DrawString(spriteFont, coords, new Vector2(isoCoords.X + (tileWidth/2) -12, isoCoords.Y + (tileHeight/2+12)-8), Color.White);
                    }
                     
                    tilesDrawed++;
                }
                //mouse
                string mousecoords = string.Format(" {0},{1} ", pointingAt.X, pointingAt.Y);
                spriteBatch.DrawString(spriteFont, mousecoords, new Vector2(mousePosition.X + 16, mousePosition.Y - 14), Color.Black);
                spriteBatch.DrawString(spriteFont, mousecoords, new Vector2(mousePosition.X + 15, mousePosition.Y - 15), Color.White);

                Tile selectTile = tiles.Find(selectedTile => selectedTile.positionNumber.X == (float)pointingAt.X && selectedTile.positionNumber.Y == (float)pointingAt.Y);
                if(selectTile != null) { 
                    int x1 = (int)selectTile.positionNumber.X * tileWidth / 2;
                    int y1 = (int)selectTile.positionNumber.Y * tileHeight / 2;

                    int ix1 = x1 - y1;
                    int iy1 = (x1 + y1) / 2;
                    Point isoCoords1 = new Point(ix1, iy1);
                    isoCoords1.X -= camera.xOffset;
                    isoCoords1.Y -= camera.yOffset;
                    isoCoords1.Y += 48;

                    Texture2D renderTexture1 = textureList.Find(texture => texture.type.Equals("basic") && texture.id == 999).getTexture2D();
                    selectTile.drawTile(isoCoords1, renderTexture1, tileWidth, tileHeight/2);
                }
                //end mouse
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
            int tilesX = 700;
            int tilesY = 700;

            this.mapSize = 0;
            Random randomNum = new Random();
            for (int i = 0; i <= tilesX; i++)
            {
                for (int j = 0; j <= tilesY; j++)
                {
                    mapSize++;
                    counter++;
                    tiles.Add(new Tile(counter, randomNum.Next(1,18), new Vector2(0, 0), new Vector2(j, i), spriteBatch, spriteFont, content));
                }
            }
            loadTextures();
        }

        private void loadTextures()
        {
            int grassTexturesCount = 17;
            // grass textures
            for(int i = 1; i <= grassTexturesCount; i++)
            {
                textureList.Add(new GameTextures2D(content.Load<Texture2D>("Textures/Grass/"+i.ToString()), "grass", i));
            }
            textureList.Add(new GameTextures2D(content.Load<Texture2D>("Textures/selectTile"), "basic", 999));
        }

        public void handleMouse(GameMouse mouse, Camera camera)
        {
            mousePosition = mouse.state.Position;

            Point tile = mousePosition;
            tile.X += camera.xOffset;
            tile.Y += camera.yOffset;
            pointingAt = getTileCoordinates(tile);
            int temp = 0;
            if (pointingAt.X < 0)
            {
                temp = pointingAt.X;
                pointingAt.X = pointingAt.Y;
                pointingAt.Y = -temp;
            }
            if (pointingAt.Y < 0)
            {
                temp = pointingAt.Y;
                pointingAt.Y = pointingAt.X;
                pointingAt.X = -temp;
            }

        }

        public Point getTileCoordinates(Point pt){
            Point tempPt = new Point(0, 0);
                tempPt.X = (int)Math.Floor((decimal)pt.X / (decimal)(tileHeight / 2+32));
                tempPt.Y = (int)Math.Floor((decimal)pt.Y / (decimal)(tileHeight / 2+32));
             return(tempPt);
        }

}


}
