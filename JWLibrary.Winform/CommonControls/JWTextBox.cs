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
    public class JWTextBox : TextBox, IBindingObject
    {
        private INPUT_MODE _inputMode;
        public INPUT_MODE InputMode
        {
            get { return _inputMode; }
            set
            {
                _inputMode = value;

                if (value == INPUT_MODE.IP)
                    this.MaxLength = 15;
                else
                    this.MaxLength = 32767;
            }
        }

        /// <summary>
        /// String Format for Double
        /// </summary>
        public string NumberFormat { get; set; }
        public string Number
        {
            get
            {
                return this.Text.Replace(",", "");
            }
        }

        public JWTextBox()
        {
            _inputMode = INPUT_MODE.GENERAL;

            this.TextChanged += JWTextBox_TextChanged;
            this.Leave += JWTextBox_Leave;
        }

        private void JWTextBox_Leave(object sender, EventArgs e)
        {
            if (((BindingsCollection)this.DataBindings).Count > 0)
            {
                ((BindingsCollection)this.DataBindings)[0].WriteValue();
            }
        }

        private void JWTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_inputMode == INPUT_MODE.CURRENCY)
            {
                if (string.IsNullOrEmpty(this.Text)) return;

                string str = this.Text.Replace(",", "");

                if (str == "0")
                {
                    this.TextChanged -= JWTextBox_TextChanged;
                    this.Text = string.Empty;
                    this.TextChanged += JWTextBox_TextChanged;
                    return;
                }

                if (!string.IsNullOrEmpty(str))
                {
                    bool isChk = false;
                    long num = 0;

                    isChk = long.TryParse(str, out num);

                    if (isChk)
                    {
                        this.TextChanged -= JWTextBox_TextChanged;
                        this.Text = string.Format("{0:" + NumberFormat + "}", num);
                        this.TextChanged += JWTextBox_TextChanged;

                        this.SelectionStart = this.Text.Length;
                        this.SelectionLength = 0;
                    }
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool isChk = true;

            if (_inputMode == INPUT_MODE.GENERAL)
            {
                isChk = false;
            }
            else if (_inputMode == INPUT_MODE.CURRENCY || _inputMode == INPUT_MODE.IP)
            {
                //숫자키
                if ((keyData >= Keys.D0 && keyData <= Keys.D9))
                {
                    isChk = false; ;
                }

                //넘버패드
                if ((keyData >= Keys.NumPad0 && keyData <= Keys.NumPad9))
                {
                    isChk = false;
                }

                if ((int)keyData == 109)
                {
                    isChk = false;
                }


                //넘버패드
                if (keyData == Keys.OemMinus)
                {
                    isChk = false;
                }

                //OemPeriod RAlt 위의 점버튼
                //Decimal number pad의 점버튼
                if (_inputMode == INPUT_MODE.IP)
                {
                    if (keyData == Keys.OemPeriod || keyData == Keys.Decimal)
                    {
                        isChk = false;
                    }
                }

                //기능키 모음
                if (keyData == Keys.Back || keyData == Keys.Delete || keyData == Keys.Tab || keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.Enter)
                {
                    isChk = false;
                }

                if (
                    (keyData == (Keys.Control | Keys.C)) ||
                    (keyData == (Keys.Control | Keys.V))
                    )
                {
                    isChk = false;
                }
            }
            return isChk;
            //return base.ProcessCmdKey(ref msg, keyData);

        }

        private string _bindingName;
        public string BindingName
        {
            get { return _bindingName; }
            set
            {
                _bindingName = value;                
            }
        }

        public enum INPUT_MODE
        {
            CURRENCY,
            IP,
            GENERAL
        }
    }
}
