using System;
using System.Dynamic;

namespace dotNet5781_00_1038_0685
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcom1038();
            Console.ReadLine();
        }

        static partial void Welcom0685();
        private static void Welcom1038()
        {
            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first consol application", name);
        }
    }
}
