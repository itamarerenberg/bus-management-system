namespace BO
{
    public class LineStation
    {
        public AdjastStations Last_section { get; set; }
        public Station BaseStation
        {
            get => Last_section.stationA;
            set => Last_section.stationA = value;
        }
        public Station PrevStation { 
            get => Last_section.stationB;
            set => Last_section.stationB = value;
        }
        public int Place_in_line { get; set; }
    }
}