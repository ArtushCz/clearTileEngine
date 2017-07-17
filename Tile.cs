using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame
{
    class Tile
    {
        private SpriteBatch spriteBatch;
        public int textureId;

        public Vector2 position;
        public Vector2 positionNumber;
        private SpriteFont spriteFont;
        private int number;

        public int tileHeight { get; }
        public int tileWidth { get; }


        public Tile(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        public Tile(int number, int textureId, Vector2 position, Vector2 positionNumber, SpriteBatch spriteBatch, SpriteFont spriteFont, ContentManager content)
        {
            this.number = number;
            this.position = position;
            this.positionNumber = positionNumber;
            this.spriteFont = spriteFont;
            this.textureId = textureId;
            this.spriteBatch = spriteBatch;

            this.tileHeight = 128;
            this.tileWidth = 128;
        }



        internal void drawTile(Point p, Texture2D texture, int tileWidth, int tileHeight)
        {

            spriteBatch.Draw(texture, new Rectangle(p.X, p.Y, tileWidth, tileHeight), Color.White);

           

        }

        internal int getId()
        {
            return this.textureId;
        }
    }
}
