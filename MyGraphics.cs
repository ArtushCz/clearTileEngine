using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TestGame
{
    public class MyGraphics
    {
        private  ContentManager  content;

        public MyGraphics(ContentManager content, SpriteBatch spriteBatch)
        {
            this.content = content;
        }

        public static void drawBox(Point startPosition, Point endPosition, SpriteBatch spriteBatch,Texture2D pixel, Color color)
        {
            if (spriteBatch != null)
            {
                spriteBatch.Draw(pixel, new Rectangle(startPosition.X,startPosition.Y , endPosition.X, 1), color);
                spriteBatch.Draw(pixel, new Rectangle(startPosition.X, startPosition.Y, 1, endPosition.Y), color);
                spriteBatch.Draw(pixel, new Rectangle(endPosition.X + startPosition.X, startPosition.Y, 1 , endPosition.Y), color);
                spriteBatch.Draw(pixel, new Rectangle(startPosition.X, endPosition.Y + startPosition.Y, endPosition.X, 1 ), color);
            }
        }

 
    }
}
