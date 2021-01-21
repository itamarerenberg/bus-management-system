using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BL.BLApi;
using BLApi;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using PLGui.Models;
using PLGui.Models.PO;
using PLGui.utilities;

namespace PLGui.ViewModels
{
    public class ManegerViewModel : ObservableRecipient 
    {
        #region fileds

        ManegerModel manegerModel;
        IBL source;
        TabItem selectedTabItem; 
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
        #endregion

        #region collections

        public ObservableCollection<Bus> Buses
        {
            get => manegerModel.Buses;
            set => SetProperty(ref manegerModel.Buses, value);
        }
        public ObservableCollection<Line> Lines
        {
            get => manegerModel.Lines;
            set => SetProperty(ref manegerModel.Lines, value);
        }
        public ObservableCollection<Station> Stations
        {
            get => manegerModel.Stations;
            set => SetProperty(ref manegerModel.Stations, value);
        }
        public ObservableCollection<LineTrip> Linetrips
        {
            get => manegerModel.LineTrips;
            set => SetProperty(ref manegerModel.LineTrips, value);
        }
        ObservableCollection<BO.LineStation> lineStation;
        public ObservableCollection<BO.LineStation> LineStations
        {
            get => lineStation;
            set => SetProperty(ref lineStation, value);
        }
        #endregion

