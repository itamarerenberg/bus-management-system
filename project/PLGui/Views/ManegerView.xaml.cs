
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
                //get the current presented List, get his name, and create a copy collection for searching
                ListView currentList = ((mainTab.SelectedItem as TabItem).Content as ListView);
                string listName = ((mainTab.SelectedItem as TabItem).Header.ToString());
                var tempList = vModel.GetType().GetProperty(listName).GetValue(vModel, null) as ObservableCollection<Station>;//צריך שיהיה גנרי
                if (tempList != null)
                {
                    //return a new collection according to the searching letters
                    currentList.ItemsSource = tempList.Where(c => c.GetType().GetProperty(string.Concat(ComboBoxSearch.Text.Where(s => !char.IsWhiteSpace(s)))).GetValue(c, null).ToString().Contains(SearchBox.Text));
                }            }

        }

        private void StationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Station selectedStation = (Station)StationList.SelectedItem;
            if (selectedStation != null)
            {
                Header1.Visibility = Visibility.Visible;
                Header1.Text = "Name:";
                content1.Visibility = Visibility.Visible;
                content1.Content = selectedStation.Name;

                Header2.Visibility = Visibility.Visible;
                Header2.Text = "Code:";
                content2.Visibility = Visibility.Visible;
                content2.Content = selectedStation.Code;

                Header3.Visibility = Visibility.Visible;
                Header3.Text = "Address:";
                content3.Visibility = Visibility.Visible;
                content3.Content = selectedStation.Address;

                Header4.Visibility = Visibility.Visible;
                Header4.Text = "Location:";
                content4.Visibility = Visibility.Visible;
                content4.Content = selectedStation.Location;
            }
        }

        private void tab_selactionChange(object sender, SelectionChangedEventArgs e)
        {
            if ((mainTab.SelectedItem as TabItem).Content is ListView)
            {
                GridView currentGridView = ((mainTab.SelectedItem as TabItem).Content as ListView).View as GridView;
                List<string> comboList = (((mainTab.SelectedItem as TabItem).Content as ListView).View as GridView).Columns.Where(g => g.DisplayMemberBinding != null).Select(C => C.Header.ToString()).ToList();
                ComboBoxSearch.ItemsSource = comboList;
            }
        }
    }
}
