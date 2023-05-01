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
        Sprite coin;

        int score;

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

        public Map(Size size)
        {            
            coin        = new Sprite(new Size(35, 33), new Size(nTileWidth, nTileHeight), new Point(), Resource1.coin, Resource1.coin);
            soundPlayer = new SoundPlayer(Resource1.coimp);
            
            layer1 = Resource1.beach;

            l1_x1 = l2_x2 = 0;
            l1_x2 = l2_x2 = width;

            Play();
            score = 0;


            //Map made of characters
            {
                sLevel = "";
                sLevel += "..........................................................................................................................0000000000.............................................00000000000000.....0000000000000000000000000..........*........................";
                sLevel += "..........................................................................................................................0000000000...........00000000000000000.................00000000000000.....0000000000000000000000000...........*.......................";
                sLevel += "..........................................................................................................................0000000000...........00000000000000000.................00000000000000.....0000000000000000000000000............*......................";
                sLevel += "..........................................................................................####################..................................................................................................................................................";
                sLevel += "..............................................................................................................................................#############################################################################################*###################.";
                sLevel += "............................................................................................................................................................................................................................................*...................";
                sLevel += ".............................................................................................................................................................................................................................................*..................";
                sLevel += "...........................................................................................................................................*********************************************************************************************************************";
                sLevel += ".........................................................................................................................................................................................********...*****************************..............*................";
                sLevel += ".....................................................................................00000000000000000000000000000000...................ooo..............................................................//////.................................*......*........";
                sLevel += ".....................................................................................0000000BBBBBBBBBBBBBBB0B00000000**.................ooo......................................................................................................*......*.......";
                sLevel += ".................................................#############.......................0000000000BBBBBBBBBBB00000000000..000000000000000000000000000000000000.........................11100000000000000000000000000000000000000000000000000000000...*......*......";
                sLevel += ".....................................................................................0000000BBBBBBBBBBBBBBBB0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000.................*......**....";
                sLevel += ".....0...............................................................................00000000000000000000000000000000...........*************************************************..............555..................................................*.......*...";
                sLevel += "........?oa..BBBBBBB........................BBBB.......##########.......................000000000000000000000000........................ooo.......................................................5555...............................................*..........";
                sLevel += ".......................................##########................##.....................................................................ooo............................................................66.............................................*.........";
                sLevel += "...............................................................................................................................*****....ooo............................................................................................................*........";
                sLevel += "................aa................................................***.....................................................-****.........ooo.............................................................................................................*.......";
                sLevel += "..........................o...............k.......................o..............BBBBBBBBBBBBBB00000000000000000000005555555555.........ooo..............................................................................................................*......";
                sLevel += "BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBbbbbbbbbbbbbbb.............................................................................ooo...............................................................................................................*.....";
                sLevel += "..............................................................SSSSSSSSSSSSSSSSSSSSSSSSS.................................................ooo................................................................................................................*....";
                sLevel += "........................................................................................................................................ooo.................................................................................................................*...";
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
            int grassHeight = 6;

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

                        case 'B': //Grass blocks
                            // Fill the rectangles with two different browns
                            for (int i = 0; i < 4; i++)
                            {
                                for (int j = 0; j < 4; j++)
                                {
                                    g.FillRectangle((i + j) % 2 == 0 ? dirtBrush : dirt2Brush, stepX + i * quarterWidth, stepY + j * quarterHeight, quarterWidth, quarterHeight);
                                }
                            }

                            using (Pen grassPen = new Pen(grass, grassHeight))// Fill with green line to simulate grass
                            {
                                g.DrawLine(grassPen, stepX, stepY, stepX + nTileWidth, stepY);
                            }
                            break;

                        //WEIRD COIN---
                        /*
                        case 'a'://Where coins are declared, might be changed for rings
                            coin.posX = stepX;
                            coin.posY = stepY;
                            coin.MoveSlow(2);
                            coin.Display(g);
                            if (coin.counter > 50)
                            {
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                coin.counter = 0;
                            }
                            break;
                        
                        /*
                        case '#': //Extra block
                            g.FillRectangle(Brushes.Black, stepX, stepY, nTileWidth, nTileHeight);
                            g.FillPolygon(Brushes.Gray, new PointF[] { new PointF(stepX, stepY), new PointF(stepX + nTileWidth, stepY), new PointF(stepX, stepY+nTileHeight) });
                            g.FillRectangle(Brushes.DarkGray, stepX + nTileHeight / 4, stepY + nTileHeight / 4, nTileWidth / 2, nTileHeight / 2);
                            g.DrawLine(Pens.DarkGray, stepX, stepY, stepX + nTileWidth, stepY + nTileHeight);
                            break;
                        */

                        case '?': //Declaration of 
                            g.FillRectangle(Brushes.Yellow, stepX, stepY, nTileWidth, nTileHeight);
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

                        case 'S':
                            g.FillEllipse(Brushes.Gray, stepX, stepY, nTileWidth, nTileHeight);
                            break;
                        default:          
                            g.FillRectangle(Brushes.Black, stepX , stepY , nTileWidth , nTileHeight );
                            g.FillRectangle(Brushes.DarkRed, stepX+1, stepY+1, nTileWidth-2, nTileHeight-2);
                            g.DrawLine(Pens.Black, stepX + nTileHeight / 2, stepY + nTileHeight / 2, stepX + nTileHeight, stepY + nTileHeight -3);
                            g.DrawLine(Pens.Maroon, stepX + nTileHeight / 2, 2+stepY + nTileHeight / 2,1+ stepX + nTileHeight, stepY + nTileHeight - 2);
                            g.DrawLine(Pens.Black, stepX + nTileHeight / 2, stepY, stepX + nTileHeight / 2, stepY + nTileHeight * 2 / 3);
                            g.DrawLine(Pens.Black, 1+stepX + nTileHeight / 2, stepY+1, 2+stepX + nTileHeight / 2,3+ stepY + nTileHeight * 2 / 3);
                            g.DrawLine(Pens.Maroon, 2+stepX + nTileHeight / 2, stepY, 1+stepX + nTileHeight / 2, stepY + nTileHeight * 2 / 3);
                            g.DrawLine(Pens.Black, stepX + nTileHeight / 2, stepY + nTileHeight * 2 / 3, stepX, stepY + nTileHeight / 3);
                            g.DrawRectangle(Pens.Black, stepX + nTileHeight / 2, stepY, nTileWidth, nTileHeight-1);
                            g.DrawRectangle(Pens.Gray, stepX, stepY, nTileWidth, nTileHeight-1);
                            
                            //g.DrawImage(Resource1.sGrass, stepX, stepY, nTileWidth, nTileHeight);//heavy
                            break;
                    }
                    //g.DrawRectangle(Pens.Gray, stepX, stepY, nTileWidth, nTileHeight);
                }
            }

            g.DrawString("SCORE: " + score, new Font("Consolas", 10, FontStyle.Italic), Brushes.White, 5, 5);

            player.MainSprite.posX = (player.fPlayerPosX - fOffsetX) * nTileWidth;
            player.MainSprite.posY = (player.fPlayerPosY - fOffsetY) * nTileHeight;            
        }

        public void SetTile(float x, float y, char c)//changes the tile
        {
            if (x >= 0 && x < nLevelWidth && y >= 0 && y < nLevelHeight)
            {
                int index = (int)y * nLevelWidth + (int)x;
                sLevel = sLevel.Remove(index, 1).Insert(index, c.ToString());
                Play();
                score += 100; 
              
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
