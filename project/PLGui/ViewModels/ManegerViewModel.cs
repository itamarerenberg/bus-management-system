using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using PLGui.Models;
using PLGui.Models.PO;
using PLGui.utilities;

namespace PLGui.utilities
{
    public class ManegerViewModel : ObservableRecipient  
    {
        #region fileds

        ManegerModel manegerModel = new ManegerModel();
        IBL source;
        TabItem selectedTabItem;

        private Station stationDisplay;
        private Line lineDisplay;
        private LineTrip lineTripDisplay;
        private Bus busDisplay;

        ObservableCollection<BO.LineStation> lineStation;
        private ObservableCollection<LineTrip> lineTripsOfLine;
        private ObservableCollection<Line> linesOfStation;


        #endregion

        #region help properties
        public TabItem SelectedTabItem 
        {
            get => selectedTabItem;
            set
            {
                SetProperty(ref selectedTabItem, value);
                OnPropertyChanged(nameof(IsSelcetdItemList));
            }
        }
        public ManegerView Mview { get; set; }
        public bool IsSelcetdItemList
        {
            get
            {
                if (SelectedTabItem != null)
                {
                    if (SelectedTabItem.Content is ListView currentList)
                    {
                        if (currentList.SelectedItem != null)
                        {
                            return true;
                        }
                    } 
                }
                return false;
            }
        }
        public Line PreviousLine{ get; set; }
        public bool IsPreviousLine { get => PreviousLine != null; }
        public Stack<object> MemoryStack { get; set; } = new Stack<object>();
        public bool StackIsNotEmpty{ get => MemoryStack.Count > 0; }
        public SnackbarMessageQueue MyMessageQueue { get; set; } = new SnackbarMessageQueue();
        public DateTime Time{ get; set; }

        public Station StationDisplay
        {
            get => stationDisplay;
            set => SetProperty(ref stationDisplay, value);
        }
        public Line LineDisplay
        {
            get => lineDisplay;
            set => SetProperty(ref lineDisplay, value);
        }
        public LineTrip LineTripDisplay
        {
            get => lineTripDisplay;
            set => SetProperty(ref lineTripDisplay, value);
        }
        public Bus BusDisplay
        {
            get => busDisplay;
            set => SetProperty(ref busDisplay, value);
        }


        #endregion

        #region collections

        public ObservableCollection<Bus> buses
        {
            get => manegerModel.Buses;
            set => SetProperty(ref manegerModel.Buses, value);
        }
        public ICollectionView Buses
        {
            get { return CollectionViewSource.GetDefaultView(buses); }
        }
        public ObservableCollection<Line> lines
        {
            get => manegerModel.Lines;
            set => SetProperty(ref manegerModel.Lines, value);
        }
        public ICollectionView Lines
        {
            get { return CollectionViewSource.GetDefaultView(lines); }
        }
        public ObservableCollection<Station> stations
        {
            get => manegerModel.Stations;
            set => SetProperty(ref manegerModel.Stations, value);
        }
        public ICollectionView Stations
        {
            get { return CollectionViewSource.GetDefaultView(stations); }
        }
        public ObservableCollection<LineTrip> lineTrips
        {
            get => manegerModel.LineTrips;
            set => SetProperty(ref manegerModel.LineTrips, value);
        }
        public ICollectionView LineTrips
        {
            get { return CollectionViewSource.GetDefaultView(lineTrips); }
        }
        public ObservableCollection<BO.LineStation> LineStations
        {
            get => lineStation;
            set => SetProperty(ref lineStation, value);
        }
        public ObservableCollection<LineTrip> LineTripsOfLine
        {
            get => lineTripsOfLine;
            set => SetProperty(ref lineTripsOfLine, value);
        }
        public ObservableCollection<Line> LinesOfStation
        {
            get => linesOfStation;
            set => SetProperty(ref linesOfStation, value);
        }
        #endregion

