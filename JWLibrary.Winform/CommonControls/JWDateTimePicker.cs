using JWLibrary.Winform.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JWLibrary.Winform.CommonControls
{
    public class JWDateTimePicker : DateTimePicker, IBindingObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        private string _bindingName;
        public string BindingName
        {
            get { return _bindingName; }
            set
            {
                _bindingName = value;
                OnPropertyChanged("_bindingName");
            }
        }
    }
}
