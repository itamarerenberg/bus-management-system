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

        private BO.LineTrip bOlineTrip = new BO.LineTrip();
        public BO.LineTrip BOlineTrip {
            get => bOlineTrip;
            set
            {
                SetProperty(ref bOlineTrip, value, true);
                OnPropertyChanged(nameof(ID));
                OnPropertyChanged(nameof(LineId));
                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(Frequency));
                OnPropertyChanged(nameof(Finish));
            }
        }

        public int ID 
        { 
            get => bOlineTrip.ID;
            set
            {
                int temp = bOlineTrip.ID;
                SetProperty(ref temp, value, true);
                bOlineTrip.ID = temp;
            }
        }
        public int LineId
        { 
            get => bOlineTrip.LineId;
            set
            {
                int temp = bOlineTrip.LineId;
                SetProperty(ref temp, value, true);
                bOlineTrip.LineId = temp;
            }
        }
        public TimeSpan StartTime
        {
            get => bOlineTrip.StartTime;
            set
            {
                TimeSpan temp = bOlineTrip.StartTime;
                SetProperty(ref temp, value, true);
                bOlineTrip.StartTime = temp;
            }
        }
        public TimeSpan Frequency
        {
            get => bOlineTrip.Frequency;
            set
            {
                TimeSpan temp = bOlineTrip.Frequency;
                SetProperty(ref temp, value, true);
                bOlineTrip.Frequency = temp;
            }
        }
        public TimeSpan Finish
        { 
            get => bOlineTrip.FinishAt; 
            set
            {
                TimeSpan temp = bOlineTrip.FinishAt;
                SetProperty(ref temp, value, true);
                bOlineTrip.FinishAt = temp;
            }
        }
    }
}
