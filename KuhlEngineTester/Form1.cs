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
        string uuid;

        Texture mBackTexture;
        private int mFPS;

        //Image gifImage = new Bitmap(@"C:\Users\Roll\Desktop\1.gif");
        int currentFrame = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Parent = pictureBox1;
            Renderer.newFrame += new Renderer.RenderHandler(rendererEvent);
            mBackTexture = new Texture(@"E:\Eigene Daten\Pictures\rw3\current-rw3.jpg");
            mBackTexture.Stretch = true;

            renderer.Width = 300;
            renderer.Height = 300;
            renderer.FPS = 10;
            renderer.Background = mBackTexture;

            renderer.Start();

            uuid = renderer.CreateItem();

            Item item = renderer.GetItem(uuid);

            item.Texture = new Texture(@"E:\Eigene Daten\Pictures\PNGs 150px\0.png");
            item.Texture.Stretch = true;
            item.X = 34;
            item.Y = 68;
            item.Width = 15;
            item.Height = 15;
            item.Visible = true;

            renderer.SetItem(uuid, item);              

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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 15; i < 150; i++)
            {
                renderer.SetItemSize(uuid, i, i);
                Application.DoEvents();
                Thread.Sleep(100);
            }
        }
    }
}
