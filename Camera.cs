using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace TestGame
{
    class Camera
    {
        public Vector2 cameraPosition;
        public int width;
        public int height;
        private int speed = 1;

        public int xOffset = 0;
        public int yOffset = 0;
        internal bool debug;
        public int zoom = 1;
        public bool isReleased;

        public Camera()
        {
            this.cameraPosition = new Vector2(0, 0);
            this.width = 800;
            this.height = 600;
        }

        public Camera(int height, int width)
        {
            this.width = width;
            this.height = height;
            this.cameraPosition = new Vector2(0, 0);
            xOffset = 0;
        }

        public bool isTileVisibe(int x, int y)
        {
            bool visible = false;

            if (x + 128 > this.width + xOffset - this.width && x - 128 < this.width + xOffset)
            {
                if (y + 64 > this.height + yOffset - this.height && y - 64 < this.height + yOffset)
                {
                    visible = true;
                }
            }
            return visible;
        }

        public void move()
        {


        }

        internal void controls(Keys[] pressedKeys)
        {
           
            for (int i = 0; i < pressedKeys.Length; i++)
            {

                switch (pressedKeys[i])
                {
                    case Keys.Left:
                        xOffset -= 1 * speed;
                        break;
                    case Keys.Right:
                        xOffset += 1 * speed;
                        break;
                    case Keys.Up:
                        yOffset -= 1 * speed;
                        break;
                    case Keys.Down:
                        yOffset += 1 * speed;
                        break;

                    case Keys.Subtract:
                        zoom = 1;
                        break;
                    case Keys.Add:
                        zoom = 2;
                        break;
                    case Keys.NumPad1:
                        if (speed == 1) speed = 10;
                        else speed = 1;
                        break;

                    case Keys.D:

                        if (debug)
                        {
                            debug = false;
                        }
                        else
                        {
                            debug = true;
                        }
                        break;


                }
            }

        }
     
}
