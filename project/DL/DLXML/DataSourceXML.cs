using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DLXML
{
    class DataSourceXML
    {
        static string FileName = "DataSource.xml";

        #region files Names
        static string BusesFileName = "Buses.xml";
        static string AdjacentStationsFileName = "AdjacentStations.xml";
        static string BusesOnTripFileName = "BusesOnTrip.xml";
        static string StationsFileName = "Stations.xml";
        static string LineStationsFileName = "LineStations.xml";
        static string LineTripsFileName = "LineTrips.xml";
        static string UsersFileName = "Users.xml";
        static string UsersTripsFileName = "UsersTrips.xml";
        #endregion

        #region files paths
        static string BusesFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database", BusesFileName);
        static string AdjacentStationsFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database", AdjacentStationsFileName);
        static string BusesOnTripFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database", BusesOnTripFileName);
        static string StationsFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database", StationsFileName);
        static string LineStationsFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database", LineStationsFileName);
        static string LineTripsFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database", LineTripsFileName);
        static string UsersFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database", UsersFileName);
        static string UsersTripsFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database", UsersTripsFileName); 
        #endregion

        static XElement dsRoot;
        #region data Access
        public static IEnumerable<XElement> Buses
        {
            get
            {
                LoadData();
                return dsRoot.Element("Buses").Elements();
            }
        }
        public static IEnumerable<XElement> AdjacentStations
        {
            get
            {
                LoadData();
                return dsRoot.Element("AdjacentStations").Elements();
            }
        }
        public static IEnumerable<XElement> Lines
        {
            get
            {
                LoadData();
                return dsRoot.Element("Lines").Elements();
            }
        }
        public static IEnumerable<XElement> BusesOnTrip
        {
            get
            {
                LoadData();
                return dsRoot.Element("BusesOnTrip").Elements();
            }
        }
        public static IEnumerable<XElement> Stations
        {
            get
            {
                LoadData();
                return dsRoot.Element("Stations").Elements();
            }
        }
        public static IEnumerable<XElement> LineStations
        {
            get
            {
                LoadData();
                return dsRoot.Element("LineStations").Elements();
            }
        }
        public static IEnumerable<XElement> LineTrips
        {
            get
            {
                LoadData();
                return dsRoot.Element("LineTrips").Elements();
            }
        }
        public static IEnumerable<XElement> Users
        {
            get
            {
                LoadData();
                return dsRoot.Element("Users").Elements();
            }
        }
        public static IEnumerable<XElement> UsersTrips
        {
            get
            {
                LoadData();
                return dsRoot.Element("UsersTrips").Elements();
            }
        }
        #endregion
        public static int serialLineID;
        static string dataBasePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database");
        static readonly string FilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database", FileName);

        static DataSourceXML()
        {
            serialLineID = 0;
            List<string> filesPaths = new List<string>() { BusesFilePath, AdjacentStationsFilePath, BusesOnTripFilePath,
            StationsFilePath, LineStationsFilePath, LineTripsFilePath, UsersFilePath, UsersTripsFilePath};
            foreach (string filePath in filesPaths)
            {
                if (!File.Exists(filePath))//if the file don't exist
                {
                    (new XElement("elements")).Save(filePath);//create new file with root <elements>
                }
            }
            //serialLineID = 0;
            //if (!File.Exists(FilePath))//if the file don't exist
            //{
            //    CreateFile();
            //}
            //else
            //{
            //    LoadData();
            //}
        }

        private static void CreateFile()
        {
            //dsRoot = new XElement("DS");
            //dsRoot.Add(
            //         new XElement("Stations"),//add new label in wich the stations will be save
            //         new XElement("Lines"),//add new label in wich the lines will be save
            //         new XElement("users"),//...users...
            //         new XElement("AdjacentStations"),//...AdjacentStations...
            //         new XElement("BusesOnTrip"),//...BusesOnTrip...
            //         new XElement("LineStations"),//...LineStations...
            //         new XElement("LineTrips"),//...LineTrips...
            //         new XElement("Users"),//...Users...
            //         new XElement("UsersTrips")//...UsersTrips...
            //         );
            //string StationsPath = System.IO.Path.Combine(dataBasePath, "Stations.db");
            //using (SQLiteConnection stations = new SQLiteConnection(StationsPath))
            //{
            //    stations.CreateTable<Station>();
            //    Stations = stations.Table<Station>().ToList();
            //}
            //Users = new List<User>() { new User() { Name = "Admin", Password = "1234", Admin = true, IsActive = true } };
            //List<Station> newStationList = Stations.GroupBy(c => c.Code, (key, c) => c.FirstOrDefault()).ToList();
            //foreach (var st in newStationList)
            //{
            //    SaveObj(st, "Stations");
            //}
            //dsRoot.Save(FilePath);
        }

        public static XElement LoadData(string path = "")
        {
            try
            {
                return XElement.Load(path);
            }
            catch
            {
                throw new Exception($"{path} File upload problem");
            }
        }

        /// <summary>
        /// adding a new object to the DataSource's xml file
        /// </summary>
        /// <param name="obj">the object to add</param>
        /// <param name="lable">the label to add the new object to</param>
        public static void SaveObj(object obj, string lable)
        {
            if (obj == null)
            {
                return;
            }
            XElement newObj = new XElement(obj.GetType().Name);
            foreach (var prop in obj.GetType().GetProperties())
            {
                object temp = prop.GetValue(obj);

                if (temp != null)
                {
                    String val = temp != null ? temp.ToString() : "";
                    newObj.Add(new XElement(prop.Name, val));//insert new label <prop.Name> prop.GetValue(prop).ToString() </prop.Name>
                }
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
