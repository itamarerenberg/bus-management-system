using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_1038_0685
{
    public enum MyEnum
    {
        NEW_BUS,
        RIDE,
        MAINTENANCE,
        BUS_STATUS,
        EXIT = -1
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"0: add new bus\n1: take a ride\n2: maintenance\n3: print buses's status\n -1: exit");
            MyEnum choice;
            do
            {
                choice = (MyEnum)Console.Read();
                switch (choice)
                {
                    case MyEnum.NEW_BUS:
                        Console.WriteLine("enter bus's id");
                        string id = Console.ReadLine();
                        Console.WriteLine("enter start date(yyyy:mm:dd)");
                        DateTime time;
                        DateTime.TryParse(Console.ReadLine(), out time);

                        break;
                    case MyEnum.RIDE:
                        break;
                    case MyEnum.MAINTENANCE:
                        break;
                    case MyEnum.BUS_STATUS:
                        break;
                    case MyEnum.EXIT:
                        break;
                    default:
                        break;
                } 
            } while (true);
        }
    }
}