        #region constractor
        public ManegerViewModel()
        {
            source = BLFactory.GetBL("admin");
            loadData();

            //commands initialize
            SearchCommand = new RelayCommand<Window>(SearchBox_TextChanged);
            TabChangedCommand = new RelayCommand<ManegerView>(tab_selactionChange);
            ListChangedCommand = new RelayCommand<ManegerView>(List_SelectionChanged);
            NewLine = new RelayCommand(Add_newLine);
            NewStation = new RelayCommand(Add_newStation);
            NewLineTrip = new RelayCommand<ManegerView>(Add_newLineTrip);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            Enter_asAnotherUserCommand = new RelayCommand<Window>(enter_asAnotherUser);
            ManegerView_ClosingCommand = new RelayCommand<Window>(manegerView_Closing);
            WindowLoaded_Command = new RelayCommand<ManegerView>(Window_Loaded);
            LostFocus_Command = new RelayCommand<ManegerView>(LostFocus);
            BackCommand = new RelayCommand<ManegerView>(Back);
            Play_Command = new RelayCommand(Play);
            RandomBus_Command = new RelayCommand(RandomBus);

            //messengers initalize
            RequestStationMessege();
            RequestLineMessege();
            RequestLineTripMessege();
        }

        #endregion

        #region load data
        private void loadData()
        {
            loadLines();
            loadStations();
            loadBuses();
            loadLineTrips();
        }

