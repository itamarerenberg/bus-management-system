using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace correctDataset
{
    class Program
    {
        static void Main(string[] args)
        {
            string lineStationsFilePath = @"C:\Users\erenb\Source\Repos\dotNet5781_1038_06852\project\DL\DS\DataBase\LineStations.xml";
            string stationFilePath = @"C:\Users\erenb\Source\Repos\dotNet5781_1038_06852\project\DL\DS\DataBase\Stations.xml";
            XElement lineStations;
            XElement stations;
            try
            {
                lineStations = XElement.Load(lineStationsFilePath);
                stations = XElement.Load(stationFilePath);
            }
            catch (Exception)
            {
                throw;
            }
            foreach (var lineStation in lineStations.Elements())
            {
                if(lineStation.Element("Name") == null)
                {
                    string lsName = (from st in stations.Elements()
                                     where st.Element("Code").Value == lineStation.Element("StationNumber").Value
                                     select st.Element("Name").Value).FirstOrDefault();
                    lineStation.Add(new XElement("Name", lsName));
                }
            }
            lineStations.Save(lineStationsFilePath);
        }
    }
}
