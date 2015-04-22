using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KuhlEngine;

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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Parent = pictureBox1;
            Renderer.newFrame += new Renderer.RenderHandler(rendererEvent);
            mBackTexture = new Texture(KuhlEngineTester.Properties.Resources.ground);
            mBackTexture.Stretch = false;

            renderer.Width = 640;
            renderer.Height = 320;
            renderer.FPS = 100;
            renderer.Background = mBackTexture;

            renderer.Start();

            Texture playerTexture = new Texture(KuhlEngineTester.Properties.Resources.player);
            Texture wallTexture = new Texture(KuhlEngineTester.Properties.Resources.wall);
            wallTexture.Stretch = false;

            uuid.Add("WallTop", renderer.CreateItem());
            uuid.Add("WallBottom", renderer.CreateItem());
            uuid.Add("WallRight", renderer.CreateItem());
            uuid.Add("WallLeft", renderer.CreateItem());
            uuid.Add("Player", renderer.CreateItem());
            uuid.Add("ObjectAbove", renderer.CreateItem());
            uuid.Add("ObjectBelow", renderer.CreateItem());

            Item item = renderer.GetItem(uuid["WallTop"]);
            item.Texture = new Texture(125, 0, 125, 126);
            item.Texture.Stretch = false;
            item.Width = 640;
            item.Height = 32;
            item.Visible = true;
            renderer.SetItem(uuid["WallTop"], item);

            item = renderer.GetItem(uuid["WallBottom"]);
            item.Texture = new Texture(125,0,125);
            item.Texture.Stretch = false;
            item.Y = 288;
            item.Width = 640;
            item.Height = 32;
            item.Visible = true;
            renderer.SetItem(uuid["WallBottom"], item);

            item = renderer.GetItem(uuid["WallRight"]);
            item.Texture = wallTexture;
            item.Y = 32;
            item.X = 608;
            item.Width = 32;
            item.Height = 256;
            item.Visible = true;
            renderer.SetItem(uuid["WallRight"], item);

            item = renderer.GetItem(uuid["WallLeft"]);
            item.Texture = wallTexture;
            item.Y = 32;
            item.Width = 32;
            item.Height = 256;
            item.Visible = true;
            renderer.SetItem(uuid["WallLeft"], item);

            item = renderer.GetItem(uuid["Player"]);
            item.Texture = playerTexture;
            item.Y = 54;
            item.X = 32;
            item.Width = 100;
            item.Height = 100;
            item.Visible = true;
            item.Layer = 5;
            renderer.SetItem(uuid["Player"], item);

            item = renderer.GetItem(uuid["ObjectAbove"]);
            item.Texture = new Texture(125, 0, 0, 200);
            item.Texture.Stretch = false;
            item.Y = 54;
            item.X = 200;
            item.Width = 54;
            item.Height = 54;
            item.Visible = true;
            item.Layer = 6;
            renderer.SetItem(uuid["ObjectAbove"], item);

            item = renderer.GetItem(uuid["ObjectBelow"]);
            item.Texture = new Texture(0, 125, 0);
            item.Texture.Stretch = false;
            item.Y = 54;
            item.X = 300;
            item.Width = 54;
            item.Height = 54;
            item.Visible = true;
            item.Layer = 4;
            renderer.SetItem(uuid["ObjectBelow"], item);
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
            if (jump > 0) item.X = item.X + 3;
            renderer.SetItem(uuid["Player"], item);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 39)
            {
                tmrleft.Enabled = true;
            }
            if (e.KeyValue == 37)
            {
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
            if(e.KeyCode == Keys.Space)
            {
                if(jump == 0) tmrjump.Enabled = true;
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
            if (jump > 0) item.X = item.X - 3;
            renderer.SetItem(uuid["Player"], item);
        }

        private void tmrup_Tick(object sender, EventArgs e)
        {
            Item item = renderer.GetItem(uuid["Player"]);
            //item.FlipX = false;
            item.Y = item.Y - 2;
            renderer.SetItem(uuid["Player"], item);
        }
        
        private void tmrdown_Tick(object sender, EventArgs e)
        {
            Item item = renderer.GetItem(uuid["Player"]);
            //item.FlipX = false;
            item.Y = item.Y + 2;
            renderer.SetItem(uuid["Player"], item);
        }

        private void tmrjump_Tick(object sender, EventArgs e)
        {
            if(jump < 8)
            {
                Item item = renderer.GetItem(uuid["Player"]);
                item.Y = item.Y - 7;
                renderer.SetItem(uuid["Player"], item);
                jump++;
            }
            else if(jump < 16)
            {
                Item item = renderer.GetItem(uuid["Player"]);
                item.Y = item.Y + 7;
                renderer.SetItem(uuid["Player"], item);
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