        #region constractor
        public ManegerViewModel()
        {
            manegerModel = new ManegerModel();
            source = BLFactory.GetBL("admin");
            loadData();

            //commands initialize
            SearchCommand = new RelayCommand<Window>(SearchBox_TextChanged);
            TabChangedCommand = new RelayCommand<Window>(tab_selactionChange);
            ListChangedCommand = new RelayCommand<object>(List_SelectionChanged);
            NewLine = new RelayCommand(Add_newLine);
            NewStation = new RelayCommand(Add_newStation);
            NewLineTrip = new RelayCommand(Add_newLineTrip);
            UpdateCommand = new RelayCommand<Window>(Update);
            DeleteCommand = new RelayCommand<Window>(Delete);
            Enter_asAnotherUserCommand = new RelayCommand<Window>(enter_asAnotherUser);
            ManegerView_ClosingCommand = new RelayCommand<Window>(manegerView_Closing);

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
            //...
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
                        OnPropertyChanged("Lines");
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
                    ObservableCollection<Station> result = new ObservableCollection<Station>(source.GetAllStations().Select(st => new Station() { BOstation = st }));//get all lines from source
                    args.Result = worker.CancellationPending ? null : result;
                };//this function will execute in the BackgroundWorker thread
            loadStationWorker.RunWorkerAsync();
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
                //get the current presented List, get his name, and create a copy collection for searching
                ListView currentList = ((Mview.mainTab.SelectedItem as TabItem).Content as ListView);
                string listName = ((Mview.mainTab.SelectedItem as TabItem).Header.ToString());
                var tempList = Mview.vModel.GetType().GetProperty(listName).GetValue(Mview.vModel, null) as IEnumerable<dynamic>;
                if (tempList != null)
                {
                    //return a new collection according to the searching letters
                    currentList.ItemsSource = tempList.Where(c => c.GetType().GetProperty(string.Concat(Mview.ComboBoxSearch.Text.Where(s => !char.IsWhiteSpace(s)))).GetValue(c, null).ToString().Contains(Mview.SearchBox.Text));
                }
            }

        }

        /// <summary>
        /// when the tab selection is changed. replace the option in the combo box according to the entity's properties of the current list
        /// </summary>
        /// <param name="window"></param>
        private void tab_selactionChange(Window window)
        {
            if (window is ManegerView)
            {
                ManegerView Mview = window as ManegerView;
                ListView currentListView = (Mview.mainTab.SelectedItem as TabItem).Content as ListView;
                if (currentListView.SelectedItem == null)
                {
                    List<string> comboList = (((Mview.mainTab.SelectedItem as TabItem).Content as ListView).View as GridView).Columns.Where(g => g.DisplayMemberBinding != null).Select(C => C.Header.ToString()).ToList();
                    Mview.ComboBoxSearch.ItemsSource = comboList;
                    DetailsVisibility(Mview, false);
                    Mview.LineStations_view.Visibility = Visibility.Collapsed; 
                }

                OnPropertyChanged(nameof(selectedTabItem));
                OnPropertyChanged(nameof(IsSelcetdItemList));
            }
        }

        /// <summary>
        /// when a row in the list has selected. show in the window his properties deatails etc.
        /// </summary>
        /// <param name="sender"></param>
        private void List_SelectionChanged(object sender)
        {
            ManegerView Mview = (((((sender as ListView).Parent as TabItem).Parent as TabControl).Parent as Grid).Parent) as ManegerView;
            if ((sender as ListView).SelectedItem is Station selectedStation)
            {
                Mview.Header1.Text = "Name:";
                Mview.content1.Content = selectedStation.Name;

                Mview.Header2.Text = "Code:";
                Mview.content2.Content = selectedStation.Code;

                Mview.Header3.Text = "Address:";
                Mview.content3.Content = selectedStation.Address;

                Mview.Header4.Text = "Location:";
                Mview.content4.Content = selectedStation.Location;

                DetailsVisibility(Mview, true);
            }

            if ((sender as ListView).SelectedItem is Line selectedLine)
            {
                Mview.LineStations_view.Visibility = Visibility.Visible;
                LineStations = selectedLine.Stations;

                Mview.Header1.Text = "Line Number:";
                Mview.content1.Content = selectedLine.LineNumber;

                Mview.Header2.Text = "Area:";
                Mview.content2.Content = selectedLine.Area;

                Mview.Header1.Visibility = Visibility.Visible;
                Mview.content1.Visibility = Visibility.Visible;
                Mview.Header2.Visibility = Visibility.Visible;
                Mview.content2.Visibility = Visibility.Visible;
                Mview.Header3.Visibility = Visibility.Collapsed;
                Mview.content3.Visibility = Visibility.Collapsed;
                Mview.Header4.Visibility = Visibility.Collapsed;
                Mview.content4.Visibility = Visibility.Collapsed;
            }
            OnPropertyChanged(nameof(selectedTabItem));
            OnPropertyChanged(nameof(IsSelcetdItemList));
        }

        private void Add_newLine()
        {
            new NewLineView().ShowDialog();
            loadLines();
        }

        private void Add_newStation()
        {
            new NewStationView().ShowDialog();
            loadStations();
        }
        private void Add_newLineTrip()
        {
            new NewLineTripsView().ShowDialog();
            //loadLineTrips();///////////////////////////////////////////////////////////////////////
        }

        /// <summary>
        /// generic update command
        /// </summary>
        private void Update(Window window)
        {
            ManegerView Mview = window as ManegerView;

            if (Mview.Stations_view.IsSelected)//station
            {
                Station station = Mview.StationList.SelectedItem as Station;
                UpdateStation(station);
            }
            if (Mview.Lines_view.IsSelected)//line
            {
                Line line = Mview.LinesList.SelectedItem as Line;
                Update_Line(line);
            }
            if (Mview.LineTrip_view.IsSelected)//lineTrip
            {
                LineTrip lineTrip = Mview.LineTrip.SelectedItem as LineTrip;
                Line line = Lines.Where(l => l.ID == lineTrip.LineId).FirstOrDefault();
                UpdateLineTrip(lineTrip, line);
            }
        }

        private void MouseRightButtonDown(object sender)
        {
            ManegerView Mview = (((((sender as ListView).Parent as TabItem).Parent as TabControl).Parent as Grid).Parent) as ManegerView;
            ContextMenu CMenu = Mview.FindResource("rightClickMenuStrip") as ContextMenu;
            ListView currentList = sender as ListView;
            currentList.ContextMenu = CMenu;
            //CMenu.Show(this, new Point(e.X, e.Y));
            //if ((sender as ListView).SelectedItem is Station selectedStation) ;

        }
        /// <summary>
        /// generic delete command
        /// </summary>
        /// <param name="window"></param>
        private void Delete(Window window)
        {
            if (window is ManegerView)
            {
                ManegerView Mview = window as ManegerView;
                
                if (Mview.Stations_view.IsSelected)//station
                {
                    Station station = Mview.StationList.SelectedItem as Station;
                    DeleteStation(station);
                }
                if (Mview.Lines_view.IsSelected)//Line
                {
                    Line line = Mview.LinesList.SelectedItem as Line;
                    DeleteLine(line);
                }
            }
        }
        private void enter_asAnotherUser(Window window)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            ManegerView Mview = window as ManegerView;
            Mview.Close();
        }
        private void manegerView_Closing(Window window)
        {
            new MainWindow().Show();
            window.Close();
        }
        #endregion

        #region help methods
        private void DeleteStation(Station station)
        {
            if (station != null)
            {
                MessageBoxResult result = MessageBox.Show($"station: {station.Name} code: {station.Code} will be deleted! do you want to continue?", "Atantion", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    source.DeleteStation(station.Code);
                    loadStations();
                    MessageBox.Show($"station: {station.Name} code: {station.Code} was deleted successfully!");
                }
            }
        }
        private void DeleteLine(Line line)
        {
            if (line != null)
            {
                MessageBoxResult result = MessageBox.Show($"line number: {line.LineNumber} will be deleted! do you want to continue?", "Atantion", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    source.DeleteLine(line.ID);
                    loadLines();
                    MessageBox.Show($"line number: {line.LineNumber} was deleted successfully!");
                }
            }
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
            loadData();
        }
        private void UpdateLineTrip(LineTrip lineTrip, Line line)
        {
            lineToSend = line;
            lineTripToSend = lineTrip;
            new NewLineTripsView();
            //loadLineTrips();///////////////////////////////////////////////////////////////////////
        }

        private void DetailsVisibility(ManegerView Mview, bool flag)
        {
            if (flag)
            {
                Mview.Header1.Visibility = Visibility.Visible;
                Mview.content1.Visibility = Visibility.Visible;
                Mview.Header2.Visibility = Visibility.Visible;
                Mview.content2.Visibility = Visibility.Visible;
                Mview.Header3.Visibility = Visibility.Visible;
                Mview.content3.Visibility = Visibility.Visible;
                Mview.Header4.Visibility = Visibility.Visible;
                Mview.content4.Visibility = Visibility.Visible;
            }
            else
            {
                Mview.Header1.Visibility = Visibility.Collapsed;
                Mview.content1.Visibility = Visibility.Collapsed;
                Mview.Header2.Visibility = Visibility.Collapsed;
                Mview.content2.Visibility = Visibility.Collapsed;
                Mview.Header3.Visibility = Visibility.Collapsed;
                Mview.content3.Visibility = Visibility.Collapsed;
                Mview.Header4.Visibility = Visibility.Collapsed;
                Mview.content4.Visibility = Visibility.Collapsed;
            }
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
