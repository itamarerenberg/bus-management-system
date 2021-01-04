using BLApi;
using PLGui.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        UserViewModel vModel;
        public MainWindow()
        {
            vModel = new UserViewModel();
            InitializeComponent();
            this.Closing += MainWindow_Closing;
            userName.TextChanged += UserNameChanged;
            password.PasswordChanged += passwordChanged;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void passwordChanged(object sender, RoutedEventArgs e)
        {
            vModel.Password = password.Password;
        }

        private void UserNameChanged(object sender, TextChangedEventArgs e)
        {
            vModel.Name = userName.Text;
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            bool isChecked = IsAdmin.IsChecked == null ? false : (bool)IsAdmin.IsChecked;
            try
            {
                vModel.signIn(isChecked);
            }
            catch
            {
                return;
            }
        }
    }
}