        BackgroundWorker loadLinesWorker;
        private void loadLines()
        {
            if (loadLinesWorker == null)
            {
                loadLinesWorker = new BackgroundWorker();
            }

            loadLinesWorker.RunWorkerCompleted +=
                (object sender, RunWorkerCompletedEventArgs args) =>
                {
                    if (!((BackgroundWorker)sender).CancellationPending)//if the BackgroundWorker didn't 
                    {                                                   //terminated befor he done execute DoWork
                        manegerModel.Lines = (ObservableCollection<Line>)args.Result;
                        OnPropertyChanged(nameof(Lines));
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
            loadLinesWorker.RunWorkerAsync();
        }

        BackgroundWorker loadStationWorker;
        private void loadStations()
        {
            if (loadStationWorker == null)
            {
                loadStationWorker = new BackgroundWorker();
            }

            loadStationWorker.RunWorkerCompleted +=
                (object sender, RunWorkerCompletedEventArgs args) =>
                {
                    if (!((BackgroundWorker)sender).CancellationPending)//if the BackgroundWorker didn't 
                    {                                                   //terminated befor he done execute DoWork
                        manegerModel.Stations = (ObservableCollection<Station>)args.Result;
                        OnPropertyChanged("Stations");
                    }
                };//this function will execute in the main thred

            loadStationWorker.DoWork +=
                (object sender, DoWorkEventArgs args) =>
                {
                    BackgroundWorker worker = (BackgroundWorker)sender;
                    ObservableCollection<Station> result = new ObservableCollection<Station>(source.GetAllStations().Select(st => new Station() { BOstation = st }));//get all Stations from source
                    args.Result = worker.CancellationPending ? null : result;
                };//this function will execute in the BackgroundWorker thread
            loadStationWorker.RunWorkerAsync();
        }

        BackgroundWorker loadBusesWorker;
        private void loadBuses()
        {
            if (loadBusesWorker == null)
            {
                loadBusesWorker = new BackgroundWorker();
                loadBusesWorker.WorkerSupportsCancellation = true;
            }

            loadBusesWorker.RunWorkerCompleted +=
                (object sender, RunWorkerCompletedEventArgs args) =>
                {
                    if (!((BackgroundWorker)sender).CancellationPending)//if the BackgroundWorker didn't 
                    {                                                   //terminated befor he done execute DoWork
                        manegerModel.Buses = (ObservableCollection<Bus>)args.Result;
                        OnPropertyChanged(nameof(Buses));
                    }
                };//this function will execute in the main thred

            loadBusesWorker.DoWork +=
                (object sender, DoWorkEventArgs args) =>
                {
                    BackgroundWorker worker = (BackgroundWorker)sender;
                    ObservableCollection<Bus> result = new ObservableCollection<Bus>(source.GetAllBuses().Select(bus => new Bus() { BObus = bus }));//get all buses from source
                    args.Result = worker.CancellationPending ? null : result;
                };//this function will execute in the BackgroundWorker thread
            loadBusesWorker.RunWorkerAsync();

        }

        BackgroundWorker loadLineTripesWorker;
        private void loadLineTrips()
        {
            if (loadLineTripesWorker == null)
            {
                loadLineTripesWorker = new BackgroundWorker();
            }

            loadLineTripesWorker.RunWorkerCompleted +=
                (object sender, RunWorkerCompletedEventArgs args) =>
                {
                    if (!((BackgroundWorker)sender).CancellationPending)//if the BackgroundWorker didn't 
                    {                                                   //terminated befor he done execute DoWork
                        manegerModel.LineTrips = (ObservableCollection<LineTrip>)args.Result;
                        OnPropertyChanged(nameof(LineTrips));
                    }
                };//this function will execute in the main thred

            loadLineTripesWorker.DoWork +=
                (object sender, DoWorkEventArgs args) =>
                {
                    BackgroundWorker worker = (BackgroundWorker)sender;
                    ObservableCollection<LineTrip> result = new ObservableCollection<LineTrip>(source.GetAllLineTrips().Select(lineTrip => new LineTrip() { BOlineTrip = lineTrip }));//get all line trips from source
                    args.Result = worker.CancellationPending ? null : result;
                };//this function will execute in the BackgroundWorker thread
            loadLineTripesWorker.RunWorkerAsync();
        }

        #endregion

        #region commands

        #region Icommands
        public ICommand SearchCommand { get; }
        public ICommand TabChangedCommand { get; }
        public ICommand ListChangedCommand { get; }
        public ICommand NewLine { get; }
        public ICommand NewStation { get; }
        public ICommand NewLineTrip { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand Enter_asAnotherUserCommand { get; }
        public ICommand ManegerView_ClosingCommand { get; }
        public ICommand WindowLoaded_Command { get; }
        public ICommand LostFocus_Command { get; }
        public ICommand BackCommand { get; }
        public ICommand Play_Command { get; }
        public ICommand RandomBus_Command { get; }




        #endregion

        /// <summary>
        /// accured when search box is changing. replace the list in the window into list that contains the search box text.
        /// the search is according to the conbo box picking
        /// </summary>
        private void SearchBox_TextChanged(Window window)
        {
            //get the ManegerView instance
            ManegerView Mview = window as ManegerView;
            if (Mview.ComboBoxSearch.SelectedItem != null)
            {
                string propertyName = string.Concat(Mview.ComboBoxSearch.Text.Where(s => !char.IsWhiteSpace(s)));//gets the property name
                string listName = ((Mview.mainTab.SelectedItem as TabItem).Tag.ToString());                      //gets the list name

                switch (listName)
                {
                    case "Buses":
                        if (buses != null)
                        {
                            Buses.Filter = l => l.GetType().GetProperty(propertyName).GetValue(l).ToString().Contains(Mview.SearchBox.Text);
                            Buses.Refresh(); 
                        }
                        break;
                    case "Stations":
                        if (Stations != null)
                        {
                            Stations.Filter = l => l.GetType().GetProperty(propertyName).GetValue(l).ToString().Contains(Mview.SearchBox.Text);
                            Stations.Refresh();
                        }
                        break;
                    case "LineTrips":
                        if (LineTrips != null)
                        {
                            LineTrips.Filter = l => l.GetType().GetProperty(propertyName).GetValue(l).ToString().Contains(Mview.SearchBox.Text);
                            LineTrips.Refresh();
                        }
                        break;
                    case "Lines":
                        if (buses != null)
                        {
                            Lines.Filter = l => l.GetType().GetProperty(propertyName).GetValue(l).ToString().Contains(Mview.SearchBox.Text);
                            Lines.Refresh();
                        }
                        break;
                }

                
                //var fieldList = this.GetType().GetField(listName.ToLower(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                //var propList = this.GetType().GetProperty(listName).GetSetMethod(true);
                //if (Mview.SearchBox.Text == "")
                //{
                //    fieldList.SetValue(this, Mview.vModel.GetType().GetProperty(listName).GetValue(Mview.vModel, null));
                //    propList.Invoke(this, new object[] { fieldList.GetValue(this) });
                //}
                //else
                //{
                //    var tempList2 = Mview.vModel.GetType().GetProperty(listName).GetValue(Mview.vModel, null) as IEnumerable<Bus>;
                //    tempList2 = tempList2.Where(c => c != null && c.GetType().GetProperty(string.Concat(Mview.ComboBoxSearch.Text.Where(s => !char.IsWhiteSpace(s)))).GetValue(c, null).ToString().Contains(Mview.SearchBox.Text));
                //    fieldList.SetValue(this, new ObservableCollection<Bus>(tempList2));
                //    propList.Invoke(this, new object[] { fieldList.GetValue(this) });
                //}
                //get the current presented List, get his name, and create a copy collection for searching
                //ListView currentList = ((Mview.mainTab.SelectedItem as TabItem).Content as ListView);
                //var tempList = Mview.vModel.GetType().GetProperty(listName).GetValue(Mview.vModel, null) as IEnumerable<object>;
                //if (tempList != null)
                //{
                //    currentList.ItemsSource = tempList.Where(c => c != null && c.GetType().GetProperty(string.Concat(Mview.ComboBoxSearch.Text.Where(s => !char.IsWhiteSpace(s)))).GetValue(c, null).ToString().Contains(Mview.SearchBox.Text));
                //}
            }
        }
        /// <summary>
        /// when the tab selection is changed. replace the option in the combo box according to the entity's properties of the current list
        /// </summary>
        /// <param name="window"></param>
        private void tab_selactionChange(ManegerView Mview)
        {
            ListView currentListView = (Mview.mainTab.SelectedItem as TabItem).Content as ListView;

            List<string> comboList = (((Mview.mainTab.SelectedItem as TabItem).Content as ListView).View as GridView).Columns
                                      .Where(g => g.DisplayMemberBinding != null).Select(C => C.Header.ToString()).ToList();
            Mview.ComboBoxSearch.ItemsSource = comboList;
            Mview.ComboBoxSearch.SelectedIndex = 0;//display the first item in the combo box
            Mview.SearchBox.Text = "";//clear the search box !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            SearchBox_TextChanged(Mview);

            OnPropertyChanged(nameof(SelectedTabItem));
            OnPropertyChanged(nameof(IsSelcetdItemList));
        }
        /// <summary>
        /// when a row in the list has selected. show in the window his properties deatails, etc.
        /// </summary>
        /// <param name="sender"></param>
        private void List_SelectionChanged(ManegerView Mview)
        {
            if ((selectedTabItem.Content as ListView).SelectedItem is Station SelectedStation)
            {
                GetLineOfStation(SelectedStation);
                StationDisplay = SelectedStation;
            }
            else if ((selectedTabItem.Content as ListView).SelectedItem is Line selectedLine)
            {
                LineDisplay = selectedLine;
            }
            else if ((selectedTabItem.Content as ListView).SelectedItem is LineTrip selectedLineTrip)
            {
                LineTripDisplay = selectedLineTrip;
            }
            else if ((selectedTabItem.Content as ListView).SelectedItem is Bus selectedbus)
            {
                BusDisplay = selectedbus;
            }
        }
        BackgroundWorker GetLineOfStationWorker;
        private void GetLineOfStation(Station station)
        {
            if (station.LinesNums.Count > 0)
            {
                if (GetLineOfStationWorker == null)
                {
                    GetLineOfStationWorker = new BackgroundWorker();
                    GetLineOfStationWorker.WorkerSupportsCancellation = true;
                }

                GetLineOfStationWorker.RunWorkerCompleted +=
                    (object sender, RunWorkerCompletedEventArgs args) =>
                    {
                        if (!((BackgroundWorker)sender).CancellationPending)//if the BackgroundWorker didn't 
                    {                                                   //terminated befor he done execute DoWork
                        LinesOfStation = (ObservableCollection<Line>)args.Result;
                            OnPropertyChanged(nameof(LinesOfStation));
                        }
                    };//this function will execute in the main thred

                GetLineOfStationWorker.DoWork +=
                    (object sender, DoWorkEventArgs args) =>
                    {
                        BackgroundWorker worker = (BackgroundWorker)sender;
                        ObservableCollection<Line> result = new ObservableCollection<Line>();
                        try
                        {
                            result = new ObservableCollection<Line>(source.GetAllLinesBy(l => l.Stations.Exists(s => s.StationNumber == station.Code)).Select(l => l.Line_BO_PO()));//get the lines from source
                    }
                        catch (Exception msg)
                        {
                            MessageBox.Show(msg.Message, "ERROR");
                            GetLineOfStationWorker.CancelAsync();
                        }
                        args.Result = worker.CancellationPending ? null : result;
                    };//this function will execute in the BackgroundWorker thread
                GetLineOfStationWorker.RunWorkerAsync(); 
            }
        }
        private void LostFocus( ManegerView MView)
        {
            if (SelectedTabItem != null)
            {

            }
            //MView.StationList.SelectedItem = null;
            //MView.LinesList.SelectedItem = null;
            //MView.LineTripList.SelectedItem = null;
            //MView.BusesList.SelectedItem = null;
        }
        private void Add_newLine()
        {
            lineToSend = null;
            new NewLineView().ShowDialog();

            loadLines();
            loadStations();
        }
        private void Add_newStation()
        {
            stationToSend = null;
            new NewStationView().ShowDialog();
            loadStations();
        }
        private void Add_newLineTrip(ManegerView Mview)
        {
            if (Mview.LinesList.SelectedItem != null)
            {
                lineToSend = Mview.LinesList.SelectedItem as Line;
                new NewLineTripsView().ShowDialog();

                loadLineTrips();
                loadLines();
                List_SelectionChanged(Mview);//refrash the view
            }
            else
            {
                MessageBox.Show("please select a line", "ERROR");
            }
        }
        /// <summary>
        /// generic update command
        /// </summary>
        private void Update()
        {
            if (Mview != null)
            {
                if (Mview.Stations_view.IsSelected)//station
                {
                    Station station = Mview.StationList.SelectedItem as Station;
                    UpdateStation(station);
                }
                else if (Mview.Lines_view.IsSelected)//line
                {
                    Line line = Mview.LinesList.SelectedItem as Line;
                    Update_Line(line);
                }
                else if (Mview.LineTrip_view.IsSelected)//lineTrip
                {
                    LineTrip lineTrip = Mview.LinesTripList.SelectedItem as LineTrip;
                    Line line = lines.Where(l => l.ID == lineTrip.LineId).FirstOrDefault();
                    UpdateLineTrip(lineTrip, line);
                } 
            }
            List_SelectionChanged(Mview);//refrash the view
        }     
        /// <summary>
        /// generic delete command
        /// </summary>
        /// <param name="window"></param>
        private void Delete()
        {
            if (Mview.Stations_view.IsSelected)//station
            {
                Station station = Mview.StationList.SelectedItem as Station;
                if (DeleteStation(station))
                {
                    MyMessageQueue.Enqueue($"station: {station.Name} code: {station.Code} was deleted successfully!");
                    OnPropertyChanged(nameof(MyMessageQueue));
                }
            }
            else if (Mview.Lines_view.IsSelected)//Line
            {
                Line line = Mview.LinesList.SelectedItem as Line;
                if (DeleteLine(line))
                {
                    MyMessageQueue.Enqueue($"line number: {line.LineNumber} was deleted successfully!");
                    OnPropertyChanged(nameof(MyMessageQueue));
                }
            }
            else if (Mview.LineTrip_view.IsSelected)//LineTrip
            {
                LineTrip lineTrip = Mview.LinesTripList.SelectedItem as LineTrip;
                if (DeleteLineTrip(lineTrip))
                {
                    OnPropertyChanged(nameof(MyMessageQueue));
                }
            }
            else if (Mview.Bus_view.IsSelected)//bus
            {
                Bus bus = Mview.BusesList.SelectedItem as Bus;
                if (bus != null)
                {
                    MyMessageQueue.Enqueue($"bus license number: {bus.LicenseNumber} will be deleted!","UNDO", new Action(DeleteBusWorker.CancelAsync));
                    OnPropertyChanged(nameof(MyMessageQueue));
                    DeleteBus(bus);
                }
            }

        }
        private void enter_asAnotherUser(Window window)
        {
            new MainWindow().Show();

            ManegerView Mview = window as ManegerView;
            Mview.Close();
        }
        private void manegerView_Closing(Window window)
        {
            new MainWindow().Show();
            window.Close();
        }
        private void Window_Loaded(ManegerView manegerView)
        {
            Mview = manegerView;

            tab_selactionChange(manegerView);

            manegerView.LinesList.MouseDoubleClick += List_MouseDoubleClick;
            manegerView.LineTrip_Details.MouseDoubleClick += List_MouseDoubleClick;
            manegerView.LineStations_view.MouseDoubleClick += List_MouseDoubleClick;
            manegerView.LinesTripList.MouseDoubleClick += List_MouseDoubleClick;

            manegerView.StationList.MouseRightButtonUp += List_MouseRightButtonUp;
            manegerView.LinesList.MouseRightButtonUp += List_MouseRightButtonUp;
            manegerView.LinesTripList.MouseRightButtonUp += List_MouseRightButtonUp;
            manegerView.BusesList.MouseRightButtonUp += List_MouseRightButtonUp;

            manegerView.ClockDialog.DialogOpened += ClockDialog_Opened;
            manegerView.ClockDialog.DialogClosing += ClockDialog_Closing;

        }
        private void Play()
        {
            if (Mview.PlayButton.ToolTip.ToString() == "Play")
            {
                Mview.PlayButton.Content = new PackIcon() { Kind = PackIconKind.Stop, Foreground = System.Windows.Media.Brushes.Red };
                Mview.PlayButton.ToolTip = "Stop";
            }
            else
            {
                Mview.PlayButton.Content = new PackIcon() { Kind = PackIconKind.Play };
                Mview.PlayButton.ToolTip = "Play";
            }
        }
        private void Back(ManegerView Mview)
        {
            object obj = MemoryStack.Pop();

            if (obj is Line line)
            {
                Mview.mainTab.SelectedIndex = 1;
                Mview.LinesList.SelectedIndex = Mview.LinesList.Items.IndexOf(line);
                Mview.LinesList.ScrollIntoView(line);
            }
            else if (obj is Station station)
            {
                Mview.mainTab.SelectedIndex = 0;
                Mview.StationList.SelectedIndex = Mview.StationList.Items.IndexOf(station);
                Mview.StationList.ScrollIntoView(station);
            }
            else if (obj is LineTrip lineTrip)
            {
                Mview.mainTab.SelectedIndex = 2;
                Mview.LinesTripList.SelectedIndex = Mview.LinesTripList.Items.IndexOf(lineTrip);
                Mview.LinesTripList.ScrollIntoView(lineTrip);
            }
            OnPropertyChanged(nameof(StackIsNotEmpty));

        }
        BackgroundWorker GetRandomBusWorker;

        private void RandomBus()
        {
            if (GetRandomBusWorker == null)
            {
                GetRandomBusWorker = new BackgroundWorker();
                GetRandomBusWorker.WorkerSupportsCancellation = true;
            }

            GetRandomBusWorker.RunWorkerCompleted +=
                (object sender, RunWorkerCompletedEventArgs args) =>
                {
                    if (!((BackgroundWorker)sender).CancellationPending)//if the BackgroundWorker didn't 
                    {                                                   //terminated befor he done execute DoWork
                        loadBuses();
                    }
                };//this function will execute in the main thred

            GetRandomBusWorker.DoWork +=
                (object sender, DoWorkEventArgs args) =>
                {
                    try
                    {
                        source.AddRandomBus();
                    }
                    catch (Exception msg)
                    {
                        MessageBox.Show(msg.Message, "ERROR");
                    }
                };//this function will execute in the BackgroundWorker thread
            GetRandomBusWorker.RunWorkerAsync();
            
        }
        #endregion

        #region events
        private void ClockDialog_Closing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "1"))
            {
                Time = Mview.Clock.Time;
                OnPropertyChanged(nameof(Time));
            }
        }
        private void ClockDialog_Opened(object sender, DialogOpenedEventArgs eventArgs)
        {
            Mview.Clock.Time = Time;
        }
        private void List_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Control control)
            {
                ManegerView Mview = control.FindWindowOfType<ManegerView>();//find the window instance

                ContextMenu CMenu = Mview.FindResource("RightClickMenuStrip") as ContextMenu;
                if (sender is ListView currentList)
                {
                    currentList.ContextMenu = CMenu;
                    CMenu.IsOpen = true;
                }
            }
        }
        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Control control)
            {
                ManegerView Mview = control.FindWindowOfType<ManegerView>();// try to get the source window
                if (sender is ListView listV)
                {
                    if (listV == Mview.LineStations_view)
                    {
                        if (listV.SelectedItem is BO.LineStation SelectedLineStation)
                        {
                            MemoryStack.Push(Mview.LinesList.SelectedItem);// push the line into the stack
                            Station SelectedStation = stations.Where(s => s.Code == SelectedLineStation.StationNumber).FirstOrDefault();
                            if (SelectedStation != null)
                            {
                                Mview.mainTab.SelectedIndex = 0;
                                Mview.StationList.SelectedIndex = Mview.StationList.Items.IndexOf(SelectedStation);
                                Mview.StationList.ScrollIntoView(SelectedStation);
                                OnPropertyChanged(nameof(StackIsNotEmpty));
                            }
                        }
                    }
                    if (listV == Mview.LinePasses_view)
                    {
                        if (listV.SelectedItem is Line SelectedLine)
                        {
                            MemoryStack.Push(Mview.StationList.SelectedItem);// push the station into the stack
                            if (SelectedLine != null)
                            {
                                Mview.mainTab.SelectedIndex = 1;
                                Mview.LinesList.SelectedIndex = Mview.LinesList.Items.IndexOf(SelectedLine);
                                Mview.LinesList.ScrollIntoView(SelectedLine);
                                OnPropertyChanged(nameof(StackIsNotEmpty));
                            }
                        }
                    }
                    if (listV == Mview.LinesTripList)
                    {
                        if (listV.SelectedItem is LineTrip SelectedLineTrip)
                        {
                            MemoryStack.Push(Mview.LinesTripList.SelectedItem);// push the Line trip into the stack
                            Line line = lines.Where(l => l.ID == SelectedLineTrip.LineId).FirstOrDefault();
                            if (SelectedLineTrip != null)
                            {
                                Mview.mainTab.SelectedIndex = 1;
                                Mview.LinesList.SelectedIndex = Mview.LinesList.Items.IndexOf(line);
                                Mview.LinesList.ScrollIntoView(line);
                                OnPropertyChanged(nameof(StackIsNotEmpty));
                            }
                        }
                    }
                    if (listV == Mview.LineTrip_Details)
                    {
                        if (listV.SelectedItem is LineTrip SelectedLineTrip)
                        {
                            MemoryStack.Push(Mview.LinesList.SelectedItem);// push the station into the stack
                            if (SelectedLineTrip != null)
                            {
                                Mview.mainTab.SelectedIndex = 2;
                                Mview.LinesTripList.SelectedIndex = Mview.LinesTripList.Items.IndexOf(SelectedLineTrip);//-------------------------------------------------------------------------
                                Mview.LinesTripList.ScrollIntoView(SelectedLineTrip);
                                OnPropertyChanged(nameof(StackIsNotEmpty));
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region help methods
        //------------------------------------------------------------------------------------------------
        BackgroundWorker DeleteStationWorker = new BackgroundWorker() { WorkerSupportsCancellation = true };
        BackgroundWorker DeleteLineWorker = new BackgroundWorker() { WorkerSupportsCancellation = true };
        BackgroundWorker DeleteLineTripWorker = new BackgroundWorker() { WorkerSupportsCancellation = true };
        BackgroundWorker DeleteBusWorker = new BackgroundWorker() {WorkerSupportsCancellation = true };
        private bool DeleteStation(Station station)
        {
            if (station != null)
            {
                MessageBoxResult result = MessageBox.Show($"station: {station.Name} code: {station.Code} will be deleted! do you want to continue?", "Attention", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        source.DeleteStation(station.Code);
                        return true;
                    }
                    catch (Exception msg)
                    {
                        MessageBox.Show(msg.Message, "ERROR");
                    }
                    loadStations();
                }
            }
            return false;
        }
        private bool DeleteLine(Line line)
        {
            if (line != null)
            {
                MessageBoxResult result = MessageBox.Show($"line number: {line.LineNumber} will be deleted! do you want to continue?", "Attention", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        source.DeleteLine(line.ID);
                        return true;
                    }
                    catch (Exception msg)
                    {
                        MessageBox.Show(msg.Message, "ERROR");
                    }
                    loadLines();
                }
            }
            return false;
        }
        private bool DeleteLineTrip(LineTrip lineTrip)
        {
            if (lineTrip != null)
            {
                int? lineNum = lines.Where(l => l.ID == lineTrip.LineId).FirstOrDefault().LineNumber;

                MessageBoxResult result = MessageBox.Show($"Line trip of line number: {lineNum}(ID = {lineTrip.LineId}) will be deleted! do you want to continue?", "Attention", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        source.DeleteLineTrip(lineTrip.BOlineTrip);
                        MyMessageQueue.Enqueue($"Line trip of line number: {lineNum}(ID = {lineTrip.LineId}) was deleted successfully!");
                        return true;
                    }
                    catch (Exception msg)
                    {
                        MessageBox.Show(msg.Message, "ERROR");
                    }
                    loadLineTrips();
                    loadLines();
                }
            }
            return false;
        }
        private void DeleteBus(Bus bus)
        {
            DeleteBusWorker.RunWorkerCompleted +=
                (object sender, RunWorkerCompletedEventArgs args) =>
                {
                    if (!args.Cancelled)//if the BackgroundWorker didn't 
                    {                                                   //terminated befor he done execute DoWork
                        loadBuses();
                        MyMessageQueue.Enqueue($"bus license number: {bus.LicenseNumber} was deleted successfully!");
                        OnPropertyChanged(nameof(MyMessageQueue));
                    }
                    else                                                //Cancelled!!
                    {
                        MyMessageQueue.Enqueue("Cancelled!");
                        OnPropertyChanged(nameof(MyMessageQueue));
                    }
                };//this function will execute in the main thread
            DeleteBusWorker.DoWork +=
                (object sender, DoWorkEventArgs args) =>
                {
                    BackgroundWorker worker = (BackgroundWorker)sender;
                    Thread.Sleep(3000);// let time for cancelation
                    if (worker.CancellationPending) { args.Cancel = true;}
                    else
                    {
                        try
                        {
                            source.DeleteBus(bus.LicenseNumber);
                        }
                        catch (Exception msg)
                        {
                            MessageBox.Show(msg.Message, "ERROR");
                        } 
                    }
                };//this function will execute in the BackgroundWorker thread
            DeleteBusWorker.RunWorkerAsync();
        }

        private void Update_Line(Line line)
        {
            lineToSend = line;
            new NewLineView().ShowDialog();
            loadLines();
        }
        private void UpdateStation(Station station)
        {
            stationToSend = station;
            new NewStationView().ShowDialog();
            loadStations();
            loadLines();
        }
        private void UpdateLineTrip(LineTrip lineTrip, Line line)
        {
            lineToSend = line;
            lineTripToSend = lineTrip;
            new NewLineTripsView().ShowDialog();
            loadLineTrips();
            loadLines();
        }
        
        #endregion

        #region Messenger

        private Line lineToSend;
        private Station stationToSend;
        private LineTrip lineTripToSend;

        private void RequestStationMessege()
        {
            //reply to the RequestStation messege by sending the station
            WeakReferenceMessenger.Default.Register<ManegerViewModel, RequestStation>(this, (r, m) =>
            {
                m.Reply(r.stationToSend);
            });
        }
        private void RequestLineMessege()
        {
            //reply to the RequestLine messege by sending the line
            WeakReferenceMessenger.Default.Register<ManegerViewModel, RequestLine>(this, (r, m) =>
            {
                m.Reply(r.lineToSend);
            });
        }
        private void RequestLineTripMessege()
        {
            //reply to the RequestLineTrip messege by sending the lineTrip
            WeakReferenceMessenger.Default.Register<ManegerViewModel, RequestLineTrip>(this, (r, m) =>
            {
                m.Reply(r.lineTripToSend);
            });
        }

        #endregion
    }
}
