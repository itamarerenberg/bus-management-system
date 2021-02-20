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

        public static int GetLineTripId
        {
            get
            {
                LoadData();
                int id = int.Parse(Root.Element("LineTripId").Value);
                Root.Element("LineTripId").Value = (id + 1).ToString();
                Root.Save(SerialIDPath);
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
                Root.Save(SerialIDPath);
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
            /*
             * string index = "Buses";
                try
                {
                    for (int i = 0; i < tryNtimes; i++)
                    {
                        try
                        {
                            if (files[index].Root != null)
                                files[index].Root.Save(files[index].Path);
                            files[index].Root = XElement.Load(files[index].Path);
                        }
                        catch (IOException)
                        {
                            Thread.Sleep(tryAginIn);
                        }
                    }
                }
                catch (IOException)
                {
                    throw new FileLoadException(files[index].Path);
                }
                return files[index].Root;
             */
            try
            {
                for (int i = 0; i < tryNtimes; i++)
                {
                    try
                    {
                        if (Root != null)
                            Root.Save(SerialIDPath);
                        Root = XElement.Load(SerialIDPath);
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
