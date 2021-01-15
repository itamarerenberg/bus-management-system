using DL;
using DO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DLXML
{
    static class DataSourceXML
    {
        static string FileName = "DataSource.xml";

        #region files Names
        static string LinesFileName = "Lines.xml";
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
        static string LinesFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database", LinesFileName);
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
        #region files roots
        static XElement busesRoot;
        static XElement AdjacentStationsRoot;
        static XElement linesRoot;
        static XElement BusesOnTripRoot;
        static XElement StationsRoot;
        static XElement LineStationsRoot;
        static XElement LineTripsRoot;
        static XElement UsersRoot;
        static XElement UsersTripsRoot;
        #endregion

        #region data acces properties


        public static XElement Buses
        {
            get
            {
                busesRoot = XElement.Load(BusesFilePath);
                return busesRoot;
            }
        }
        public static XElement AdjacentStations
        {
            get
            {
                AdjacentStationsRoot = XElement.Load(AdjacentStationsFilePath);
                return AdjacentStationsRoot;
            }
        }
        public static XElement Lines
        {
            get
            {
                linesRoot = XElement.Load(LinesFilePath);
                return linesRoot;
            }
        }
        public static XElement BusesOnTrip
        {
            get
            {
                BusesOnTripRoot = XElement.Load(BusesOnTripFilePath);
                return BusesOnTripRoot;
            }
        }
        public static XElement Stations
        {
            get
            {
                StationsRoot = XElement.Load(StationsFilePath);
                return StationsRoot;
            }
        }
        public static XElement LineStations
        {
            get
            {
                LineStationsRoot = XElement.Load(LineStationsFilePath);
                return LineStationsRoot;
            }
        }
        public static XElement LineTrips
        {
            get
            {
                LineTripsRoot = XElement.Load(LineTripsFilePath);
                return LineTripsRoot;
            }
        }
        public static XElement Users
        {
            get
            {
                UsersRoot = XElement.Load(UsersFilePath);
                return UsersRoot;
            }
        }
        public static XElement UsersTrips
        {
            get
            {
                UsersTripsRoot = XElement.Load(UsersTripsFilePath);
                return UsersTripsRoot;
            }
        } 
        #endregion
        #endregion

        static Dictionary<string, bool> IsChenged = new Dictionary<string, bool> {
            [BusesFilePath] = false,
            [AdjacentStationsFilePath] = false,
            [LinesFilePath] = false,
            [BusesOnTripFilePath] = false,
            [StationsFilePath] = false,
            [LineStationsFilePath] = false,
            [LineTripsFilePath] = false,
            [UsersFilePath] = false,
            [UsersTripsFilePath] = false
        };

        public static int serialLineID;
        static string dataBasePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database");
        static readonly string FilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database", FileName);

        static DataSourceXML()
        {
            busesRoot.Changed += (object sender, XObjectChangeEventArgs e) => IsChenged[BusesFilePath] = true;
            serialLineID = 0;
            List<string> filesPaths = new List<string>() { LinesFileName, BusesFilePath, AdjacentStationsFilePath, BusesOnTripFilePath,
            StationsFilePath, LineStationsFilePath, LineTripsFilePath, UsersFilePath, UsersTripsFilePath};
            foreach (string filePath in filesPaths)
            {
                if (!File.Exists(filePath))//if the file don't exist
                {
                    (new XElement("elements")).Save(filePath);//create new file with root <elements>
                }
            }
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


      public static void SaveListSerializer<T>(this List<T> list, string typename)
        {
            switch (typename)
            {
                case "Lines":
                    XMLTools.SaveListToXMLSerializer<T>(list, "Lines.xml");
                    break;
                default:
                    break;
            }
            

        }
        public static void SaveList(this XElement root,string typename)
        {
            switch (typename)
            {
                case "Lines":
                    root.Save(LinesFilePath);
                    break;
                default:
                    break;
            }
            //dsRoot.Save(path);
            throw new NotImplementedException("DataSourceXML.Save");
        }
    }
}
