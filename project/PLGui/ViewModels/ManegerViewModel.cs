using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BLApi;
using BLApi;
using PLGui.Models;
using PLGui.Models.PO;

namespace PLGui.ViewModels
{
    public class ManegerViewModel : INotifyPropertyChanged
    {
        ManegerModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        IBL source;

        #region properties

        public ObservableCollection<Bus> Buses
        {
            get => model.Buses;
            set
            {
                model.Buses = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Buses"));
            }
        }

        public ObservableCollection<Line> Lines
        {
            get => model.Lines;
            set
            {
                model.Lines = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Lines"));
            }
        }
        public ObservableCollection<Station> Stations
        {
            get => model.Stations;
            set
            {
                model.Stations = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Stations"));
            }
        }

        #endregion

        #region constractors

        public ManegerViewModel()
        {
            model = new ManegerModel();
            source = BLFactory.GetBL("admin");
            loadData();
        }

        #endregion

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
                        model = (ManegerModel)args.Result;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Stations"));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Buses"));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Lines"));
                    }
                };//this function will execute in the main thred

            load_data.DoWork +=
                (object sender, DoWorkEventArgs args) =>
                {
                    BackgroundWorker worker = (BackgroundWorker)sender;
                    ManegerModel result = new ManegerModel();
                    result.Stations = new ObservableCollection<Station>();
                    //result.Buses = (ObservableCollection<BO.Bus>)source.GetAllBuses();//!possible problem: ther is no conversion from IEnumerable to ObservableColection
                    //result.Lines = (ObservableCollection<BO.Line>)source.GetAllLines();//same⬆
                    foreach (var st in source.GetAllStations())
                    {
                        result.Stations.Add(CopyHelper.Station_BO_PO(st));
                    }
                    args.Result = worker.CancellationPending ? null : result;
                };//this function will execute in the BackgroundWorker thread
            load_data.RunWorkerAsync();
        }


    }
}
