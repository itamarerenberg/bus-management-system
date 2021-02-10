using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using PLGui.Models.PO;

namespace PLGui.utilities
{
    public class RequestLine : RequestMessage<Line>
    {        
    }
    public class RequestStation : RequestMessage<Station>
    {
    }
    public class RequestLineTrip : RequestMessage<LineTrip>
    {
    }
    public class RequestPassenger : RequestMessage<BO.Passenger>
    {
    }
}
