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
            BusLine[] linesArr = { Rand_BusLine(8), Rand_BusLine(8), Rand_BusLine(8), Rand_BusLine(8), Rand_BusLine(8) };
            Lines lines = new Lines(linesArr);
            for (int i = 0; i < 5; i++)
            {
                Random r = new Random(DateTime.Now.Millisecond);
                lines.add_line(new BusLine(++code, lines.))

        }
    }
}
