using BLApi;
using PLGui.ViewModels;
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
        UserViewModel vModel;
        public MainWindow()
        {
            vModel = new UserViewModel();
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
            vModel.Name = userName.Text;
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vModel.signIn();
            }
            catch
            {
                this.Close();
                return;
            }
        }
    }
}
