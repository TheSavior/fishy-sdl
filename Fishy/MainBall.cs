using System;
using System.Collections.Generic;
using System.Drawing;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;

namespace Fishy
{
    class MainBall
    {
        private static double size;
        private double x;
        private double y;

        private double dx;
        private double dy;

        private double increase_speed = .10;
        private double decrease_speed = .10;

        private double max_speed = 3;

        private Random rand;

        // Constructor that initiates a new Random
        //      object based on time, and sets the
        //      default location of the ball to
        //      center court.
        public MainBall()
        {
            size = 4;
            rand = new Random();
            x = Game.screenX/2;
            y = Game.screenY / 2;
        }
        

        public static int getSize()
        {
            return (int) MainBall.size;
        }
        public double getDY()
        {
            return dy;
        }
        public double getDX()
        {
            return dx;
        }
        public Color getColor()
        {
            int sizetemp = Math.Min(30 + (int)(size * 8), 255);
            return Color.FromArgb(sizetemp * -1 + 255, sizetemp * -1 + 255, sizetemp);
        }
        public void drawAll(Surface surf)
        {
            surf.Draw(Draw(), getColor(), true, true);
            surf.Draw(DrawSmall(), Color.Red, true, true);
        }
        public Circle Draw()
        {
            return new Circle((short)this.x, (short)this.y, (short)MainBall.size);
        }
        public Circle DrawSmall()
        {
            return new Circle((short)this.x, (short)this.y, (short)Math.Max(1,(MainBall.size/3)));
        }

        // Called per frame, per ball. Moves the x and
        //      y coordinate of the ball by dx and dy.
        //      Also makes sure that the ball isn't off
        //      the screen.
        public void Tick()
        {
            if (Game.Pause)
            {
                return;
            }

            x += dx;
            if (x > Game.screenX)
            {
                x = Game.screenX - size;
                dx = 0;
            }
            if (x < 0)
            {
                x = 0 + size;
                dx = 0;
            }
            y += dy;
            if (y > Game.screenY)
            {
                y = Game.screenY - size;
                dy = 0;
            }
            if (y < 0)
            {
                y = 0 + size;
                dy = 0;
            }

            for (int i = Game.enemy.Count - 1; i > 0; i--)
            {
                double dist = Math.Sqrt(Math.Pow(this.x - Game.enemy[i].getX(), 2) + Math.Pow(this.y - Game.enemy[i].getY(), 2));
                if (dist < (size+Game.enemy[i].getSize()))
                {
                    Console.WriteLine("Collision detected with ball num " + Game.enemy[i].getNum() + " of size " + Game.enemy[i].getSize() + ", I'm size " + size);
                    if (size > Game.enemy[i].getSize())
                    {
                        Console.WriteLine("I'm bigger! *MUNCH*");
                        size += .1*Game.enemy[i].getSize();
                        Game.enemy.Remove(Game.enemy[i]);
                    }
                    else
                    {
                        Game.GameOver = true;
                        Console.WriteLine("Oh No! I was eaten by a bigger fish. Game Over.");
                    }
                }
            }
        }

        // Method called when you hold down a X-direction
        //      key. Speeds up to a max.
        public void addDX(bool pos)
        {
            if (pos == true)
            {
                dx += increase_speed;
            }
            else
            {
                dx -= increase_speed;
            }
            dx = Math.Min(dx, max_speed);
            dx = Math.Max(dx, max_speed * -1);
        }

        // Method called when you hold down a Y-direction
        //      key. Speeds up to a max.
        public void addDY(bool pos)
        {
            if (pos == true)
            {
                dy += increase_speed;
            }
            else
            {
                dy -= increase_speed;
            }
            dy = Math.Min(dy, max_speed);
            dy = Math.Max(dy, max_speed * -1);
        }

        // Method called when you just let go of the X-direction
        //      keys. It will slow down the ball, giving the feel
        //      of momentum.
        public void slowDX()
        {
            if (dx > 0)
            {
                dx -= decrease_speed;
                dx = Math.Max(dx, 0);
            }
            else if (dx < 0)
            {
                dx += decrease_speed;
                dx = Math.Min(dx, 0);
            }
        }

        // Method called when you just let go of the Y-direction
        //      keys. It will slow down the ball, giving the feel
        //      of momentum.
        public void slowDY()
        {
            if (dy > 0)
            {
                dy -= decrease_speed;
                dy = Math.Max(dy, 0);
            }
            else if (dy < 0)
            {
                dy += decrease_speed;
                dy = Math.Min(dy, 0);
            }
        }
    }
}
