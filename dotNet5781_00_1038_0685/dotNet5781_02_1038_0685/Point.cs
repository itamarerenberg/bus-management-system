namespace dotNet5781_02_1038_0685
{
    public struct Point
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        static public bool operator ==(Point a, Point b)
        {
            return ((a.Latitude == b.Latitude) && (a.Longitude == b.Longitude));
        }
        static public bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }
    }
}
