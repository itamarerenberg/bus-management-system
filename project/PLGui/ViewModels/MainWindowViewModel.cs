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

namespace PLGui.utilities
{
    public class MainWindowViewModel : ObservableRecipient
    {

        #region properties and fields
        IBL mangerBl;
        IBL passangerBl;
        private string name;
        private string password;
        private string newPasswod;
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

        public string NewPassword
        {
            get { return newPasswod; }
            set { newPasswod = value; }
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
            SignUpCommand = new RelayCommand<Window>(SignUp);
            DebugButtonCommand = new RelayCommand<Window>(debugButton);
            CloseCommand = new RelayCommand<MainWindow>(MainWindow_Closing);
            RegisterCommand = new RelayCommand<MainWindow>(Register);
            BackToSignInCommand = new RelayCommand<MainWindow>(BackToSignIn);
        }
        #endregion

        #region commands

        public ICommand SignInCommand { get; }
        public ICommand SignUpCommand { get; }
        public ICommand DebugButtonCommand { get; }//temporery
        public ICommand CloseCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand BackToSignInCommand { get; }

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
                try
                {
                    BO.Passenger passenger = passangerBl.GetPassenger(Name, Password);
                    MessageBox.Show("צריך לעשות חלון של נוסע");
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.Message, "the access is denied");
                }
            }
        }
        private void SignUp(Window window)
        {
            if (Password == newPasswod)
            {
                try
                {
                    passangerBl.AddPassenger(Name, Password);
                    MessageBox.Show("the user was added successfully!\nPlease sign in");
                    SwitchView(window as MainWindow, true);//return the view into sign in mode
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.Message, "the access is denied");
                }
            }
            else
            {
                MessageBox.Show("the passwords didn't match", "ERROR");
            }
        }
        private void debugButton(Window window)
        {
            new ManegerView().Show();
            window.Close();
        }

        private void MainWindow_Closing(MainWindow window)
        {
            window.Close();
            Environment.Exit(Environment.ExitCode);
        }

        private void Register(MainWindow window)
        {
            SwitchView(window);
        }
        private void BackToSignIn(MainWindow window)
        {
            SwitchView(window, true);
        }
        /// <summary>
        /// Switch the View between the sign in/sign up modes
        /// </summary>
        /// <param name="window"></param>
        /// <param name="reverse">true: back to sign in mode.  false: replace to sign up mode</param>
        private void SwitchView(MainWindow window, bool reverse = false)
        {
            if (reverse)
            {
                window.SignUpCard.Visibility = Visibility.Collapsed;
                window.SignUpButton.Visibility = Visibility.Collapsed;
                window.BackToSignInButton.Visibility = Visibility.Collapsed;

                window.SignInCard.Visibility = Visibility.Visible;
                window.SignInButton.Visibility = Visibility.Visible;
                window.RegisterButton.Visibility = Visibility.Visible;

                //clear the fields
                Name = "";
                Password = "";
                NewPassword = "";
            }
            else//change to sign up mode
            {
                window.SignUpCard.Visibility = Visibility.Visible;
                window.SignUpButton.Visibility = Visibility.Visible;
                window.BackToSignInButton.Visibility = Visibility.Visible;

                window.SignInCard.Visibility = Visibility.Collapsed;
                window.SignInButton.Visibility = Visibility.Collapsed;
                window.RegisterButton.Visibility = Visibility.Collapsed;
            }
        }
        #endregion
    }
}
