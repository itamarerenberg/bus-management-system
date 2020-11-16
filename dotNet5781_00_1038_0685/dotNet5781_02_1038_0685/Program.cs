using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_1038_0685
{
    class Program
    {
        static void Main(string[] args)
        {
            //creating repository of random lines------------------

            Lines lines = new Lines();
            MyRand.Rand_lines(lines, 8, 8);
            MyRand.Rand_cross_lines(lines, 5);

            //-----------------------------------------------------

            bool exit = false;
            int line_number;
            BusLine bl = null;

            do
            {
                //printing area ------------------

                for (int i = 0; i < 8; i++)
                {
                    Console.WriteLine($"{i} - {Enum.GetName(typeof(Options), i)}\n");
                }
                Console.WriteLine("for exit press -1\n\nPlease, make your choice:");
                bool success = Enum.TryParse(Console.ReadLine(), out Options choice);
                if (!success)
                {
                    continue;
                }
                //--------------------------------
                switch (choice)
                {
                    #region ADD_BUS_LINE
                    case Options.ADD_BUS_LINE:
                        Console.WriteLine("enter line number:");
                        while (!int.TryParse(Console.ReadLine(), out line_number))
                        {
                            Console.WriteLine("please enter a number");
                        }
                        try
                        {
                            lines.Add_line(MyRand.Rand_BusLine(8, line_number));
                            Console.WriteLine($"line {line_number} added succesfuly");
                        }
                        catch (Exception msg)
                        {
                            Console.WriteLine(msg.Message);
                        }
                        Console.ReadLine();
                        break;
                    #endregion
                    #region ADD_STATION
                    case Options.ADD_STATION:
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
                        catch (Exception msg)
                        {
                            Console.WriteLine(msg.Message);
                            Console.ReadLine();
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
                            bl.Add_station(MyRand.Rand_station(st_code), dist, code_next_st);
                        }
                        catch (Exception msg)
                        {

                            Console.WriteLine(msg.Message);
                            Console.ReadLine();
                            break;
                        }
                        Console.ReadLine();
                        break;
                    #endregion
                    #region DELETE_BUS_LINE
                    case Options.DELETE_BUS_LINE:
                        Console.WriteLine("enter line number:");
                        while (!int.TryParse(Console.ReadLine(), out line_number))
                        {
                            Console.WriteLine("please enter a number");
                        }
                        lines.Remove_line(line_number);
                        Console.WriteLine($"line {line_number} reemoved succesfuly");
                        Console.ReadLine();
                        break;
                    #endregion
                    #region DELETE_STATION
                    case Options.DELETE_STATION:
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
                            Console.ReadLine();
                            break;
                        }

                        Console.WriteLine("enter the code of the station to remove");
                        while (!int.TryParse(Console.ReadLine(), out st_code))
                        {
                            Console.WriteLine("please enter a number");
                        }

                        bl.Remove_station(st_code);//the new distance between the stations befor and after this station will be by defult the same distance as before
                        Console.WriteLine($"station {st_code} removed from line {line_number} succesfuly");
                        Console.ReadLine();
                        break;
                    #endregion
                    #region LINES_STOPPING_AT_THE_STATION
                    case Options.LINES_STOPPING_AT_THE_STATION:
                        Console.WriteLine("Enter station code:");
                        while (!int.TryParse(Console.ReadLine(), out st_code))
                        {
                            Console.WriteLine("please enter a number");
                        }
                        try
                        {
                            List<BusLine> lbl = Station.get_station(st_code).Pass_here;
                            Console.WriteLine($"the lines which stops at station {st_code} are: ");
                            foreach (var item in lbl)
                            {
                                Console.Write(item.LineNum + ", ");
                            }
                        }
                        catch (Exception msg)
                        {
                            Console.WriteLine(msg.Message);
                        }
                        Console.ReadLine();
                        break;
                    #endregion
                    #region FIND_RIDE_BETWEEN_2_STATIONS
                    case Options.FIND_RIDE_BETWEEN_2_STATIONS:
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
                            Console.ReadLine(); 
                            break;
                        }
                        Console.ReadLine();
                        break;
                    #endregion
                    #region PRINT_ALL_LINES
                    case Options.PRINT_ALL_LINES:
                        Console.WriteLine(lines);
                        Console.ReadLine();
                        break;
                    #endregion
                    #region PRINT_ALL_STATIONS
                    case Options.PRINT_ALL_STATIONS:
                        foreach (var st in Station.exists_stations)
                        {
                            Console.Write($"{st.StationCode, -6} : ");
                            foreach (var line in st.Pass_here.Distinct())
                            {
                                Console.Write(line.LineNum + " ");
                            }
                            Console.Write('\n');
                        }
                        Console.ReadLine();
                        break;
                    #endregion
                    #region EXIT
                    case Options.EXIT:
                        Console.WriteLine("finish!");
                        exit = true;
                        break;
                    default:
                        break; 
                        #endregion
                }
            } while (!exit);

        }
    }
}
