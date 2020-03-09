using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkList<int> intlist = new LinkList<int>();
            for (int i = 1; i <= 10; i++)
                intlist.Add(i);
            //print
            intlist.ForEach(p => Console.WriteLine($"Node data:{p.Data}"));
            //get max
            int max = Int32.MinValue;
            intlist.ForEach(p => max = p.Data > max ? p.Data : max);
            Console.WriteLine($"Max:{max}");
            int min = Int32.MaxValue;
            intlist.ForEach(p => min = p.Data < min ? p.Data : min);
            Console.WriteLine($"Min:{min}");
            int sum = 0;
            intlist.ForEach(p => sum += p.Data);
            Console.WriteLine($"Sum:{sum}");

        }
    }
    class LinkNode<T>
    {
        public LinkNode<T> Next { get; set; }
        public T Data { get; set; }
        public LinkNode(T t)
        {
            Next = null;
            Data = t;
        }   
    }
    class LinkList<T>
    {
        private LinkNode<T> head, tail;
        public LinkList()
        {
            head = tail = null;
        }
        public void Add(T t)
        {
            LinkNode<T> node = new LinkNode<T>(t);
            if (tail == null)
                head = tail = node;
            else
            {
                tail.Next = node;
                tail = node;
            }
        }
        public void ForEach(Action<LinkNode<T>> action)
        {
            LinkNode<T> p=head;
            while(p!=null)
            {
                action(p);
                p = p.Next;
            }
        }
    }
}
