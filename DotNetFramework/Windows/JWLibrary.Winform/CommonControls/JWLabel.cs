using JWLibrary.Winform.Abstract;
using System.Windows.Forms;

namespace JWLibrary.Winform.CommonControls {

    public class JWLabel : Label, IBindingObject {
        private string _bindingName;

        public string BindingName {
            get { return _bindingName; }
            set {
                _bindingName = value;
            }
        }
    }
}