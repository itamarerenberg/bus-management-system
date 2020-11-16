using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_1038_0685
{
    public enum Areas
    {
        General,North,South,Center,Jerusalem
    }
    public enum Options
    {
        ADD_BUS_LINE, ADD_STATION, DELETE_BUS_LINE, DELETE_STATION
            , LINES_STOPPING_AT_THE_STATION, FIND_RIDE_BETWEEN_2_STATIONS, PRINT_ALL_LINES, 
        PRINT_ALL_STATIONS, EXIT = -1
    }
}
