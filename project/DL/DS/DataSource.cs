using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DO;
using SQLite;

namespace DS
{
    public static class DataSource
    {
        #region lists
        public static List<AdjacentStations> AdjacentStations = new List<AdjacentStations>();
        public static List<Bus> Buses = new List<Bus>();
        public static List<Line> Lines = new List<Line>();
        public static List<BusOnTrip> BusesOnTrip = new List<BusOnTrip>();
        public static List<Station> Stations = new List<Station>();
        public static List<LineStation> LineStations = new List<LineStation>();
        public static List<LineTrip> LineTrips = new List<LineTrip>();
        public static List<User> Users = new List<User>();
        public static List<UserTrip> UsersTrips = new List<UserTrip>();
        #endregion
        static string FileName = "DataSource.xml";
        public static XElement dsRoot;
        public static int serialLineID;
        static string dataBasePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database");
        static string FilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database", FileName);

        static DataSource()
        {
            serialLineID = 0;
            if(!File.Exists(FilePath))//if the file don't exist
            {
                CreateFile();
            }
            else
            {
                LoadData();
            }
        }

        private static void CreateFile()
        {
            dsRoot = new XElement("DS");
            dsRoot.Add(
                     new XElement("Stations"),//add new label in wich the stations will be save
                     new XElement("Lines"),//add new label in wich the lines will be save
                     new XElement("users"),//...users...
                     new XElement("AdjacentStations"),//...AdjacentStations...
                     new XElement("BusesOnTrip"),//...BusesOnTrip...
                     new XElement("LineStations"),//...LineStations...
                     new XElement("LineTrips"),//...LineTrips...
                     new XElement("Users"),//...Users...
                     new XElement("UsersTrips")//...UsersTrips...
                     );
            string StationsPath = System.IO.Path.Combine(dataBasePath, "Stations.db");
            using (SQLiteConnection stations = new SQLiteConnection(StationsPath))
            {
                stations.CreateTable<Station>();
                Stations = stations.Table<Station>().ToList();
            }
            Users = new List<User>() { new User() { Name = "Admin", Password = "1234", Admin = true, IsActive = true } };
            foreach (var st in Stations)
            {
                SaveObj(st, "Stations");
            }
            dsRoot.Save(FilePath);
        }

        public static void LoadData()
        {
            try
            {
                dsRoot = XElement.Load(FilePath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }

        /// <summary>
        /// adding a new object to the DataSource's xml file
        /// </summary>
        /// <param name="obj">the object to add</param>
        /// <param name="lable">the label to add the new object to</param>
        public static void SaveObj(object obj, string lable)
        {
            XElement newObj = new XElement(obj.GetType().Name);
            foreach(var prop in obj.GetType().GetProperties())
            {
                string val = prop.GetValue(obj).ToString();
                newObj.Add(new XElement(prop.Name, val));//insert new label <prop.Name> prop.GetValue(prop).ToString() </prop.Name>
            }
            dsRoot.Element(lable).Add(newObj);
            dsRoot.Save(FilePath);
        }

        public static void Save()
        {
            dsRoot.Save(FilePath);
        }
    }
}