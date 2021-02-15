using System;

namespace BO
{
    public class LineStation
    {
        public int LineId { get; set; }
        public int StationNumber { get; set; }
        public int LineStationIndex { get; set; }
        public int Location { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }


        public AdjacentStations PrevToCurrent { get; set; }
        public AdjacentStations CurrentToNext { get; set; }

        public double Distance_from_start { get; set; }
        public TimeSpan Time_from_start { get; set; }

        public override string ToString()
        {
            return StationNumber.ToString();
        }
    }
}