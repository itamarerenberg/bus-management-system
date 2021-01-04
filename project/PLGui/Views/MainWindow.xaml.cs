using BLApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PLGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL bl;
        User user;
        public MainWindow()
        {
            InitializeComponent();
            bl = null;//=>GetBl();
            userName.TextChanged += UserNameChanged;
            password.PasswordChanged += passwordChanged;
            bl = BL.BLApi.BLFactory.GetBL("admin");
        }

        private void passwordChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserNameChanged(object sender, TextChangedEventArgs e)
        {
            user.Name = userName.Text;
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            //if(!IsValidUser(user))
            //{
            //    ErrorLabel.Content = "ERROR";
            //    ErrorLabel.Visibility = Visibility.Visible;
            //}
            //if(isAdmin(user))
            //{
            //    Maneger
            //}
        }
    }
}
