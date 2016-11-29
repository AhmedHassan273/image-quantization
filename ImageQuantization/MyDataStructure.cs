using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class Node
    {
        public static RGBPixel data;
        public static Node next;
        Node()
        {

        }
        Node(RGBPixel T)
        {
            data.blue = T.blue;
            data.green = T.green;
            data.red = T.red;
        }
    }
    
    class adjList
    {
        private static Node head;
        private static int LengthOf;

        public static int Length
        {
            get
            {
                return LengthOf;
            }

            set
            {
                LengthOf = value;
            }
        }

        public static void insert(RGBPixel T)
        {

        }

        public static s(RGBPixel T)
        {

        }
    }
}