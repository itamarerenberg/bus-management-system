using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BL.BLApi;
using BLApi;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using PLGui.Models.PO;
using PLGui.utilities;

namespace PLGui
{
    public class PassengerViewModel : ObservableRecipient
    {

        readonly IBL source;

        private BO.LineStation fromStation;
        private BO.LineStation toStation;
        private string fromText;
        private string toText;
        private TimeTrip timeOfTrip;
        private PassengerView passengerView;
        private BO.Passenger passenger;

        #region properties

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
        public string FromText
        {
            get => fromText;
            set
            {
                if (SetProperty(ref fromText, value))//if the text has changed
                {
                    value = value ?? "";        //preventing from value to be null
                    FromStations.Filter = s => ((BO.LineStation)s).Name.Contains(value);
                    FromStations.Refresh();
                }
            }
        }
        public string ToText
        {
            get => toText;
            set
            {
                if (SetProperty(ref toText, value))//if the text has changed
                {
                    value = value ?? "";        //preventing from value to be null
                    ToStations.Filter = s => ((BO.LineStation)s).Name.Contains(value);
                    ToStations.Refresh();
                }
            }
        }
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
        

        #endregion

        #region collections
        private List<BO.LineTrip> LineTrips { get; set; }

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
        }
        #endregion

        #region constructor
        public PassengerViewModel()
        {
            source = BLFactory.GetBL("passenger");
            loadStations();
            loadLineTrips();

            //commands initialize
            WindowLoaded_Command = new RelayCommand<PassengerView>(Window_Loaded);

        }
        #endregion

        #region commands and events
        public ICommand WindowLoaded_Command { get; }

        private void Window_Loaded(PassengerView window)
        {

            try
            {
                passenger = WeakReferenceMessenger.Default.Send<RequestPassenger>();//requests the passenger
            }
            catch (Exception)
            {
                MessageBox.Show("your details and trips history is unavailable! \nplease login again with your correct name and password" +
                    "\n  ובתרגום לעיברית: אם נכנסת דרך כפתור הדיבאג, לא תופיע היסטוריית הנסיעות, כי הוא לא קיבל passenger ", "ERROR");
            }

            passengerView = window;

            passengerView.FromComboBox.DropDownOpened += ComboBox_DropDownOpened;
            passengerView.FromComboBox.TextInput += ComboBox_TextInput;
            passengerView.ToComboBox.DropDownOpened += ComboBox_DropDownOpened;
            passengerView.ToComboBox.TextInput += ComboBox_TextInput;
        }
        /// <summary>
        /// open the combo box whill typing
        /// </summary>
        private void ComboBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            (sender as ComboBox).IsDropDownOpen = true;
        }
        /// <summary>
        /// clear the text when combo box get open(for reset the collectionView filter)
        /// </summary>
        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex != -1)
            {
                (sender as ComboBox).Text = null; 
            }
        }

        
        #endregion

        #region private nethods
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
        #endregion
    }
}
