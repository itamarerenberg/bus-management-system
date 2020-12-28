using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Manager
    {
        public string Name { get; set; }
        public string Password { get; set; }
        Managment managment { get => Managment.Instance; }
    }
}
