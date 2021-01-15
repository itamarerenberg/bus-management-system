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
        public ObservableCollection<Station> Stations { get; set; }//new/updated line stations

        public bool IsMinStation { get => Stations.Count >= 2; }

        #endregion

        #region constructors
        public NewLineViewModel()
        {
            source = BLFactory.GetBL("admin");
            Stations = new ObservableCollection<Station>();
            ComboList = new List<string>() { "Name", "Code", "Address" };
            loadData();

            SelectDBStationCommand = new RelayCommand<object>(SelectDBStation);
            SelectStationCommand = new RelayCommand<Window>(SelectStation);
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
        public ICommand AddLineButton { get; }
        public ICommand SearchCommand { get; }

        /// <summary>
        /// double click on data base station, the station will be added to the new/updated line stations
        /// </summary>
        private void SelectDBStation(object sender)
        {
            if ((sender as ListView).SelectedItem is Station selectedStation)
            {
                Stations.Add(selectedStation);
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
            if ((sender as ListView).SelectedItem is Station selectedStation)
            {
                DBStations.Add(selectedStation);
                Stations.Remove(selectedStation);
                OnPropertyChanged(nameof(IsMinStation));

                NewLineView Lview = (((sender as ListView).Parent as StackPanel).Parent as Grid).Parent as NewLineView;
                SearchBox_TextChanged(Lview);
            }
        }
        private void SelectStation(Window window)
        {
            NewLineView newLineView = window as NewLineView;
            if (newLineView.StationList.SelectedItem is Station selectedStation)
            {
                //if( 1= null)
                //toNext = selectedStation.
                //toBsck = selectedStation
                //OnPropertyChanged(nameof(toNext));
                //OnPropertyChanged(nameof(toBack));

                newLineView.StationName.Text = selectedStation.Name;

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
                    source.AddLine(BOline, Stations.Select(st => st.BOstation));
                    //source.AddLine(BOline, Stations.Select(s => (BO.Station)s.DeepCopyToNew(typeof(BO.Station))));הצעה
                };//this function will execute in the BackgroundWorker thread
            creatNewLine.RunWorkerAsync();

        }
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
