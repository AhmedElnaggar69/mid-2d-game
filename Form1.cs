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
        public class daway
        {
            int x;
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
        public class bullet
        {
            public int x, y;
            public int width, height;
        }

        //flags
        public bool isrunning = false;
        public bool runn_nd_shot = false;
        public bool isRunningBackward = false;
        public bool multi_shot_fired = false;
        public bool single_shot_fired = false;

        // Track if single shot key is currently held to avoid rapid-fire on holding
        private bool singleShotKeyDown = false;

        //lists and objects
        public List<background> layers = new List<background>();
        public muli_img playerobj = new muli_img();
        public List<Bitmap> idle_frames = new List<Bitmap>();
        public List<Bitmap> run_frames = new List<Bitmap>();
        public List<one_img> groundlist = new List<one_img>();
        public List<Bitmap> run_shot_frames = new List<Bitmap>();
        public List<bullet> multi_bullets = new List<bullet>();
        public List<Bitmap> up_shot_frames = new List<Bitmap>();
        public List<bullet> single_bullets = new List<bullet>();

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

        //ints
        int ct = 0;

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
            if (isrunning && !runn_nd_shot && !single_shot_fired)
            {
                playerobj.frames = run_frames;
             
                if (layers[2].des.X <= 0)
                {
                    layers[1].scr.X = 0;
                    layers[2].des.X = this.ClientSize.Width;
                    layers[5].scr.X = 0;
                    layers[6].des.X = this.ClientSize.Width;
                }

                layers[1].scr.X++;  
                layers[5].scr.X++;
                layers[2].des.X -= 5;
                layers[6].des.X -= 5;
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
            else if (runn_nd_shot)
            {
                playerobj.frames = run_shot_frames;
                if (layers[2].des.X <= 0)
                {
                    layers[1].scr.X = 0;
                    layers[2].des.X = this.ClientSize.Width;
                    layers[5].scr.X = 0;
                    layers[6].des.X = this.ClientSize.Width;
                }

                layers[1].scr.X++;
                layers[5].scr.X++;
                layers[2].des.X -= 5;
                layers[6].des.X -= 5;
                playerobj.frame_index++;
                if (playerobj.frame_index >= run_shot_frames.Count)
                {
                    playerobj.frame_index = 0; 
                }
                else
                {
                    playerobj.x += 20;
                }
            }
            else if (single_shot_fired)
            {
                playerobj.frames = up_shot_frames; 
            }
            else
            {
                playerobj.frames = idle_frames;
                playerobj.frame_index++;
                if (playerobj.frame_index >= idle_frames.Count)
                {
                    playerobj.frame_index = 0;  
                }
            }

            if (multi_shot_fired)
            {
                if (ct % 2 == 0)
                {
                    bullet bullet = new bullet();
                    bullet.x = playerobj.x + 160;
                    bullet.y = playerobj.y + 140;
                    bullet.width = 20;
                    bullet.height = 20;

                    multi_bullets.Add(bullet); 
                }
            }

            if (single_shot_fired)
            {
                bullet bullet = new bullet();
                bullet.x = playerobj.x + 160;  
                bullet.y = playerobj.y + 100;
                bullet.width = 20;
                bullet.height = 20;
                single_bullets.Add(bullet);

                single_shot_fired = false;
            }

            foreach (bullet i in multi_bullets)
            {
                i.x += 80;
            }

            for (int i = single_bullets.Count - 1; i >= 0; i--)
            {
                bullet b = single_bullets[i];
                b.y -= 20;  
                if (b.y + b.height < 0)
                {
                    single_bullets.RemoveAt(i);  
                }
            }

            drawdubb(this.CreateGraphics());
            ct++;
        }

        public void loading(object sender, EventArgs e)
        {
            off = new Bitmap(this.Width, this.Height);
            baclground_code();
            player();
            run_shot();
            shot_up();
        }

        public void shot_up()
        {
            string[] path = new string[]
            {
                "player-shoot-up.png",
            };

            for (int j = 0; j < path.Length; j++)
            {
                Bitmap img = new Bitmap(path[j]);
                img.MakeTransparent(img.GetPixel(0, 0));
                up_shot_frames.Add(img);
            }
        }

        public void run_shot()
        {
            string[] path = new string[]
            {
                "player-run-shot/player-run-shot-1.png",
                "player-run-shot/player-run-shot-2.png",
                "player-run-shot/player-run-shot-3.png",
                "player-run-shot/player-run-shot-4.png",
                "player-run-shot/player-run-shot-5.png",
                "player-run-shot/player-run-shot-6.png",
                "player-run-shot/player-run-shot-7.png",
                "player-run-shot/player-run-shot-8.png",
                "player-run-shot/player-run-shot-9.png",
                "player-run-shot/player-run-shot-10.png",
            };

            for (int j = 0; j < path.Length; j++)
            {
                Bitmap img = new Bitmap(path[j]);
                img.MakeTransparent(img.GetPixel(0, 0));
                run_shot_frames.Add(img);  
            }
        }

        public void baclground_code()
        {
            scene = new Bitmap("background/scene.png");
            trees = new Bitmap("background/trees.png");
            mountains = new Bitmap("background/mountains.png");
            mountain = new Bitmap("background/mountain.png");
            grass = new Bitmap("background/grass.png");

            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;
            Rectangle fullScreen = new Rectangle(0, 0, w, h);

            layers.Add(new background { img = scene, des = fullScreen, scr = new Rectangle(0, 0, scene.Width, scene.Height) });
            layers.Add(new background { img = mountains, des = fullScreen, scr = new Rectangle(0, 0, mountains.Width, mountains.Height) });
            layers.Add(new background { img = mountains, des = new Rectangle(this.ClientSize.Width, 0, this.ClientSize.Width, this.ClientSize.Height), scr = new Rectangle(0, 0, mountains.Width, mountains.Height) });
            layers.Add(new background { img = trees, des = fullScreen, scr = new Rectangle(0, 0, trees.Width, trees.Height) });
            layers.Add(new background { img = mountain, des = fullScreen, scr = new Rectangle(0, 0, mountain.Width, mountain.Height) });
            layers.Add(new background { img = grass, des = fullScreen, scr = new Rectangle(0, 0, grass.Width, grass.Height) });
            layers.Add(new background { img = grass, des = new Rectangle(this.ClientSize.Width, 0, this.ClientSize.Width, this.ClientSize.Height), scr = new Rectangle(0, 0, grass.Width, grass.Height) });

            
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

            if (e.KeyCode == Keys.R)
            {
                multi_shot_fired = !multi_shot_fired;
                runn_nd_shot = !runn_nd_shot;
                playerobj.frame_index = 0;
            }

            if (e.KeyCode == Keys.E)
            {
                if (!singleShotKeyDown)
                {
                    single_shot_fired = true;
                    playerobj.frame_index = 0;
                    singleShotKeyDown = true;  
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

            if (e.KeyCode == Keys.E)
            {
                singleShotKeyDown = false;
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

            for (int i = 0; i < layers.Count; i++)
            {
                g.DrawImage(layers[i].img, layers[i].des, layers[i].scr, GraphicsUnit.Pixel);
            }

            int scale = 4;
            int newWidth = playerobj.frames[playerobj.frame_index].Width * scale;
            int newHeight = playerobj.frames[playerobj.frame_index].Height * scale;

            if (isRunningBackward)
            {
                g.TranslateTransform(playerobj.x + newWidth, playerobj.y);
                g.ScaleTransform(-1, 1);
                g.DrawImage(playerobj.frames[playerobj.frame_index], new Rectangle(0, 0, newWidth, newHeight));
                g.ResetTransform(); 
            }
            else
            {
                g.DrawImage(playerobj.frames[playerobj.frame_index], new Rectangle(playerobj.x, playerobj.y, newWidth, newHeight));
            }

            foreach (one_img i in groundlist)
            {
                g.DrawImage(i.img, new Rectangle(i.x, i.y, 200, 20));
            }

            foreach (bullet i in multi_bullets)
            {
                using (Pen p = new Pen(color: Color.Red))
                {
                    g.DrawEllipse(p, i.x, i.y, i.width, i.height);
                    g.FillEllipse(Brushes.Red, i.x, i.y, i.width, i.height);
                }
            }

            foreach (bullet i in single_bullets)
            {
                using (Pen p = new Pen(color: Color.Red))
                {
                    g.DrawEllipse(p, i.x, i.y, i.width, i.height);
                    g.FillEllipse(Brushes.Red, i.x, i.y, i.width, i.height);
                }
            }
        }
    }
}


