using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using BL.BLApi;
using BLApi;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PLGui.Models;
using PLGui.Models.PO;

namespace PLGui.ViewModels
{
    public class ManegerViewModel : ObservableRecipient 
    {
        ManegerModel manegerModel;
        IBL source;

        #region properties

        public ObservableCollection<Bus> Buses
        {
            get => manegerModel.Buses;
            set => SetProperty(ref manegerModel.Buses, value, true);
        }

        public ObservableCollection<Line> Lines
        {
            get => manegerModel.Lines;
            set => SetProperty(ref manegerModel.Lines, value, true);
        }
        public ObservableCollection<Station> Stations
        {
            get => manegerModel.Stations;
            set => SetProperty(ref manegerModel.Stations, value, true);
        }

        #endregion

        #region constractor
        public ManegerViewModel()
        {
            manegerModel = new ManegerModel();
            source = BLFactory.GetBL("admin");
            loadData();

            SearchCommand = new RelayCommand<object>(SearchBox_TextChanged);
            TabChangedCommand = new RelayCommand<object>(tab_selactionChange);
            StationListChangedCommand = new RelayCommand<object>(StationList_SelectionChanged);
            LineListChangedCommand = new RelayCommand<object>(LinesList_SelectionChanged);
            NewLine = new RelayCommand(Add_newLine);
        }

        #endregion

        #region load data
        BackgroundWorker load_data;
        private void loadData()
        {
            if(load_data == null)
            {
                load_data = new BackgroundWorker();
            }

            load_data.RunWorkerCompleted +=
                (object sender, RunWorkerCompletedEventArgs args) =>
                {
                    if (!((BackgroundWorker)sender).CancellationPending)//if the BackgroundWorker didn't 
                    {                                                   //terminated befor he done execute DoWork
                        manegerModel = (ManegerModel)args.Result;
                        OnPropertyChanged("Stations");
                        OnPropertyChanged("Buses");
                        OnPropertyChanged("Lines");
                    }
                };//this function will execute in the main thred

            load_data.DoWork +=
                (object sender, DoWorkEventArgs args) =>
                {
                    BackgroundWorker worker = (BackgroundWorker)sender;
                    ManegerModel result = new ManegerModel
                    {
                        Stations = new ObservableCollection<Station>(),
                        Buses = new ObservableCollection<Bus>(),
                        Lines = new ObservableCollection<Line>()
                    };
                    source.GetAllStations().DeepCopyToCollection(result.Stations);
                    source.GetAllLines().DeepCopyToCollection(result.Lines);
                    source.GetAllBuses().DeepCopyToCollection(result.Buses);


                    args.Result = worker.CancellationPending ? null : result;
                };//this function will execute in the BackgroundWorker thread
            load_data.RunWorkerAsync();
        }
        #endregion

        #region commands
        public ICommand SearchCommand { get; }
        public ICommand TabChangedCommand { get; }
        public ICommand StationListChangedCommand { get; }
        public ICommand LineListChangedCommand { get; }
        public ICommand NewLine { get; }



        private void SearchBox_TextChanged(object sender)
        {
            //get the ManegerView instance
            ManegerView Mview = (((((sender as TextBox).Parent as StackPanel).Parent) as Grid).Parent) as ManegerView;
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
        private void tab_selactionChange(object sender)
        {
            ManegerView Mview = (((sender as TabControl).Parent as Grid).Parent) as ManegerView;
            if ((Mview.mainTab.SelectedItem as TabItem).Content is ListView)
            {
                GridView currentGridView = ((Mview.mainTab.SelectedItem as TabItem).Content as ListView).View as GridView;
                List<string> comboList = (((Mview.mainTab.SelectedItem as TabItem).Content as ListView).View as GridView).Columns.Where(g => g.DisplayMemberBinding != null).Select(C => C.Header.ToString()).ToList();
                Mview.ComboBoxSearch.ItemsSource = comboList;
            }
        }
        private void StationList_SelectionChanged(object sender)
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
            }
        }
        private void LinesList_SelectionChanged(object sender)
        {
            ManegerView Mview = (((((sender as ListView).Parent as TabItem).Parent as TabControl).Parent as Grid).Parent) as ManegerView;
            if ((sender as ListView).SelectedItem is Line selectedLine)
            {
                Mview.Header1.Text = "Line Number:";
                Mview.content1.Content = selectedLine.LineNumber;

                Mview.Header2.Text = "Area:";
                Mview.content2.Content = selectedLine.Area;

                //Mview.Header3.Text = "Address:";
                //Mview.content3.Content = selectedLine.Address;

                //Mview.Header4.Text = "Location:";
                //Mview.content4.Content = selectedLine.Location;
            }
        }
        private void Add_newLine()
        {
            NewLineView newLineView = new NewLineView();
            newLineView.ShowDialog();
            loadData();
        }
        #endregion
    }
}
