using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BL.BLApi;
using BLApi;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using PLGui.Models.PO;

namespace PLGui
{
    public class PassengerViewModel : ObservableRecipient
    {

        readonly IBL source;

        private BO.LineStation fromStation;
        private BO.LineStation toStation;

        #region constructor
        public PassengerViewModel()
        {
            source = BLFactory.GetBL("passenger");
            loadStations();
            loadLineTrips();

            //commands initialize
            //SearchCommand = new RelayCommand<Window>(SearchBox_TextChanged);

        }
        #endregion

        #region collections and properties

        private List<BO.LineTrip> LineTrips { get; set; }
        public BO.LineStation FromStation
        {
            get => fromStation;
            set
            {
                if (SetProperty(ref fromStation, value) && value != null)//if the another station has selected in the combo box
                {
                    //show all the available stations that are next in the lines that passed in selected station("FromStation")
                    toStations = new ObservableCollection<BO.LineStation>(stations.Where(s => s.LineId == value.LineId && s.Time_from_start < value.Time_from_start));
                    DepartureTimes = CalculatesTime(value);
                }
            }
        }

        public BO.LineStation ToStation
        {
            get => toStation;
            set
            {
                if (SetProperty(ref toStation, value))//if the another station has selected in the combo box
                {
                    if (TimeOfTrip != null && value != null)
                    {
                        TimeOfTrip.FinishTime = TimeOfTrip.StartTime + (value.Time_from_start - FromStation.Time_from_start);//FinishTime = start time + the diferent between the stations
                    }                
                }
            }
        }
 

        private string fromText;

        public string FromText
        {
            get => fromText;
            set
            {
                if (SetProperty(ref fromText, value))//if the text has changed
                {
                    FromStations.Filter = s => ((BO.LineStation)s).Name.Contains(value);
                    FromStations.Refresh();
                }
            }
        }

        private string toText;

        public string ToText
        {
            get => toText;
            set
            {
                if (SetProperty(ref toText, value))//if the text has changed
                {
                    ToStations.Filter = s => ((BO.LineStation)s).Name.Contains(value);
                    ToStations.Refresh();
                }
            }
        }
        private TimeTrip timeOfTrip;

        public TimeTrip TimeOfTrip
        {
            get => timeOfTrip;
            set
            {
                if (SetProperty(ref timeOfTrip, value) && value != null)
                {
                    if (ToStation != null)
                    {
                        TimeOfTrip.FinishTime = TimeOfTrip.StartTime + (ToStation.Time_from_start - FromStation.Time_from_start);//FinishTime = start time + the diferent between the stations
                        OnPropertyChanged(nameof(TimeOfTrip));
                    }
                }
            }
        }


        private List<TimeTrip> departureTimes;

        public List<TimeTrip> DepartureTimes
        {
            get => departureTimes;
            set => SetProperty(ref departureTimes, value);
        }

        private ObservableCollection<BO.LineStation> _stations;

        private ObservableCollection<BO.LineStation> stations
        {
            get => _stations;
            set => SetProperty(ref _stations, value);
        }
        
        private ObservableCollection<BO.LineStation> _fromStations;

        private ObservableCollection<BO.LineStation> fromStations
        {
            get => _fromStations;
            set => SetProperty(ref _fromStations, value);
        }
        public ICollectionView FromStations
        {
            get { return CollectionViewSource.GetDefaultView(fromStations); }
        }


        private ObservableCollection<BO.LineStation> _toStations;

        private ObservableCollection<BO.LineStation> toStations
        {
            get => _toStations;
            set => SetProperty(ref _toStations, value);
        }
        public ICollectionView ToStations
        {
            get { return CollectionViewSource.GetDefaultView(toStations); }
        }
        #endregion

        #region load data

        BackgroundWorker loadStationWorker;
        private void loadStations()
        {
            if (loadStationWorker == null)
            {
                loadStationWorker = new BackgroundWorker();
                loadStationWorker.RunWorkerCompleted +=
                    (object sender, RunWorkerCompletedEventArgs args) =>
                    {
                        if (!((BackgroundWorker)sender).CancellationPending)//if the BackgroundWorker didn't 
                        {                                                   //terminated befor he done execute DoWork
                            stations = (ObservableCollection<BO.LineStation>)args.Result;
                            fromStations = new ObservableCollection<BO.LineStation>(stations.Where(s => s.CurrentToNext != null));//take all the stations that aren't last in the line
                        }
                    };//this function will execute in the main thred

                loadStationWorker.DoWork +=
                    (object sender, DoWorkEventArgs args) =>
                    {
                        BackgroundWorker worker = (BackgroundWorker)sender;
                        ObservableCollection<BO.LineStation> result = new ObservableCollection<BO.LineStation>(source.GetAllLineStations().OrderBy(s => s.Name));//get all Stations from source
                        args.Result = worker.CancellationPending ? null : result;
                    };//this function will execute in the BackgroundWorker thread
            }
            if (!loadStationWorker.IsBusy)//if the worker is not busy run immediately
            {
                loadStationWorker.RunWorkerAsync();
            }
        }
        BackgroundWorker loadLineTripesWorker;
        private void loadLineTrips()
        {
            if (loadLineTripesWorker == null)
            {
                loadLineTripesWorker = new BackgroundWorker();

                loadLineTripesWorker.RunWorkerCompleted +=
                    (object sender, RunWorkerCompletedEventArgs args) =>
                    {
                        if (!((BackgroundWorker)sender).CancellationPending)//if the BackgroundWorker didn't 
                        {                                                   //terminated befor he done execute DoWork
                            LineTrips = (List<BO.LineTrip>)args.Result;
                        }

                    };//this function will execute in the main thred

                loadLineTripesWorker.DoWork +=
                    (object sender, DoWorkEventArgs args) =>
                    {
                        BackgroundWorker worker = (BackgroundWorker)sender;
                        List<BO.LineTrip> result = new List<BO.LineTrip>(source.GetAllLineTrips());//get all line trips from source
                        //List<LineTrip> result = new List<LineTrip>(source.GetAllLineTrips().Select(lineTrip => new LineTrip() { BOlineTrip = lineTrip }));//get all line trips from source
                        args.Result = worker.CancellationPending ? null : result;
                    };//this function will execute in the BackgroundWorker thread
            }
            if (!loadLineTripesWorker.IsBusy)//if the worker is not busy run 
            {
                loadLineTripesWorker.RunWorkerAsync();
            }
            #endregion
        }
        /// <summary>
        /// Calculates the Times of Departure Lines from the selected station
        /// </summary>
        /// <param name="station"></param>
        /// <returns>
        /// List of TimeTrips that contains "StartTime" and "LineId"
        /// </returns>
        private List<TimeTrip> CalculatesTime(BO.LineStation station)
        {
            List<TimeTrip> tempTimeTrips = new List<TimeTrip>();
            foreach (BO.LineTrip LT in LineTrips)
            {
                if (LT.LineId == station.LineId)
                {
                    if (LT.Frequency == TimeSpan.Zero)//if the line trips is only once a day
                    {
                        tempTimeTrips.Add(new TimeTrip() { LineId = LT.LineId, StartTime = LT.StartTime + station.Time_from_start });
                    }
                    else                               //the line trips is more then once a day
                    {
                        TimeSpan tempStart = LT.StartTime;
                        while (LT.FinishAt >= tempStart)
                        {
                            tempTimeTrips.Add(new TimeTrip() { LineId = LT.LineId, StartTime = tempStart + station.Time_from_start });
                            tempStart += LT.Frequency;
                        }
                    }
                }
            }
            return tempTimeTrips;
        }
    }
}
