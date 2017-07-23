using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame
{
    public class GameMouse
    {
        public MouseState state;
        private Texture2D mouseMoveMap;
        private Point windowSize;
        public int drawMouse;
        internal bool moving;

        public GameMouse(Texture2D mouseMoveTexture,Point windowSize)
        {
            this.mouseMoveMap = mouseMoveTexture;
            this.windowSize = windowSize;
        }

        public void updateCursor()
        {
            if ((state.Position.X > 0 && state.Position.X < windowSize.X)
                && (state.Position.Y > 0 && state.Position.Y < windowSize.Y))
                { 
                uint[] myUint = new uint[1];

                mouseMoveMap.GetData(0, new Rectangle(state.Position.X, state.Position.Y, 1,1), myUint, 0,1);
              
                if (moving) { 
                    if (myUint[0] == 0xFF0000FF) // Red
                    {
                        drawMouse = 6;
                    }
                    if (myUint[0] == 4278209272) // Orange
                    {
                        drawMouse = 4;
                    }

                    if (myUint[0] == 0) // PURPLE
                    {
                        drawMouse = 0;
                    }

                    if (myUint[0] == 4294967040) // light blue
                    {
                        drawMouse = 3;
                    }

                    if (myUint[0] == 4278255360) // Green
                    {
                        drawMouse = 7;
                    }

                    if (myUint[0] == 4278255615) // Yellow
                    {
                        drawMouse = 5;
                    }

                    if (myUint[0] == 4294901760) // Blue
                    {
                        drawMouse = 2;
                    }
                    if (myUint[0] == 4294967295) // WHITE
                    {
                        drawMouse = 1;
                    }
                    if (myUint[0] == 4278190080) // Black
                    {
                        drawMouse = 8;
                    }
                   
                   
                }
            }
        }

        public void handleMouse(Camera camera)
        {
            

            if (state.RightButton == ButtonState.Pressed)
            {
                moving = true;

                switch (drawMouse)
                {
               
                    case 3:
                        if (camera.moveableXLeft)
                            camera.xOffset -= 1 * camera.speed;
                        break;
                    case 6:
                        if (camera.moveableXLeft)
                            camera.xOffset -= 1 * camera.speed;
                        if (camera.moveableYTop)
                            camera.yOffset -= 1 * camera.speed;
                        break;
                    case 4:
                        if (camera.moveableXRight)
                            camera.xOffset += 1 * camera.speed;
                        break;
                    case 7:
                        if (camera.moveableXRight)
                            camera.xOffset += 1 * camera.speed;
                        if (camera.moveableYTop)
                            camera.yOffset -= 1 * camera.speed;
                        break;
                    case 5:
                        if (camera.moveableYTop)
                            camera.yOffset -= 1 * camera.speed;
                        break;
                    case 0:
                        if (camera.moveableYDown)
                            camera.yOffset += 1 * camera.speed;
                        break;
                    case 1:
                        if (camera.moveableXLeft)
                            camera.xOffset -= 1 * camera.speed;
                        if (camera.moveableYDown)
                            camera.yOffset += 1 * camera.speed;
                        break;
                    case 2:
                        if (camera.moveableXRight)
                            camera.xOffset += 1 * camera.speed;
                        if (camera.moveableYDown)
                            camera.yOffset += 1 * camera.speed;
                        break;
                }
            }
            else
            {
                moving = false;
            }
        }
    }
}