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
using System.Media;

using NAudio.Wave;
namespace game
{
    public partial class Form1 : Form
    {
        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;

        // classes
        public class background
        {
            public Rectangle des;
            public Rectangle scr;
            public Bitmap img;
        }

        public class muli_img
        {
            public int x, y,w,h;
            public List<Bitmap> frames;
            public int frame_index;
            public float scale = 1.0f;
            public int heath  ;
            public float lastShotTime = 0;
            public float lastLaserDamageTime = 0;
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
            public int dx, dy;
        }

        //flags
        public bool isrunning = false;
        public bool runn_nd_shot = false;
        public bool isRunningBackward = false;
        public bool multi_shot_fired = false;
        public bool single_shot_fired = false;
        public bool jump = false;
        public bool dumb_guy_onthemove = false;
        public bool dumb_guy_go_left = false;
        public bool dumb_guy_go_right = false;
        public bool dead = false;
        private bool singleShotKeyDown = false;
        bool onplatform = true;
        bool laser_range = false;
        bool laser_for_0 = true;
        bool laser_for_1 = true;
        bool laser_for_2 = true;

        //lists and objects
        public List<background> layers = new List<background>();
        public muli_img playerobj = new muli_img();
        public List<Bitmap> idle_frames = new List<Bitmap>();
        public List<Bitmap> run_frames = new List<Bitmap>();
        public List<one_img> groundlist = new List<one_img>();
        public List<one_img> platforms = new List<one_img>();

        public List<Bitmap> run_shot_frames = new List<Bitmap>();
        public List<bullet> multi_bullets = new List<bullet>();
        public List<Bitmap> up_shot_frames = new List<Bitmap>();
        public List<bullet> single_bullets = new List<bullet>();
        public List<Bitmap> playerdeadframes = new List<Bitmap>();
        public List<Bitmap> jump_frames = new List<Bitmap>();
        public List<bullet> laser_beams = new List<bullet>();

        
        public List<Bitmap>dumbguy_idle = new List<Bitmap>();
        public List<Bitmap> dumbguy_walk = new List<Bitmap>();
        public muli_img dumbguy_obj = new muli_img();
        public muli_img dumbguy_obj2 = new muli_img();
        public muli_img dumbguy_obj3 = new muli_img();
        public muli_img dumbguy_obj4= new muli_img();
        public muli_img dumbguy_obj5 = new muli_img();
        public muli_img dumbguy_obj6 = new muli_img();
        public List<muli_img> dumbguys = new List<muli_img>();
        public List<bullet> dumb_guy_bullets = new List<bullet>();
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
        int dumbguy_health = 5;
        int player_health = 10;


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

