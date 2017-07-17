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
        private Tile[][] twoDTiles;
        private int tilesX;
        private int tilesY;

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

        public Point isoTo2D(Point pt){
          Point tempPt = new Point(0, 0);
          tempPt.X = (2 * pt.Y + pt.X) / 2;
          tempPt.Y = (2 * pt.Y - pt.X) / 2;
          return(tempPt);
        }
        public Point twoDToIso(Point pt)
        {
            Point tempPt = new Point(0,0);
                tempPt.X = pt.X - pt.Y;
                tempPt.Y = (pt.X + pt.Y) / 2;
            return(tempPt);
        }
        private Point worldToIso(Point point)
        {
            point.X /= tileWidth/2;
            point.Y = (point.Y - tileHeight / 2) / tileHeight + point.X;
            point.Y -= point.Y - point.X;

            //if (point.Y < 0) point.Y = 0;
            return point;
        }


        public void drawMap(Camera camera)
        {
            this.zoom = camera.zoom;
            tileWidth = 128 / zoom;
            tileHeight = 128 / zoom;


            
            if (spriteBatch != null)
            {

                tilesDrawed = 0;
                Point start = new Point(0,0);
                Point bottomRight = new Point(0, 0);

                int leftX = (camera.xOffset) ;

                start = worldToIso(new Point(-camera.xOffset - camera.width /2, camera.yOffset));
               

                if (start.X < 0) start.X = 0;
                if (start.Y < 0) start.Y = 0;
                 
                

                
                for (int i = start.X; i <= start.X + 25; i++)
                {
                    for (int j = start.Y; j <= start.Y +25; j++)
                    {
                        if (tileExists(i, j))
                        {
                            Tile tile = twoDTiles[i][j];
                            int x = (int)tile.positionNumber.X * tileWidth / 2;
                            int y = (int)tile.positionNumber.Y * tileHeight / 2;

                            int ix = x - y;
                            int iy = (x + y) / 2;
                            Point isoCoords = new Point(ix, iy);
                            isoCoords.X -= camera.xOffset;
                            isoCoords.Y -= camera.yOffset;

                            tile.position.X = isoCoords.X;
                            tile.position.Y = isoCoords.Y;
                            Texture2D renderTexture = textureList.Find(texture => texture.type.Equals("grass") && texture.id == tile.textureId).getTexture2D();
                            tile.drawTile(isoCoords, renderTexture, tileWidth, tileHeight);

                            if (camera.debug)
                            {
                                string coords = string.Format(" {0},{1} ", tile.positionNumber.X, tile.positionNumber.Y);
                                spriteBatch.DrawString(spriteFont, coords, new Vector2(isoCoords.X + (tileWidth / 2) - 12, isoCoords.Y + (tileHeight / 2 + 12) - 8), Color.White);
                            }

                            tilesDrawed++;

                        }
                    }
                }

                /*
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

                    tile.position.X = isoCoords.X;
                    tile.position.Y = isoCoords.Y;
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

                //Tile selectTile = tiles.Find(selectedTile => selectedTile.positionNumber.X == (float)pointingAt.X && selectedTile.positionNumber.Y == (float)pointingAt.Y);
                Tile selectTile = visibleTiles.FindLast(selectedTile =>
                                (mousePosition.X > selectedTile.position.X && mousePosition.X < selectedTile.position.X + tileWidth / camera.zoom) &&
                                (mousePosition.Y > selectedTile.position.Y + tileHeight /camera.zoom / 2 && mousePosition.Y < selectedTile.position.Y + tileHeight / camera.zoom));
               

                

                if (selectTile != null) { 
                    int x1 = (int)selectTile.positionNumber.X * tileWidth  / 2;
                    int y1 = (int)selectTile.positionNumber.Y * tileHeight / 2;

                    int ix1 = x1 - y1;
                    int iy1 = (x1 + y1) / 2;
                    Point isoCoords1 = new Point(ix1, iy1);
                    isoCoords1.X -= camera.xOffset;
                    isoCoords1.Y -= camera.yOffset;
                    isoCoords1.Y += 48;

                    Texture2D renderTexture1 = textureList.Find(texture => texture.type.Equals("basic") && texture.id == 999).getTexture2D();
                    selectTile.drawTile(isoCoords1, renderTexture1, tileWidth, tileHeight/2);
                    if (camera.debug)
                    {
                        string mousecoords = string.Format(" {0},{1} ", selectTile.positionNumber.X, selectTile.positionNumber.Y);
                        spriteBatch.DrawString(spriteFont, mousecoords, new Vector2(mousePosition.X + 16, mousePosition.Y - 14), Color.Black);
                        spriteBatch.DrawString(spriteFont, mousecoords, new Vector2(mousePosition.X + 15, mousePosition.Y - 15), Color.White);
                    }
                }
                //end mouse
                */
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
            this.tilesY = 1000;

            this.mapSize = 0;
            this.twoDTiles = new Tile[tilesX][];
            Random randomNum = new Random();
            for (int i = 0; i < tilesX; i++)
            {
                twoDTiles[i] = new Tile[tilesY];
                for (int j = 0; j < tilesY; j++)
                {
                    mapSize++;
                    counter++;
                    twoDTiles[i][j] = new Tile(counter, randomNum.Next(1, 18), new Vector2(0, 0), new Vector2(j, i), spriteBatch, spriteFont, content);
                   // tiles.Add(new Tile(counter, randomNum.Next(1,18), new Vector2(0, 0), new Vector2(j, i), spriteBatch, spriteFont, content));
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

            //mousePosition.X -= camera.xOffset;
            //mousePosition.Y -= camera.yOffset;

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
