using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{ 
    class FibbHeap
    {
        public int sz;
        private static readonly double OneOverLogPhi = 1.0 / Math.Log((1.0 + Math.Sqrt(5.0)) / 2.0);
        private HeapNode head;
        public HeapNode[] pos;
        public FibbHeap(int cap)
        {
            pos = new HeapNode[cap];
            head = InitializeHeap();
        }
        public HeapNode InitializeHeap()
        {
            HeapNode n;
            n = null;
            return n;
        }
        public void Insert(int v,double c)
        {
            HeapNode neww = new HeapNode();
            neww.cost = c;
            neww.key = v;
            neww.degree = 0;
            neww.parent = null;
            neww.child = null;
            neww.left = neww;
            neww.right = neww;
            neww.can = false;
            neww.mark = false;
            pos[v] = neww;
            if(head!=null)
            {
                neww.left = head;
                neww.right = head.right;
                head.right = neww;
                neww.right.left = neww;
                pos[v] = neww;
                if (neww.cost < head.cost)
                {
                   head = neww;
                }
            }
            else
            {
                head = neww;
            }
            sz++;
        }
        public void fibb_link(HeapNode y,HeapNode z)
        {
            y.left.right = y.right;
            y.right.left = y.left;

            y.parent = z;
           
            if (z.child == null)
            {
                z.child = y;
                y.right = y;
                y.left = y;

            }
            else
            {
                y.left = z.child;
                y.right = z.child.right;
                z.child.right = y;
                y.right.left = y;

            }
            z.degree++;
            y.mark = false;
        }
        public void Consolidate()
        {
            int i;
            int D = ((int)Math.Floor(Math.Log(sz) * OneOverLogPhi)) + 1;
            HeapNode[] A=new HeapNode[D];
           for (i = 0; i < D; i++)
                 A[i] = null;
           int roots = 0;
           HeapNode x = head;
            if(x!=null)
            {
                roots++;
                x = x.right;
                while(x!=head)
                {
                    roots++;
                    x = x.right;
                }
            }
            while(roots>0)
            {
                int d = x.degree;
                HeapNode next = x.right;
                while(true)
                {
                    HeapNode y = A[d];
                    if(y==null)
                    {
                        break;
                    }
                    if(x.cost>y.cost)
                    {
                        HeapNode tmp = x;
                        x = y;
                        y = tmp;
                    }
                    fibb_link(y, x);
                    A[d] = null;
                    d++;
                }
                A[d] = x;
                x = next;
                roots--;
            }
            head = null;
            for(i=0;i<D;i++)
            {
                HeapNode y = A[i];
                if (y == null) continue;
                if(head!=null)
                {
                    y.left.right = y.right;
                    y.right.left = y.left;
                    y.left = head;
                    y.right = head.right;
                    head.right = y;
                    y.right.left = y;
                    if(head.cost>y.cost)
                    {
                        head = y;
                    }
                }
                else
                {
                    head = y;
                }
            }
           
    }
        public void decrease_key(int v, double c)
        {
            HeapNode y;
            HeapNode ptr = pos[v];
            ptr.cost = c;
            y = ptr.parent;
            if (y != null && ptr.cost < y.cost)
            {
                Cut(ptr,y);
                Cascase_cut(y);
            }
            if (ptr.cost < head.cost)
                head = ptr;
        }
        public void Cut(HeapNode x,HeapNode y)
        {
            (x.left).right = x.right;
            (x.right).left = x.left;
            y.degree --;
            if (x == y.child)
                y.child = x.right;
            if(y.degree==0)
            {
                y.child = null;
            }
            x.left = head;
            x.right = head.right;
            head.right = x;
            x.right.left = x;
            x.parent = null;
            x.mark = false;
        }
        public void Cascase_cut(HeapNode y)
        {
            HeapNode z = y.parent;
            if (z != null)
            {
                if (y.mark == false)
                {
                    y.mark = true;
                }
                else
                {
                    Cut(y,z);
                    Cascase_cut(z);
                }
            }
        }
        public HeapNode Extract_min()
        {
            HeapNode ptr=head;
            if (ptr != null)
            {
                int kids = ptr.degree;
                HeapNode oldchild = ptr.child;
                while(kids>0)
                {
                    HeapNode tmp = oldchild.right;
                    oldchild.left.right = oldchild.right;
                    oldchild.right.left = oldchild.left;

                    oldchild.left = head;
                    oldchild.right = head.right;
                    head.right = oldchild;
                    oldchild.right.left = oldchild;

                    oldchild.parent = null;
                    oldchild = tmp;
                    kids--;
                }
            }
            ptr.left.right = ptr.right;
            ptr.right.left = ptr.left;

            if (ptr == ptr.right)
                head = null;
            else
            {
                head = ptr.right;
                Consolidate();
            }
            sz--;
            return ptr;
        }
    }
}
