using JWLibrary.Winform.Abstract;
using System;
using System.Windows.Forms;

namespace JWLibrary.Winform.CommonControls {

    public class JWRichTextBox : RichTextBox, IBindingObject {

        public JWRichTextBox() {
            this.TextChanged += JWRichTextBox_TextChanged;
        }

        private void JWRichTextBox_TextChanged(object sender, EventArgs e) {
            if (((BindingsCollection)this.DataBindings).Count > 0) {
                ((BindingsCollection)this.DataBindings)[0].WriteValue();
            }
        }

        private string _bindingName;

        public string BindingName {
            get { return _bindingName; }
            set {
                _bindingName = value;
            }
        }
    }
}