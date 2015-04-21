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
        Texture mBackTexture;

        Image gifImage = new Bitmap(@"C:\Users\Roll\Desktop\1.gif");
        int currentFrame = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Renderer.newFrame += new Renderer.RenderHandler(rendererEvent);
            mBackTexture = new Texture(@"E:\Eigene Daten\Pictures\PNGs 150px\0.png");
            mBackTexture.Stretch = false;
            renderer.initializeMap(300, 200, mBackTexture);

            //renderer.FPS = 60;
        }

        private void renderGif()
        {
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
            
        }

        private void rendererEvent(Image aFrame, int aWidth, int aHeight)
        {
            //pictureBox1.Width = aWidth;
            //pictureBox1.Height = aHeight;
            //pictureBox1.Image = aFrame;
            if (this.InvokeRequired) this.Invoke(new emptyFunction(delegate() { label1.Text = Convert.ToString(Convert.ToInt32(label1.Text) + 1); }));
                

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            renderGif();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(textBox1.Text);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            textBox1.Text = Convert.ToString(Convert.ToDouble(label1.Text) / 10);
        }
    }
}
