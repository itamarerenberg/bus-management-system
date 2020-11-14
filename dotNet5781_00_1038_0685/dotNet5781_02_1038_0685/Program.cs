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
                    return new LineStation(new Station(r.Next(999999), Rand_double(31, 33.3), Rand_double(34.3, 35.5)), distance, new TimeSpan(0, (int)distance, 0));
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
        /// <summary>
        /// generating a "Lines" from random values
        /// </summary>
        /// <param name="arr">an existing "Lines"</param>
        /// <param name="num">the requested number of busLines </param>
        /// <param name="numOfStation"></param>
        /// <returns>the </returns>
        public static void Rand_lines(Lines arr, int num, int numOfStation)
        {
            for (int i = 0; i < num; i++)
            {
                arr.Add_line(Rand_BusLine(numOfStation));
            }
        }
        /// <summary>
        /// add to a "Lines" busLines that cross the existing busLines 
        /// </summary>
        /// <param name="arr">an existing "Lines"</param>
        /// <param name="num">the requested number of busLines </param>
        public static void Rand_cross_lines(Lines arr, int num)
        {
            int k = 0;
            if (arr.Lines_list.Count > 0)// return the lines number of shortest line
            {
                k = arr.Lines_list[0].Stations.Count;
                foreach (var item in arr.Lines_list)
                {
                    if (item.Stations.Count() < k)
                    {
                        k = item.Stations.Count();
                    }
                } 
            }
            else
            {
                k = 0;
            }
            for (int i = 0; i < num; i++)
            {
                List<LineStation> bl = new List<LineStation>();
                Random r = new Random(DateTime.Now.Millisecond);
                for (int j = 0; j < k; j++)
                {
                    bl.Add(arr.Lines_list[i].Stations[((int)r.Next(8))]);
                }
                arr.Add_line(new BusLine(++code, bl, 0));
            }
        }

        static void Main(string[] args)
        {
            
            Lines lines = new Lines();
            Rand_lines(lines, 8, 8);
            Rand_cross_lines(lines, 5);

            //-----------------------------------------------------
            
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine("\n" + i + " - " + Enum.GetName(typeof(MyEnum2), i));
            }
            Console.WriteLine("\nfor exit press -1\n");
            MyEnum2 choice;
            bool exit = false;
            do
            {
                Console.WriteLine("Please, make your choice:");
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
                        int station1, station2;
                        while (!int.TryParse(Console.ReadLine(), out station1))
                        {
                            Console.WriteLine("please enter a number");
                        }
                        while (!int.TryParse(Console.ReadLine(), out station2))
                        {
                            Console.WriteLine("please enter a number");
                        }
                        try
                        {
                            List<BusLine> lbl = lines.Which_stops_at(station1, station2);

                            if(lbl.Count == 0)
                            {
                                Console.WriteLine("");
                            }

                            lbl.Sort((b1, b2) => b1.Get_time(station1, station2).CompareTo(b2.Get_time(station1, station2)));
                            foreach (var item in lbl)
                            {
                                Console.WriteLine($"line {item.LineNum}: {item.Get_time(station1, station2)} minutes");
                            }
                            Dictionary<int, TimeSpan> tempDic = new Dictionary<int, TimeSpan> { };
                            foreach (BusLine bl in lines)
                            {
                                if (bl.Station_in_the_line(station1) && bl.Station_in_the_line(station2))
                                {
                                    int key = bl.LineNum;
                                    TimeSpan value = bl.Get_time(station1, station2);
                                    tempDic.Add(key, value);
                                }
                            }
                            if (tempDic.Count != 0)
                            {
                                var dic = from i in tempDic orderby i.Value ascending select i;

                                foreach (var item in dic)
                                {
                                    Console.WriteLine($"line {item.Key}: {item.Value.TotalMinutes} minutes");
                                }
                            }
                            else
                            {
                                Console.WriteLine("station are not exist or they aren't on the same line");
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
