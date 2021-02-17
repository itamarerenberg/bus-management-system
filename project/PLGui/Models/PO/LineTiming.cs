using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLGui.Models.PO
{
    public class LineTiming : ObservableObject
    {
        BO.LineTiming boLineTiming;
        public BO.LineTiming BoLineTiming 
        {
            get => boLineTiming;
            set
            {
                SetProperty(ref boLineTiming, value);
                OnPropertyChanged(nameof(ArrivalTime));//all the proprties dipens on this property
            }
        }

        /// <summary>
        /// read only
        /// </summary>
        public int LineId 
        {
            get => boLineTiming.LineId;
        }

        /// <summary>
        /// read only
        /// </summary>
        public int LineNum {
            get => boLineTiming.LineNum;
        }

        /// <summary>
        /// read only
        /// </summary>
        public TimeSpan StartTime 
        {
            get => boLineTiming.StartTime;
        }

        /// <summary>
        /// read only
        /// </summary>
        public string LastStation 
        {
            get => boLineTiming.LastStation;
        }

        /// <summary>
        /// read only
        /// </summary>
        public int StationCode 
        {
            get => boLineTiming.StationCode;
        }

        /// <summary>
        /// read only
        /// </summary>
        public TimeSpan ArrivalTime 
        {
            get => boLineTiming.ArrivalTime;
        }
    }
}
