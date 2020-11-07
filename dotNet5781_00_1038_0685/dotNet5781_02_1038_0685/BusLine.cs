using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace dotNet5781_02_1038_0685
{
    class BusLine : IComparable
    { //fields
        public int LineNum { get; private set; }
        public LineStation FirstStation { get =>  Stations[0];  private set { Stations.Insert(0, value); } }
        public LineStation LastStation { get =>  Stations[Stations.Count - 1]; private set { Stations[Stations.Count - 1] = value; } }
        public Areas Area { get; private set; }
        private List<LineStation> Stations { get; set; }
    
        //constructor
        public BusLine(int lineNum, LineStation[] stations, Areas area = Areas.General)
        {
            LineNum = lineNum;
            Stations = new List<LineStation>(stations);
        }
        //methods
        public override string ToString()
        {
            string output = string.Format("Line number : {0} \narea : {1} \nstations: " , LineNum, Area);
            for (int i = 0; i < Stations.Count; i++)
            {
                output += string.Format("\n{0} - {1}", i + 1, Stations[i].StationCode);
            }
            return output;
        }

        public void Add_station(LineStation station, double distance = -1, int code = -1)
        {
            if (code != -1)
            {
                for (int i = 0; i < Stations.Count; i++)
                {
                    if (Stations[i].StationCode == code)
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
        
        public void Remove_station(int code, int distance = -1)
        {
            for (int i = 0; i < Stations.Count; i++)
            {
                if (Stations[i].StationCode == code)
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
        public bool Station_in_the_line(Station station)
        {
            return (Stations.Contains(station));
        }
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
        public TimeSpan Get_time(LineStation station1, LineStation station2)
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
        public BusLine Sub_line(LineStation station1, LineStation station2)
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

            return new BusLine(0, subList.ToArray(), this.Area);
        }
        
        public int CompareTo(object bl)
        {
            if(!(bl is BusLine))
            {
                throw new InvalidCastException("error");//**
            }
            if (Get_time())
            {

            }
        }
    }
}
