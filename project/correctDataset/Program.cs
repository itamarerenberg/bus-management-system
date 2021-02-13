using PriorityQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace correctDataset
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> l= new List<int>() { 1, 6, 4, 6, 2, 7, 3 };
            PriorityQueue<int> queue = new PriorityQueue<int>((a, b) => { return a > b; }, l);
            while(queue.Count != 0)
            {
                Console.WriteLine(queue.Dequeue() + ", ");
            }
            Console.ReadKey();
        }
    }
}
