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

        private PassengerStation fromPassengerStation;
        private PassengerStation toStation;
        private string fromText;
        private string toText;
        private BO.TimeTrip timeOfTrip;
        private PassengerView passengerView;

        #region properties

        public BO.Passenger passenger { get; set; }

        public PassengerStation FromPassengerStation
        {
            get => fromPassengerStation;
            set
            {
                if (SetProperty(ref fromPassengerStation, value) && value != null)//if the another station has selected in the combo box
                {
                    toStations = GetToStations(value.LineStations);
                    CalculatesTime(value);
                    OnPropertyChanged(nameof(ReadyToGo));
                }
            }
        }

        public BO.LineStation FromStation { get; set; }

        public PassengerStation ToStation
        {
            get => toStation;
            set
            {
                if (SetProperty(ref toStation, value) && value != null)//if the another station has selected in the combo box
                {
                    FromStation = FromPassengerStation.LineStations.Find(s => s.LineId == value.LineStations.First().LineId);//select the fromStation from the list of passengerStation according the line id
                    if (TimeOfTrip != null && value != null)
                    {
                        TimeOfTrip.FinishTime = TimeOfTrip.StartTime + (value.LineStations.First().Time_from_start - FromStation.Time_from_start);//FinishTime = start time + the diferent between the stations
                    }
                    OnPropertyChanged(nameof(ReadyToGo));
                    OnPropertyChanged(nameof(TimeOfTrip));
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
                    FromStations.Filter = s => ((PassengerStation)s).ToString().Contains(value);//filter all the stations that contains letters of "fromText"
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
                        ToStations.Filter = s => ((PassengerStation)s).ToString().Contains(value);
                        ToStations.Refresh(); 
                    }
                }
            }
        }
        public BO.TimeTrip TimeOfTrip
        {
            get => timeOfTrip;
            set
            {
                if (SetProperty(ref timeOfTrip, value) && value != null)
                {
                    if (ToStation != null)
                    {
                        TimeOfTrip.FinishTime = TimeOfTrip.StartTime + (ToStation.LineStations.First().Time_from_start - 
                            FromPassengerStation.LineStations.Find(s => s.LineId == ToStation.LineStations.First().LineId).Time_from_start);//FinishTime = start time + the diferent between the stations
                        OnPropertyChanged(nameof(TimeOfTrip));
                    }
                    OnPropertyChanged(nameof(ReadyToGo));
                }
            }
        }
        public bool ReadyToGo { get => FromPassengerStation != null && ToStation != null && TimeOfTrip != null; }
  
        #endregion

        #region collections
        public ObservableCollection<Line> Lines { get; set; }
        private List<BO.LineTrip> LineTrips { get; set; }

        private List<BO.TimeTrip> departureTimes;

        public List<BO.TimeTrip> DepartureTimes
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

        private ObservableCollection<PassengerStation> _fromStations;

        private ObservableCollection<PassengerStation> fromStations
        {
            get => _fromStations;
            set => SetProperty(ref _fromStations, value);
        }
        public ICollectionView FromStations
        {
            get { return CollectionViewSource.GetDefaultView(fromStations); }
        }


        private ObservableCollection<PassengerStation> _toStations;

        private ObservableCollection<PassengerStation> toStations
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
                            fromStations = new ObservableCollection<PassengerStation>(stations.GroupBy(s => s.StationNumber)//group into <station number, list of line stations>
                                            .Where(p => p.Any(t => t.CurrentToNext != null))// make sure at least 1 line station is not in the end
                                            .Select(ps => new PassengerStation() { LineStations = ps.ToList() }));//creats passenger station
                        }
                    };//this function will execute in the main thred

                loadStationWorker.DoWork +=
                    (object sender, DoWorkEventArgs args) =>
                    {
                        BackgroundWorker worker = (BackgroundWorker)sender;
                        ObservableCollection<BO.LineStation> result = new ObservableCollection<BO.LineStation>(source.GetAllLineStations());//get all Stations from source
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
            ClosingCommand = new RelayCommand(Closing);
            CloseCoammsnd = new RelayCommand<Window>(Close);
            TripCommand = new RelayCommand(GoTrip);
        }

        #endregion

        #region commands and events
        public ICommand WindowLoaded_Command { get; }
        public ICommand LogOut_Command { get; }
        public ICommand ClosingCommand { get; }
        public ICommand CloseCoammsnd { get; }
        public ICommand TripCommand { get; }

        BackgroundWorker AddUserTripWorker;

        private void GoTrip()
        {
            Line line = Lines.FirstOrDefault(l => l.ID == FromStation.LineId);
            BO.UserTrip newUserTrip = new BO.UserTrip()
            {
                UserName = passenger.Name,
                LineId = line.ID,
                LineNum = (int)line.LineNumber,
                InStation = FromStation.StationNumber,
                InStationName = FromStation.Name,
                InTime = DateTime.Today + TimeOfTrip.StartTime,
                OutStation = ToStation.StationNumber,
                OutStationName = ToStation.LineStations.First().Name,
                OutTime = DateTime.Today + TimeOfTrip.FinishTime
            };

            AddUserTrip(newUserTrip);
            //clear the fileds
            FromText = "";
            ToText = "";
        }

        private void LogOut(PassengerView window)
        {
            window.Close();
        }
        private void Closing()
        {
            new MainWindow().Show();
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
            if ((sender as ComboBox).Name == "FromComboBox")
            {
                FromStations.Filter = s => true;
                FromStations.Refresh();
            }
            else if((sender as ComboBox).Name == "ToComboBox" && ToStation != null)
            {
                ToStations.Filter = s => true;
                ToStations.Refresh();
            }
        }


        #endregion

        #region private nethods
        BackgroundWorker CalculatesTimeWorker;
        /// <summary>
        /// Calculates the Times of Departure Lines from the selected station in the BL Layer
        /// and set the departureTimes list to the result of "source.CalculateTimeTrip"
        /// </summary>
        private void CalculatesTime(PassengerStation station)
        {
            if (CalculatesTimeWorker == null)
            {
                CalculatesTimeWorker = new BackgroundWorker();
                CalculatesTimeWorker.RunWorkerCompleted +=
                    (object sender, RunWorkerCompletedEventArgs args) =>
                    {
                        DepartureTimes = (List<BO.TimeTrip>)args.Result;
                    };//this function will execute in the main thred

                CalculatesTimeWorker.DoWork +=
                    (object sender, DoWorkEventArgs args) =>
                    {
                        List<BO.LineStation> lineStations = args.Argument as List<BO.LineStation>;
                        args.Result = source.CalculateTimeTrip(lineStations);
                    };
            }
            if (!CalculatesTimeWorker.IsBusy)//if the worker is not busy run immediately
            {
                CalculatesTimeWorker.RunWorkerAsync(station.LineStations);
            }
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

        /// <summary>
        /// show all the available stations that are next in the lines that passed in selected station("FromStation")
        /// </summary>
        /// <returns>List of BO.LineStation</returns>
        private ObservableCollection<PassengerStation> GetToStations(List<BO.LineStation> lineStations)
        {
            return new ObservableCollection<PassengerStation>(from ls in lineStations
                                                              from s in stations
                                                              where s.LineId == ls.LineId && s.LineStationIndex > ls.LineStationIndex
                                                              let ps = new PassengerStation(s)
                                                              select ps);
        }
        #endregion
    }
}
