using System;
using System.Collections.Generic;
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
        private Station tempStation;

        #region properties

        public Station TempStation
        {
            get => tempStation;
            set => SetProperty(ref tempStation, value);
        }

        public string ButtonCaption { get; set; }
        public bool NewStationMode { get; set; }

        #endregion

        #region constructor

        public MewStationViewModel()
        {
            TempStation = WeakReferenceMessenger.Default.Send<RequestStation>();//requests the old station (if exist)

            if (tempStation != null)// we are on updateing mode
            {
                NewStationMode = false;
                ButtonCaption = "Update Station"; 
            }
            else                    // we are on new station mode
            {
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
        private void AddStation()
        {
            throw new NotImplementedException();
        }

        private void UpdateStation()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
