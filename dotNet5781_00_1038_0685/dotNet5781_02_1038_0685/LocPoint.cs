namespace dotNet5781_02_1038_0685
{
    public struct LocPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        static public bool operator ==(LocPoint a, LocPoint b)
        {
            return ((a.Latitude == b.Latitude) && (a.Longitude == b.Longitude));
        }
        static public bool operator !=(LocPoint a, LocPoint b)
        {
            return !(a == b);
        }
        public override string ToString()
        {
            return $"{Latitude + "°N"}, {Longitude + "°E"}";
        }
    }
}
