using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Scroll
{
    public class Player
    {
        //Declaration of Pos and speed within getters and setters
        Sprite mainSprite;
        
        public float fPlayerPosX = 1.0f;
        public float fPlayerPosY = 1.0f;

        private float fPlayerVelX = 0.0f;
        private float fPlayerVelY = 0.0f;

        public int fRings = 0;
        float Antideath;

        public int FRings
        {
            get { return fRings; }
            set { fRings = value; }
        }


        public Sprite MainSprite
        {
            get { return mainSprite; }
        }

        public float FPlayerVelX
        {
            get { return fPlayerVelX; }
            set { fPlayerVelX = value; }
        }        

        public float FPlayerVelY
        {
            get { return fPlayerVelY; }
            set { fPlayerVelY = value; }
        }

        public Player()
        {
            mainSprite = new Sprite(new Size(44, 36), new Size(20, 25), new Point(), Resource1.tails_r, Resource1.tails_l, Resource1.tails_idle_r, Resource1.tails_idle_l);
        }

        public void Right(float fElapsedTime)
        {
            FPlayerVelX += (bPlayerOnGround ? 25.0f : 15.0f) * fElapsedTime;
            if(bPlayerOnGround)
                mainSprite.MoveRight();
        }

        public void Left(float fElapsedTime)
        {
            FPlayerVelX += (bPlayerOnGround ? -25.0f : -15.0f) * fElapsedTime;
            if(bPlayerOnGround)
                mainSprite.MoveLeft();
        }

        public void Frame(int x)
        {
            mainSprite.Frame(x);
        }
        public void Stop()
        {
            mainSprite.Frame(0);
        }

        public bool bPlayerOnGround = false;

        public void Update(float fElapsedTime, Map map)
        {
            //Gravity
            fPlayerVelY += 20.0f * fElapsedTime;//---------------

            // Drag
            if (bPlayerOnGround)
            {
                fPlayerVelX += -3.0f * fPlayerVelX * fElapsedTime;
                if (Math.Abs(fPlayerVelX) < 0.01f)
                    fPlayerVelX = 0.0f;
            }

            // Clamp velocities
            if (fPlayerVelX > 10.0f)
                fPlayerVelX = 10.0f;

            if (fPlayerVelX < -10.0f)
                fPlayerVelX = -10.0f;

            if (fPlayerVelY > 100.0f)
                fPlayerVelY = 100.0f;

            if (fPlayerVelY < -100.0f)
                fPlayerVelY = -100.0f;

            float fNewPlayerPosX = fPlayerPosX + fPlayerVelX * fElapsedTime;
            float fNewPlayerPosY = fPlayerPosY + fPlayerVelY * fElapsedTime;

            CheckRing(map, fNewPlayerPosX, fNewPlayerPosY, 'o', fRings);
            LoseRing(map, fNewPlayerPosX, fNewPlayerPosY, 's', fRings);
            LoseRing(map, fNewPlayerPosX, fNewPlayerPosY, 'e', fRings);
            LoseRing(map, fNewPlayerPosX, fNewPlayerPosY, 'u', fRings);


            CheckPicks(map, fNewPlayerPosX, fNewPlayerPosY, 'o','.');
            CheckPicks(map, fNewPlayerPosX, fNewPlayerPosY, '*', 'a');
            CheckSpikes(map, fNewPlayerPosX, fNewPlayerPosY);



            // COLLISION
            if (fPlayerVelX <= 0)//left
            {
                if ((map.GetTile((int)(fNewPlayerPosX + 0.0f), (int)(fPlayerPosY + 0.0f)) != '.') || (map.GetTile((int)(fNewPlayerPosX + 0.0f), (int)(fPlayerPosY + 0.9f)) != '.'))
                {
                    if (fPlayerVelX != 0)
                        fNewPlayerPosX = (int)fNewPlayerPosX + 1;
                    fPlayerVelX = 0;                   
                }
            }
            else//right
            {
                if ((map.GetTile((int)(fNewPlayerPosX + 1.0f), (int)(fPlayerPosY + 0.0f)) != '.') || (map.GetTile((int)(fNewPlayerPosX + 1.0f), (int)(fPlayerPosY + 0.9f)) != '.'))
                {
                    if (fPlayerVelX != 0)
                        fNewPlayerPosX = (int)fNewPlayerPosX;

                    fPlayerVelX = 0;                   
                }
            }

            //bPlayerOnGround = false;
            if (fPlayerVelY <= 0)// up
            {
                if ((map.GetTile((int)(fNewPlayerPosX + 0.0f), (int)(fNewPlayerPosY + 0.0f)) != '.') || (map.GetTile((int)(fNewPlayerPosX + 0.9f), (int)(fNewPlayerPosY + 0.0f)) != '.'))
                {
                    fNewPlayerPosY = (int)fNewPlayerPosY + 1;
                    fPlayerVelY = 0;                   
                }
            }
            else
            {
                if ((map.GetTile((int)(fNewPlayerPosX + 0.0f), (int)(fNewPlayerPosY + 1.0f)) != '.') || (map.GetTile((int)(fNewPlayerPosX + 0.9f), (int)(fNewPlayerPosY + 1f)) != '.'))
                {
                    fNewPlayerPosY = (int)fNewPlayerPosY;
                    fPlayerVelY = 0;
                    if(!bPlayerOnGround)
                        Frame(0);
                    
                    bPlayerOnGround = true;                    
                }
            }
            
            fPlayerPosX = fNewPlayerPosX;
            fPlayerPosY = fNewPlayerPosY;

            mainSprite.Display(map.g);
        }

        private static void CheckPicks(Map map, float fNewPlayerPosX, float fNewPlayerPosY,char c, char c2)
        {
            // Check for pickups!
            if (map.GetTile(fNewPlayerPosX + 0.0f, fNewPlayerPosY + 0.0f) == c)
                map.SetTile(fNewPlayerPosX + 0.0f, fNewPlayerPosY + 0.0f, c2);

            if (map.GetTile(fNewPlayerPosX + 0.0f, fNewPlayerPosY + 1.0f) == c)
                map.SetTile(fNewPlayerPosX + 0.0f, fNewPlayerPosY + 1.0f, c2);

            if (map.GetTile(fNewPlayerPosX + 1.0f, fNewPlayerPosY + 0.0f) == c)
                map.SetTile(fNewPlayerPosX + 1.0f, fNewPlayerPosY + 0.0f, c2);

            if (map.GetTile(fNewPlayerPosX + 1.0f, fNewPlayerPosY + 1.0f) == c)
                map.SetTile(fNewPlayerPosX + 1.0f, fNewPlayerPosY + 1.0f, c2);
        
        }

        private static void CheckSpikes(Map map, float fNewPlayerPosX, float fNewPlayerPosY)
        {
            // Check for pickups!
            if (map.GetTile(fNewPlayerPosX + 0.0f, fNewPlayerPosY + 0.0f) == 'S')
            {
                MessageBox.Show("Game over", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                Environment.Exit(0);
            }
            if (map.GetTile(fNewPlayerPosX + 0.0f, fNewPlayerPosY + 1.0f) == 'S')
            {
                MessageBox.Show("Game over", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                Environment.Exit(0);
            }

            if (map.GetTile(fNewPlayerPosX + 1.0f, fNewPlayerPosY + 0.0f) == 'S')
            {
                MessageBox.Show("Game over", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                Environment.Exit(0);
            }

            if (map.GetTile(fNewPlayerPosX + 1.0f, fNewPlayerPosY + 1.0f) == 'S')
            {
                MessageBox.Show("Game over", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                Environment.Exit(0);
            }

        }


        public void CheckRing(Map map, float fNewPlayerPosX, float fNewPlayerPosY, char c, int Rings)
        {

            if (map.GetTile(fNewPlayerPosX + 0.0f, fNewPlayerPosY + 0.0f) == c)
                Rings += 1;
            Antideath = 1;

            if (map.GetTile(fNewPlayerPosX + 0.0f, fNewPlayerPosY + 1.0f) == c)
                Rings += 1;
            Antideath = 1;

            if (map.GetTile(fNewPlayerPosX + 1.0f, fNewPlayerPosY + 0.0f) == c)
                Rings += 1;
            Antideath = 1;

            if (map.GetTile(fNewPlayerPosX + 1.0f, fNewPlayerPosY + 1.0f) == c)
                Rings += 1;
            Antideath = 1;

            fRings = Rings;
            //fAntideath = Antideath;
        }


        public void LoseRing(Map map, float fNewPlayerPosX, float fNewPlayerPosY, char c, int Rings)
        {

            if (map.GetTile(fNewPlayerPosX + 0.0f, fNewPlayerPosY + 0.0f) == c)
            {

                if (Rings != 0)
                {
                    Rings = 0;
                    Antideath -= 0.5f;
                }

                else
                {
                    MessageBox.Show("Game over", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    Environment.Exit(0);
                }

            }

            if (map.GetTile(fNewPlayerPosX + 0.0f, fNewPlayerPosY + 1.0f) == c)
            {

                if (Rings != 0)
                {
                    Rings = 0;
                    //Antideath -= 0.5f;
                }

                else
                {
                    MessageBox.Show("Game over", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    Environment.Exit(0);
                }

            }

            if (map.GetTile(fNewPlayerPosX + 1.0f, fNewPlayerPosY + 0.0f) == c)
            {

                if (Rings != 0)
                {
                    Rings = 0;
                    //Antideath -= 0.5f;
                }

                else
                {
                    MessageBox.Show("Game over", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    Environment.Exit(0);
                }

            }

            if (map.GetTile(fNewPlayerPosX + 1.0f, fNewPlayerPosY + 1.0f) == c)
            {

                if (Rings != 0)
                {
                    Rings = 0;
                    //Antideath -= 0.5f;
                }

                else
                {
                    MessageBox.Show("Game over", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    Environment.Exit(0);
                }

            }


            fRings = Rings;
            //fAntideath = Antideath;
        }
    }
}
