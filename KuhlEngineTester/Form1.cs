using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KuhlEngine;
using System.Drawing.Drawing2D;

using System.IO;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

namespace KuhlEngineTester
{
    public partial class Form1 : Form
    {
        public delegate void emptyFunction();

        Renderer renderer = new Renderer();
        Dictionary<string, string> uuid = new Dictionary<string, string>();

        Texture mBackTexture;
        private int mFPS;
        private int jump = 0;
        private bool toRight = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Parent = pictureBox1;
            Event.NewFrame += new Event.RenderHandler(rendererEvent);
            Event.Collision += new Event.CollisionHandler(collisionEvent);
            mBackTexture = new Texture(KuhlEngineTester.Properties.Resources.ground);
            mBackTexture.Stretch = false;

            renderer.Background = mBackTexture;
            renderer.Width = 640;
            renderer.Height = 320;
            renderer.FPS = 60;
            
            //renderer.SetCamera(32, 32, 160, 80);

            //High quallity
            //renderer.SmoothingMode = SmoothingMode.AntiAlias;
            //renderer.InterpolationMode = InterpolationMode.Bilinear;
            //renderer.CompositingQuality = CompositingQuality.HighQuality;
            //renderer.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //renderer.ForceGarbageCollector = true;
            //
            renderer.Start();

            Texture playerTexture = new Texture(KuhlEngineTester.Properties.Resources.player);
            Texture wallTexture = new Texture(KuhlEngineTester.Properties.Resources.wall);
            wallTexture.Stretch = false;

            uuid.Add("WallTop", renderer.CreateItem().Uuid);
            uuid.Add("WallBottom", renderer.CreateItem().Uuid);
            uuid.Add("WallRight", renderer.CreateItem().Uuid);
            uuid.Add("WallLeft", renderer.CreateItem().Uuid);
            uuid.Add("Player", renderer.CreateItem().Uuid);
            uuid.Add("ObjectAbove", renderer.CreateItem().Uuid);
            uuid.Add("ObjectBelow", renderer.CreateItem().Uuid);
            uuid.Add("Platform", renderer.CreateItem().Uuid);
            uuid.Add("Text", renderer.CreateItem().Uuid);

            // object benchmarker
            //for(int i = 0; i < 100; i++)
            //{
            //    uuid.Add("TestItem" + i.ToString(), renderer.CreateItem());
            //    Item itm = renderer.GetItem(uuid["TestItem" + i.ToString()]);
            //    itm.Texture = new Texture(243, 123, 84);
            //    itm.Width = 100;
            //    itm.Height = 100;
            //    itm.X = i;
            //    itm.Y = i;
            //    itm.Enabled = true;
            //    itm.CheckCollision = true;
            //    renderer.SetItem(uuid["TestItem" + i.ToString()], itm);
            //}

            Item item = renderer.GetItem(uuid["WallTop"]);
            item.Texture = new Texture(125, 0, 125, 126);
            item.Texture.Stretch = false;
            item.Width = 640;
            item.Height = 32;
            item.Enabled = true;
            item.CheckCollision = true;
            renderer.SetItem(item);

            item = renderer.GetItem(uuid["WallBottom"]);
            item.Texture = new Texture(125, 0, 125);
            item.Texture.Stretch = false;
            item.Y = 288;
            item.Width = 640;
            item.Height = 32;
            item.Enabled = true;
            item.CheckCollision = true;
            renderer.SetItem(item);

            item = renderer.GetItem(uuid["Platform"]);
            item.Texture = new Texture(123, 84, 3);
            item.Texture.Stretch = false;
            item.Y = 250;
            item.X = 320;
            item.Width = 288;
            item.Height = 32;
            item.Enabled = true;
            item.CheckCollision = true;
            renderer.SetItem(item);

            item = renderer.GetItem(uuid["WallRight"]);
            item.Texture = wallTexture;
            item.Y = 32;
            item.X = 608;
            item.Width = 32;
            item.Height = 256;
            item.Enabled = true;
            item.CheckCollision = true;
            renderer.SetItem(item);

            item = renderer.GetItem(uuid["WallLeft"]);
            item.Texture = wallTexture;
            item.Y = 32;
            item.Width = 32;
            item.Height = 256;
            item.Enabled = true;
            item.CheckCollision = true;
            renderer.SetItem(item);

            item = renderer.GetItem(uuid["Player"]);
            item.Texture = playerTexture;
            item.Y = 188;
            item.X = 32;
            item.Width = 100;
            item.Height = 100;
            item.Enabled = true;
            item.Layer = 5;
            item.CheckCollision = true;
            renderer.SetItem(item);

            item = renderer.GetItem(uuid["ObjectAbove"]);
            item.Texture = new Texture(125, 0, 0, 200);
            item.Texture.Stretch = false;
            item.Y = 54;
            item.X = 200;
            item.Width = 54;
            item.Height = 54;
            item.Enabled = true;
            item.Layer = 6;
            item.CheckCollision = true;
            renderer.SetItem(item);

            item = renderer.GetItem(uuid["ObjectBelow"]);
            item.Texture = new Texture(0, 125, 0);
            item.Texture.Stretch = false;
            item.Y = 54;
            item.X = 300;
            item.Width = 54;
            item.Height = 54;
            item.Enabled = true;
            item.Layer = 4;
            item.CheckCollision = true;
            renderer.SetItem(item);


