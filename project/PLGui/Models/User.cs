using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PLGui.Models
{
    public class User : DependencyObject
    {


        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(User));




        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(User));




        public bool Admin
        {
            get { return (bool)GetValue(AdminProperty); }
            set { SetValue(AdminProperty, value); }
        }
        public static readonly DependencyProperty AdminProperty =
            DependencyProperty.Register("Admin", typeof(bool), typeof(User));




        public List<UserTrip> Trips
        {
            get { return (List<UserTrip>)GetValue(TripsProperty); }
            set { SetValue(TripsProperty, value); }
        }
        public static readonly DependencyProperty TripsProperty =
            DependencyProperty.Register("Trips", typeof(List<UserTrip>), typeof(User), new PropertyMetadata(0));


    }
}


