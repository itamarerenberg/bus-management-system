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
        #region private fields
        private ObservableCollection<Bus> buses;
        private List<Bus> busesList;
        private string readyBussesMsg;
        private string busyBussesMsg;
        private string needTreatBussesMsg;
        private string totalBaussesMsg;
        #endregion

        #region properties
        public ObservableCollection<Bus> Buses
        {
            get => buses;
            set
            {
                buses = value;
                BusesList = value.ToList<Bus>();
                TotalBaussesMsg = $"Total busses: {Buses.Count}";
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
        /// <summary>
        /// string maessage: how many busses are ready for drive
        /// </summary>
        public string ReadyBussesMsg
        {
            get => readyBussesMsg;
            set
            {
                readyBussesMsg = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReadyBussesMsg"));
            }
        }
        /// <summary>
        /// string maessage: how many busses are busy
        /// </summary>
        public string BusyBussesMsg
        {
            get => busyBussesMsg;
            set
            {
                busyBussesMsg = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BusyBussesMsg"));
            }
        }
        /// <summary>
        /// string maessage: how many busses need treatment
        /// </summary>
        public string NeedTreatBussesMsg
        {
            get => needTreatBussesMsg;
            set
            {
                needTreatBussesMsg = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NeedTreatBussesMsg"));
            }
        }
        /// <summary>
        /// string maessage: how many busses in this collection
        /// </summary>
        public string TotalBaussesMsg
        {
            get => totalBaussesMsg;
            set
            {
                totalBaussesMsg = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TotalBaussesMsg"));
            }
        } 
        #endregion

        #region constractor
        public busCollection(ObservableCollection<Bus> list)
        {
            Buses = list;
            Buses.CollectionChanged += Buses_CollectionChanged;
            //register each PropertyChangedEvent of Bus to the item_PropertyChanged Handler
            foreach (Object item in Buses)
            {
                (item as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
            }
        }
        #endregion

        #region events handeling

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// occur when adding a bus to the collection.
        /// actions:  
        /// 1. register the PropertyChangedEvent of the new Bus to the item_PropertyChanged Handler
        /// 2. update the list
        /// 3. update all messages
        /// </summary>
        private void Buses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                //register the PropertyChangedEvent of the new Bus to the item_PropertyChanged Handler
                foreach (Object item in e.NewItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
                }
                BusesList = Buses.ToList<Bus>();
                TotalBaussesMsg = $"Total busses: {Buses.Count}";
                update_messages();
            }
        }

        /// <summary>
        /// occur when Property of any Bus in the collection is changed.
        /// </summary>
        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            update_messages();
        }


        private void update_messages()
        {
            int num = BusesList.FindAll(b => b.Stat == StatEnum.READY).Count();//number of busses with the status READY
            switch (num)
            {
                case 0:
                    ReadyBussesMsg = "no busses available for driving!";
                    break;
                case 1:
                    ReadyBussesMsg = "one bus is ready for driving!";
                    break;
                default:
                    ReadyBussesMsg = $"{num} busses are ready for driving";
                    break;
            }

            num = BusesList.FindAll(b => b.Stat == StatEnum.IN_REFUELING ||
            b.Stat == StatEnum.IN_TREATMENT || b.Stat == StatEnum.IS_TRAVELING).Count();//number of busses with the status IN_REFUELING or IN_TREATMENT
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
            num = BusesList.FindAll(b => b.Stat == StatEnum.NEED_TREATMENT).Count();// number of busses with the status NEED_TREATMENT
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
        #endregion
    }


}