            item = renderer.GetItem(uuid["Text"]);
            FontFamily fontFamily = new FontFamily("Arial");
            Font font = new Font(fontFamily, 20, FontStyle.Bold, GraphicsUnit.Pixel);
            item.Texture = new Texture("Dies ist ein Text", font, Color.Black);
            item.Texture.Stretch = false;
            item.Y = 33;
            item.X = 33;
            item.Width = 160;
            item.Height = 25;
            item.Enabled = true;
            item.CheckCollision = false;
            renderer.SetItem(item);
        }

        private void rendererEvent(Image aFrame)
        {
            try
            {
                if (this.InvokeRequired) this.Invoke(new emptyFunction(delegate()
                    {
                        pictureBox1.Width = aFrame.Width;
                        pictureBox1.Height = aFrame.Height;
                        pictureBox1.Image = aFrame;
                        mFPS++;
                    }));
            }
            catch { Environment.Exit(0); }

        }

        private void collisionEvent(CollisionEventArgs e)
        {
            e.Cancel = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            renderGif();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label1.Text = Convert.ToString(mFPS);
            mFPS = 0;
        }



        private void renderGif()
        {/*
            FrameDimension dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
            int frameCount = gifImage.GetFrameCount(dimension);

            currentFrame += 1;

            if (currentFrame >= frameCount)
            {
                currentFrame = Math.Abs(currentFrame - frameCount);
            }
            gifImage.SelectActiveFrame(dimension, currentFrame);
            Bitmap curGif = new Bitmap(gifImage.Width, gifImage.Height);

            Graphics g = Graphics.FromImage(curGif);
            g.DrawImage((Bitmap)gifImage.Clone(), new Point(0, 0));
            g.Dispose();

            pictureBox1.Width = gifImage.Width;
            pictureBox1.Height = gifImage.Height;
            pictureBox1.Image = curGif;
            */
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void tmrleft_Tick(object sender, EventArgs e)
        {
            Item item = renderer.GetItem(uuid["Player"]);
            //item.FlipX = false;
            item.X = item.X + 2;
            if (jump > 0) item.X = item.X + 6;
            if (item.X > renderer.Width) item.X = -item.Width;
            if (!renderer.SetItemPosition(uuid["Player"], item.X, item.Y)) tmrleft.Enabled = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 39)
            {
                if (!toRight)
                {
                    Item item = renderer.GetItem(uuid["Player"]);
                    item.Texture.FlipX();
                    renderer.SetItem(item);
                    toRight = true;
                }
                tmrleft.Enabled = true;
            }
            if (e.KeyValue == 37)
            {
                if (toRight)
                {
                    Item item = renderer.GetItem(uuid["Player"]);
                    item.Texture.FlipX();
                    renderer.SetItem(item);
                    toRight = false;
                }
                tmrright.Enabled = true;
            }
            if (e.KeyValue == 38)
            {
                tmrup.Enabled = true;
            }
            if (e.KeyValue == 40)
            {
                tmrdown.Enabled = true;
            }
            if (e.KeyCode == Keys.Space)
            {
                if (jump == 0) tmrjump.Enabled = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 39)
            {
                tmrleft.Enabled = false;
            }
            if (e.KeyValue == 37)
            {
                tmrright.Enabled = false;
            }
            if (e.KeyValue == 38)
            {
                tmrup.Enabled = false;
            }
            if (e.KeyValue == 40)
            {
                tmrdown.Enabled = false;
            }
        }

        private void tmrright_Tick(object sender, EventArgs e)
        {
            Item item = renderer.GetItem(uuid["Player"]);
            //item.FlipX = false;
            item.X = item.X - 2;
            if (jump > 0) item.X = item.X - 6;
            if (item.X < -item.Width) item.X = renderer.Width;
            renderer.SetItem(item);
        }

        private void tmrup_Tick(object sender, EventArgs e)
        {
            //Item item = renderer.GetItem(uuid["Player"]);
            ////item.FlipX = false;
            //item.Y = item.Y - 2;
            //if (item.Y < -item.Height) item.Y = renderer.Height;
            //renderer.SetItem(uuid["Player"], item);
        }

        private void tmrdown_Tick(object sender, EventArgs e)
        {
            //Item item = renderer.GetItem(uuid["Player"]);
            ////item.FlipX = false;
            //item.Y = item.Y + 2;
            //if (item.Y > renderer.Height) item.Y = -item.Height;
            //renderer.SetItem(uuid["Player"], item);
        }

        private void tmrjump_Tick(object sender, EventArgs e)
        {
            if (jump < 3)
            {
                Item item = renderer.GetItem(uuid["Player"]);
                item.Y = item.Y - 14;
                renderer.SetItem(item);
                jump++;
            }
            else if (jump < 6)
            {
                Item item = renderer.GetItem(uuid["Player"]);
                item.Y = item.Y - 10;
                renderer.SetItem(item);
                jump++;
            }
            else if (jump < 8)
            {
                Item item = renderer.GetItem(uuid["Player"]);
                item.Y = item.Y - 6;
                renderer.SetItem(item);
                jump++;

            }
            else if (jump < 10)
            {
                jump++;

            }
            else if (jump < 12)
            {
                Item item = renderer.GetItem(uuid["Player"]);
                item.Y = item.Y + 6;
                renderer.SetItem(item);
                jump++;

            }
            else if (jump < 15)
            {
                Item item = renderer.GetItem(uuid["Player"]);
                item.Y = item.Y + 10;
                renderer.SetItem(item);
                jump++;

            }
            else if (jump < 18)
            {
                Item item = renderer.GetItem(uuid["Player"]);
                item.Y = item.Y + 14;
                renderer.SetItem(item);
                jump++;
            }
            else
            {
                jump = 0;
                tmrjump.Enabled = false;
            }
        }

    }
}
