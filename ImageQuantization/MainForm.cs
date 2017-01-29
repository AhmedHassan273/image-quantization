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
        RGBPixel[,] ImageMatrix;
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1,0);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
        }
        private void btnQuan(object sender, EventArgs e)
        {
          long before = System.Environment.TickCount;          // get the current time in miliseconds
          int distinict_col = 0 ;
          int K = (int)ClusterK.Value ;
          double sum = 0 ;
          ImageOperations.Run(ref sum,ref distinict_col,ref K, radioButton1, ImageMatrix); // the whole Project
          ImageOperations.DisplayImage(ImageMatrix, pictureBox2, 1); //O(N^2) where N is the height or the weight of image
          ClusterK.Value = K;                        // print the number of cluster if changed
          textBox1.Text = distinict_col.ToString();  // Print the number of distinct colors
          textBox2.Text = sum.ToString();            // print the sum of the tree
          long after = System.Environment.TickCount; // get the current time
          double total = after - before;             // Calculate the taken time
          total /= 60000;                            // convert miliseconds to minutes
          total = Math.Round(total,3);               // Round the Minutes to three decimal digits
          MessageBox.Show("Time Elapsed : " + total.ToString() + " Minute (s)");// show the taken Time
        }
        private void ClusterK_ValueChanged(object sender, EventArgs e)
        {
           
        }
    }
}