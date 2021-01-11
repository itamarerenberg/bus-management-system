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

            SignInCommand = new RelayCommand(SignIn);
            DebugButtonCommand = new RelayCommand(debugButton);
        }
        #endregion


        #region commands

        public ICommand SignInCommand { get; }
        public ICommand DebugButtonCommand { get; }

        private void SignIn()
        {
            if (ManegerCheckBox == true)
            {
                try
                {
                    if (mangerBl.GetManagar(Name, Password))
                    {
                        new ManegerView().ShowDialog();
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
        }
        #endregion
    }
}
