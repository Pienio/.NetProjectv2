using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visits.EventArgsTypes;
using Visits.Services;

namespace Visits.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        protected DataServiceReference.IDataService _service;
        protected ILogUserService _loggedUser;

        public event PropertyChangedEventHandler PropertyChanged;

        protected ViewModel(DataServiceReference.IDataService factory, ILogUserService userLog)
        {
            _service = factory;
            _loggedUser = userLog;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
