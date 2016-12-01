using JWLibrary.Winform.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JWLibrary.Winform.Container
{
    public class JWFlowLayoutPanel : FlowLayoutPanel
    {
        private IList<Control> _addedControls = new List<Control>();
        private object _dataSource;
        public object DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                this._dataSource = value;

                if (value != null)
                {                    
                    SetMatchControlById();
                    this.Enabled = true;
                }
                else
                {
                    this.Clear();
                    this.Enabled = false;
                }
            }
        }

        public Control FirstControl
        {
            get
            {
                return _addedControls[0];
            }
        }

        public Control LastControl
        {
            get
            {
                return _addedControls[_addedControls.Count - 1];
            }
        }

        public JWFlowLayoutPanel()
        {
            this.ControlAdded += JWFlowLayoutPanel_ControlAdded;
        }

        private void JWFlowLayoutPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control is TextBox || e.Control is DateTimePicker || e.Control is ComboBox || e.Control is Label)
            {
                e.Control.TabIndex = _addedControls.Count + 1;
                e.Control.KeyUp += Control_KeyUp;
                _addedControls.Add(e.Control);
            }
        }

        private void Control_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) {
               if (sender is Label) return;

                //find self index
                int findIdx = 0;
                foreach(var ctrl in _addedControls)
                {
                    if(sender == ctrl) { break; }
                    findIdx++;
                }

                for (int i = findIdx; i < _addedControls.Count; i++) {
                    if (i < _addedControls.Count - 1)
                    {
                        if (_addedControls[i + 1] is Label) continue;
                        _addedControls[i + 1].Focus();
                        _addedControls[i + 1].Select();
                        break;
                    }
                    else
                    {
                        _addedControls[i].Focus();
                        _addedControls[i].Select();
                        break;
                    }
                }
            }
        }

        public bool LockControl
        {
            set
            {
                if (value)
                {
                    LockControlValues(this);
                }
            }
        }

        private void LockControlValues(System.Windows.Forms.Control Container)
        {
            try
            {
                foreach (Control ctrl in _addedControls)
                {
                    if (ctrl.GetType() == typeof(TextBox))
                        ((TextBox)ctrl).ReadOnly = true;
                    if (ctrl.GetType() == typeof(ComboBox))
                        ((ComboBox)ctrl).Enabled = false;
                    if (ctrl.GetType() == typeof(CheckBox))
                        ((CheckBox)ctrl).Enabled = false;

                    if (ctrl.GetType() == typeof(DateTimePicker))
                        ((DateTimePicker)ctrl).Enabled = false;

                    if (ctrl.Controls.Count > 0)
                        LockControlValues(ctrl);
                }

            }
            catch
            {
                throw;
            }
        }

        private void SetMatchControlById()
        {
            for (int i = 0; i < _addedControls.Count; i++)
            {
                if (_addedControls[i] is Control)
                {
                    (_addedControls[i]).DataBindings.Clear();

                    if (((IBindingObject)_addedControls[i]).BindingName == "NONE") continue;

                    if (_addedControls[i] is TextBox) (_addedControls[i]).DataBindings.Add("Text", this.DataSource, ((IBindingObject)_addedControls[i]).BindingName);
                    else if (_addedControls[i] is CheckBox) (_addedControls[i]).DataBindings.Add("Value", this.DataSource, ((IBindingObject)_addedControls[i]).BindingName);
                    else if (_addedControls[i] is ComboBox) (_addedControls[i]).DataBindings.Add("Value", this.DataSource, ((IBindingObject)_addedControls[i]).BindingName);
                    else if (_addedControls[i] is DateTimePicker) (_addedControls[i]).DataBindings.Add("Value", this.DataSource, ((IBindingObject)_addedControls[i]).BindingName);
                    else if (_addedControls[i] is Label) (_addedControls[i]).DataBindings.Add("Text", this.DataSource, ((IBindingObject)_addedControls[i]).BindingName);
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _addedControls.Count; i++)
            {
                if (_addedControls[i] is TextBox) ((TextBox)_addedControls[i]).Text = string.Empty;
                else if (_addedControls[i] is ComboBox) ((ComboBox)_addedControls[i]).Text = string.Empty;
                else if (_addedControls[i] is DateTimePicker) ((DateTimePicker)_addedControls[i]).Value = DateTime.Now;
                else if (_addedControls[i] is CheckBox) ((CheckBox)_addedControls[i]).Checked = false;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //MessageBox.Show("ProcessCmdKey");
            return base.ProcessCmdKey(ref msg, keyData);
        }
    } 
}
