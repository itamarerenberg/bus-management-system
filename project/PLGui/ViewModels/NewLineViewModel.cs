using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            loadData();

            SelectStationCommand = new RelayCommand<object>(SelectStation);
            AddLineButton = new RelayCommand(AddLineButton_click);
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
                    source.GetAllStations().DeepCopyToCollection(result);
                    args.Result = worker.CancellationPending ? null : result;
                };//this function will execute in the BackgroundWorker thread
            load_data.RunWorkerAsync();
        }
        #endregion

        #region commands
        public ICommand SelectStationCommand { get; }
        public ICommand AddLineButton { get; }

        private void SelectStation(object sender)
        {
            if ((sender as ListView).SelectedItem is Station selectedStation)
            {
                Stations.Add(selectedStation);
                OnPropertyChanged("IsMinStation");
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
                    source.AddLine(BOline);

                    for (int i = 0; i < Stations.Count; i++)
                    {
                        source.AddLineStation(1, Stations[i].Code, i);//creating new BO line stations//צריך לשנות!!!!!!!!!!!!
                    }
                };//this function will execute in the BackgroundWorker thread
            creatNewLine.RunWorkerAsync();

        }
        #endregion
    }
}
