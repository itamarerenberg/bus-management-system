using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_1038_0685
{
    static class MyRand
    {
        private static readonly Random r = new Random(DateTime.Now.Millisecond);

        #region Rand_double
        /// <summary>
        /// generate a random number between a given range
        /// </summary>
        /// <param name="min">minimum number</param>
        /// <param name="max">maximum number</param>
        /// <returns>double number between min and max</returns>
        public static double Rand_double(double min, double max)
        {
            return r.NextDouble() * (max - min) + min;
        }
        #endregion
        #region Rand_station
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
                    Double distance = Rand_double(1, 60);
                    if (code != -1)
                    {
                        return new LineStation(new Station(code, Rand_double(31, 33.3), Rand_double(34.3, 35.5)), distance, new TimeSpan(0, (int)distance, 0));
                    }
                    return new LineStation(new Station(r.Next(999999), Rand_double(31, 33.3), Rand_double(34.3, 35.5)), distance, new TimeSpan(0, (int)distance, 0));
                }
                catch (ArgumentException)
                {
                    continue;
                }
                catch (Exception)
                {
                    throw;
                }
            } while (true);
        }
        #endregion
        #region Rand_BusLine
        /// <summary>
        /// generating a Bus Line from random values
        /// </summary>
        /// <param name="stationNum"></param>
        /// <param name="line_num"></param>
        /// <returns></returns>
        public static BusLine Rand_BusLine(int stationNum, int line_num = -1)
        {
            List<LineStation> stations = new List<LineStation>();
            for (int i = 0; i < stationNum; i++)
            {
                stations.Add(Rand_station());
            }
            if (line_num != -1)
            {
                return new BusLine(line_num, stations, (Areas)r.Next(5));
            }
            do
            {
                try
                {
                    return new BusLine(r.Next(100), stations, (Areas)r.Next(5));
                }
                catch (Exception)
                {
                    continue;
                }                
            } while (true);
        }
        #endregion
        #region Rand_lines
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
        #endregion
        #region Rand_cross_lines
        /// <summary>
        /// add to a "Lines" busLines that cross the existing busLines 
        /// </summary>
        /// <param name="arr">an existing "Lines"</param>
        /// <param name="num">the requested number of busLines </param>
        public static void Rand_cross_lines(Lines arr, int num)
        {
            int k;
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
                for (int j = 0; j < k; j++)
                {
                    bl.Add(arr.Lines_list[i].Stations[((int)r.Next(8))]);
                }
                arr.Add_line(new BusLine(r.Next(100), bl, 0));
            }
            #endregion
        }
    }
}
