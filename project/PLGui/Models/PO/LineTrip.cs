using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace PLGui.Models.PO
{
    public class LineTrip : ObservableValidator
    {
        private int id;
        private string lineId;
        private TimeSpan startTime;
        private TimeSpan frequency;
        private TimeSpan finishAt;

        public int ID 
        { 
            get => id;
            set
            {
                SetProperty(ref id, value, true);
            }
        }
        public string LineId
        { 
            get => lineId;
            set
            {
                SetProperty(ref lineId, value, true);
            }
        }
        public TimeSpan StartTime
        {
            get => startTime;
            set
            {
                SetProperty(ref startTime, value, true);
            }
        }
        public TimeSpan Frequency
        {
            get => frequency;
            set
            {
                SetProperty(ref frequency, value, true);
            }
        }
        public TimeSpan FinishAt
        { 
            get => finishAt; 
            set
            {
                SetProperty(ref finishAt, value, true);
            }
        }
    }
}
