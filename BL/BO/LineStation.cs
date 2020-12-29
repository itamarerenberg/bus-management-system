namespace BO
{
    public class LineStation
    {
        public int LineId { get; set; }
        public int StationNumber { get; set; }
        public int LineStationIndex { get; set; }

        public AdjastStations PrevToCurrent { get; set; }
        public AdjastStations CurrentToNext { get; set; }

        public double DistanceBack { get => PrevToCurrent.Distance; }
        public double DistanceNext { get => CurrentToNext.Distance; }
        public int PrevStation { get => PrevToCurrent.StationCode1; }
        public int NextStation { get => CurrentToNext.StationCode2; }

        public int location { get; set; }
    }
}