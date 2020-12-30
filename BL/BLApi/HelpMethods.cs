using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DLApi;
using System.Device.Location;

namespace BL.BLApi
{
    public static class HelpMethods
    {
        static IDL dl = DLFactory.GetDL();

        #region AdjacentStations
        //void AddAdjacentStations(AdjacentStations adjacentStations);
        public static AdjacentStations GetAdjacentStation(int? stationCode1,int? stationCode2)
        {
            try
            {
                AdjacentStations adjacentStations = (AdjacentStations)dl.GetAdjacentStation(stationCode1,stationCode2).CopyPropertiesToNew(typeof(AdjacentStations));

                //get DO stations
                DO.Station tempS1 = dl.GetStation(adjacentStations.StationCode1);
                DO.Station tempS2 = dl.GetStation(adjacentStations.StationCode2);

                //get the locations by Latitude and Longitude of the station
                var locationStation1 = new GeoCoordinate(tempS1.Latitude, tempS1.Longitude);
                var locationStation2 = new GeoCoordinate(tempS2.Latitude, tempS2.Longitude);

                //calculating the distance between the stations
                adjacentStations.Distance = locationStation1.GetDistanceTo(locationStation2);//distance in meters

                return adjacentStations;
            }
            catch (Exception msg)
            {
                throw msg.InnerException;
            }
        }
        
        //void UpdateAdjacentStations(AdjacentStations adjacentStations);
        //void DeleteAdjacentStations(AdjacentStations adjacentStations);
        #endregion

        #region Line station

        public static LineStation GetLineStation(int lineId, int stationNum)
        {
            LineStation lineStationBO = (LineStation)dl.GetLineStation(lineId, stationNum).CopyPropertiesToNew(typeof(LineStation));

            lineStationBO.PrevToCurrent = GetAdjacentStation(lineStationBO.PrevStationCode, stationNum);
            lineStationBO.CurrentToNext = GetAdjacentStation(stationNum, lineStationBO.NextStationCode);

            return lineStationBO;
        }
        public static IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> pred)
        {
            try
            {
                return from lineStationDO in dl.GetAllLineStations()
                       let lineStationBO = GetLineStation(lineStationDO.LineId,lineStationDO.StationNumber)
                       where pred(lineStationBO)
                       select lineStationBO;
            }
            catch (Exception msg)
            {
                throw msg.InnerException;
            }
        }
        #endregion
    }
}
