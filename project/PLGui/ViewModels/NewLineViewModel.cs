using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BL.BLApi;
using BLApi;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PLGui.Models.PO;

namespace PLGui.ViewModels
{
    public class NewLineViewModel : ObservableRecipient
    {

        #region fields
        IBL source;
        BackgroundWorker load_data;
        BackgroundWorker creatNewLine;

        Line newLine = new Line();
        private BO.AdjacentStations toNext;
        private BO.AdjacentStations toBack;
        private bool addManually;

        #endregion

        #region properties
        public List<string> ComboList { get; set; }

        public Line NewLine
        {
            get => newLine;
            set
            {
                SetProperty(ref newLine, value);
            }
        }

        public BO.AdjacentStations ToNext
        {
            get => toNext;
            set
            {
                SetProperty(ref toNext, value);
            }
        }

        public BO.AdjacentStations ToBack
        {
            get => toBack;
            set
            {
                SetProperty(ref toBack, value);
            }
        }


        public ObservableCollection<Station> DBStations { get; set; }//data base stations
        public ObservableCollection<LineStation> Stations { get; set; }//new updated line stations

        public bool IsMinStation { get => Stations.Count >= 2; }
        public bool AddManually 
        { 
            get => addManually;
            set
            {
                SetProperty(ref addManually, value);
            }
        }

        #endregion

        #region constructors
        public NewLineViewModel()
        {
            source = BLFactory.GetBL("admin");
            Stations = new ObservableCollection<LineStation>();
            newLine.Stations = new ObservableCollection<BO.LineStation>();
            ComboList = new List<string>() { "Name", "Code", "Address" };
            loadData();

            SelectDBStationCommand = new RelayCommand<object>(SelectDBStation);
            SelectStationCommand = new RelayCommand<Window>(SelectStation);
            SaveButton = new RelayCommand<Window>(saveButton);
            AddLineButton = new RelayCommand(AddLineButton_click);
            DeleteStationCommand = new RelayCommand<object>(DeleteStation);
            SearchCommand = new RelayCommand<object>(SearchBox_TextChanged);
        }
        /// <summary>
        /// updating an existing line
        /// </summary>
        /// <param name="oldLine"></param>
        public NewLineViewModel(Line oldLine)//constructor for updating purpose
        {

        }

        #endregion

        #region load data
        private void loadData()
        {
            if (load_data == null)
            {
                load_data = new BackgroundWorker();
            }
            load_data.RunWorkerCompleted +=
                (object sender, RunWorkerCompletedEventArgs args) =>
                {
                    if (!((BackgroundWorker)sender).CancellationPending)//if the BackgroundWorker didn't 
                    {                                                   //terminated before he done execute DoWork
                        DBStations = (ObservableCollection<Station>)args.Result;
                        OnPropertyChanged("DBStations");
                    }
                };//this function will execute in the main thred

            load_data.DoWork +=
                (object sender, DoWorkEventArgs args) =>
                {
                    BackgroundWorker worker = (BackgroundWorker)sender;
                    ObservableCollection<Station> result = new ObservableCollection<Station>();
                    result = new ObservableCollection<Station>(source.GetAllStations().Select(st => new Station() { BOstation = st }));
                    args.Result = worker.CancellationPending ? null : result;
                };//this function will execute in the BackgroundWorker thread
            load_data.RunWorkerAsync();
        }
        #endregion

        #region commands
        public ICommand SelectDBStationCommand { get; }
        public ICommand SelectStationCommand { get; }
        public ICommand DeleteStationCommand { get; }
        public ICommand SaveButton { get; }
        public ICommand AddLineButton { get; }
        public ICommand SearchCommand { get; }

