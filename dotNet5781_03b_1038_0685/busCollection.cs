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
        private string readyBussesMsg;
        private string busyBussesMsg;
        private string needTreatBussesMsg;

        public ObservableCollection<Bus> Buses
        {
            get => buses;
            set
            {
                buses = value;
                BusesList = value.ToList<Bus>();
                update_messages();
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
        public string ReadyBussesMsg
        {
            get => readyBussesMsg;
            set
            {
                readyBussesMsg = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReadyBussesMsg"));
            }
        }
        public string BusyBussesMsg { get => busyBussesMsg;
            set
            {
                busyBussesMsg = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BusyBussesMsg"));
            }
        }
        public string NeedTreatBussesMsg
        {
            get => needTreatBussesMsg;
            set
            {
                needTreatBussesMsg = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NeedTreatBussesMsg"));
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
                BusesList = Buses.ToList<Bus>();
                update_messages();
            }
        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            update_messages();
        }

        private void update_messages()
        {
            int num = BusesList.FindAll(b => b.Stat == StatEnum.READY).Count();
            switch (num)
            {
                case 0:
                    ReadyBussesMsg = "no busses available for driving!";
                    break;
                case 1:
                    ReadyBussesMsg = "one bus is ready for driving!";
                    break;
                default:
                    ReadyBussesMsg= $"{num} busses are ready for driving";
                    break;
            }
            num = BusesList.FindAll(b => b.Stat == StatEnum.IN_REFUELING || 
            b.Stat == StatEnum.IN_TREATMENT || b.Stat == StatEnum.IS_TRAVELING).Count();
            switch (num)
            {
                case 0:
                    BusyBussesMsg = "";
                    break;
                case 1:
                    BusyBussesMsg = "one bus is busy";
                    break;
                default:
                    BusyBussesMsg = $"{num} busses are busy";
                    break;
            }
            num = BusesList.FindAll(b => b.Stat == StatEnum.NEED_TREATMENT).Count();
            switch (num)
            {
                case 0:
                    NeedTreatBussesMsg = "";
                    break;
                case 1:
                    NeedTreatBussesMsg = "one bus needs treatment!";
                    break;
                default:
                    NeedTreatBussesMsg = $"{num} busses need treatment!";
                    break;
            }
        }
    }


}
