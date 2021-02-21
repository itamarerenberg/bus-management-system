using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DLXML
{
    
    /// <summary>
    /// this class provids a Serials IDs for DL for Line and UserTrip
    /// </summary>
    public class SerialNumbers
    {
        const int tryAginIn = 10;
        const int tryNtimes = 10;

        static XElement Root;
        static string SerialIDPath = @"SerialNumbers.xml";
        public static int GetLineId { 
            get 
            {
                LoadData();
                int id = int.Parse(Root.Element("LineId").Value);
                Root.Element("LineId").Value = (id + 1).ToString();
                Save();
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
                Save();
                return id;
            }
        }

        public static int GetLineTripId
        {
            get
            {
                LoadData();
                int id = int.Parse(Root.Element("LineTripId").Value);
                Root.Element("LineTripId").Value = (id + 1).ToString();
                Save();
                return id;
            }
        }

        public static int GetBusTripId
        { 
            get
            {
                LoadData();
                int id = int.Parse(Root.Element("LineBusId").Value);
                Root.Element("LineBusId").Value = (id + 1).ToString();
                Save();
                return id;
            }
        }

        static SerialNumbers()
        {
            if (!File.Exists(SerialIDPath))//case there isn't a file of the serials IDs yet 
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
                for (int i = 0; i < tryNtimes; i++)
                {
                    try
                    {
                        if (Root != null)
                            Root.Save(SerialIDPath);
                        Root = XElement.Load(SerialIDPath);
                        break;//if no exeption was throwed then stop try
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(tryAginIn);
                    }
                }
            }
            catch
            {
                throw new Exception("File upload problem");
            }

        }

        private static void Save()
        {
            try
            {
                for (int i = 0; i < tryNtimes; i++)
                {
                    try
                    {
                        if (Root != null)
                            Root.Save(SerialIDPath);
                        break;//if no exeption was throwed then stop try
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(tryAginIn);
                    }
                }
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }
    }
}
