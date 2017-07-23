using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace TestGame
{
    public class Camera
    {
        public Vector2 cameraPosition;
        public int width;
        public int height;
        public int speed = 5;

        public int xOffset = 0;
        public int yOffset = 0;
        public bool debug = false;

        public bool moveableXLeft = true;
        public bool moveableXRight = true;
        public bool moveableYTop = true;
        public bool moveableYDown = true;

        public Camera(int height, int width)
        {
            this.width = width;
            this.height = height;
            this.cameraPosition = new Vector2(0, 0);
            xOffset = 128;
            yOffset = 96;

        }
    }
}
