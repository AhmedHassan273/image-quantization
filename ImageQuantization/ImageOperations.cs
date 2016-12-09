using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ImageQuantization
{
    /// <summary>
    /// Holds the pixel color in 3 byte values: red, green and blue
    /// </summary>
    public struct RGBPixel
    {
        public byte red, green, blue;
        public double point;

        public void RGBPixelConstructor(byte R, byte G, byte B)
        {
            red = R;
            green = G;
            blue = B;
        }
    }
    /// <summary>
    /// Library of static functions that deal with images
    /// </summary>
    public class ImageOperations
    {
        /// <summary>
        /// Open an image and load it into 2D array of colors (size: Height x Width)
        /// </summary>
        /// <param name="ImagePath">Image file path</param>
        /// <returns>2D array of colors</returns>
        public static RGBPixel[,] OpenImage(string ImagePath)
        {
            Bitmap original_bm = new Bitmap(ImagePath);
            int Height = original_bm.Height;
            int Width = original_bm.Width;

            RGBPixel[,] Buffer = new RGBPixel[Height, Width];

            unsafe
            {
                BitmapData bmd = original_bm.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, original_bm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool Format32 = false;
                bool Format24 = false;
                bool Format8 = false;

                if (original_bm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Format24 = true;
                    nWidth = Width * 3;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format32bppArgb || original_bm.PixelFormat == PixelFormat.Format32bppRgb || original_bm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    Format32 = true;
                    nWidth = Width * 4;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Format8 = true;
                    nWidth = Width;
                }
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (y = 0; y < Height; y++)
                {
                    for (x = 0; x < Width; x++)
                    {
                        if (Format8)
                        {
                            Buffer[y, x].red = Buffer[y, x].green = Buffer[y, x].blue = p[0];
                            p++;
                        }
                        else
                        {
                            Buffer[y, x].red = p[0];
                            Buffer[y, x].green = p[1];
                            Buffer[y, x].blue = p[2];
                            if (Format24) p += 3;
                            else if (Format32)
                            {
                                p += 4;
                            }
                        }
                    }
                    p += nOffset;
                }
                original_bm.UnlockBits(bmd);
            }

            return Buffer;
        }

        /// <summary>
        /// Get the height of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Height</returns>
        public static int GetHeight(RGBPixel[,] ImageMatrix)
        {
            try
            {
                return ImageMatrix.GetLength(0);
            }
            catch
            {
                MessageBox.Show("No Image were added.");
                return 0;
            }
        }

        /// <summary>
        /// Get the width of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Width</returns>
        public static int GetWidth(RGBPixel[,] ImageMatrix)
        {
            try
            {
                return ImageMatrix.GetLength(1);
            }
            catch
            {
                MessageBox.Show("No Image were added.");
                return 0;
            }
        }

        /// <summary>
        /// Display the given image on the given PictureBox object
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <param name="PicBox">PictureBox object to display the image on it</param>
        public static void DisplayImage(RGBPixel[,] ImageMatrix, PictureBox PicBox)
        {
            // Create Image:
            //==============
            try
            {
                int Height = ImageMatrix.GetLength(0);
                int Width = ImageMatrix.GetLength(1);

                Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

                unsafe
                {
                    BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                    int nWidth = 0;
                    nWidth = Width * 3;
                    int nOffset = bmd.Stride - nWidth;
                    byte* p = (byte*)bmd.Scan0;
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            p[0] = ImageMatrix[i, j].red;
                            p[1] = ImageMatrix[i, j].green;
                            p[2] = ImageMatrix[i, j].blue;
                            p += 3;
                        }

                        p += nOffset;
                    }
                    ImageBMP.UnlockBits(bmd);
                }
                PicBox.Image = ImageBMP;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Image Charactaristics
        /// </summary>
        private static bool[,,] Distinct;
        private static List<RGBPixel> UniqueColors;
        private static AdjacencyList Graph;
        private static List<List<RGBPixel>> Clusters;

        /// <summary>
        /// Finds All Pixels With Unique Colors and returns a List of Pixels
        /// </summary>
        public static void DistinctColors(RGBPixel[,] ImageMatrix)
        {
            try
            {
                long Height = GetHeight(ImageMatrix);
                long Width = GetWidth(ImageMatrix);
                long length = 0;
                Distinct = new bool[257, 257, 257];
                UniqueColors = new List<RGBPixel>();
                for (long i = 0; i < Height; i++)
                {
                    for (long j = 0; j < Width; j++)
                    {
                        int R = ImageMatrix[i, j].red;
                        int B = ImageMatrix[i, j].blue;
                        int G = ImageMatrix[i, j].green;
                        if (Distinct[R, G, B] == false)
                        {
                            UniqueColors.Add(ImageMatrix[i, j]);
                            length++;
                            Distinct[R, G, B] = true;
                        }
                    }
                }
                CompleteGraph(UniqueColors, length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return;
        }



        /// <summary>
        /// Construct the Minimum Spanning Tree
        /// </summary>
        public static AdjacencyList CompleteGraph(List<RGBPixel> Verticies, long Number_of_verticies)
        {
            try
            {
                Distinct = new bool[256, 256, 256];
                Graph = new AdjacencyList();
                for (int i = 0; i < Number_of_verticies; i++)
                {
                    RGBPixel V1 = Verticies[i];
                    byte R1 = V1.red;
                    byte G1 = V1.green;
                    byte B1 = V1.blue;

                    byte VisitedNodeR = 0;
                    byte VisitedNodeG = 0;
                    byte VisitedNodeB = 0;
                    double Minimum_weight = 1000000000000;
                    for (int j = 0; j < Number_of_verticies; j++)
                    {
                        if(i == j)
                        {
                            continue;
                        }
                        RGBPixel Tmp2 = Verticies[j];

                        byte R2 = Tmp2.red;
                        byte G2 = Tmp2.green;
                        byte B2 = Tmp2.blue;

                        double W1 = (R1 - R2) * (R1 - R2) + (B1 - B2) * (B1 - B2) + (G1 - G2) * (G1 - G2);
                        W1 = Math.Sqrt(W1);
                        if (Distinct[R2, G2, B2] == false)
                        {
                            if (W1 < Minimum_weight)
                            {
                                Minimum_weight = W1;
                                VisitedNodeR = R2;
                                VisitedNodeG = G2;
                                VisitedNodeB = B2;
                            }
                        }
                    }
                    RGBPixel V2 = new RGBPixel();
                    V2.RGBPixelConstructor(VisitedNodeR, VisitedNodeG, VisitedNodeB);
                    Graph.Add_Edge(V1, V2, Minimum_weight);
                    Distinct[R1, G1, B1] = true;
                    MessageBox.Show("[" + V1.red.ToString() + "," + V1.green.ToString() + "," + V1.blue.ToString() + "]" + "--" + Minimum_weight.ToString() + "-->" + "[" + V2.red.ToString() + "," + V2.green.ToString() + "," + V2.blue.ToString() + "]", "Edge Created ^_^*");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return Graph;
        }

        /// <summary>
        /// Find The K Clusters for a given minimum spanning tree
        /// </summary>
        public static List<List<RGBPixel>> KmeasnCluster(AdjacencyList MST,List<RGBPixel> Colors,int length, int k)
        {
            try
            {
                // here :"D 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return new List<List<RGBPixel>>();
        }
    }
}