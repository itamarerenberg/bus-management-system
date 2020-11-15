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
        public static LineStation Rand_station(int code = -1)
        {
            do
            {
                try
                {
                    Random r = new Random(DateTime.Now.Millisecond);
                    Double distance = Rand_double(1, 60);
                    if (code != -1)
                    {
                        return new LineStation(new Station(r.Next(999999), Rand_double(31, 33.3), Rand_double(34.3, 35.5)), distance, new TimeSpan(0, (int)distance, 0)); 
                    }
                    return new LineStation(new Station(code, Rand_double(31, 33.3), Rand_double(34.3, 35.5)), distance, new TimeSpan(0, (int)distance, 0));
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
        public static BusLine Rand_BusLine(int stationNum, int line_num = -1)
        {
            List<LineStation> stations = new List<LineStation>();
            for (int i = 0; i < stationNum; i++)
            {
                stations.Add(Rand_station());
            }
            if (line_num != -1)
            {
                return new BusLine(++code, stations, ((Areas)code - 1)); 
            }
            return new BusLine(line_num, stations, ((Areas)code - 1));
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

            try
            {
                new Station(1, 1, 1);
                new Station(1, 1, 1);
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg.Message);
            }

            //-----------------------------------------------------

            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine("\n" + i + " - " + Enum.GetName(typeof(MyEnum2), i));
            }
            Console.WriteLine("\nfor exit press -1\n");
            MyEnum2 choice;
            bool exit = false;
            int line_number;
            BusLine bl = null;
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
                        Console.WriteLine("enter line number:");
                        while (!int.TryParse(Console.ReadLine(), out line_number))
                        {
                            Console.WriteLine("please enter a number");
                        }
                        try
                        {
                            lines.Add_line(Rand_BusLine(8, line_number));
                            Console.WriteLine($"line {line_number} added succesfuly");
                        }
                        catch (Exception msg)
                        {
                            Console.WriteLine(msg.Message);
                        }
                        break;
                    case MyEnum2.ADD_STATION:
                        //getting the number of the line which the new station will add to
                        Console.WriteLine("enter line number:");
                        int ln;
                        while (!int.TryParse(Console.ReadLine(), out ln))
                        {
                            Console.WriteLine("please enter a number");
                        }
                        try
                        {
                            bl = lines[ln];
                        }
                        catch(Exception msg)
                        {
                            Console.WriteLine(msg.Message);
                            break;
                        }
                        Console.WriteLine(bl);

                        //getting the new station's info
                        int st_code, dist, code_next_st;
                        Console.WriteLine("enter the code of the station");
                        while (!int.TryParse(Console.ReadLine(), out st_code))
                        {
                            Console.WriteLine("please enter a number");
                        }

                        //locate the new station in the line
                        Console.WriteLine("enter the code of the station you want to be after the new station and the distance between them");
                        while (!int.TryParse(Console.ReadLine(), out code_next_st))
                        {
                            Console.WriteLine("please enter a number");
                        }

                        Console.WriteLine("enter distance");
                        while (!int.TryParse(Console.ReadLine(), out dist))
                        {
                            Console.WriteLine("please enter a number");
                        }

                        try
                        {
                            bl.Add_station(Rand_station(st_code), dist, code_next_st);
                        }
                        catch (Exception msg)
                        {

                            Console.WriteLine(msg.Message);
                            break;
                        }

                        break;
                    case MyEnum2.DELETE_BUS_LINE:
                        Console.WriteLine("enter line number:");
                        while (!int.TryParse(Console.ReadLine(), out line_number))
                        {
                            Console.WriteLine("please enter a number");
                        }
                        lines.Remove_line(line_number);
                        break;
                    case MyEnum2.DELETE_STATION:
                        Console.WriteLine("enter line number:");
                        while (!int.TryParse(Console.ReadLine(), out line_number))
                        {
                            Console.WriteLine("please enter a number");
                        }

                        try
                        {
                            bl = lines[line_number];
                        }
                        catch (Exception msg)
                        {
                            Console.WriteLine(msg.Message);
                            break;
                        }

                        Console.WriteLine("enter the code of the station to remove");
                        while (!int.TryParse(Console.ReadLine(), out st_code))
                        {
                            Console.WriteLine("please enter a number");
                        }

                        bl.Remove_station(st_code);//the new distance between the stations befor and after this station will be by defult the same distance as before

                        break;
                    case MyEnum2.FIND_LINES_IN_THE_STATION:
                        Console.WriteLine("Enter station code:");
                        while (!int.TryParse(Console.ReadLine(), out st_code))
                        {
                            Console.WriteLine("please enter a number");
                        }

                        try
                        {
                            List<BusLine> lbl = Station.get_station(st_code).Pass_here;
                            Console.WriteLine($"the lines whch stops at station{st_code} are: ");
                            foreach (var item in lbl)
                            {
                                Console.Write(item.LineNum + ", ");
                            }
                        }
                        catch (Exception msg)
                        {
                            Console.WriteLine(msg.Message);
                        }
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

                            if (lbl.Count != 0)
                            {
                                lbl.Sort((b1, b2) => b1.Get_time(station1, station2).CompareTo(b2.Get_time(station1, station2)));
                                foreach (var item in lbl)
                                {
                                    Console.WriteLine($"line {item.LineNum}: " +
                                        $"{item.Get_time(station1, station2).Hours} hours and {item.Get_time(station1, station2).Hours} minutes");
                                } 
                            }
                            else
                            {
                                Console.WriteLine("station are not exist or they aren't on the same line");
                            }
                        }
                        catch (Exception msg)
                        {
                            Console.WriteLine(msg.Message);
                            break;
                        }

                        break;
                    case MyEnum2.PRINT_ALL_LINES:
                        Console.WriteLine(lines);

                        break;
                    case MyEnum2.PRINT_ALL_STATIONS:
                        foreach (var st in Station.exists_stations)
                        {
                            Console.Write(st + " : ");
                            foreach (var line in st.Pass_here)
                            {
                                Console.Write(line.LineNum + ", ");
                            }
                            Console.Write('\n');
                        }

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
