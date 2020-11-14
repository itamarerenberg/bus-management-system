using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace dotNet5781_02_1038_0685
{
    public class BusLine : IComparable
    { //fields
        public int LineNum { get; private set; }
        public LineStation FirstStation { get =>  Stations[0];  private set { Stations.Insert(0, value); } }
        public LineStation LastStation { get =>  Stations[Stations.Count - 1]; private set { Stations[Stations.Count - 1] = value; } }
        public Areas Area { get; private set; }
        public List<LineStation> Stations { get; private set; }
    
        //constructor
        public BusLine(int lineNum, List<LineStation> stations, Areas area = Areas.General)
        {
            LineNum = lineNum;
            Stations = new List<LineStation>(stations);
            foreach (var item in stations)
            {
                item.base_station.Add_line(this);
            }
        }
        //methods
        public override string ToString()
        {
            string output = string.Format("Line number : {0} \narea : {1} \nstations: " , LineNum, Area);
            for (int i = 0; i < Stations.Count; i++)
            {
                output += string.Format("   {0} - {1}", i + 1, Stations[i].Code);
            }
            return output + "\n";
        }

        /// <summary>
        /// adding a station to the line in any place 
        /// </summary>
        /// <param name="station">object of type LineStation</param>
        /// <param name="distance">the distance from the next station </param>
        /// <param name="code">the code of the next station</param>
        public void Add_station(LineStation station, double distance = -1, int code = -1)//enumerator.moveNext
        {
            if (code != -1)
            {
                for (int i = 0; i < Stations.Count; i++)
                {
                    if (Stations[i].Code == code)
                    {
                        Stations[i].Distance = distance;
                        Stations.Insert(i, station);
                        return;
                    }
                }
                throw new KeyNotFoundException("error: the station code is not exist");
            }
            Stations.Add(station);
        }
        
        /// <summary>
        /// removing a station from the line
        /// </summary>
        /// <param name="code">station code</param>
        /// <param name="distance">fill the new distance after removing</param>
        public void Remove_station(int code, int distance = -1)
        {
            for (int i = 0; i < Stations.Count; i++)
            {
                if (Stations[i].Code == code)
                {
                    if (i < Stations.Count - 1)
                    {
                        if (distance != -1)
                        {
                            Stations[i + 1].Distance += Stations[i].Distance;
                        }
                        else
                        {
                            Stations[i + 1].Distance = distance;
                        }
                    }

                    Stations.RemoveAt(i);
                    return;
                }
            }  
        }
        /// <summary>
        /// return if the station is in the line
        /// </summary>
        /// <param name="stationCode"></param>
        /// <returns></returns>
        public bool Station_in_the_line(int stationCode)
        {
            foreach (LineStation item in Stations)
            {
                if(item.Code == stationCode)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// find the station by its code number
        /// </summary>
        /// <param name="code"></param>
        /// <returns>return a LineStation</returns>
        public LineStation Get_station_by_code(int code)
        {
            if (Station_in_the_line(code))
            {
                return this.Stations.Find(s => s.Code == code);
            }
            else
            {
                throw new NotExist("the station is not exist");
            }
        }
        /// <summary>
        /// calculate the distance between 2 given stations
        /// </summary>
        /// <param name="station1"></param>
        /// <param name="station2"></param>
        /// <returns>the distance between the provided stations</returns>
        public double Get_distance(LineStation station1, LineStation station2)
        {
            int index1 = Stations.IndexOf(station1);
            if (index1 == -1) { throw new KeyNotFoundException("error: the station 1 is not exist!"); }
            int index2 = Stations.IndexOf(station2);
            if (index2 == -1) { throw new KeyNotFoundException("error: the station 2 is not exist!"); }
            if (index1 > index2)//swap func
            {
                index1 += index2;
                index2 = index1 - index2;
                index1 -= index2;
            }
            double sum = 0;
            for (int i = index1 + 1; i <= index2; i++)
            {
                sum += Stations[i].Distance;
            }
            return sum;
        }
        /// <summary>
        /// calculateing the time ride of all line by default (no parameters required) 
        /// </summary>
        /// <returns>the time ride of all line</returns>
        public TimeSpan Get_time()
        {
            TimeSpan sum = new TimeSpan();
            for (int i = 0 + 1; i <= Stations.Count; i++)
            {
                sum += Stations[i].RideTime;
            }
            return sum;
        }
        /// <summary>
        /// calculating the ride time between 2 given stations
        /// </summary>
        /// <param name="station1"></param>
        /// <param name="station2"></param>
        /// <returns>the ride time between the provided stations</returns>
        public TimeSpan Get_time(LineStation station1 , LineStation station2)
        {
            int index1 = Stations.IndexOf(station1);
            if (index1 == -1) { throw new KeyNotFoundException("error: the station 1 is not exist!"); }
            int index2 = Stations.IndexOf(station2);
            if (index2 == -1) { throw new KeyNotFoundException("error: the station 2 is not exist!"); }
            if (index1 > index2)//swap func
            {
                index1 += index2;
                index2 = index1 - index2;
                index1 -= index2;
            }
            TimeSpan sum = new TimeSpan();
            for (int i = index1 + 1; i <= index2; i++)
            {
                sum += Stations[i].RideTime;
            }
            return sum;
        }
        /// <summary>
        /// creating a new subline by the range between 2 given stations
        /// </summary>
        /// <param name="station1"></param>
        /// <param name="station2"></param>
        /// <returns>new subline by the range between 2 provided stations</returns>
        public BusLine Sub_line(LineStation station1 , LineStation station2)
        {
            int index1 = Stations.IndexOf(station1);
            if (index1 == -1) { throw new KeyNotFoundException("error: the station 1 is not exist!"); }
            int index2 = Stations.IndexOf(station2);
            if (index2 == -1) { throw new KeyNotFoundException("error: the station 2 is not exist!"); }
            if (index1 > index2)//swap func
            {
                index1 += index2;
                index2 = index1 - index2;
                index1 -= index2;
            }
            List<LineStation> subList = Stations.GetRange(index1, index2 - index1 + 1);

            return new BusLine(0, subList, this.Area);
        }
        //public int CompareTo(object bl)
        //{
        //    if(!(bl is BusLine))
        //    {
        //        throw new InvalidCastException(string.Format($"you cannot compare type BusLine to type {bl.GetType()}"));
        //    }
        //    if (Get_time(this.FirstStation, this.LastStation) == Get_time(((BusLine)bl).FirstStation, ((BusLine)bl).LastStation))
        //    {
        //        return 0;
        //    }
        //    else if (Get_time(this.FirstStation, this.LastStation) > Get_time(((BusLine)bl).FirstStation, ((BusLine)bl).LastStation))
        //    {
        //        return 1;
        //    }
        //    else 
        //    {
        //        return -1;
        //    }
        //}
        public int CompareTo(object obj)
        {
            BusLine bl = obj as BusLine;
            if (bl == null)
            {
                throw new InvalidCastException(string.Format($"you cannot compare type BusLine to type {bl.GetType()}"));
            }
            return this.Get_time().CompareTo(bl.Get_time());
        }
    }
}
