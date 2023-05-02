using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace Scroll
{
    public class Map
    {
        //Map division tiles 
        int divs = 3;
        public int nTileWidth = 20,nTileHeight = 20;
        public int nLevelWidth, nLevelHeight;
        public float fOffsetX, fOffsetY;
        public int nVisibleTilesX, nVisibleTilesY;
        private string sLevel;

        //Essential drawings
        public Bitmap bmp;
        public Graphics g;

        //Sprites
        Sprite coin,SpikeBall,snowman,star;
        
        //Score
        int score;
        public int fRings;

        //Audio variables
        bool isP1 = true;
        SoundPlayer soundPlayer;
        Thread thread, threadStop;

        //Background variables
        public int l1_x1, l1_x2, l2_x1, l2_x2;
        Bitmap layer1,layer2;
        public int motion1 = 2, motion2 = 4;
        public int width = Resource1.beach.Width;
        //Colors
        Color dirt = Color.FromArgb(214, 138, 0);
        Color dirt2 = Color.FromArgb(140, 65, 0);
        Color grass = Color.FromArgb(90, 235, 0);
        Color grass2 = Color.FromArgb(49, 138, 0);

        public Map(Size size)
        {            
            coin        = new Sprite(new Size(21, 18), new Size(nTileWidth, nTileHeight), new Point(), Resource1.ring_s, Resource1.ring_s);
            SpikeBall = new Sprite(new Size(162, 150), new Size(nTileWidth, nTileHeight), new Point(), Resource1.My_project__4_, Resource1.My_project__4_);
            star = new Sprite(new Size(157, 156), new Size(nTileWidth, nTileHeight), new Point(), Resource1.My_project__6_, Resource1.My_project__6_);
            snowman = new Sprite(new Size(334, 314), new Size(nTileWidth, nTileHeight), new Point(), Resource1.My_project__3_, Resource1.My_project__3_);

            soundPlayer = new SoundPlayer(Resource1.coimp);
            
            layer1 = Resource1.beach;

            l1_x1 = l2_x2 = 0;
            l1_x2 = l2_x2 = width;

            Play();
            score = 0;


            //Map made of characters
            {
                sLevel = "";
                sLevel += "................................................................................................................................................................................................................................................................";
                sLevel += "..................................................................................................o.o.o.o.o.o.o.o.o.....................................................oo............................oo........................................................";
                sLevel += "................................................................................................GGGGGGGGGGGGGGGGGGGGG..................................................GGGG..........................GGGG.......................................................";
                sLevel += ".......................................................................................................................................................................................oo.......................................................................";
                sLevel += "............................................................................................oo........................................................................................GGGG......................................................................";
                sLevel += "...........................................................................................GGGG............................................................o.......oo...........................................................................................";
                sLevel += "...........................................................................................................................................................o......GGGG..........................................................................................";
                sLevel += "......................................................................................oo...................................................................o....................................................................................................";
                sLevel += ".....................................................................................GGGG....................e.............................................o....................................................................................................";
                sLevel += ".........................................................................................................................................................o.o.o..................................................................................................";
                sLevel += "............................................................................ooooo...........................................e..........................oGGGGGGG.................................................................................................";
                sLevel += "...............s.e.u.......................................................GGGGGGG...................................................................oGGDDDDDDDGG...............................................................................................";
                sLevel += ".......................................s.e.u............................GGGDDDDDDG.................................................................oGGDDDDDDDDDDDGG.............................................................................................";
                sLevel += ".......................................................o.o.o.o.o.o...GGGDDDDDDDDDG...............................................................oGGDDDD.o.o.o.DDDDGG...........................................................................................";
                sLevel += "......FFFF....FFFF.....o.o.o.o.o.................n....GGGGGGGGGGGGGGGGGDDDDDDDDDDDDG......o.o.o.o.o............o.o.o.............................nGGDDD.o.o.o.o.o.o.DDDGG.......................................................................................";
                sLevel += "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG.....GGGGGGGGGGGGGGDDDDDDDDDDDDDDDDDDDDDDDDDDDDDG....GGGGGGGGGGGGG......SSSGGGGGGG..........................oGGDDDD.o.o.o.o.o.o..DDDDDGGGGGGG...................................................GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG";
                sLevel += "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDSSSSSDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDppppDDDDDDDDDDDDDGGGGGGGGGDDDDDDDGGGGGGG.................oGGDDDDD.o.o.o.o.o.o.o......................................................GGGGGGGGGGDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD";
                sLevel += "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDGG..o.o.o.o.o.o.GGDDDDD.n..o.o.o.o.o.o.o..............e.............................GGGGGGGGGDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD";
                sLevel += "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDGGGGGGGGGGGGGGDDDDDDDDDDDDDDDDDDDDDDDDDDDGDDDGGDDDDGGGGDGGGGGGGGGGGGGGGGGGGGGGGGGGDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD";
                sLevel += "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD";
                sLevel += "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD";
                sLevel += "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD";
            }      
      
            nLevelWidth = 256;
            nLevelHeight = 22;

            bmp = new Bitmap(size.Width / divs, size.Height / divs);

            g = Graphics.FromImage(bmp);
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            g.SmoothingMode = SmoothingMode.HighSpeed;
        }

        public void Draw(PointF cameraPos, string message, Player player)
        {

            //Draw background first of all
            //Layer 1
            g.DrawImage(layer1, l1_x1, 0, width, bmp.Height);
            g.DrawImage(layer1, l1_x2, 0, width, bmp.Height);

            // Draw Level based on the visible tiles on our picturebox (canvas)
            nVisibleTilesX = bmp.Width / nTileWidth;
            nVisibleTilesY = bmp.Height / nTileHeight;

            // Calculate Top-Leftmost visible tile
            fOffsetX = cameraPos.X - (float)nVisibleTilesX / 2.0f;
            fOffsetY = cameraPos.Y - (float)nVisibleTilesY / 2.0f;

            // Clamp camera to game boundaries
            if (fOffsetX < 0) fOffsetX = 0;
            if (fOffsetY < 0) fOffsetY = 0;
            if (fOffsetX > nLevelWidth - nVisibleTilesX) fOffsetX = nLevelWidth - nVisibleTilesX;
            if (fOffsetY > nLevelHeight - nVisibleTilesY) fOffsetY = nLevelHeight - nVisibleTilesY;

            float fTileOffsetX = (fOffsetX - (int)fOffsetX) * nTileWidth;
            float fTileOffsetY = (fOffsetY - (int)fOffsetY) * nTileHeight;
            GC.Collect();

            //Draw visible tile map
            char c;
            float stepX, stepY;

            //Creating divisions for drawings
            int quarterWidth = nTileWidth / 4;
            int quarterHeight = nTileHeight / 4;
            int grassHeight = 3;

            //Declaring brushes
            SolidBrush dirtBrush = new SolidBrush(dirt);
            SolidBrush dirt2Brush = new SolidBrush(dirt2);


            //For to draw map based on the element
            for (int x = -1; x < nVisibleTilesX + 2; x++)
            {
                for (int y = -1; y < nVisibleTilesY + 2; y++)
                {
                    c = GetTile(x + (int)fOffsetX, y + (int)fOffsetY);
                    stepX = x * nTileWidth - fTileOffsetX;
                    stepY = y * nTileHeight - fTileOffsetY;

                    //Drawing with every character declared on the map
                    switch (c)
                    {
                        case '.': //empty and transparent tiles
                            g.FillRectangle(Brushes.Transparent, stepX, stepY, nTileWidth, nTileHeight);
                            break;

                        case 'G': //Grass blocks
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    for (int j = 0; j < 4; j++)
                                    {
                                        g.FillRectangle((i + j) % 2 == 0 ? dirtBrush : dirt2Brush, stepX + i * quarterWidth, stepY + j * quarterHeight, quarterWidth, quarterHeight);
                                    }
                                }

                                int numberOfGrassLines = 7; // Number of vertical grass lines, considering the existing horizontal grass line
                                float grassLineWidth = (float)nTileWidth / (numberOfGrassLines + 1);

                                using (Pen grassPen = new Pen(grass, grassHeight)) // Fill with green line to simulate grass
                                {
                                    g.DrawLine(grassPen, stepX, stepY, stepX + nTileWidth, stepY);

                                    for (int i = 1; i <= numberOfGrassLines; i++)
                                    {
                                        using (Pen grassPenVertical = new Pen(i % 2 == 0 ? grass : grass2, grassHeight))
                                        {
                                            float lineX = stepX + i * grassLineWidth;
                                            g.DrawLine(grassPenVertical, lineX, stepY - grassHeight, lineX, stepY + grassHeight);
                                        }
                                    }
                                }
                            }
                            break;

                        case 'D': //Dirt blocks
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    for (int j = 0; j < 4; j++)
                                    {
                                        g.FillRectangle((i + j) % 2 == 0 ? dirtBrush : dirt2Brush, stepX + i * quarterWidth, stepY + j * quarterHeight, quarterWidth, quarterHeight);
                                    }
                                }
                            }
                            break;

                        case 'F': //Sunflowers
                            { 
                                    using (Pen greenPen = new Pen(Color.Green, 2))
                                    {
                                        g.DrawLine(greenPen, stepX + nTileWidth / 2, stepY, stepX + nTileWidth / 2, stepY + nTileHeight);
                                    }

                                    PointF circleCenter = new PointF(stepX + nTileWidth / 2, stepY);

                                    float bigCircleRadius = nTileHeight / 4;
                                    float smallCircleRadius = bigCircleRadius / 2;

                                    using (SolidBrush yellowBrush = new SolidBrush(Color.Yellow))
                                    {
                                        g.FillEllipse(yellowBrush, circleCenter.X - bigCircleRadius, circleCenter.Y - bigCircleRadius, bigCircleRadius * 2, bigCircleRadius * 2);
                                    }

                                    using (SolidBrush brownBrush = new SolidBrush(Color.Brown))
                                    {
                                        g.FillEllipse(brownBrush, circleCenter.X - smallCircleRadius, circleCenter.Y - smallCircleRadius, smallCircleRadius * 2, smallCircleRadius * 2);
                                    }
                            }
                            break;

                        case 'o':
                            coin.posX = stepX;
                            coin.posY = stepY;
                            coin.MoveSlow(7);
                            coin.Display(g);
                            break;

                        case '*': 
                            g.DrawImage(Resource1.questionS, stepX, stepY, nTileWidth, nTileHeight);
                            break;

                        case 'S'://Spikes
                            {
                                float sideLength = (float)(2 * nTileHeight / Math.Sqrt(3));

                                float triangleHeight = nTileHeight;

                                PointF[] triangle1Points = {
                            new PointF(stepX + sideLength / 6, stepY),
                            new PointF(stepX, stepY + triangleHeight),
                            new PointF(stepX + sideLength / 3, stepY + triangleHeight)
                            };

                                PointF[] triangle2Points = {
                            new PointF(stepX + sideLength / 2, stepY),
                            new PointF(stepX + sideLength / 3, stepY + triangleHeight),
                            new PointF(stepX + sideLength * 2 / 3, stepY + triangleHeight)
                            };

                                PointF[] triangle3Points = {
                            new PointF(stepX + sideLength * 5 / 6, stepY),
                            new PointF(stepX + sideLength * 2 / 3, stepY + triangleHeight),
                            new PointF(stepX + sideLength, stepY + triangleHeight)
                           };

                                using (SolidBrush grayBrush = new SolidBrush(Color.Gray))
                                using (Pen blackPen = new Pen(Color.Black))
                                {
                                    g.FillPolygon(grayBrush, triangle1Points);
                                    g.DrawPolygon(blackPen, triangle1Points);

                                    g.FillPolygon(grayBrush, triangle2Points);
                                    g.DrawPolygon(blackPen, triangle2Points);

                                    g.FillPolygon(grayBrush, triangle3Points);
                                    g.DrawPolygon(blackPen, triangle3Points);
                                }


                            }
                        break;


                        default:          
                            g.FillRectangle(Brushes.Black, stepX , stepY , nTileWidth , nTileHeight );
                            break;


                        case 's':
                            SpikeBall.posX = stepX;
                            SpikeBall.posY = stepY;
                            SpikeBall.RotationEnemies(2);
                            SpikeBall.Display(g);
                            break;
                        case 'e':
                            star.posX = stepX;
                            star.posY = stepY;
                            star.RotationEnemies(2);
                            star.Display(g);
                            break;
                        case 'u':
                            snowman.posX = stepX;
                            snowman.posY = stepY;
                            snowman.RotationEnemies(3);
                            snowman.Display(g);
                            break;
                    }
                    //g.DrawRectangle(Pens.Gray, stepX, stepY, nTileWidth, nTileHeight);
                }
            }

            g.DrawString("SCORE: " + score, new Font("Consolas", 10, FontStyle.Italic), Brushes.White, 5, 5);
            g.DrawString("RINGS: " + fRings, new Font("Consolas", 10, FontStyle.Italic), Brushes.White, 5, 18);


            player.MainSprite.posX = (player.fPlayerPosX - fOffsetX) * nTileWidth;
            player.MainSprite.posY = (player.fPlayerPosY - fOffsetY) * nTileHeight;
            fRings = player.fRings;

           
        }

        public void SetTile(float x, float y, char c)//changes the tile
        {
            if (x >= 0 && x < nLevelWidth && y >= 0 && y < nLevelHeight)
            {
                int index = (int)y * nLevelWidth + (int)x;
                sLevel = sLevel.Remove(index, 1).Insert(index, c.ToString());
                Play();
                score += 100;
                /*
                if (c == 'o')
                {
                    score += 100;
                }
                else if (c == 'S')
                {
                    MessageBox.Show("Game over", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    Environment.Exit(0);
                }*/

              
                

            }
        }

        public char GetTile(float x, float y)
        {
            if (x >= 0 && x < nLevelWidth && y >= 0 && y < nLevelHeight)
                return sLevel[(int)y * nLevelWidth + (int)x];
            else
                return ' ';
        }
        public void Play()
        {
            if (isP1)
            {
                thread = new Thread(PlayThread);
                thread.Start();
            }
            threadStop = new Thread(PlayStop);
            threadStop.Start();
        }
        private void PlayThread()
        {
            isP1 = false;
            soundPlayer.PlaySync();
            isP1 = true;            
        }
        private void PlayStop()
        {
            soundPlayer.Stop();
        }

    }
}
