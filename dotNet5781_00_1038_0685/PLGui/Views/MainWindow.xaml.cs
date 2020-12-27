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
        PO.User user;
        public MainWindow()
        {
            InitializeComponent();
            userName.TextChanged += UserNameChanged;
            password.TextChanged += passwordChanged;
        }

        private void passwordChanged(object sender, TextChangedEventArgs e)
        {
            user.Password = password.Text;
        }

        private void UserNameChanged(object sender, TextChangedEventArgs e)
        {
            user.Name = userName.Text;
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            if(!IsValidUser(user))
            {
                ErrorLabel.Content = "ERROR";
                ErrorLabel.Visibility = Visibility.Visible;
            }
            if(isAdmin(user))
            {
                Maneger
            }
        }
    }
}
