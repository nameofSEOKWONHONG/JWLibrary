using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JWLibrary.Winform.DataViewControls
{
    public class DataGridViewProgressbarColumn : DataGridViewImageColumn
    {
        public DataGridViewProgressbarColumn()
        {
            //CellTemplate - DataGridViewImageColumn 상속
            CellTemplate = new DataGridViewProgressbarCell();
        }
    }

    #region GridViewCheckBoxColumn


    [System.Drawing.ToolboxBitmap(typeof(System.Windows.Forms.DataGridViewCheckBoxColumn))]
    public class GridViewCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        #region Constructor

        public GridViewCheckBoxColumn()
        {
            DatagridViewCheckBoxHeaderCell datagridViewCheckBoxHeaderCell = new DatagridViewCheckBoxHeaderCell();

            this.HeaderCell = datagridViewCheckBoxHeaderCell;
            this.Width = 50;

            //this.DataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grvList_CellFormatting);
            datagridViewCheckBoxHeaderCell.OnCheckBoxClicked += new CheckBoxClickedHandler(datagridViewCheckBoxHeaderCell_OnCheckBoxClicked);

        }

        #endregion

        #region Methods

        void datagridViewCheckBoxHeaderCell_OnCheckBoxClicked(int columnIndex, bool state)
        {
            DataGridView.RefreshEdit();

            foreach (DataGridViewRow row in this.DataGridView.Rows)
            {
                if (!row.Cells[columnIndex].ReadOnly)
                {
                    row.Cells[columnIndex].Value = state;
                }
            }
            DataGridView.RefreshEdit();
        }



        #endregion
    }

    #endregion

    #region DatagridViewCheckBoxHeaderCell

    public delegate void CheckBoxClickedHandler(int columnIndex, bool state);
    public class DataGridViewCheckBoxHeaderCellEventArgs : EventArgs
    {
        bool _bChecked;
        public DataGridViewCheckBoxHeaderCellEventArgs(int columnIndex, bool bChecked)
        {
            _bChecked = bChecked;
        }
        public bool Checked
        {
            get { return _bChecked; }
        }
    }
    class DatagridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
    {
        Point checkBoxLocation;
        Size checkBoxSize;
        bool _checked = false;
        Point _cellLocation = new Point();
        System.Windows.Forms.VisualStyles.CheckBoxState _cbState =
        System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
        public event CheckBoxClickedHandler OnCheckBoxClicked;

        public DatagridViewCheckBoxHeaderCell()
        {
        }

        protected override void Paint(System.Drawing.Graphics graphics,
        System.Drawing.Rectangle clipBounds,
        System.Drawing.Rectangle cellBounds,
        int rowIndex,
        DataGridViewElementStates dataGridViewElementState,
        object value,
        object formattedValue,
        string errorText,
        DataGridViewCellStyle cellStyle,
        DataGridViewAdvancedBorderStyle advancedBorderStyle,
        DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex,
            dataGridViewElementState, value,
            formattedValue, errorText, cellStyle,
            advancedBorderStyle, paintParts);
            Point p = new Point();
            Size s = CheckBoxRenderer.GetGlyphSize(graphics,
            System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
            p.X = cellBounds.Location.X +
            (cellBounds.Width / 2) - (s.Width / 2);
            p.Y = cellBounds.Location.Y +
            (cellBounds.Height / 2) - (s.Height / 2);
            _cellLocation = cellBounds.Location;
            checkBoxLocation = p;
            checkBoxSize = s;
            if (_checked)
                _cbState = System.Windows.Forms.VisualStyles.
                CheckBoxState.CheckedNormal;
            else
                _cbState = System.Windows.Forms.VisualStyles.
                CheckBoxState.UncheckedNormal;
            CheckBoxRenderer.DrawCheckBox
            (graphics, checkBoxLocation, _cbState);
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            Point p = new Point(e.X + _cellLocation.X, e.Y + _cellLocation.Y);
            if (p.X >= checkBoxLocation.X && p.X <=
            checkBoxLocation.X + checkBoxSize.Width
            && p.Y >= checkBoxLocation.Y && p.Y <=
            checkBoxLocation.Y + checkBoxSize.Height)
            {
                _checked = !_checked;
                if (OnCheckBoxClicked != null)
                {
                    OnCheckBoxClicked(e.ColumnIndex, _checked);
                    this.DataGridView.InvalidateCell(this);
                }
            }
            base.OnMouseClick(e);
        }

    }

    #endregion

    #region ColumnSelection

    class DataGridViewColumnSelector
    {
        // the DataGridView to which the DataGridViewColumnSelector is attached
        private DataGridView mDataGridView = null;
        // a CheckedListBox containing the column header text and checkboxes
        private CheckedListBox mCheckedListBox;
        // a ToolStripDropDown object used to show the popup
        private ToolStripDropDown mPopup;

        /// <summary>
        /// The max height of the popup
        /// </summary>
        public int MaxHeight = 300;
        /// <summary>
        /// The width of the popup
        /// </summary>
        public int Width = 200;

        /// <summary>
        /// Gets or sets the DataGridView to which the DataGridViewColumnSelector is attached
        /// </summary>
        public DataGridView DataGridView
        {
            get { return mDataGridView; }
            set
            {
                // If any, remove handler from current DataGridView
                if (mDataGridView != null) mDataGridView.CellMouseClick -= new DataGridViewCellMouseEventHandler(mDataGridView_CellMouseClick);
                // Set the new DataGridView
                mDataGridView = value;
                // Attach CellMouseClick handler to DataGridView
                if (mDataGridView != null) mDataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(mDataGridView_CellMouseClick);
            }
        }

        // When user right-clicks the cell origin, it clears and fill the CheckedListBox with
        // columns header text. Then it shows the popup.
        // In this way the CheckedListBox items are always refreshed to reflect changes occurred in
        // DataGridView columns (column additions or name changes and so on).
        void mDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex == -1 && e.ColumnIndex == 0)
            {
                mCheckedListBox.Items.Clear();
                foreach (DataGridViewColumn c in mDataGridView.Columns)
                {
                    mCheckedListBox.Items.Add(c.HeaderText, c.Visible);
                }
                int PreferredHeight = (mCheckedListBox.Items.Count * 16) + 7;
                mCheckedListBox.Height = (PreferredHeight < MaxHeight) ? PreferredHeight : MaxHeight;
                mCheckedListBox.Width = this.Width;
                mPopup.Show(mDataGridView.PointToScreen(new Point(e.X, e.Y)));
            }
        }

        // The constructor creates an instance of CheckedListBox and ToolStripDropDown.
        // the CheckedListBox is hosted by ToolStripControlHost, which in turn is
        // added to ToolStripDropDown.
        public DataGridViewColumnSelector()
        {
            mCheckedListBox = new CheckedListBox();
            mCheckedListBox.CheckOnClick = true;
            mCheckedListBox.ItemCheck += new ItemCheckEventHandler(mCheckedListBox_ItemCheck);

            ToolStripControlHost mControlHost = new ToolStripControlHost(mCheckedListBox);
            mControlHost.Padding = Padding.Empty;
            mControlHost.Margin = Padding.Empty;
            mControlHost.AutoSize = false;

            mPopup = new ToolStripDropDown();
            mPopup.Padding = Padding.Empty;
            mPopup.Items.Add(mControlHost);
        }

        public DataGridViewColumnSelector(DataGridView dgv)
            : this()
        {
            this.DataGridView = dgv;
        }

        // When user checks / unchecks a checkbox, the related column visibility is
        // switched.
        void mCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            mDataGridView.Columns[e.Index].Visible = (e.NewValue == CheckState.Checked);
        }
    }

    #endregion

    public class DataGridViewProgressbarCell : DataGridViewImageCell
    {
        static Image emptyimage;    //DataGridViewImageCell의 반환값과 동일하게 반환할 이미지객체

        static DataGridViewProgressbarCell()
        {
            emptyimage = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        public DataGridViewProgressbarCell()
        {
            this.ValueType = typeof(int);       //Cell값 타입 설정 : DataGridViewImageCell.ValueType
        }

        //Method 호출시 Progressbar Image를 반환한다.
        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, System.ComponentModel.TypeConverter valueTypeConverter, System.ComponentModel.TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            return emptyimage;
        }

        protected override void Paint(
            Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates elementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            if (null == value) value = 0;
            var vVal = value;
            int progressValue = (int)Convert.ChangeType(vVal, typeof(int));

            //double ProgressValue = 0;
            //if (value != null) ProgressValue = (double)value;

            float Percentage = ((float)progressValue / 100.0f); //현재 진행률 계산(float형)

            Brush backColorBrush = new SolidBrush(cellStyle.BackColor);
            Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor);

            //Progress Color 지정
            Brush ProgressColorBrush = new SolidBrush(Color.FromArgb(2, 148, 202));

            //Progressbar 시작위치(포인트), 크기 설정
            Rectangle ProgressBarBounds = new Rectangle();
            ProgressBarBounds.X = cellBounds.X + 2;     //가로 시작 위치(왼쪽 여백)
            ProgressBarBounds.Y = cellBounds.Y + (int)(cellBounds.Height / 4);     //세로 시작 위치(위쪽 여백)
            ProgressBarBounds.Width = Convert.ToInt32(Percentage * (cellBounds.Width - 4));  //ProgressBar 길이(오른쪽 여백)
            ProgressBarBounds.Height = (int)(cellBounds.Height * 0.55);   //높이(아래쪽 여백)

            //Progressbar 진행률 Text 위치 설정
            PointF ProgressStrPoint = new PointF();
            ProgressStrPoint.X = (float)cellBounds.X + (cellBounds.Width / 2) - 12;   //Cell 텍스트 시작 위치
            ProgressStrPoint.Y = (float)cellBounds.Y + (cellBounds.Height / 2) - 8;


            //           Color textColor = cellStyle.ForeColor;
            Color textColor = cellStyle.ForeColor;
            if ((elementState & DataGridViewElementStates.Selected) ==
            DataGridViewElementStates.Selected)
            {
                textColor = cellStyle.SelectionForeColor;
            }

            using (SolidBrush brush = new SolidBrush(textColor))
            //            using (SolidBrush brush = new SolidBrush(Color.FromArgb(65, 90, 140)))
            {
                //Default Cell을 그린다.
                base.Paint(
                    graphics,
                    clipBounds,
                    cellBounds,
                    rowIndex,
                    elementState,
                    value,
                    formattedValue,
                    errorText,
                    cellStyle,
                    advancedBorderStyle,
                    paintParts);

                if (Percentage >= 1.0)
                {
                    ProgressStrPoint.X = (float)cellBounds.X + (cellBounds.Width / 2) - 24;   //Cell 텍스트 시작 위치
                    //                     if (this.DataGridView.CurrentRow.Index == rowIndex)  //현재 Row
                    //                         graphics.DrawString("대기중..", cellStyle.Font, new SolidBrush(cellStyle.SelectionForeColor), ProgressStrPoint);
                    //                     else
                    graphics.DrawString("100%", cellStyle.Font, brush, ProgressStrPoint);
                }
                else if (Percentage >= 0.0)
                {
                    //ProgressBar를 그린다
                    graphics.FillRectangle(ProgressColorBrush, ProgressBarBounds);
                    graphics.DrawString(progressValue.ToString() + "%", cellStyle.Font, brush, ProgressStrPoint);

                }
                else
                {
                    ///****************************************
                    /// ProgressBar가 시작전일때(준비중, 대기중)
                    /// Row 선택됨에 따라 Font Color 변경
                    ///****************************************


                    if (this.DataGridView.CurrentRow != null)
                    {
                        ProgressStrPoint.X = (float)cellBounds.X + (cellBounds.Width / 2) - 24;   //Cell 텍스트 시작 위치
                        //                     if (this.DataGridView.CurrentRow.Index == rowIndex)  //현재 Row
                        //                         graphics.DrawString("대기중..", cellStyle.Font, new SolidBrush(cellStyle.SelectionForeColor), ProgressStrPoint);
                        //                     else
                        graphics.DrawString("0%", cellStyle.Font, brush, ProgressStrPoint);
                    }
                }
            }
        }
    }
}
