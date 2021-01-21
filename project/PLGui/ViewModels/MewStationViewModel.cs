using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BL.BLApi;
using BLApi;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using PLGui.Models.PO;
using PLGui.utilities;

namespace PLGui.ViewModels
{
    public class MewStationViewModel : ObservableRecipient
    {
        IBL source;
        private Station station;

        #region properties

        public Station Station
        {
            get => station;
            set => SetProperty(ref station, value);
        }

        public string ButtonCaption { get; set; }
        public bool NewStationMode { get; set; }

        #endregion

        #region constructor

        public MewStationViewModel()
        {
            Station = WeakReferenceMessenger.Default.Send<RequestStation>();//requests the old station (if exist)

            if (station != null)// we are on updateing mode
            {
                NewStationMode = false;
                ButtonCaption = "Update Station"; 
            }
            else                    // we are on new station mode
            {
                station = new Station();
                NewStationMode = true;
                ButtonCaption = "Add Station";
            }

            source = BLFactory.GetBL("admin");

            ButtonCommand = new RelayCommand(Add_Update_Button);
        }

        #endregion

        #region commands
        public ICommand ButtonCommand { get; }

        private void Add_Update_Button()
        {
            if (NewStationMode == false)//if the view model on "updating mode"
            {
                UpdateStation();
            }
            else                        //New Station Mode
            {
                AddStation();
            }
        }
        #endregion

        #region Help methods
        BackgroundWorker addStationWorker;
        private void AddStation()
        {
            if(addStationWorker == null)
            {
                addStationWorker = new BackgroundWorker();
            }
            addStationWorker.DoWork +=
                (object sender, DoWorkEventArgs args) =>
                {
                    BackgroundWorker worker = (BackgroundWorker)sender;
                    source.AddStation(station.BOstation);
                };//this function will execute in the BackgroundWorker thread
            addStationWorker.RunWorkerAsync();
        }

        BackgroundWorker updateStationWorker;
        private void UpdateStation()
        {
            if (updateStationWorker == null)
            {
                updateStationWorker = new BackgroundWorker();
            }
            updateStationWorker.DoWork +=
                (object sender, DoWorkEventArgs args) =>
                {
                    source.UpdateStation(station.BOstation);
                };//this function will execute in the BackgroundWorker thread
            updateStationWorker.RunWorkerAsync();
        }
        #endregion
    }
}
