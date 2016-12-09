using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] MyImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                MyImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(MyImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(MyImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(MyImageMatrix).ToString();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void imgQuantization_Click(object sender, EventArgs e)
        {
            ImageOperations.DistinctColors(MyImageMatrix);
            ImageOperations.DisplayImage(MyImageMatrix, pictureBox2);
        }
    }
}