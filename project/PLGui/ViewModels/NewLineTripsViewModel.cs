using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    class NewLineTripsViewModel : ObservableRecipient
    {
        #region fields
        IBL source;
        private LineTrip lTrip; 
        #endregion

        #region properties
        public LineTrip LTrip
        {
            get => lTrip;
            set => SetProperty(ref lTrip, value);
        }

        public bool NewLineTripMode { get; set; }
        public string ButtonCaption { get; set; }
        public string WindowCaption { get; set; }

        #endregion

        #region constructor
        public NewLineTripsViewModel()
        {
            source = BLFactory.GetBL("admin");

            LTrip = WeakReferenceMessenger.Default.Send<RequestLineTrip>();//requests the old LineTrip (if exist)
            Line tempLine = WeakReferenceMessenger.Default.Send<RequestLine>();//requests the line of the lineTrip

            if (LTrip != null)// we are on updateing mode
            {
                NewLineTripMode = false;
                ButtonCaption = "Update";
                WindowCaption = $"Update the line trip of line number: {tempLine.LineNumber}";
            }
            else                    // we are on new LineTrip mode
            {
                LTrip = new LineTrip() {LineId = tempLine.ID };
                NewLineTripMode = true;
                ButtonCaption = "Set";
                WindowCaption = $"Set the line trip of line number: {tempLine.LineNumber}";
            }

            ButtonCommand = new RelayCommand<Window>(Set_Update_Button);
        } 
        #endregion

        #region commands
        public ICommand ButtonCommand { get; }

        private void Set_Update_Button(Window window)
        {
            if (NewLineTripMode == false)//if the view model on "updating mode"
            {
                try
                {
                    UpdateLineTrip();
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.Message, "ERROR");
                }            
            }
            else                        //New LineTrip Mode
            {
                try
                {
                    AddLineTrip();
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.Message, "ERROR");
                }
            }
            window.Close();
        }

        #endregion

        #region help methods

        BackgroundWorker addLineTripWorker;
        private void AddLineTrip()
        {
            if (addLineTripWorker == null)
            {
                addLineTripWorker = new BackgroundWorker();
            }
            addLineTripWorker.DoWork +=
                (object sender, DoWorkEventArgs args) =>
                {
                    BackgroundWorker worker = (BackgroundWorker)sender;
                    source.AddLineTrip(LTrip.BOlineTrip);
                };//this function will execute in the BackgroundWorker thread
            addLineTripWorker.RunWorkerAsync();
        }
        BackgroundWorker updateLineTripWorker;
        private void UpdateLineTrip()
        {
            if (updateLineTripWorker == null)
            {
                updateLineTripWorker = new BackgroundWorker();
            }
            updateLineTripWorker.DoWork +=
                (object sender, DoWorkEventArgs args) =>
                {
                    BackgroundWorker worker = (BackgroundWorker)sender;
                    source.UpdateLineTrip(LTrip.BOlineTrip);
                };//this function will execute in the BackgroundWorker thread
            updateLineTripWorker.RunWorkerAsync();
        }

        #endregion
    }
}