        /// <summary>
        /// double click on data base station, the station will be added to the new/updated line stations
        /// </summary>
        private void SelectDBStation(object sender)
        {
            if ((sender as ListView).SelectedItem is Station selectedStation)
            {
                if (Stations.Count > 0)
                    Stations.Last().NotLast = true;//now the previus last station is no longer the last
                Stations.Add(new LineStation() { Station = selectedStation, NotLast = false});//this is the last station in Stations
                NewLine.Stations.Add(new BO.LineStation() 
                { Address = selectedStation.Address, StationNumber = selectedStation.Code, LineStationIndex = newLine.Stations.Count });
                DBStations.Remove(selectedStation);
                OnPropertyChanged(nameof(IsMinStation));

                NewLineView Lview = ((sender as ListView).Parent as Grid).Parent as NewLineView;
                SearchBox_TextChanged(Lview);
            }
        }
        /// <summary>
        /// double click on the new/updated line station, the station will be deleted from the stations list
        /// </summary>
        private void DeleteStation(object sender)
        {
            if ((sender as ListView).SelectedItem is LineStation selectedStation)
            {
                int index = (sender as ListView).SelectedIndex;

                DBStations.Add(selectedStation.Station);
                Stations.Remove(selectedStation);
                if (selectedStation.NotLast == false && Stations.Count > 0)//if the selectet station was the last station
                    Stations.Last().NotLast = false;//set the new last station to not NotLast
                newLine.Stations.RemoveAt(index);
                OnPropertyChanged(nameof(IsMinStation));

                NewLineView Lview = (((sender as ListView).Parent as StackPanel).Parent as Grid).Parent as NewLineView;
                SearchBox_TextChanged(Lview);
            }
        }
        /// <summary>
        /// when a station in the new line's stations is selected. add an option to add a distance and time between the stations
        /// </summary>
        private void SelectStation(Window window)
        {
            NewLineView newLineView = window as NewLineView;
            if (newLineView.StationList.SelectedItem is Station selectedStation)
            {
                newLineView.StationName.Text = selectedStation.Name;

                int index = newLineView.StationList.SelectedIndex;
                int lastIndex = newLineView.StationList.Items.Count - 1;

                //preventing the option to add distance to back/next if the station's index is first/last respectivly
                if (index == 0)
                {
                    newLineView.DisToBack.IsEnabled = false;
                    newLineView.TimeToBack.IsEnabled = false;
                    newLineView.DisToNext.IsEnabled = true;
                    newLineView.TimeToNext.IsEnabled = true;
                }
                else
                {
                    if (index == lastIndex)
                    {
                        newLineView.DisToBack.IsEnabled = true;
                        newLineView.TimeToBack.IsEnabled = true;
                        newLineView.DisToNext.IsEnabled = false;
                        newLineView.TimeToNext.IsEnabled = false;
                    }
                    else
                    {
                        newLineView.DisToBack.IsEnabled = true;
                        newLineView.TimeToBack.IsEnabled = true;
                        newLineView.DisToNext.IsEnabled = true;
                        newLineView.TimeToNext.IsEnabled = true;
                    }
                }

                toNext = newLine.Stations[index].CurrentToNext;
                toBack = newLine.Stations[index].PrevToCurrent;
                OnPropertyChanged(nameof(toNext));
                OnPropertyChanged(nameof(toBack));
            }
        }
        /// <summary>
        /// save the changes of distance/time
        /// </summary>
        private void saveButton(Window window)
        {
            NewLineView newLineView = window as NewLineView;
            if (newLineView.StationList.SelectedItem is Station selectedStation)
            {
                int index = newLineView.StationList.SelectedIndex;

                newLine.Stations[index].CurrentToNext = toNext;
                newLine.Stations[index].PrevToCurrent = toBack;

                ToNext = null;
                toBack = null;
                OnPropertyChanged(nameof(toNext));
                OnPropertyChanged(nameof(toBack));
            }
        }
        private void AddLineButton_click()
        {
            if (creatNewLine == null)
            {
                creatNewLine = new BackgroundWorker();
            }
            creatNewLine.RunWorkerCompleted +=
                (object sender, RunWorkerCompletedEventArgs args) =>
                {
                    if (!((BackgroundWorker)sender).CancellationPending)//if the BackgroundWorker didn't 
                    {                                                   //terminated before he done execute DoWork

                    }
                };//this function will execute in the main thread

            creatNewLine.DoWork +=
                (object sender, DoWorkEventArgs args) =>
                {
                    BackgroundWorker worker = (BackgroundWorker)sender;

                    BO.Line BOline = new BO.Line()//creating new BO line
                    {
                        LineNumber = (int)NewLine.LineNumber,
                        Area = (BO.AreasEnum)NewLine.Area
                    };
                    List<int?> distances = Stations.Select(lst => lst.Distance).ToList();
                    List<int?> times = Stations.Select(lst => lst.Time).ToList();
                    source.AddLine(BOline, Stations.Select(st => st.Station.BOstation), distances, times);
                };//this function will execute in the BackgroundWorker thread
            creatNewLine.RunWorkerAsync();

        }
        /// <summary>
        /// accured when search box is changing. replace the list in the window into list that contains the search box text.
        /// the search is according to the conbo box picking
        /// </summary>
        private void SearchBox_TextChanged(object sender)
        {
            NewLineView Lview = new NewLineView();
            if (sender is TextBox)
            {
                //get the ManegerView instance
                Lview = (((((sender as TextBox).Parent as StackPanel).Parent) as Grid).Parent) as NewLineView;
            }
            else if (sender is NewLineView)
            {
                Lview = sender as NewLineView;
            }
            if (Lview.ComboBoxSearch.SelectedItem != null)
            {
                //get the current presented List, get his name, and create a copy collection for searching
                var tempList = DBStations;
                if (tempList != null)
                {
                    //return a new collection according to the searching letters
                    Lview.DBStationList.ItemsSource = tempList.Where(c => c.GetType().GetProperty(Lview.ComboBoxSearch.Text).GetValue(c, null).ToString().Contains(Lview.SearchBox.Text));
                }
            }
        }
        #endregion

        #region help methods



        #endregion
    }
}