        public void actor_related()
        {

            if (isrunning && !runn_nd_shot && !single_shot_fired)
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
                    if (layers[2].des.X <= 0 || layers[3].des.X >= 0)
                    {
                        layers[1].des.X = 0;
                        layers[2].des.X = this.ClientSize.Width - 5;
                        layers[3].des.X = 0 - this.ClientSize.Width + 5;
                        layers[6].des.X = 0;
                        layers[7].des.X = this.ClientSize.Width - 5;
                        layers[8].des.X = 0 - this.ClientSize.Width + 5;
                    }
                    //mountian mov
                    layers[1].des.X += 20;
                    layers[2].des.X += 20;
                    layers[3].des.X += 20;
                    //grass move
                    layers[6].des.X += 20;
                    layers[7].des.X += 20;
                    layers[8].des.X += 20;
                }
                else
                {
                    playerobj.x += 20;
                    if (layers[2].des.X <= 0 || layers[3].des.X >= 0)
                    {
                        layers[1].des.X = 0;
                        layers[2].des.X = this.ClientSize.Width - 5;
                        layers[3].des.X = 0 - this.ClientSize.Width + 5;
                        layers[6].des.X = 0;
                        layers[7].des.X = this.ClientSize.Width - 5;
                        layers[8].des.X = 0 - this.ClientSize.Width + 5;
                    }
                    //mountian mov
                    layers[1].des.X -= 20;
                    layers[2].des.X -= 20;
                    layers[3].des.X -= 20;
                    //grass move
                    layers[6].des.X -= 20;
                    layers[7].des.X -= 20;
                    layers[8].des.X -= 20;
                }
            }
            else if (runn_nd_shot)
            {
                playerobj.frames = run_shot_frames;
                if (layers[2].des.X <= 0 || layers[3].des.X >= 0)
                {
                    layers[1].des.X = 0;
                    layers[2].des.X = this.ClientSize.Width - 5;
                    layers[3].des.X = 0 - this.ClientSize.Width + 5;
                    layers[6].des.X = 0;
                    layers[7].des.X = this.ClientSize.Width - 5;
                    layers[8].des.X = 0 - this.ClientSize.Width + 5;
                }
                //mountian mov
                layers[1].des.X -= 20;
                layers[2].des.X -= 20;
                layers[3].des.X -= 20;
                //grass move
                layers[6].des.X -= 20;
                layers[7].des.X -= 20;
                layers[8].des.X -= 20;
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
            else if (jump)
            {
                playerobj.frames = jump_frames;

                if (layers[2].des.X <= 0 || layers[3].des.X >= 0)
                {
                    layers[1].des.X = 0;
                    layers[2].des.X = this.ClientSize.Width - 5;
                    layers[3].des.X = 0 - this.ClientSize.Width + 5;
                    layers[6].des.X = 0;
                    layers[7].des.X = this.ClientSize.Width - 5;
                    layers[8].des.X = 0 - this.ClientSize.Width + 5;
                }
                //mountian mov
                layers[1].des.X -= 20;
                layers[2].des.X -= 20;
                layers[3].des.X -= 20;
                //grass move
                layers[6].des.X -= 20;
                layers[7].des.X -= 20;
                layers[8].des.X -= 20;

                playerobj.frame_index++;
                if (playerobj.frame_index >= jump_frames.Count)
                {
                    playerobj.frame_index = 0;
                    jump = false;
                }
                else
                {
                    playerobj.x += 50;
                }
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
        }


        public void updateDumbGuy(muli_img dumbguy, muli_img player, bool isSpecial = false)
        {
            int threshold = 150;
            if (dumbguy.x > player.x + threshold)
            {
                dumb_guy_go_left = true;
                dumb_guy_go_right = false;
                dumb_guy_onthemove = true;
            }
            else if (dumbguy.x < player.x - threshold)
            {
                dumb_guy_go_left = false;
                dumb_guy_go_right = true;
                dumb_guy_onthemove = true;
            }
            else
            {
                dumb_guy_onthemove = false;
            }

            int speed = isSpecial ? 8 : 5;
            if (dumb_guy_onthemove)
            {
                if (dumb_guy_go_left)
                    dumbguy.x -= speed;
                if (dumb_guy_go_right)
                    dumbguy.x += speed;

                dumbguy.frames = dumbguy_walk;
                dumbguy.frame_index++;
                if (dumbguy.frame_index >= dumbguy_walk.Count)
                    dumbguy.frame_index = 0;
            }
            else
            {
                dumbguy.frames = dumbguy_idle;
                dumbguy.frame_index++;
                if (dumbguy.frame_index >= dumbguy_idle.Count)
                    dumbguy.frame_index = 0;
            }
            float closeness = Math.Abs(player.x + 85 - dumbguy.x);
            float currentTime = ct * 0.016f; 
            float cooldown = 0.5f; 
            if (closeness < 300 && currentTime - dumbguy.lastShotTime >= cooldown)
            {
                bullet bullet = new bullet();
                bullet.x = dumbguy.x + (dumbguy.w / 2);
                bullet.y = dumbguy.y + dumbguy.h;
                bullet.width = isSpecial ? 30 : 20;
                bullet.height = isSpecial ? 30 : 20;

                if (isSpecial)
                {
                    int dx = player.x - dumbguy.x;
                    int dy = (player.y - dumbguy.y) / 2;
                    float length = (float)Math.Sqrt(dx * dx + dy * dy);
                    bullet.dx = (int)(dx / length * 20); 
                    bullet.dy = (int)(dy / length * 15);
                }
                else
                {
                    bullet.dx = 0;
                    bullet.dy = 12; 
                }

                dumb_guy_bullets.Add(bullet);
                dumbguy.lastShotTime = currentTime; 
            }

            for (int i = dumb_guy_bullets.Count - 1; i >= 0; i--)
            {
                bullet b = dumb_guy_bullets[i];
                b.x += b.dx;
                b.y += b.dy;
                if (b.y > this.ClientSize.Height || b.x < 0 || b.x > this.ClientSize.Width)
                    dumb_guy_bullets.RemoveAt(i);
            }
        }



        public void dumbguy_related()
        {
            for (int i = 0; i < dumbguys.Count; i++)
            {
                bool isSpecial = (i == 5);
                updateDumbGuy(dumbguys[i], playerobj, isSpecial);
            }

            for (int i = dumbguys.Count - 1; i >= 0; i--)
            {
                Rectangle dumbRect = new Rectangle(dumbguys[i].x, dumbguys[i].y, dumbguys[i].w, dumbguys[i].h);
                for (int j = single_bullets.Count - 1; j >= 0; j--)
                {
                    Rectangle bulletRect = new Rectangle(single_bullets[j].x, single_bullets[j].y, single_bullets[j].width, single_bullets[j].height);
                    if (dumbRect.IntersectsWith(bulletRect))
                    {
                        single_bullets.RemoveAt(j);
                        if (dumbguys[i].heath <= 0)
                        {

                            dumbguys.RemoveAt(i);

                            break;
                        }
                        dumbguys[i].heath--;

                    }
                }




            }


            Rectangle playerrec = new Rectangle(playerobj.x, playerobj.y, playerobj.w, playerobj.h);

            for (int j = dumb_guy_bullets.Count - 1; j >= 0; j--)
            {
                Rectangle bulletRect = new Rectangle(dumb_guy_bullets[j].x, dumb_guy_bullets[j].y, dumb_guy_bullets[j].width, dumb_guy_bullets[j].height);
                if (playerrec.IntersectsWith(bulletRect))
                {
                    dumb_guy_bullets.RemoveAt(j);
                    if (playerobj.heath <= 0)
                    {
                        dead = true;

                        break;
                    }

                    playerobj.heath--;

                }
            }


        }
        public void update(object sender, EventArgs e)
        {
            actor_related();
            dumbguy_related();

            player_in_da_hole();


            if (onplatform)
            {
                laser_range = false;
            }
            if (dead && playerdeadframes.Count > 0)
            {
                if (ct % 6 == 0) 
                {
                    playerobj.frame_index++;
                    if (playerobj.frame_index >= playerdeadframes.Count)
                    {
                        playerobj.frame_index = playerdeadframes.Count - 1; 
                    }
                }
            }


                for(int i=0; i < laser_beams.Count; i++)
                {
                    bullet pnn =laser_beams[i];
                    if (i == 0 &&laser_range)
                    {
                        playerobj.heath -= 5;

                    }

                }
            

            if (ct % 5 == 0)
            {
                
                    bullet pnn = new bullet();
                    pnn.x = platforms[0].x - 85;
                    pnn.y = 0;
                    pnn.width = 10;
                    pnn.height = this.ClientSize.Height;
                    laser_beams.Add(pnn);
                    
                
                    bullet pnn2 = new bullet();
                    pnn2.x = platforms[1].x - 85;
                    pnn2.y = 0;
                    pnn2.width = 10;
                    pnn2.height = this.ClientSize.Height;
                    laser_beams.Add(pnn2);

               
                    bullet pnn3 = new bullet();
                    pnn3.x = platforms[2].x - 85;
                    pnn3.y = 0;
                    pnn3.width = 10;
                    pnn3.height = this.ClientSize.Height;
                    laser_beams.Add(pnn3);

                

            }

            if (ct % 2 == 0)
            {
                if (laser_beams.Count>=3)
                {
                    laser_beams.Remove(laser_beams[0]);
                    laser_beams.Remove(laser_beams[1]);
                  

                }

            }

           

            drawdubb(this.CreateGraphics());
            ct++;
        }
        public void player_in_da_hole()
        {
            int playerLeft = playerobj.x;
            int playerRight = playerobj.x + playerobj.frames[playerobj.frame_index].Width;
           if (playerRight < platforms[0].x - 90 && playerobj.x > groundlist[groundlist.Count - 1].x && !jump)
            {
                onplatform = false;
            }
            if (playerRight < platforms[0].x - 90 && playerobj.x > groundlist[groundlist.Count - 1].x )
            {
                laser_range = true;

            }


            if (playerRight < platforms[1].x - 90 && playerobj.x > platforms[0].x && !jump)
            {
                onplatform = false;
            }

            if (playerRight < platforms[1].x - 90 && playerobj.x > platforms[0].x )
            {
                laser_range = true;


            }



            if (playerRight < platforms[2].x - 90 && playerobj.x > platforms[1].x && !jump)
            {
                onplatform = false;
            }

            if (playerRight < platforms[2].x - 90 && playerobj.x > platforms[1].x )
            {
                laser_range = true;
            }


            if (!onplatform)
            {
                playerobj.y += 12;
            }

        }
        public void loading(object sender, EventArgs e)
        {
            off = new Bitmap(this.Width, this.Height);
            baclground_code();
            player();
            run_shot();
            shot_up();
            jumping_frames();


            // music 
            
            //waveOutDevice = new WaveOutEvent();
            //string musicPath = System.IO.Path.Combine(Application.StartupPath, "Song.mp3"); 
          
            //audioFileReader = new AudioFileReader(musicPath);
         
            //waveOutDevice.Init(audioFileReader);
           // waveOutDevice.PlaybackStopped += (s, ev) =>
            //{
            //    audioFileReader.Position = 0; 
            //    waveOutDevice.Play();
            //};
            //waveOutDevice.Play();


            // dumbguy


     

            loaddumbguy();
        }
        public void loaddumbguy()
        {
            dumbguyidle();
            dumbguywalk();

            dumbguys.Clear();
            for (int i = 0; i < 6; i++)
            {
                muli_img dumbguy = new muli_img
                {
                    heath = 5,
                    x = 200 + i * 300,
                    y = 70,
                    w = 100,
                    h = 70,
                    frame_index = 0,
                    frames = dumbguy_idle
                };
                if (i == 5) 
                {
                    dumbguy.scale = 1.2f; 
                    dumbguy.w = (int)(100 * dumbguy.scale);
                    dumbguy.h = (int)(70 * dumbguy.scale);
                }
                dumbguys.Add(dumbguy);
            }
        }

        public void dumbguyidle()
        {
            string[] path = new string[]
          {
                "crab-idle/crab-idle-1.png",
                "crab-idle/crab-idle-2.png",
                "crab-idle/crab-idle-3.png",
                "crab-idle/crab-idle-4.png",

          };
            dumbguy_idle.Clear();
            for (int j = 0; j < path.Length; j++)
            {
                Bitmap img = new Bitmap(path[j]);
                img.MakeTransparent(img.GetPixel(0, 0));
                dumbguy_idle.Add(img);
            }
        }

        public void dumbguywalk()
        {
            string[] path = new string[]
          {
                "crab-walk/crab-walk-1.png",
                "crab-walk/crab-walk-2.png",
                "crab-walk/crab-walk-2.png",
                "crab-walk/crab-walk-3.png",
                "crab-walk/crab-walk-4.png",

          };
            dumbguy_walk.Clear();
            for (int j = 0; j < path.Length; j++)
            {
                Bitmap img = new Bitmap(path[j]);
                img.MakeTransparent(img.GetPixel(0, 0));
                dumbguy_walk.Add(img);
            }

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
            //0
            layers.Add(new background { img = scene, des = fullScreen, scr = new Rectangle(0, 0, scene.Width, scene.Height) });
            //1
            layers.Add(new background { img = mountains, des = fullScreen, scr = new Rectangle(0, 0, mountains.Width, mountains.Height) });
            //2 mount right
            layers.Add(new background { img = mountains, des = new Rectangle(this.ClientSize.Width - 5, 0, this.ClientSize.Width, this.ClientSize.Height), scr = new Rectangle(0, 0, mountains.Width, mountains.Height) });
            //3 mount left
            layers.Add(new background { img = mountains, des = new Rectangle(0 - this.ClientSize.Width + 5, 0, this.ClientSize.Width, this.ClientSize.Height), scr = new Rectangle(0, 0, mountains.Width, mountains.Height) });
            //4
            layers.Add(new background { img = trees, des = fullScreen, scr = new Rectangle(0, 0, trees.Width, trees.Height) });
            //5
            layers.Add(new background { img = mountain, des = fullScreen, scr = new Rectangle(0, 0, mountain.Width, mountain.Height) });
            //6
            layers.Add(new background { img = grass, des = fullScreen, scr = new Rectangle(0, 0, grass.Width, grass.Height) });
            //7 grass right
            layers.Add(new background { img = grass, des = new Rectangle(this.ClientSize.Width - 5, 0, this.ClientSize.Width, this.ClientSize.Height), scr = new Rectangle(0, 0, grass.Width, grass.Height) });
            //8 grass left
            layers.Add(new background { img = grass, des = new Rectangle(0 - this.ClientSize.Width + 5, 0, this.ClientSize.Width, this.ClientSize.Height), scr = new Rectangle(0, 0, grass.Width, grass.Height) });



            ground = new Bitmap("ground.png");
            int groundWidth = 130;
            int groundY = this.ClientSize.Height - 52;

            for (int i = 0; i <= 7; i++)
            {
                    one_img ground_obj = new one_img();
                    ground_obj.img = ground;
                    ground_obj.x = i * groundWidth;
                    ground_obj.y = groundY;
                    groundlist.Add(ground_obj);

                
            }
            // platform
            one_img ground_obj_platform = new one_img();
            ground_obj_platform.img = ground;
            ground_obj_platform.x = this.ClientSize.Width - 700;
            ground_obj_platform.y = groundY;
            platforms.Add(ground_obj_platform);


            one_img ground_obj_platform2 = new one_img();
            ground_obj_platform2.img = ground;
            ground_obj_platform2.x = this.ClientSize.Width - 400;
            ground_obj_platform2.y = groundY;
            platforms.Add(ground_obj_platform2);


            one_img ground_obj_platform3 = new one_img();
            ground_obj_platform3.img = ground;
            ground_obj_platform3.x = this.ClientSize.Width - 100;
            ground_obj_platform3.y = groundY;
            platforms.Add(ground_obj_platform3);


        }

        public void player()
        {
            player_idel_frames();
            player_running_frames();
            playerdead();
            playerobj.heath = 10;
            playerobj.x = 100;
            playerobj.y = this.Height - 350;
            playerobj.w = (int)(idle_frames[0].Width * 4 * 0.5f);
            playerobj.h = (int)(idle_frames[0].Height * 4 * 0.5f);
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
        
        public void playerdead()
        {
            string[] path = new string[]
            {
                "player-hurt/player-hurt-1.png",
                "player-hurt/player-hurt-2.png",
                
            };

            for (int j = 0; j < path.Length; j++)
            {
                Bitmap img = new Bitmap(path[j]);
                img.MakeTransparent(img.GetPixel(0, 0));
                playerdeadframes.Add(img);
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
        public void jumping_frames()
        {
            string[] path = new string[]
            {
                "player-jump/player-jump-1.png",
                "player-jump/player-jump-2.png",
                "player-jump/player-jump-3.png",
                "player-jump/player-jump-4.png",
                "player-jump/player-jump-5.png",
                "player-jump/player-jump-6.png",

            };

            for (int j = 0; j < path.Length; j++)
            {
                Bitmap img = new Bitmap(path[j]);
                img.MakeTransparent(img.GetPixel(0, 0));
                jump_frames.Add(img);
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

            if (e.KeyCode == Keys.Space)
            {
                if (!jump)
                {
                    jump = true;
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

            

            // draw dumb guy
            foreach (var dumbguy in dumbguys)
            {
                g.DrawImage(dumbguy.frames[dumbguy.frame_index],
                    new Rectangle(dumbguy.x, dumbguy.y, dumbguy.w, dumbguy.h));
            }

            foreach (one_img i in groundlist)
            {
                g.DrawImage(i.img, new Rectangle(i.x, i.y, 200, 20));
            }
            foreach (one_img i in platforms)
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

            foreach (bullet i in laser_beams)
            {
                using (Pen p = new Pen(color: Color.LightBlue))
                {
                    g.DrawRectangle(p, i.x, i.y, i.width, i.height);
                    g.FillRectangle(Brushes.LightBlue, i.x, i.y, i.width, i.height);
                }
            }

            foreach (bullet i in dumb_guy_bullets)
            {
                using (Pen p = new Pen(color: Color.Gold))
                {
                    g.DrawEllipse(p, i.x, i.y, i.width, i.height);
                    g.FillEllipse(Brushes.Gold, i.x, i.y, i.width, i.height);
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

            if (!dead)
            {
                g.FillRectangle(Brushes.Green, 10, 10, playerobj.heath * 4, 20); 
                g.DrawString($"health: {playerobj.heath}", new Font("Fantasy", 12), Brushes.White, 10, 30);
            }
        }




        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}