using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DLXML
{
    /// <summary>
    /// this class provids a Serials IDs for DL for Line and UserTrip
    /// </summary>
    public class SerialNumbers
    {
        static XElement Root;
        static string SerialIDPath = @"SerialNumbers.xml";

        public static int GetLineId { 
            get 
            {
                LoadData();
                int id = int.Parse(Root.Element("LineId").Value);
                Root.Element("LineId").Value = (id + 1).ToString();
                Root.Save(SerialIDPath);
                return id;
            }
        }

        public static int GetUserTripId
        {
            get
            {
                LoadData();
                int id = int.Parse(Root.Element("UserTripId").Value);
                Root.Element("UserTripId").Value = (id + 1).ToString();
                Root.Save(SerialIDPath);
                return id;
            }
        }

        static SerialNumbers()
        {
            if (!File.Exists(SerialIDPath))//case ther isn't a file of the serials IDs yet 
                    CreateFile();
                else
                    LoadData();
        }

        private static void CreateFile()
        {
            Root = new XElement("IDs");
            Root.Add(
                     new XElement("LineId", 0),
                     new XElement("UserTripId", 0)
                     );
            Root.Save(SerialIDPath);
        }

        private static void LoadData()
        {
            try
            {
                Root = XElement.Load(SerialIDPath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }
    }
}
