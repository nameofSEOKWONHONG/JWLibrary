using JWLibrary.Winform.Abstract;
using System;
using System.Windows.Forms;

namespace JWLibrary.Winform.CommonControls {

    public class JWComboBox : ComboBox, IBindingObject {

        public JWComboBox() {
            InitializeComponent();
            this.SelectedValueChanged += JWComboBox_SelectedValueChanged;
        }

        private void JWComboBox_SelectedValueChanged(object sender, EventArgs e) {
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

        private void InitializeComponent() {
            this.SuspendLayout();
            //
            // StoneCircleComboBox
            //
            this.ResumeLayout(false);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            //return base.ProcessCmdKey(ref msg, keyData);
            if (keyData == Keys.Enter) return false;
            if (keyData == Keys.Tab) return false;
            if (keyData == Keys.Up || keyData == Keys.Down) return false;
            return true;
        }
    }
}