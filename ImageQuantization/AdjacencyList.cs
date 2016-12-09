using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    public class AdjacencyList
    {
        private List<Tuple<Tuple<RGBPixel, RGBPixel>, double>> CompleteGraph;

        public AdjacencyList()
        {
            CompleteGraph = new List<Tuple<Tuple<RGBPixel, RGBPixel>, double>>();
        }

        /// <summary>
        /// Adds an Undirected Edge of Weight W1 between vertices V1 and V2 respectively
        /// </summary>
        public void Add_Edge(RGBPixel V1, RGBPixel V2, double W1)
        {
            int R1 = V1.red;
            int G1 = V1.green;
            int B1 = V1.blue;

            int R2 = V2.red;
            int G2 = V2.green;
            int B2 = V2.blue;
            Tuple<RGBPixel, RGBPixel> T = new Tuple<RGBPixel, RGBPixel>(V1, V2);
            Tuple<Tuple<RGBPixel, RGBPixel>, double> T1 = new Tuple<Tuple<RGBPixel, RGBPixel>, double>(T, W1);
            CompleteGraph.Add(T1);
            return;
        }

        /// <summary>
        /// Find The K Clusters for a given minimum spanning tree
        /// </summary>
        public void sort()
        {
            // here
        }

        public RGBPixel SourcePixelAt(int index)
        {
            return CompleteGraph[index].Item1.Item1;
        }

        public RGBPixel BasePixelAt(int index)
        {
            return CompleteGraph[index].Item1.Item2;
        }

        public double weightAt(int index)
        {
            return CompleteGraph[index].Item2;
        }

    }
}