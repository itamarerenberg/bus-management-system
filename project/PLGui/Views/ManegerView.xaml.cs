
using PLGui.Models;
using PLGui.Models.PO;
using PLGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace PLGui
{
    /// <summary>
    /// Interaction logic for Maneger.xaml
    /// </summary>
    public partial class ManegerView : Window
    {
        ManegerViewModel vModel;
        public ManegerView()
        {
            vModel = new ManegerViewModel();
            this.DataContext = vModel;
            InitializeComponent();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ComboBoxSearch.SelectedItem != null)
            {
                ListView currentList = ((mainTab.SelectedItem as TabItem).Content as ListView);
                string listName = ((mainTab.SelectedItem as TabItem).Header.ToString());
                var tempList = vModel.GetType().GetProperty(listName).GetValue(vModel, null) as ObservableCollection<Station>;//צריך שיהיה גנרי
                currentList.ItemsSource = tempList.Where(c => c.GetType().GetProperty(ComboBoxSearch.Text).GetValue(c, null).ToString().Contains(SearchBox.Text)); 
            }
        }

        private void StationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Station selectedStation = (Station)StationList.SelectedItem;
            if (selectedStation != null)
            {
                Header1.Visibility = Visibility.Visible;
                Header1.Text = "Name";
                content1.Visibility = Visibility.Visible;
                content1.Content = selectedStation.Name;
            }
        }

        private void tab_selactionChange(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                GridView currentGridView = ((mainTab.SelectedItem as TabItem).Content as ListView).View as GridView;
            }
            catch (Exception)
            {
                return;
            }            
            List<string> comboList = (((mainTab.SelectedItem as TabItem).Content as ListView).View as GridView).Columns.Where(g => g.DisplayMemberBinding != null ).Select(C => C.Header.ToString()).ToList();
            ComboBoxSearch.ItemsSource = comboList;
        }
    }
}
