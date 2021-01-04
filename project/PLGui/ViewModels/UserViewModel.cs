using PLGui.Models;
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
        UserModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public UserViewModel()
        {
            model = new UserModel();
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
    }
}
