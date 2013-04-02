using System;
using System.Collections.Generic;
using System.Drawing;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;

namespace Fishy
{
    class Balls
    {
        private double size;
        private double x = 0;
        private double y = 0;
        private double speed;

        private bool movesRight = true;


        private int ballnum;
        private static int ballcount = -1;

        private Random rand;

        public Balls()
        {
            ballcount++;
            ballnum = ballcount;
            rand = new Random();

            size = rand.Next(1, 40);
            speed = rand.Next(1, 5)/2.0;
            //Console.WriteLine("Ball " + ballnum + " reporting for duty. Size: "+size+", Speed: "+speed);
            
            y = rand.Next(0, Game.screenY);

            x = rand.Next(0, 2);
            if (x == 1)
            {
                x = Game.screenX;
                movesRight = false;
            }
            else
            {
                x = 0;
            }

        }

        public double getX()
        {
            return x;
        }

        public double getY()
        {
            return y;
        }
        public double getSize()
        {
            return size;
        }

        public int getNum()
        {
            return ballnum;
        }

        public Circle Draw()
        {
            return new Circle((short)this.x, (short)this.y, (short)this.size);
        }
        public Color getColor()
        {
            int sizetemp = Math.Min(30+(int)(size * 8), 255);
            return Color.FromArgb(sizetemp * -1 + 255, sizetemp * -1 + 255, sizetemp);
        }
        public bool Tick()
        {
            if (Game.Pause)
            {
                return true;
            }

            if (x < 0 || x > Game.screenX)
            {
                //Console.WriteLine("Ball " + ballnum + " is off the screen, x value is " + x);
                x = 0;
                return false;
            }

            if (movesRight)
            {
                x += speed;
            }
            if (!movesRight)
            {
                x -= speed;
            }
            return true;
        }
    }
}
