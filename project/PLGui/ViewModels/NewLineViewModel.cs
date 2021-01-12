using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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

        #region fields and properties
        IBL source;
        BackgroundWorker load_data;
        BackgroundWorker creatNewLine;

        public List<string> ComboList { get; set; }

        Line newLine = new Line();
        public Line NewLine
        {
            get => newLine;
            set
            {
                SetProperty(ref newLine, value);
            }
        }

        public ObservableCollection<Station> DBStations { get; set; }
        public ObservableCollection<Station> Stations { get; set; }

        public bool IsMinStation { get => Stations.Count >= 2; }

        #endregion

        public NewLineViewModel()
        {
            source = BLFactory.GetBL("admin");
            Stations = new ObservableCollection<Station>();
            ComboList = new List<string>() { "Name", "Code", "Address" };
            loadData();

            SelectStationCommand = new RelayCommand<object>(SelectStation);
            AddLineButton = new RelayCommand(AddLineButton_click);
            DeleteStationCommand = new RelayCommand<object>(DeleteStation);
            SearchCommand = new RelayCommand<object>(SearchBox_TextChanged);
        }


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
        public ICommand SelectStationCommand { get; }
        public ICommand DeleteStationCommand { get; }
        public ICommand AddLineButton { get; }
        public ICommand SearchCommand { get; }


        private void SelectStation(object sender)
        {
            if ((sender as ListView).SelectedItem is Station selectedStation)
            {
                Stations.Add(selectedStation);
                DBStations.Remove(selectedStation);
                OnPropertyChanged("IsMinStation");

                NewLineView Lview = ((sender as ListView).Parent as Grid).Parent as NewLineView;
                SearchBox_TextChanged(Lview);
            }
        }
        private void DeleteStation(object sender)
        {
            if ((sender as ListView).SelectedItem is Station selectedStation)
            {
                DBStations.Add(selectedStation);
                Stations.Remove(selectedStation);
                OnPropertyChanged("IsMinStation");

                NewLineView Lview = (((sender as ListView).Parent as StackPanel).Parent as Grid).Parent as NewLineView;
                SearchBox_TextChanged(Lview);
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
                };//this function will execute in the main thred

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
