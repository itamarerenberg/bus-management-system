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
            bool exit = false;
            Bus_company bus_Company = new Bus_company();
            Console.WriteLine(@"0: add new bus\n1: take a ride\n2: maintenance\n3: print buses's status\n -1: exit");
            MyEnum choice;
            string id;
            do
            {
                bool success = Enum.TryParse(Console.ReadLine(), out choice);
                if (!success)
                {
                    continue;
                }
                switch (choice)
                {
                    case MyEnum.NEW_BUS:
                        Console.WriteLine("enter bus's id:");
                        id = Console.ReadLine();
                        Console.WriteLine("enter bus's start date");
                        DateTime dt;
                        DateTime.TryParse(Console.ReadLine(), out dt);
                        if(!bus_Company.NewBus(id, dt))
                        {
                            Console.WriteLine("Error: bus alredy exist, or id length dosen't fit the start date");
                        }
                        break;
                    case MyEnum.RIDE:
                        Console.WriteLine("enter bus's id: ");
                        id = Console.ReadLine();
                        if(bus_Company.Ride(id, (new Random()).Next(1200)))
                        {
                            Console.WriteLine("have a nice ride");
                        }
                        else
                        {
                            Console.WriteLine("can't execute this ride: not enough feull or need a treatment");
                        }
                        break;
                    case MyEnum.MAINTENANCE:
                        Console.WriteLine("Enter bus's id:");
                        id = Console.ReadLine();
                        Console.WriteLine("enter 1 to refuel or 2 for treatment: ");
                        int kind = Console.Read();
                        if(bus_Company.Maintenance(id, kind == 1 ? true : false))
                        {
                            Console.WriteLine("Done!");
                        }
                        else
                        {
                            Console.WriteLine("Error: Bus dosen't exist");
                        }
                        break;
                    case MyEnum.BUS_STATUS:
                        bus_Company.print_stat();
                        break;
                    case MyEnum.EXIT:
                        Console.WriteLine("bay bay!");
                        exit = true;
                        break;
                    default:
                        break;
                }
            } while (!exit);
        }
    }
}
