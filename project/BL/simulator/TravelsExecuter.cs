using BL.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.simulator
{
    class TravelsExecuter
    {
        #region singelton

        TravelsExecuter() { }
        TravelsExecuter instance;
        public TravelsExecuter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TravelsExecuter();
                }
                return instance;
            }
        }

        #endregion

        event Action<LineTiming> observer;

        BackgroundWorker travelsExecuterWorker;
    }
}
