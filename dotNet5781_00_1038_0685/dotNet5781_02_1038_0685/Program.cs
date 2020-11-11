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
            return new BusLine(++code, stations, 0);
        }

        static void Main(string[] args)
        {
            TimeSpan time = new TimeSpan(756789);
            LineStation LStation1 = new LineStation(11111, Rand_double(31, 33.3), Rand_double(34.3, 35.5), 5, time);
            //LineStation LStation2 = new LineStation(22222, Rand_double(31, 33.3), Rand_double(34.3, 35.5), 5, time);
            //LineStation[] arr = { LStation1, LStation2 };
            //BusLine busLine = new BusLine(667, arr);
            //Console.WriteLine(busLine.ToString());

            // BusLine line = new BusLine()

        }
    }
}
