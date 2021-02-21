using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BLApi;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace PLGui.utilities
{
    public class MainWindowViewModel : ObservableValidator
    {

        #region properties and fields
        IBL mangerBl;
        IBL passengerBl;
        private string name = "Admin";
        private string password ="1234";
        private string newPasswod;
        private bool manegerCheckBox = true;

        private BO.Passenger passenger;

        [Required(ErrorMessage = "Name cannot be empty", AllowEmptyStrings =false)]
        public string Name
        {
            get => name;
            set
            {
                value = value ?? "";
                SetProperty(ref name, value, true);
            }

        }
        [Required(ErrorMessage = "Password cannot be empty")]
        public string Password
        {
            get => password;
            set
            {
                value = value ?? "";
                SetProperty(ref password, value, true);
            }
        }
        public string NewPassword
        {
            get => newPasswod;
            set
            {
                SetProperty(ref newPasswod, value, true);
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
            passengerBl = BL.BLApi.BLFactory.GetBL("passenger");
            mangerBl = BL.BLApi.BLFactory.GetBL("admin");

            //commands initalize
            SignInCommand = new RelayCommand<MainWindow>(SignIn);
            SignUpCommand = new RelayCommand<MainWindow>(SignUp);
            CloseCommand = new RelayCommand<MainWindow>(MainWindow_Closing);
            RegisterCommand = new RelayCommand<MainWindow>(Register);
            BackToSignInCommand = new RelayCommand<MainWindow>(BackToSignIn);

            //messengers initalize
            RequestPassengerMessege();
        }
        #endregion

        #region commands

        public ICommand SignInCommand { get; }
        public ICommand SignUpCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand BackToSignInCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        private void SignIn(MainWindow window)
        {
            //invoke the binding for validation
            window.NameIn.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            window.PasswordIn.GetBindingExpression(PasswordBox.PasswordProperty).UpdateSource();
            if (!this.HasErrors)
            {
                if (ManegerCheckBox == true) //enter as maneger
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
                else                        //enter as passanger
                {
                    try
                    {
                        passenger = passengerBl.GetPassenger(Name, Password);
                        new PassengerView().Show();
                        window.Close();
                    }
                    catch (Exception msg)
                    {
                        MessageBox.Show(msg.Message, "the access is denied");
                    }
                }
            }
        }
            
        private void SignUp(MainWindow window)
        {
            //invoke the binding for validation
            window.NameUp.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            window.PasswordUp.GetBindingExpression(PasswordBox.PasswordProperty).UpdateSource();

            if (Password == newPasswod)
            {
                try
                {
                    passengerBl.AddPassenger(Name, Password);
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
        /// <param name="reverse">true: back to sign in mode. false: replace to sign up mode</param>
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

        private void RequestPassengerMessege()
        {
            //reply to the RequestPassenger messege by sending the Passenger
            WeakReferenceMessenger.Default.Register<MainWindowViewModel, RequestPassenger>(this, (r, m) =>
            {
                m.Reply(r.passenger);
            });
        }
        #endregion
    }
}
