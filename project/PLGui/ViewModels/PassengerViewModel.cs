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
using MaterialDesignThemes.Wpf;
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

        #region properties

        public BO.Passenger passenger { get; set; }

        public BO.LineStation FromStation
        {
            get => fromStation;
            set
            {
                if (SetProperty(ref fromStation, value) && value != null)//if the another station has selected in the combo box
                {
                    //show all the available stations that are next in the lines that passed in selected station("FromStation")
                    toStations = new ObservableCollection<BO.LineStation>(stations.Where(s => s.LineId == value.LineId && s.LineStationIndex > value.LineStationIndex));
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
                    if (ToStations != null)
                    {
                        ToStations.Filter = s => ((BO.LineStation)s).Name.Contains(value);
                        ToStations.Refresh(); 
                    }
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
        public ObservableCollection<Line> Lines { get; set; }
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
                            fromStations = new ObservableCollection<BO.LineStation>(stations.Where(s => s.LineStationIndex > 0));//take all the stations that aren't last in the line
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
        BackgroundWorker loadLinesWorker;
        private void loadLines()
        {
            if (loadLinesWorker == null)
            {
                loadLinesWorker = new BackgroundWorker();
                loadLinesWorker.RunWorkerCompleted +=
                    (object sender, RunWorkerCompletedEventArgs args) =>
                    {
                        if (!((BackgroundWorker)sender).CancellationPending)//if the BackgroundWorker didn't 
                        {                                                   //terminated befor he done execute DoWork
                            Lines = (ObservableCollection<Line>)args.Result;
                        }
                    };//this function will execute in the main thred

                loadLinesWorker.DoWork +=
                    (object sender, DoWorkEventArgs args) =>
                    {
                        BackgroundWorker worker = (BackgroundWorker)sender;
                        ObservableCollection<Line> result = new ObservableCollection<Line>();
                        result = new ObservableCollection<Line>(source.GetAllLines().Select(l => l.Line_BO_PO()));//get all lines from source
                        args.Result = worker.CancellationPending ? null : result;
                    };//this function will execute in the BackgroundWorker thread
            }
            if (!loadLinesWorker.IsBusy)//if the worker is not busy run immediately
            {
                loadLinesWorker.RunWorkerAsync();
            }

        }
        #endregion

        #region constructor
        public PassengerViewModel()
        {
            source = BLFactory.GetBL("passenger");
            loadStations();
            loadLineTrips();
            loadLines();

            //commands initialize
            WindowLoaded_Command = new RelayCommand<PassengerView>(Window_Loaded);
            LogOut_Command = new RelayCommand<PassengerView>(LogOut);
            ClosingCommand = new RelayCommand<Window>(Close);
            TripCommand = new RelayCommand(GoTrip);
        }

        #endregion

        #region commands and events
        public ICommand WindowLoaded_Command { get; }
        public ICommand LogOut_Command { get; }
        public ICommand ClosingCommand { get; }
        public ICommand TripCommand { get; }

        BackgroundWorker AddUserTripWorker;

        private void GoTrip()
        {
            Line line = Lines.Where(l => l.ID == FromStation.LineId).FirstOrDefault();
            BO.UserTrip newUserTrip = new BO.UserTrip()
            {
                UserName = passenger.Name,
                LineId = line.ID,
                LineNum = (int)line.LineNumber,
                InStation = FromStation.StationNumber,
                InStationName = FromStation.Name,
                InTime = DateTime.Today + TimeOfTrip.StartTime,
                OutStation = ToStation.StationNumber,
                OutStationName = ToStation.Name,
                OutTime = DateTime.Today + TimeOfTrip.FinishTime
            };

            AddUserTrip(newUserTrip);
            //clear the fileds
            FromText = "";
            ToText = "";
        }

        private void LogOut(PassengerView window)
        {
            new MainWindow().Show();
            if (window.IsActive)
            {
                window.Close();
            }
        }
        private void Close(Window window)
        {
            window.Close();
            Environment.Exit(Environment.ExitCode);
        }

        private void Window_Loaded(PassengerView window)
        {
            GetPassengerDetails();

            passengerView = window;

            passengerView.FromComboBox.DropDownOpened += ComboBox_DropDownOpened;
            passengerView.FromComboBox.PreviewTextInput += ComboBox_TextInput;
            passengerView.ToComboBox.DropDownOpened += ComboBox_DropDownOpened;
            passengerView.ToComboBox.PreviewTextInput += ComboBox_TextInput;
        }

        /// <summary>
        /// open the combo box while start typing
        /// </summary>
        private void ComboBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            (sender as ComboBox).IsDropDownOpen = true;
        }
        /// <summary>
        /// reset the collectionView filter when the combo box gets open
        /// </summary>
        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            FromStations.Filter = s => true;
            FromStations.Refresh();
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
                    int lineNum = (int)Lines.Where(l => l.ID == station.LineId).FirstOrDefault().LineNumber;
                    if (LT.Frequency == TimeSpan.Zero)//if the line trips is only once a day
                    {
                        tempTimeTrips.Add(new TimeTrip() { LineNum = lineNum, StartTime = LT.StartTime + station.Time_from_start });
                    }
                    else                               //the line trips is more then once a day
                    {
                        TimeSpan tempStart = LT.StartTime;
                        while (LT.FinishAt >= tempStart)
                        {
                            tempTimeTrips.Add(new TimeTrip() { LineNum = lineNum, StartTime = tempStart + station.Time_from_start });
                            tempStart += LT.Frequency;
                        }
                    }
                }
            }
            return tempTimeTrips;
        }
        /// <summary>
        /// send a requests massege to get the passenger details
        /// </summary>
        private void GetPassengerDetails()
        {
            try
            {
                passenger = WeakReferenceMessenger.Default.Send<RequestPassenger>();//requests the passenger
                OnPropertyChanged(nameof(passenger));
            }
            catch (Exception)
            {
                MessageBox.Show("your details and trips history is unavailable! \nyou probably enter by maneger account, please switch to passenger account"
                   , "ERROR", new MessageBoxButton());
            }
        }

        private void AddUserTrip(BO.UserTrip userTrip)
        {
            if (AddUserTripWorker == null)
            {
                AddUserTripWorker = new BackgroundWorker();
                AddUserTripWorker.RunWorkerCompleted +=
                    (object sender, RunWorkerCompletedEventArgs args) =>
                    {
                        passenger = (BO.Passenger)args.Result;
                        OnPropertyChanged(nameof(passenger));
                    };//this function will execute in the main thred

                AddUserTripWorker.DoWork +=
                    (object sender, DoWorkEventArgs args) =>
                    {
                        source.AddUserTrip((BO.UserTrip)args.Argument);     //add the user trip
                        args.Result = source.GetPassenger(passenger.Name, passenger.Password);//get the updated passenger
                    };//this function will execute in the BackgroundWorker thread
            }
            if (!AddUserTripWorker.IsBusy)//if the worker is not busy run immediately
            {
                AddUserTripWorker.RunWorkerAsync(userTrip);
            }
        }
        #endregion
    }
}
