using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BLApi;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace PLGui.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient
    {

        #region properties and fields
        IBL mangerBl;
        IBL passangerBl;
        private string name;
        private string password;
        private bool manegerCheckBox;

        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
            }
        }

        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
            }
        }

        public bool ManegerCheckBox
        {
            get => manegerCheckBox;
            set
            {
                SetProperty(ref manegerCheckBox, value);
            }
        }

        #endregion

        #region constructor
        public MainWindowViewModel()
        {
            passangerBl = BL.BLApi.BLFactory.GetBL("passenger");
            mangerBl = BL.BLApi.BLFactory.GetBL("admin");

            SignInCommand = new RelayCommand<Window>(SignIn);
            DebugButtonCommand = new RelayCommand(debugButton);
            CloseCommand = new RelayCommand(MainWindow_Closing);
        }
        #endregion

        #region commands

        public ICommand SignInCommand { get; }
        public ICommand DebugButtonCommand { get; }//temporery
        public ICommand CloseCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        private void SignIn(Window window)
        {
            if (ManegerCheckBox == true)
            {
                try
                {
                    if (mangerBl.GetManagar(Name, Password))
                    {
                        new ManegerView().Show();
                        window.Close();
                    }
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.Message, "the access is denied");
                }
            }

            else 
            {

            }
        }
        private void debugButton()
        {
            new ManegerView().ShowDialog();
            mangerBl.AddManagar("Admin", "1234");
        }

        private void MainWindow_Closing()
        {
            Environment.Exit(Environment.ExitCode);
        }
        #endregion
    }
}
