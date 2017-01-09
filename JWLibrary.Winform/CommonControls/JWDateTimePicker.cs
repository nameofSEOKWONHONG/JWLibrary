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
    public class JWDateTimePicker : DateTimePicker, IBindingObject
    {
        private string _bindingName;
        public string BindingName
        {
            get { return _bindingName; }
            set
            {
                _bindingName = value;
            }
        }
    }
}
