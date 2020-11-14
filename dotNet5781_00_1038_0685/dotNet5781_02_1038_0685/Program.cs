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
            do
            {
                try
                {
                    Random r = new Random(DateTime.Now.Millisecond);
                    Double distance = Rand_double(1, 60);
                    return new LineStation(r.Next(999999), Rand_double(31, 33.3), Rand_double(34.3, 35.5), distance, new TimeSpan(0, (int)distance, 0));
                }
                catch(ArgumentException)
                {
                    continue;
                }
                catch (Exception)
                {
                    throw;
                } 
            } while (true);
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
                for (int j = 0; j < 8; j++)
                {
                    bl.Add(lines.lines_list[i].Stations[((int)r.Next(8))]);
                }
                lines.add_line(new BusLine(++code, bl, 0));
            }
            //-----------------------------------------------------
            Console.WriteLine("Please, make your choice:");
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine("\n" + i + " - " + Enum.GetName(typeof(MyEnum2), i));
            }
            Console.WriteLine("\nfor exit press -1");
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
                        Console.WriteLine("Enter the origin's station number and destenation's station number");
                        if (!int.TryParse(Console.ReadLine(), out int station1))
                        {
                            Console.WriteLine("please enter a number");
                        }
                        if (!int.TryParse(Console.ReadLine(), out int station2))
                        {
                            Console.WriteLine("please enter a number");
                        }
                        try
                        {
                            Dictionary<int, TimeSpan> tempDic = new Dictionary<int, TimeSpan> { };
                            foreach (BusLine bl in lines)
                            {
                                if (bl.Station_in_the_line(station1) && bl.Station_in_the_line(station2))
                                {
                                    int key = bl.LineNum;
                                    TimeSpan value = bl.Get_time(bl.Get_station_by_code(station1), bl.Get_station_by_code(station2));
                                    tempDic.Add(key, value);
                                }
                            }
                            var dic = from i in tempDic orderby i.Value ascending select i;

                            foreach (var item in dic)
                            {
                                Console.WriteLine($"line {item.Key}: {item.Value.TotalMinutes} minutes");
                            }
                        }
                        catch (Exception msg)
                        {
                            Console.WriteLine(msg);
                            break;
                        }

                        break;
                    case MyEnum2.PRINT_ALL_LINES:
                        Console.WriteLine(lines);

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
