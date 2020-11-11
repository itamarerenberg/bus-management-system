using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_1038_0685
{
    class Program
    {
        /// <summary>
        /// generate a random number between a given range
        /// </summary>
        /// <param name="min">minimum number</param>
        /// <param name="max">maximum number</param>
        /// <returns>double number between min and max</returns>
        public static double Rand_double(double min, double max)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return r.NextDouble() * (max - min) + min;
        }
        /// <summary>
        /// generating a LineStation from random values
        /// </summary>
        /// <returns>a random LinStation</returns>
        public static LineStation Rand_station()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            Double distance = Rand_double(1, 60);
            return new LineStation(r.Next(999999), Rand_double(31, 33.3), Rand_double(34.3, 35.5), distance, new TimeSpan(0, (int)distance, 0));
        }
        static int code = 0;
        public static BusLine Rand_BusLine(int stationNum)
        {
            List<LineStation> stations = new List<LineStation>();
            for (int i = 0; i < stationNum; i++)
            {
                stations.Add(Rand_station());
            }
            return new BusLine(++code, stations, ((Areas)code - 1));
        }

        static void Main(string[] args)
        {
            // creating busLines
            BusLine[] linesArr = { Rand_BusLine(8), Rand_BusLine(8), Rand_BusLine(8), Rand_BusLine(8), Rand_BusLine(8) };
            Lines lines = new Lines(linesArr);
            for (int i = 0; i < 5; i++)
            {
                List<LineStation> bl = new List<LineStation>();
                Random r = new Random(DateTime.Now.Millisecond);
                for (int j = 0; j < 8; i++)
                {
                    bl.Add(new LineStation(lines.lines_list[i].Stations[((int)r.Next(8))]));
                }
                lines.add_line(new BusLine(++code, bl, 0));
            }
            //-----------------------------------------------------
            Console.WriteLine("Please, make your choice:");
            Console.WriteLine("ADD_BUS_LINE, ADD_STATION, DELETE_BUS_LINE, DELETE_STATION, FIND_LINES_IN_THE_STATION, FIND_RIDE_BETWEEN_2_STATIONS, PRINT_ALL_LINES,PRINT_ALL_STATIONS, EXIT");
            MyEnum2 choice;
            bool exit = false;
            do
            {
                bool success = Enum.TryParse(Console.ReadLine(), out choice);
                if (!success)
                {
                    continue;
                }
                switch (choice)
                {
                    case MyEnum2.ADD_BUS_LINE:
                        Console.WriteLine("enter bus's id:");

                        break;
                    case MyEnum2.ADD_STATION:
                        Console.WriteLine("enter bus's id: ");

                        break;
                    case MyEnum2.DELETE_BUS_LINE:
                        Console.WriteLine("Enter bus's id:");

                        break;
                    case MyEnum2.DELETE_STATION:
                        
                        break;
                    case MyEnum2.FIND_LINES_IN_THE_STATION:
                        Console.WriteLine("Enter bus's id:");

                        break;
                    case MyEnum2.FIND_RIDE_BETWEEN_2_STATIONS:
                        Console.WriteLine("Enter bus's id:");

                        break;
                    case MyEnum2.PRINT_ALL_LINES:
                        Console.WriteLine("Enter bus's id:");

                        break;
                    case MyEnum2.PRINT_ALL_STATIONS:
                        Console.WriteLine("Enter bus's id:");

                        break;
                    case MyEnum2.EXIT:
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
