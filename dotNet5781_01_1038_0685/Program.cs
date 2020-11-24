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
    public enum Treatment_kind
    {
        refule, treatment
    }
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            Bus_company bus_Company = new Bus_company();
            Console.WriteLine(@"0 : add new bus" + '\n' +
                                "1 : take a ride\n2: maintenance" + '\n' +
                                "3 : print buses's status" + '\n' +
                                "-1: exit");
            MyEnum choice;
            string licensNum;
            DateTime startDate;
            double km;
            bool vld_inpt;
            Bus bs;
            do
            {
                vld_inpt = Enum.TryParse(Console.ReadLine(), out choice);
                if (!vld_inpt)
                {
                    continue;
                }
                switch (choice)
                {
                case MyEnum.NEW_BUS:
                        Console.WriteLine("enter bus licens's num: ");
                        licensNum = Console.ReadLine();
                        Console.WriteLine("enter start service date(yy:mm:dd): ");
                        vld_inpt = DateTime.TryParse(Console.ReadLine(), out startDate);
                        while(!vld_inpt)
                        {
                            Console.WriteLine("plese enter a valid input: "); 
                            vld_inpt = DateTime.TryParse(Console.ReadLine(), out startDate);
                        }

                        bus_Company.Add_new_bus(licensNum, startDate);

                        Console.WriteLine("Done!");
                    break;
                case MyEnum.RIDE:
                        Console.WriteLine("enter bus licens's num: ");
                        licensNum = Console.ReadLine();

                        bs = bus_Company[licensNum];

                        Console.WriteLine("enter ride's length: ");
                        vld_inpt = double.TryParse(Console.ReadLine(), out km);

                        while (!vld_inpt)
                        {
                            Console.WriteLine("plese enter a valid input: ");
                            vld_inpt = double.TryParse(Console.ReadLine(), out km);
                        }

                        if (bs.ride(km))
                        {
                            Console.WriteLine("have a nice ride!");
                        }
                        else
                        {
                            Console.WriteLine("can't preform this ride");
                        }

                        break;
                case MyEnum.MAINTENANCE:
                        Console.WriteLine("enter bus licens's num: ");
                        licensNum = Console.ReadLine();

                        bs = bus_Company[licensNum];

                        Treatment_kind kind;
                        Console.WriteLine("for refule enter 0, for treatment enter 1");
                        vld_inpt = Enum.TryParse(Console.ReadLine(), out kind);
                        while (!vld_inpt)
                        {
                            Console.WriteLine("plese enter a valid input: ");
                            vld_inpt = Enum.TryParse(Console.ReadLine(), out kind);
                        }

                        if(kind == Treatment_kind.refule)
                        {
                            bs.refule();
                        }
                        if(kind == Treatment_kind.treatment)
                        {
                            bs.treatment();
                        }
                        Console.WriteLine("Done!");
                        break;
                    case MyEnum.BUS_STATUS:
                        Console.WriteLine(bus_Company);
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
