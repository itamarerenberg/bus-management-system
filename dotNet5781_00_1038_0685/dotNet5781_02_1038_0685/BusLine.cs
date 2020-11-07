using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace dotNet5781_02_1038_0685
{
    class BusLine : IEnumerable
    { //fields
        public int LineNum { get; private set; }
        public LineStation FirstStation { get =>  Stations[0];  private set { Stations.Insert(0, value); } }
        public LineStation LastStation { get =>  Stations[Stations.Count - 1]; private set { Stations[Stations.Count - 1] = value; } }
        public Areas Area { get; private set; }
        private List<LineStation> Stations { get; set; }
        private static List<int> linesList = new List<int> { };

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
        //constructor
        public BusLine(int lineNum, LineStation[] stations, Areas area = Areas.General)
        {
            LineNum = lineNum;
            Stations = new List<LineStation>(stations);
            linesList.Add(lineNum);
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

        public void Add_station(LineStation station, double distance = -1, int code)
        {
            Stations[i] =
            Stations.Insert(i;

        }
        public void Remove_station(int index)
        {
            Stations.RemoveAt(index);
        }
        public bool Station_in_the_line()
        {
            return false;
        }
    }
}
