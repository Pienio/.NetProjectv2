using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.DataServiceReference;

namespace AdminPanel.ViewModels
{
    class ViewModel : INotifyPropertyChanged
    {
        protected static DataServiceClient _service = new DataServiceClient();
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public static void CloseService()
        {
            _service.Dispose();
            _service.Abort();
        }
    }
}
