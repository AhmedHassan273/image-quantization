using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class HeapNode
    {
        public int key, degree;
        public double cost;
        public HeapNode left, right, parent, child;
        public bool mark, can;
    }
}
