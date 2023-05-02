using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

namespace Scroll
{
    public partial class MAIN : Form
    {
        Map map;
        Player player;
        
        float fElapsedTime;

        SoundPlayer sPlayer;
        Thread thread, thread2;
        bool isP1 = true;

        // Camera properties
        float fCameraPosX = 0.0f;
        float fCameraPosY = 0.0f;
        bool left, right,id_left,id_right;

        public MAIN()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            map                 = new Map(PCT_CANVAS.Size);
            player              = new Player();
            PCT_CANVAS.Image    = map.bmp;
            fElapsedTime        = 0.05f;
            left                = false;
            right               = false;
            id_left             = true;
            id_right            = true;
            sPlayer             = new SoundPlayer(Resource1.snore);
 
            Play();
        }

        public void Play()
        {
            thread = new Thread(PlayThread);
            thread.Start();
        }

        private void PlayThread()
        {
            sPlayer.PlaySync();
        }

        private void MAIN_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:                    
                    left = true;
                    id_left = false;
                    id_right = false;
                    //nDirModY = 1;
                    break;
                case Keys.Right:
                    right = true;
                    id_left = false;
                    id_right = false;
                    //nDirModY = 0;
                    break;
                case Keys.Up:
                    player.FPlayerVelY = -6.0f;
                    player.bPlayerOnGround = false;
                    break;
                case Keys.Down:
                    player.FPlayerVelY = 6.0f;
                    break;
            }

            UpdateEnv();
        }

        private void MAIN_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Space:
                    if (player.FPlayerVelY == 0)// sin brincar o cayendo
                    {
                        player.FPlayerVelY = -15;
                        player.Frame(2);
                        player.bPlayerOnGround = false;
                    }
                    break;
            }
        }

        private void MAIN_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Space)
                return;

            if (e.KeyCode == Keys.Left)
                id_left = true;

            if (e.KeyCode == Keys.Right)
                id_right = true;

            left = false;
            right = false;

            player.Stop();
        }

        private void TIMER_Tick(object sender, EventArgs e)
        {  
            //Check redraw of map
            if (map.fOffsetX > 0 && map.fOffsetX < map.nLevelWidth - map.nVisibleTilesX)
            {
                if (right)
                {
                    if (map.l1_x1 < -map.width) { map.l1_x1 = map.width - map.motion1; }
                    map.l1_x1 -= map.motion1; map.l1_x2-= map.motion1;
                    if (map.l1_x2 < map.width) { map.l1_x2 = map.width - map.motion1; }
                }

                if(left)
                {
                    if (map.l1_x1 > map.width) { map.l1_x1 = -map.width + map.motion1; }
                    map.l1_x1 += map.motion1; map.l1_x2 += map.motion1;
                    if (map.l1_x2 > map.width) { map.l1_x2 = -map.width + map.motion1; }

                }
            }
           
        //Update map
          UpdateEnv();
        }

    
        private void UpdateEnv()
        {
            if (left)
                player.Left(fElapsedTime);
            if (right)
                player.Right(fElapsedTime);
            if (id_left)
                player.MainSprite.idle_left(6);
            if (id_right)
                player.MainSprite.idle_right(6);

            fCameraPosX = player.fPlayerPosX;
            fCameraPosY = player.fPlayerPosY;

            map.Draw(new PointF(fCameraPosX,fCameraPosY),player.fPlayerPosX.ToString() , player);
            player.Update(fElapsedTime, map);
            PCT_CANVAS.Invalidate();
        }
    }
}
