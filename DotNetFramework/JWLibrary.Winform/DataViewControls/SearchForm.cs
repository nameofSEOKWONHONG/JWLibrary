using JWLibrary.Winform.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace JWLibrary.Winform.DataGridViewControls
{
    public partial class SearchForm : BaseForm
    {
        private List<SearchCondition> _searchConditions = null;
        private DataGridView _dgv = null;
        private bool _isNext = false;
        private int _moveIdx = 0;

        private readonly int LOCATION_HEIGHT_ADD_VALUE = 100;
        private readonly int LOCATION_WIDTH_ADD_VALUE = 300;

        public SearchForm()
        {
            InitializeComponent();
        }

        public SearchForm(List<SearchCondition> searchConditions, DataGridView dgv)
        {
            InitializeComponent();

            this.Opacity = 0.5;

            _searchConditions = searchConditions;
            _dgv = dgv;

            this.Load += SearchForm_Load;
            this.btnFind.Click += (s, e) =>
            {
                _isNext = true;
                _moveIdx = 0;
                Search();
            };

            this.btnNext.Click += (s, e) =>
            {
                NextNPre(s);
            };
        }

        private void NextNPre(object sender)
        {
            if (_moveIdx < 0) _moveIdx = 0;
            if (_moveIdx > _dgv.Rows.Count - 1) _moveIdx = 0;

            if (sender.Equals(this.btnNext))
            {
                _moveIdx++;
                _isNext = true;
                Search();
            }
            else if (sender.Equals(this.btnPre))
            {
                _moveIdx--;
                _isNext = false;
                Search();
            }
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(this.tb_Search.Text))
            {
                MessageBox.Show("검색어가 없습니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_moveIdx < 0) _moveIdx = 0;
            bool isSearch = false;

            if (_isNext)
            {
                for (int i = _moveIdx; i < _dgv.Rows.Count; i++)
                {
                    string compareDest = this.tb_Search.Text;

                    if (string.IsNullOrEmpty(this.cbo_Condition.SelectedValue.ToString())) return;

                    string compareSrc = _dgv[(string)this.cbo_Condition.SelectedValue, i].FormattedValue.ToString();

                    if (compareSrc.Contains(compareDest))
                    {
                        _dgv.Rows[i].Selected = true;
                        _dgv.Rows[i].Cells[(string)this.cbo_Condition.SelectedValue].Selected = true;
                        _dgv.CurrentCell = _dgv.Rows[i].Cells[(string)this.cbo_Condition.SelectedValue];

                        _moveIdx = i;

                        isSearch = true;

                        break;
                    }
                }
            }
            else
            {
                for (int i = _moveIdx; i >= 0; i--)
                {
                    string compareDest = this.tb_Search.Text;
                    string compareSrc = _dgv[(string)this.cbo_Condition.SelectedValue, i].FormattedValue.ToString();

                    if (compareSrc.Contains(compareDest))
                    {
                        _dgv.Rows[i].Selected = true;
                        _dgv.Rows[i].Cells[(string)this.cbo_Condition.SelectedValue].Selected = true;
                        _dgv.CurrentCell = _dgv.Rows[i].Cells[(string)this.cbo_Condition.SelectedValue];

                        _moveIdx = i;

                        isSearch = true;

                        break;
                    }
                }
            }

            if (!isSearch)
            {
                if (_isNext) _moveIdx--;
                else _moveIdx++;
            }
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            this.cbo_Condition.DisplayMember = "Name";
            this.cbo_Condition.ValueMember = "Value";

            if (_searchConditions != null)
                this.cbo_Condition.DataSource = _searchConditions;

            this.Location = new Point(this.Location.X + LOCATION_WIDTH_ADD_VALUE, this.Location.Y - LOCATION_HEIGHT_ADD_VALUE);

            this.cbo_Condition.SelectedIndex = 0;
            this.cbo_Condition.Select();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case (Keys.Enter):
                    _isNext = true;
                    _moveIdx = 0;
                    Search();
                    break;
                default: break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            this.Opacity = 1.0;
        }

        protected override void OnDeactivate(EventArgs e)
        {            
            base.OnDeactivate(e);
            this.Opacity = 0.5;
        }

        public class SearchCondition
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}
