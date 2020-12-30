namespace BO
{
    public class LineStation
    {
        public int LineId { get; set; }
        public int StationNumber { get; set; }
        public int LineStationIndex { get; set; }

        public AdjacentStations PrevToCurrent { get; set; }
        public AdjacentStations CurrentToNext { get; set; }
        
        public double DistanceBack { get => PrevToCurrent.Distance; }
        public double DistanceNext { get => CurrentToNext.Distance; }
        public int? PrevStationCode { get => PrevToCurrent.StationCode1; }
        public int? NextStationCode { get => CurrentToNext.StationCode2; }

        public int location { get; set; }
    }
}