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
        /// <summary>
        /// generating new DO Adjacent Stations
        /// </summary>
        /// <param name="stationCode1"></param>
        /// <param name="stationCode2"></param>
        /// <returns>true: if success.  false: if any of the given parameters is null</returns>
        public static bool AddAdjacentStations(int? stationCode1, int? stationCode2)
        {
            if (stationCode1 == null || stationCode2 == null)
                return false;

            //getting the DO stations
            DO.Station tempS1 = dl.GetStation((int)stationCode1);
            DO.Station tempS2 = dl.GetStation((int)stationCode2);

            //getting the locations by Latitude and Longitude of the stations
            var locationStation1 = new GeoCoordinate(tempS1.Latitude, tempS1.Longitude);
            var locationStation2 = new GeoCoordinate(tempS2.Latitude, tempS2.Longitude);

            //calculating the distance between the stations
            double distance = locationStation1.GetDistanceTo(locationStation2);//distance in meters

            //generating DO Adjacent Stations
            DO.AdjacentStations adjacentStationsDO = new DO.AdjacentStations()
            {
                StationCode1 = (int)stationCode1,
                StationCode2 = (int)stationCode2,
                Distance = distance
                //'Time = צריך להוסיף
            };
            dl.AddAdjacentStations(adjacentStationsDO);
            return true;
        }
        public static AdjacentStations GetAdjacentStations(int? stationCode1,int? stationCode2)
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
        public static bool DeleteAdjacentStations(AdjacentStations adjacentStations)
        {
            return dl.DeleteAdjacentStations(adjacentStations.StationCode1, adjacentStations.StationCode2);
        }
        #endregion

        #region Line station

        public static LineStation GetLineStation(int lineId, int stationNum)
        {
            DO.LineStation lineStationDO = dl.GetLineStation(lineId, stationNum);
            LineStation lineStation = (LineStation)lineStationDO.CopyPropertiesToNew(typeof(LineStation));

            lineStation.PrevToCurrent = GetAdjacentStations(lineStationDO.PrevStation, stationNum);
            lineStation.CurrentToNext = GetAdjacentStations(stationNum, lineStationDO.NextStation);

            return lineStation;
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
