using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Scroll
{

    /*hay que agregar la otra clase sprite para poder poner los fondos*/
    public class Sprite
    {
        int increment;
        RectangleF size, display;
        Bitmap imgDisplay, imgL, imgR;
        public int counter;


        //Setters and getters
        public float posX
        {
            set { display.X = value; }
        }

        public float posY
        {
            set { display.Y = value; }
        }

        public Bitmap ImgDisplay
        {
            get { return imgDisplay; }
            set { imgDisplay = value; }
        }


        //Sprite for the characters
        public Sprite(Size original, Size display, Point starting, Bitmap right, Bitmap left)
        {
            counter = 0;
            this.increment  = original.Width;
            this.display    = new RectangleF(starting.X, starting.Y, display.Width, display.Height);
            this.size       = new RectangleF(0, 0, original.Width, original.Height);
            this.imgR       = right;
            this.imgL       = left;
            this.imgDisplay = right;
        }

        public Sprite(Size original, Size display, Point starting, int increment, Bitmap right)
        {
            //pos = starting;
            this.increment = increment;
            this.display = new Rectangle(starting.X, starting.Y, display.Width, display.Height);
            this.size = new Rectangle(0, 0, original.Width, original.Height);
            this.imgR = right;
            this.imgDisplay = right;
        }
        public void Frame(int x)
        {
            size.X = (x * size.Width) % imgDisplay.Width;
        }

        //Movement methods for the sprite
        public void MoveLeft()
        {
            imgDisplay = imgL;
            size.X = (increment + size.X) % imgDisplay.Width;
        }
        public void MoveRight()
        {
            imgDisplay = imgR;
            size.X = (increment + size.X) % imgDisplay.Width;
        }
        public void MoveSlow(int value)
        {
            if(counter%value==0)
                size.X = (increment + size.X) % imgDisplay.Width;

            counter++;
        }

        public void Display(Graphics g)
        {
            g.DrawImage(imgDisplay, display, size, GraphicsUnit.Pixel);
        }
    }
}
