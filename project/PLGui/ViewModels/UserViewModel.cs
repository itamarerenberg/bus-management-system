using BLApi;
using PLGui.Models;
using PLGui.Models.PO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLGui.ViewModels
{
    class UserViewModel : INotifyPropertyChanged
    {
        IBL bl;
        UserModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public UserViewModel()
        {
            model = new UserModel();
            bl = BL.BLApi.BLFactory.GetBL("admin");
        }

        public string Name
        {
            get => model.name;
            set
            {
                model.name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public string Password
        {
            get => model.password;
            set
            {
                model.password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));
            }
        }

        public void signIn(bool isAdmin)
        {
            if(isAdmin)
            {
                if(bl.GetManagar(model.name, model.password))
                {
                    (new ManegerView()).ShowDialog();
                }
                else
                {
                    throw new InvalidDetails("Invalid details");
                }
            }
            else { }
        }
    }
}
