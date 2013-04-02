using System;
using System.Collections.Generic;
using System.Drawing;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using System.Threading;


namespace Fishy
{


    class Game
    {
        public static bool GameOver = false;
        public static bool Win = false;
        public static bool Pause = false;

        public const int screenX = 400;
        public const int screenY = 400;

        private Surface screen;
        private Surface text;
        private Surface tryAgain;
        private SdlDotNet.Graphics.Font font = new SdlDotNet.Graphics.Font("Urban Sketch.ttf", 36);
        private SdlDotNet.Graphics.Font font2 = new SdlDotNet.Graphics.Font("Urban Sketch.ttf", 14);

        public static List<Balls> enemy = new List<Balls>();
        MainBall player = new MainBall();

        private bool upArrow = false;
        private bool downArrow = false;
        private bool leftArrow = false;
        private bool rightArrow = false;
        
        private void tick(object sender, TickEventArgs t)
        {
            Color c = Color.Black; //Color.FromArgb(r, g, b);
            screen.Fill(c);
            if (!GameOver)
            {
                if (MainBall.getSize() > 800)
                {
                    GameOver = true;
                    Win = true;
                }
                player.drawAll(screen);

                if (upArrow) player.addDY(false);
                if (downArrow) player.addDY(true);
                if (rightArrow) player.addDX(true);
                if (leftArrow) player.addDX(false);

                if (!upArrow & !downArrow && player.getDY() != 0.0)
                {
                    player.slowDY();
                }
                if (!rightArrow & !leftArrow && player.getDX() != 0.0)
                {
                    player.slowDX();
                }

                player.Tick();

                if (enemy.Count < 11)
                {
                    enemy.Add(new Balls());
                    Thread.Sleep(1);
                }

                for (int i = enemy.Count - 1; i > 0; i--)
                {
                    screen.Draw(enemy[i].Draw(), enemy[i].getColor(), true, true);
                    if (!enemy[i].Tick())
                    {
                        enemy.Remove(enemy[i]);
                        //enemy[i].Remove();
                        //enemy.RemoveAt(enemy[i].getNum());
                    }
                }
                if (Game.Pause)
                {
                    text = font.Render("Paused, Click P!", Color.White);
                    screen.Blit(text, new Rectangle(new Point(Game.screenX / 2 - 100, Game.screenY / 2 - 30), text.Size));
                }
                /*foreach (Balls b in enemy)
                {
                    screen.Draw(b.Draw(), Color.Red, true, true);    
                }
                
                for (int i = enemy.Count-1; i>= 0; i--)
                {
                    if (!enemy[i].Tick())
                    {
                        //enemy.RemoveAt(enemy[i].getNum());
                        enemy.Remove(enemy[i]);
                    }
                }
                */
            }
            else
            {
                if (Win)
                {
                    text = font.Render("You WIN!", Color.White);
                }
                else
                {
                    text = font.Render("GAME OVER!", Color.White);
                }
                screen.Blit(text, new Rectangle(new Point(Game.screenX / 2 - 100, Game.screenY / 2 - 30), text.Size));
                tryAgain = font2.Render("Press any key to go again.", Color.White);
                screen.Blit(tryAgain, new Rectangle(new Point(Game.screenX/2-90, Game.screenY/2+10), tryAgain.Size));
            }
            screen.Update();

        }

        private void quit(object sender, QuitEventArgs q)
        {
            Events.QuitApplication();
        }

        private void keyPress(object sender, KeyboardEventArgs k)
        {
            if (GameOver == true)
            {
                if (Win)
                {
                    Win = false;
                }
                player = new MainBall();
                for (int i = enemy.Count - 1; i >= 0; i--)
                {
                    enemy.Remove(enemy[i]);
                }
                GameOver = false;
            }
            if (k.Key == Key.P)
            {
                if (Pause)
                {
                    Pause = false;
                }
                else
                {
                    Pause = true;
                }
            }
            if (k.Key == Key.RightArrow)
            {
                rightArrow = true;
            }
            if (k.Key == Key.LeftArrow)
            {
                leftArrow = true;
            }
            if (k.Key == Key.UpArrow)
            {
                upArrow = true;
            }
            if (k.Key == Key.DownArrow)
            {
                downArrow = true;
            }
        }
        private void keyUp(object sender, KeyboardEventArgs k)
        {
            if (k.Key == Key.RightArrow)
            {
                rightArrow = false;
            }
            if (k.Key == Key.LeftArrow)
            {
                leftArrow = false;
            }
            if (k.Key == Key.UpArrow)
            {
                upArrow = false;
            }
            if (k.Key == Key.DownArrow)
            {
                downArrow = false;
            }
        }

        private void run()
        {
            Events.Quit += new EventHandler<QuitEventArgs>(this.quit);
            Events.Tick += new EventHandler<TickEventArgs>(this.tick);
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(this.keyPress);
            Events.KeyboardUp += new EventHandler<KeyboardEventArgs>(this.keyUp);
            screen = Video.SetVideoMode(screenX, screenY);



            Video.WindowCaption = "Fishy";
            Events.Fps = 60;
            Events.Run();
            
        }

        public static void Main()
        {
            Game game = new Game();
            game.run();
        }
    }
}
