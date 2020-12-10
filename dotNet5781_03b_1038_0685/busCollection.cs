using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_03b_1038_0685
{
    public class busCollection : INotifyPropertyChanged
    {
        private ObservableCollection<Bus> buses = new ObservableCollection<Bus>();
        private List<Bus> busesList;
        private string readyBussesNomber;

        public ObservableCollection<Bus> Buses
        {
            get => buses;
            set
            {
                buses = value;
                BusesList = value.ToList<Bus>();
                ReadyBussesNomber = $"{BusesList.FindAll(b => b.Stat == StatEnum.READY).Count()}  busses are ready for driving";
            }
        }
        public List<Bus> BusesList
        {
            get => busesList;
            set
            {
                busesList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BusesList"));
            }
        }
        public string ReadyBussesNomber
        {
            get => readyBussesNomber;
            set
            {
                readyBussesNomber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReadyBussesNomber"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public busCollection(ObservableCollection<Bus> list)
        {
            Buses = list;
            Buses.CollectionChanged += Buses_CollectionChanged;
            foreach (Object item in Buses)
            {
                (item as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
            }
        }

        private void Buses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Object item in e.NewItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
        }

        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ReadyBussesNomber = $"{BusesList.FindAll(b => b.Stat == StatEnum.READY).Count()}  busses are ready for driving";
        }
    }

    
}
