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
        static XElement adjacentStationsRoot;
        static XElement linesRoot;
        static XElement busesOnTripRoot;
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
                adjacentStationsRoot = XElement.Load(AdjacentStationsFilePath);
                return adjacentStationsRoot;
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
                busesOnTripRoot = XElement.Load(BusesOnTripFilePath);
                return busesOnTripRoot;
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

        static Dictionary<string, dynamic> files = new Dictionary<string, dynamic> {
            ["Buses"] = {isChanged = false , root = busesRoot, path = BusesFilePath},
            ["AdjacentStations"] = { isChanged = false, root = adjacentStationsRoot, path = AdjacentStationsFilePath },
            ["Lines"] = { isChanged = false, root = linesRoot, path = LinesFilePath },
            ["BusesOnTrip"] = { isChanged = false, root = busesOnTripRoot, path = BusesFilePath },
            ["Stations"] = { isChanged = false, root = StationsRoot, path = StationsFilePath }, 
            ["LineStations"] = { isChanged = false, root = LineStationsRoot, path = LineStationsFilePath },
            ["LineTrips"] = { isChanged = false, root = LineTripsRoot, path = LineTripsFilePath },
            ["Users"] = { isChanged = false, root = UsersRoot, path = UsersFilePath },
            ["UsersTrips"] = { isChanged = false, root = UsersTripsRoot, path = UsersTripsFilePath }
        };

        public static int serialLineID;
        static string dataBasePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database");
        static readonly string FilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), "DL", "DS", "Database", FileName);

        static DataSourceXML()
        {
            busesRoot.Changed += (object sender, XObjectChangeEventArgs e) => files["Buses"].isChanged = true;//if the xelement is changed then set IsChenged[BusesFilePath] to true
            adjacentStationsRoot.Changed += (object sender, XObjectChangeEventArgs e) => files["AdjacentStations"].isChanged = true;//...IsChenged[AdjacentStationsFilePath]...
            linesRoot.Changed += (object sender, XObjectChangeEventArgs e) => files["Lines"].isChanged = true;//...IsChenged[LinesFilePath]...
            busesOnTripRoot.Changed += (object sender, XObjectChangeEventArgs e) => files["BusesOnTrip"].isChanged = true;//...IsChenged[BusesOnTripFilePath]...
            StationsRoot.Changed += (object sender, XObjectChangeEventArgs e) => files["Stations"].isChanged = true;//...IsChenged[StationsFilePath]...
            LineStationsRoot.Changed += (object sender, XObjectChangeEventArgs e) => files["LineStations"].isChanged = true;//...IsChenged[LineStationsFilePath]...
            LineTripsRoot.Changed += (object sender, XObjectChangeEventArgs e) => files["LineTrips"].isChanged = true;//...IsChenged[LineTripsFilePath]...
            UsersRoot.Changed += (object sender, XObjectChangeEventArgs e) => files["Users"].isChanged = true;//...IsChenged[UsersFilePath]...
            UsersTrips.Changed += (object sender, XObjectChangeEventArgs e) => files["UsersTrips"].isChanged = true;//...IsChenged[UsersTripsFilePath]...

            serialLineID = 0;
            foreach (var file in files)
            {
                if (!File.Exists(file.Value.path))//if the file don't exist
                {
                    file.Value.root = new XElement(file.Key);//create new file with root <elements>
                    file.Value.root.Save(file.Value.path);
                }
                else
                {
                    file.Value.root = XElement.Load(file.Value.path);
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

        static public void Save()
        {
            foreach(var file in files)
            {
                if(file.Value.isChanged)
                {
                    file.Value.root.Save(file.Value.path);
                    file.Value.isChanged = false;
                }
            }
        }

        public static void SaveListSerializer<T>(this List<T> list, string typename)
        {
            XMLTools.SaveListToXMLSerializer<T>(list, files[typename].path);//save the list to the file that accur in files as the file of this intety
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
