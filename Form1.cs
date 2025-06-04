using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game
{
    public partial class Form1 : Form
    {
        // classes
        public class background
        {
            public Rectangle des;
            public Rectangle scr;
            public Bitmap img;

        }
        public class muli_img
        {
            public int x, y;
            public List<Bitmap> frames;
            public int frame_index;
        }

        public class one_img
        {
            public int x, y;
            public Bitmap img;
        }



        //flags
        public bool isrunning = false;

        public bool isRunningBackward = false;


        //lists and objects
        public List<background>layers = new List<background>();
        public muli_img playerobj = new muli_img();
        public List<Bitmap> idle_frames = new List<Bitmap>();
        public List<Bitmap> run_frames = new List<Bitmap>();
        public List<one_img> groundlist = new List<one_img>();



        //boring
        Timer tt = new Timer();
        Bitmap off;

        // background/ground Bitmaps;
        Bitmap trees;
        Bitmap mountains;
        Bitmap mountain;

        Bitmap grass;
        Bitmap scene;
        Bitmap ground;



        // scrolling / cam tracking
        public Form1()
        {
            this.Load += loading;
            this.KeyUp += up;
            this.KeyDown += press;
            this.Paint += draw;
            WindowState = FormWindowState.Maximized;
            tt.Tick += update;
            tt.Start();
            tt.Interval = 16;
        }

        public void update(object sender, EventArgs e)
        {

            // if we are running get the playerobj the running frames and move the frames
            if (isrunning)
            {
                playerobj.frames = run_frames;

                playerobj.frame_index++;
                if (playerobj.frame_index >= run_frames.Count)
                {
                    playerobj.frame_index = 0;
                }
                if (isRunningBackward)
                {
                    playerobj.x -= 20;

                }
                else
                {
                    playerobj.x += 20;

                }
            }
            else
            {

                // if we are in idle get the playerobj the idle frames and move the frames
                playerobj.frames = idle_frames;

                playerobj.frame_index++;
                if (playerobj.frame_index >= idle_frames.Count)
                {
                    playerobj.frame_index = 0;
                }
            }

            drawdubb(this.CreateGraphics());
        }


        public void loading(object sender, EventArgs e)
        {
            off = new Bitmap(this.Width, this.Height);

            baclground_code();
            player();
        }

        public void baclground_code()
        {
            //load background 

            scene = new Bitmap("background/scene.png");
            trees = new Bitmap("background/trees.png");
            mountains = new Bitmap("background/mountains.png");
            mountain = new Bitmap("background/mountain.png");
            grass = new Bitmap("background/grass.png");
            

            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;
            Rectangle fullScreen = new Rectangle(0, 0, w, h);

            // add layers to the list
            layers.Add(new background { img = scene, des = fullScreen, scr = new Rectangle(0, 0, scene.Width, scene.Height) });
            layers.Add(new background { img = mountains, des = fullScreen, scr = new Rectangle(0, 0, mountains.Width, mountains.Height) });
            layers.Add(new background { img = trees, des = fullScreen, scr = new Rectangle(0, 0, trees.Width, trees.Height) });
            layers.Add(new background { img = mountain, des = fullScreen, scr = new Rectangle(0, 0, mountain.Width, mountain.Height) });
            layers.Add(new background { img = grass, des = fullScreen, scr = new Rectangle(0, 0, grass.Width, grass.Height) });

            // ground 

            ground = new Bitmap("ground.png");
            int groundWidth = 130; 
            int groundY = this.ClientSize.Height - 52;

            for (int i = 0; i < 30; i++)
            {
                one_img ground_obj = new one_img();
                ground_obj.img = ground;
                ground_obj.x = i * groundWidth;
                ground_obj.y = groundY;
                groundlist.Add(ground_obj);
            }




        }






        public void player()
        {
            player_idel_frames();
            player_running_frames();

            playerobj.x = 100;
            playerobj.y = this.Height - 350;
            playerobj.frame_index = 0;
            playerobj.frames = idle_frames;

        }

        public void player_idel_frames()
        {
            string[] path = new string[]
            {
        "player-idle/1.png",
        "player-idle/2.png",
        "player-idle/3.png",
        "player-idle/1.png",
            };

            for (int j = 0; j < path.Length; j++)
            {
                Bitmap img = new Bitmap(path[j]);
                img.MakeTransparent(img.GetPixel(0, 0));
                idle_frames.Add(img);
            }
        }

        public void player_running_frames()
        {
            string[] path = new string[]
            {
        "player-run/1.png",
        "player-run/2.png",
        "player-run/3.png",
        "player-run/4.png",
        "player-run/5.png",
        "player-run/6.png",
        "player-run/7.png",
        "player-run/8.png",
        "player-run/9.png",
        "player-run/10.png"
            };

            for (int j = 0; j < path.Length; j++)
            {
                Bitmap img = new Bitmap(path[j]);
                img.MakeTransparent(img.GetPixel(0, 0));
                run_frames.Add(img);
            }
        }









        public void press(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                if (!isrunning)
                {
                    isrunning = true;
                    playerobj.frame_index = 0; 
                }
            }


            if (e.KeyCode == Keys.Left)
            {
                if (!isrunning)
                {
                    isrunning = true;
                    isRunningBackward = true;
                    playerobj.frame_index = 0;
                }
            }
        }

        public void up(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                isrunning = false;
                playerobj.frame_index = 0; 
            }



            if (e.KeyCode == Keys.Left)
            {
                isrunning = false;
                isRunningBackward = false;
                playerobj.frame_index = 0;
            }



        }


        public void draw(object sender, PaintEventArgs e)
        {
            drawdubb(e.Graphics);


        }
        public void drawdubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            draw_stuff(g2);
            g.DrawImage(off, 0, 0);
        }
        public void draw_stuff(Graphics g)
        {
            g.Clear(Color.Black);

      


            // Draw background layers
            foreach (background layer in layers)
            {
                g.DrawImage(layer.img, layer.des, layer.scr, GraphicsUnit.Pixel);
            }


            // scale shitty assets
            int scale = 4;
            int newWidth = playerobj.frames[playerobj.frame_index].Width * scale;
            int newHeight = playerobj.frames[playerobj.frame_index].Height * scale;



            // chatgpt stuff cause there are not sprites for backward motion
            if (isRunningBackward)
            {
                // Flip horizontally when running backward
                g.TranslateTransform(playerobj.x + newWidth, playerobj.y);
                g.ScaleTransform(-1, 1);
                g.DrawImage(playerobj.frames[playerobj.frame_index], new Rectangle(0, 0, newWidth, newHeight));
                // Reset transformation so other drawings are unaffected
                g.ResetTransform();
            }
            else
            {
                // normal draw for idle or running forward
                g.DrawImage(playerobj.frames[playerobj.frame_index], new Rectangle(playerobj.x, playerobj.y, newWidth, newHeight));
            }
            //ground

            foreach (one_img i in groundlist)
            {
                g.DrawImage(i.img, new Rectangle(i.x, i.y, 200, 20));
            }
        }

    }
}
