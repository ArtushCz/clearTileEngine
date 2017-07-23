using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame
{
    public class MyKeyboard
    {

        public KeyboardState previousState;
        public Camera camera;
        private Map map;

        public MyKeyboard(Camera camera, Map map)
        {
            this.camera = camera;
            this.map = map;
        }


        internal void controls()
        {
            KeyboardState state = Keyboard.GetState();
            Keys[] pressedKeys = state.GetPressedKeys();

            for (int i = 0; i < pressedKeys.Length; i++)
            {

                switch (pressedKeys[i])
                {
                    case Keys.Left:
                        if (camera.moveableXLeft)
                            camera.xOffset -= 1 * camera.speed;
                        break;
                    case Keys.Right:
                        if (camera.moveableXRight)
                            camera.xOffset += 1 * camera.speed;
                        break;
                    case Keys.Up:
                        if (camera.moveableYTop)
                            camera.yOffset -= 1 * camera.speed;
                        break;
                    case Keys.Down:
                        if (camera.moveableYDown)
                            camera.yOffset += 1 * camera.speed;
                        break;

                
                    case Keys.S:
                        if (!previousState.IsKeyDown(Keys.S))
                        {
                           
                            if (Map.drawTileSelect == true) Map.drawTileSelect = false;
                            else Map.drawTileSelect = true;
                        }

                        break;
                    case Keys.NumPad1:
                        if (!previousState.IsKeyDown(Keys.NumPad1))
                            if (camera.speed == 5) camera.speed = 10;
                        else camera.speed = 5;
                        break;

                    case Keys.D:
                        if(!previousState.IsKeyDown(Keys.D))
                        if (camera.debug)
                        {
                            camera.debug = false;

                        }
                        else
                        {
                            camera.debug = true;

                        }
                        break;


                }
            }
            previousState = state;
        }
    }
}
