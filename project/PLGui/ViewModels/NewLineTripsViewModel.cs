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

namespace PLGui.utilities
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

        private DateTime start;

        public DateTime Start
        {
            get => start;
            set => SetProperty(ref start, value);
        }

        private DateTime finish;

        public DateTime Finish
        {
            get => finish;
            set => SetProperty(ref finish, value);
        }

        private DateTime frequency;

        public DateTime Frequency
        {
            get => frequency;
            set => SetProperty(ref frequency, value);
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

                Start += LTrip.StartTime;
                Finish += LTrip.Finish;
                Frequency += LTrip.Frequency;
            }
            else                    // we are on new LineTrip mode
            {
                LTrip = new LineTrip() {LineId = tempLine.ID };
                NewLineTripMode = true;
                ButtonCaption = "Set";
                WindowCaption = $"Set the line trip of line number: {tempLine.LineNumber}";
            }

            ButtonCommand = new RelayCommand<Window>(Set_Update_Button);
            CloseCommand = new RelayCommand<Window>(Close);
        }

        
        #endregion

        #region commands
        public ICommand ButtonCommand { get; }
        public ICommand CloseCommand { get; }


        private void Set_Update_Button(Window window)
        {
            
            if (IsValid())
            {
                LTrip.StartTime = Start.TimeOfDay;
                if (Frequency.TimeOfDay != TimeSpan.Zero)//if Frequency is set
                {
                    LTrip.Finish = Finish.TimeOfDay == TimeSpan.Zero ? new TimeSpan(1, 0, 0, 0) : Finish.TimeOfDay;//if finish is 00:00 => finish = 1 day
                }
                else
                {
                    LTrip.Finish = TimeSpan.Zero;
                }
                LTrip.Frequency = Frequency.TimeOfDay;

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
        }
        /// <summary>
        /// validation
        /// </summary>
        /// <returns></returns>
        private bool IsValid()
        {
            if (Finish != new DateTime() && Frequency == new DateTime())//finish is set while Frequency is empty
            {
                MessageBox.Show("Frequency cannot be empty", "ERROR");
                return false;
            }
            if (Frequency != new DateTime())
            {
                Finish = Finish.TimeOfDay == TimeSpan.Zero ? Finish.AddDays(1) : Finish;//if finish is 00:00 => finish = 1 day
                if (Finish < Start)     //Frequency is set while start is bigger then finish
                {
                    MessageBox.Show("finish cannot be before the start", "ERROR");
                    Finish = Finish.TimeOfDay == TimeSpan.Zero ? Finish.AddDays(-1) : Finish;//set the previous value
                    return false;
                }
                if (Frequency.TimeOfDay >= Finish - Start)
                {
                    MessageBox.Show("Frequency cannot be less the difernet between start and finish", "ERROR");
                    Finish = Finish.TimeOfDay == TimeSpan.Zero ? Finish.AddDays(-1) : Finish;//set the previous value
                    return false;
                }
            }
            return true;
        }

        private void Close(Window window)
        {
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
