using JWLibrary.Winform.DataGridViewControls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static JWLibrary.Winform.DataGridViewControls.SearchForm;

namespace JWLibrary.Winform.DataViewControls {

    public class JWDataGridView : DataGridView {

        #region member variable

        private int _checkCnt = 0;
        private bool _isCheckBoxHeaderVisible = false;
        private string _new_value;
        private string _old_value;
        private string[] _inputValidate = new string[] { "" };
        private Dictionary<string, string> _inputValue = new Dictionary<string, string>();
        private bool isAsc = false;
        private bool _isLineColor = true;

        #endregion member variable

        #region property

        public int SelectedRowCount {
            get {
                int row = this.RowCount;
                int idx = 0;

                for (int i = row - 1; i >= 0; i--) {
                    if (this["CHK", i] != null) {
                        if (this["CHK", i].Value != null) {
                            if (this["CHK", i].Value.ToString() == "True") {
                                idx++;
                            }
                        }
                    }
                }

                return idx;
            }
        }

        public List<object> SelectedItem {
            get {
                List<object> retObjs = new List<object>();

                int row = this.RowCount;

                for (int i = row - 1; i >= 0; i--) {
                    if (this["CHK", i] != null) {
                        if (this["CHK", i].Value != null) {
                            if (this["CHK", i].Value.ToString() == "True") {
                                retObjs.Add(this.Rows[i].DataBoundItem);
                            }
                        }
                    }
                }

                return retObjs;
            }
        }

        #endregion property

        #region construct

        public JWDataGridView() {
            Initialize();
        }

        private void Initialize() {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            //
            // MyDataGridView
            //
            this.ReadOnly = false;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2;
            this.RowTemplate.Height = 23;
            this.ColumnHeadersHeightChanged += JWDataGridView_ColumnHeadersHeightChanged;
            this.RowHeadersWidthChanged += JWDataGridView_RowHeadersWidthChanged;
            this.DataBindingComplete += JWDataGridView_DataBindingComplete;
            this.CellClick += JWDataGridView_CellClick;
            this.CellEndEdit += JWDataGridView_CellEndEdit;
            this.CellBeginEdit += JWDataGridView_CellBeginEdit;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.ColumnHeaderMouseClick += JWDataGridView_ColumnHeaderMouseClick;
            this.CellMouseUp += JWDataGridView_CellMouseUp;
            this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            this.AutoSize = false;
        }

        private void JWDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (this.Columns[e.ColumnIndex].Name == "index") return;

            try {
                if (!isAsc) {
                    this.Sort(this.Columns[e.ColumnIndex], System.ComponentModel.ListSortDirection.Ascending);
                    isAsc = true;
                } else {
                    this.Sort(this.Columns[e.ColumnIndex], System.ComponentModel.ListSortDirection.Descending);
                    isAsc = false;
                }
            } catch {
            }
        }

        private void JWDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {
            if (this[e.ColumnIndex, e.RowIndex].Value == null) return;

            _old_value = this[e.ColumnIndex, e.RowIndex].Value.ToString();
        }

        private void JWDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            if (this[e.ColumnIndex, e.RowIndex].Value == null) return;

            _new_value = this[e.ColumnIndex, e.RowIndex].Value.ToString();

            if (_old_value == _new_value) return;

            //if (!this.Columns.Contains("EDITOBJECT")) return;
            //if (this.Rows[e.RowIndex].Cells["EDITOBJECT"].Value == null) return;

            //if (this.Rows[e.RowIndex].Cells["EDITOBJECT"].Value.ToString() == Enums.GRID_EDIT_MODE.INSERT.ToString()) return;

            //if (this.Columns[e.ColumnIndex].Name == "CHK")
            //{
            //    if (this[e.ColumnIndex, e.RowIndex].Value != null)
            //        this.Rows[e.RowIndex].Cells["EDITOBJECT"].Value = Enums.GRID_EDIT_MODE.MODIFY;
            //}
            //else
            //{
            //    this.Rows[e.RowIndex].Cells["EDITOBJECT"].Value = Enums.GRID_EDIT_MODE.MODIFY;
            //}
        }

        private void JWDataGridView_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex > 1) {
                /*if (_helpTexts != null && this.TextItem != null)
                {
                    try
                    {
                        this.TextItem.Text = _helpTexts[e.ColumnIndex - 2];
                    }
                    catch
                    {
                        this.TextItem.Text = "...";
                    }
                }*/
            }
        }

        private void JWDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {
            if (this.Columns[0].Visible) {
                foreach (DataGridViewRow r in this.Rows) {
                    try {
                        r.Cells["index"].Value = r.Index + 1;

                        if (_isLineColor) {
                            if ((r.Index + 1) % 2 == 0) {
                                r.DefaultCellStyle.BackColor = Color.FromArgb(233, 238, 240);
                            } else {
                                r.DefaultCellStyle.BackColor = Color.White;
                            }
                        }
                    } catch { /*dt바인딩시 오류 처리*/ }
                }
            }
        }

        #endregion construct

        #region control event

        private void JWDataGridView_RowHeadersWidthChanged(object sender, EventArgs e) {
            if (_isCheckBoxHeaderVisible) {
                Control[] controls = this.Controls.Find("CHK", true);

                if (controls != null && controls.Count() > 0) {
                    Rectangle rect = this.GetCellDisplayRectangle(0, -1, true);
                    Size s = new System.Drawing.Size(rect.Width / 2, rect.Height / 2 + 2);
                    rect.X = (rect.Width / 4) + 3; //- rect //rect.Location.X + (rect.Width  / 4);
                    rect.Y = (rect.Height / 4);// +(rect.Height / 4);

                    controls[0].Location = rect.Location;
                }
            }
        }

        private void JWDataGridView_ColumnHeadersHeightChanged(object sender, EventArgs e) {
        }

        private void JWDataGridView_KeyUp(object sender, KeyEventArgs e) {
            if (e.Control && (e.KeyCode == Keys.F)) {
                List<SearchCondition> list = new List<SearchCondition>();

                for (int i = 0; i < this.Columns.Count; i++) {
                    if (this.Columns[i].Visible) {
                        if (!string.IsNullOrEmpty(this.Columns[i].HeaderText) && this.Columns[i].HeaderText != "순번")
                            list.Add(new SearchCondition() { Name = this.Columns[i].HeaderText, Value = this.Columns[i].DataPropertyName });
                    }
                }

                SearchForm form = new SearchForm(list, this);

                form.StartPosition = FormStartPosition.CenterScreen;

                form.Show();
                form.BringToFront();
            }

            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) {
                DataGridView grid = (DataGridView)sender;

                if (grid != null) {
                    try {
                        int row = grid.SelectedCells[0].RowIndex;
                        int col = grid.SelectedCells[0].ColumnIndex;

                        DataGridViewCellEventArgs args = new DataGridViewCellEventArgs(col, row);

                        this.OnCellClick(args);
                    } catch {
                    }
                }
            }

            if ((e.Control) && e.KeyCode == Keys.C)
                Clipboard.SetDataObject(this.GetClipboardContent().GetText());
        }

        private void JWDataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                DataGridView grid = (DataGridView)sender;

                if (grid != null) {
                    ContextMenuStrip menu = new ContextMenuStrip();
                    menu.Items.Add("EXCEL 출력", null, new EventHandler(itemCsv_Click));
                    Point pt = grid.PointToClient(Control.MousePosition);
                    menu.Show(this, pt.X, pt.Y);
                }
            }
        }

        private void chkHeader_CheckedChanged(object sender, EventArgs e) {
            int chkCnt = 0;

            for (int i = 0; i < this.RowCount; i++) {
                this[0, i].Value = ((CheckBox)this.Controls.Find("CHK", true)[0]).Checked;

                if (this[0, i].Value.Equals(true)) chkCnt++;
            }

            _checkCnt = chkCnt;

            this.EndEdit();
        }

        public void ExportExcel(string fileName) {
            DataTable dt = GetDataTableFromDGV();

            if (dt != null) {
                //ExcelExport.CreateExcelDocument(dt, fileName);
            }
        }

        private DataTable GetDataTableFromDGV() {
            var dt = new DataTable();
            int columnCount = 0;

            foreach (DataGridViewColumn column in this.Columns) {
                if (column.Visible) {
                    // You could potentially name the column based on the DGV column name (beware of dupes)
                    // or assign a type based on the data type of the data bound to this DGV column.
                    dt.Columns.Add(column.HeaderText);
                    columnCount++;
                }
            }

            List<object> cellValues = new List<object>();

            foreach (DataGridViewRow row in this.Rows) {
                for (int i = 0; i < row.Cells.Count; i++) {
                    if (this.Columns[i].Visible)
                        cellValues.Add(row.Cells[i].Value);
                }
                dt.Rows.Add(cellValues.ToArray());

                cellValues = new List<object>();
            }

            return dt;
        }

        public void ExportCsv(string fileName) {
            if (this.RowCount > 0) {
                StreamWriter pFilerWriter = new StreamWriter(fileName, false, System.Text.Encoding.Default);

                for (int i = 0; i < this.Columns.Count; i++) {
                    if (this.Columns[i].Visible) {
                        if (i > 0) {
                            pFilerWriter.Write(",");
                        }

                        pFilerWriter.Write(this.Columns[i].HeaderText);
                    }
                }

                pFilerWriter.WriteLine();

                // DataGridView의 각행을 읽어 CSV로 변환
                // pFileWrite가 CSV 파일 핸들
                foreach (DataGridViewRow row in this.Rows) {
                    bool bIsFirstValue = true;
                    foreach (DataGridViewCell cell in row.Cells) {
                        // 첫번째 셀 값이 없다면 이 행은 무시한다
                        if (bIsFirstValue && null == cell.FormattedValue)
                            break;

                        // 빈값
                        if (null == cell.FormattedValue)
                            continue;

                        if (cell.ColumnIndex > 0)
                            if (!cell.Visible)
                                continue;

                        if (!bIsFirstValue)
                            pFilerWriter.Write(",");

                        bIsFirstValue = false;

                        pFilerWriter.Write("\"" + cell.FormattedValue.ToString().Replace("\"", "\"\"") + "\"");
                    }
                    pFilerWriter.WriteLine();
                }

                pFilerWriter.Close();
            } else {
                MessageBox.Show("출력할 내용이 없습니다.");
            }
        }

        private void ItemExcel_Click(object sender, EventArgs e) {
            using (SaveFileDialog dlg = new SaveFileDialog()) {
                //dlg.InitialDirectory = @"C:\";
                dlg.Title = "EXCEL 저장";
                dlg.DefaultExt = "xlsx";
                dlg.Filter = "xlsx (*.xlsx)|*.xlsx";
                dlg.FilterIndex = 0;
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == DialogResult.OK) {
                    this.ExportExcel(dlg.FileName);
                }
            }
        }

        private void itemCsv_Click(object sender, EventArgs e) {
            using (SaveFileDialog dlg = new SaveFileDialog()) {
                //dlg.InitialDirectory = @"C:\";
                dlg.Title = "CSV 저장";
                dlg.DefaultExt = "csv";
                dlg.Filter = "CSV (*.csv)|*.csv";
                dlg.FilterIndex = 0;
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == DialogResult.OK) {
                    this.ExportCsv(dlg.FileName);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (keyData == Keys.Enter) {
                if (CurrentCell != null) {
                    if (this.CurrentCell.Visible) {
                        int icolumn = this.CurrentCell.ColumnIndex;
                        int irow = this.CurrentCell.RowIndex;

                        if (icolumn == this.Columns.Count - 2) {
                            if (irow == this.Rows.Count - 1) return true;

                            this.CurrentCell = this[0, irow + 1];
                        } else {
                            for (int i = icolumn + 1; i < this.Columns.Count; i++) {
                                if (this[i, irow].Visible) {
                                    this.CurrentCell = this[i, irow];
                                    break;
                                }
                            }
                        }
                    }
                }
                return true;
            } else
                return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion control event

        #region setting function

        public void SetGrid(
                   bool isCheckBoxHeaderVisible,
                   bool isImageColumnVisible,
                   bool isSearchViewEnable,
                   bool isContextMenuVisible,
                   bool isMultiSelect,
                   bool isIndexColumnVisible,
                   bool[] isColReadOnly,
                   bool[] isColVisible,
                   string[] colNames,
                   string[] colHeaderTexts,
                   int[] colWidths,
                   int[] colMinWidths,
                   int[] colFillWeights,
                   int[] displayIndexs,
                   DataGridViewContentAlignment[] rowCellStyleAlignments,
                   DataGridViewContentAlignment[] headerAlingments,
                   bool isDebugVisible = false) {

            #region 기본설정

            //다중 선택을 위한 체크박스 활성화
            this._isCheckBoxHeaderVisible = isCheckBoxHeaderVisible;

            //기본설정
            this.AutoGenerateColumns = false;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.RowHeadersVisible = false;

            int columnCnt = colNames.Length + 2;

            /*if (isIndexColumnVisible)
                columnCnt = columnCnt + 1;*/

            this.ColumnCount = columnCnt;
            int isdf = 0;
            try {
                for (int i = 0; i < columnCnt; i++) {
                    isdf = i;
                    /*if (i == 0)
                    {
                        /*DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                        chk.Name = "CHK";
                        chk.Visible = isCheckBoxHeaderVisible;
                        chk.Width = 40;
                        chk.ReadOnly = false;
                        this.Columns.Insert(0, chk);
                        /*this.Columns[i].Name = "CHK";
                        this.Columns[i].HeaderText = "";
                        this.Columns[i].Visible = isCheckBoxHeaderVisible;
                        this.Columns[i].Width = 40;                        */

                    /*Rectangle rect = this.GetCellDisplayRectangle(0, -1, true);
                    Size s = new System.Drawing.Size(rect.Width / 2, rect.Height / 2 + 2);
                    rect.X = (rect.Width / 4) + 3; //- rect //rect.Location.X + (rect.Width  / 4);
                    rect.Y = (rect.Height / 4);// +(rect.Height / 4);

                    CheckBox chkHeader = new CheckBox();
                    chkHeader.Name = "chkHeader";
                    chkHeader.Size = s;
                    chkHeader.Location = rect.Location;
                    chkHeader.CheckedChanged += chkHeader_CheckedChanged;
                    this.Controls.Add(chkHeader);
                    this.Columns[0].ReadOnly = false;*/
                    //}
                    if (i == 0) {
                        this.Columns[i].Name = "index";
                        this.Columns[i].HeaderText = "순번";
                        this.Columns[i].Visible = isIndexColumnVisible;
                        this.Columns[i].Width = 40;
                        this.Columns[i].ReadOnly = true;
                        this.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        if (isIndexColumnVisible)
                            this.DataBindingComplete -= JWDataGridView_DataBindingComplete;
                    } else if (i == this.ColumnCount - 1) {
                        this.Columns[i].Name = "EDITOBJECT"; //컬럼 이름
                        this.Columns[i].HeaderText = "EDITOBJECT"; //표시될 TEXT
                        this.Columns[i].DataPropertyName = "EDITOBJECT"; //바인딩될 이름 (컬럼명과 동일설정)
                        this.Columns[i].Width = 100; //컬럼 폭
                        this.Columns[i].Visible = isDebugVisible;
                    } else {
                        //기본설정
                        this.Columns[i].Name = colNames[i - 1];
                        this.Columns[i].HeaderText = colHeaderTexts[i - 1];
                        this.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        this.Columns[i].Tag = colNames[i - 1];
                        this.Columns[i].DataPropertyName = colNames[i - 1];

                        //읽기속성
                        if (isColReadOnly != null) this.Columns[i].ReadOnly = isColReadOnly[i - 1];
                        else this.Columns[i].ReadOnly = true;

                        //표시속성
                        if (isColVisible != null) this.Columns[i].Visible = isColVisible[i - 1];

                        //컬럼 폭
                        if (colWidths != null) this.Columns[i].Width = colWidths[i - 1];
                        //else this.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                        //컬럼 최소 폭
                        if (colMinWidths != null) this.Columns[i].MinimumWidth = colMinWidths[i - 1];

                        //컬럼 폭 비율 지정
                        //if (colFillWeights != null && this.Columns[i].Visible)
                        //this.Columns[i].FillWeight = colFillWeights[i];

                        //표시 순서 지정
                        if (displayIndexs != null && this.Columns[i].Visible)
                            this.Columns[i].DisplayIndex = displayIndexs[i - 1];

                        //컬럼 Cell 정렬상태
                        if (rowCellStyleAlignments != null)
                            this.Columns[i].DefaultCellStyle.Alignment = rowCellStyleAlignments[i - 1];
                        else
                            DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //컬럼 헤더 정렬 상태
                        if (headerAlingments != null)
                            this.Columns[i].HeaderCell.Style.Alignment = headerAlingments[i - 1];
                        else
                            this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    this.Columns[i].Resizable = DataGridViewTriState.True;
                    this.Columns[i].Frozen = false;
                    //this.Columns[i].SortMode = DataGridViewColumnSortMode.Programmatic;
                }

                this.BackgroundColor = Color.FromArgb(239, 243, 244);
                this.GridColor = Color.FromArgb(217, 218, 220);

                this.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(239, 243, 244);
                this.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                this.DefaultCellStyle.ForeColor = Color.FromArgb(65, 90, 140);
                this.DefaultCellStyle.SelectionForeColor = Color.FromArgb(255, 255, 255);
                this.DefaultCellStyle.SelectionBackColor = Color.FromArgb(104, 203, 240);

                this.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                //this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                //this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //this.ColumnHeadersHeight = 30;
                this.RowTemplate.Height = 25;
                this.RowTemplate.MinimumHeight = 25;

                //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                //this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                //this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.MultiSelect = isMultiSelect;
                this.BackgroundColor = Color.White;

                #endregion 기본설정

                #region 체크박스 컬럼 생성 시 수행

                if (isCheckBoxHeaderVisible) {
                    DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn();
                    chkColumn.Name = "CHK";
                    chkColumn.HeaderText = "";
                    chkColumn.Width = 30;
                    chkColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this.Columns.Insert(0, chkColumn);

                    /*Rectangle rect = this.GetCellDisplayRectangle(0, -1, true);
                    Size s = new System.Drawing.Size(rect.Width / 2, rect.Height / 2 + 2);
                    rect.X = (rect.Width / 4) + 3; //- rect //rect.Location.X + (rect.Width  / 4);
                    rect.Y = (rect.Height / 4);// +(rect.Height / 4);

                    CheckBox chkHeader = new CheckBox();
                    chkHeader.Name = "chkHeader";
                    chkHeader.Size = s;
                    chkHeader.Location = rect.Location;
                    chkHeader.CheckedChanged += chkHeader_CheckedChanged;
                    this.Controls.Add(chkHeader);
                    this.Columns[0].ReadOnly = false;*/
                }

                #endregion 체크박스 컬럼 생성 시 수행

                #region 이미지 컬럼 생성시 수행

                if (isImageColumnVisible) {
                    DataGridViewColumn column = null;

                    if (isCheckBoxHeaderVisible) column = this.Columns[1];
                    else column = this.Columns[0];

                    if (column != null) {
                        DataGridViewImageColumn imgCol = new DataGridViewImageColumn();

                        imgCol.Name = column.Name;
                        imgCol.HeaderText = column.HeaderText;
                        imgCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                        imgCol.Tag = column.Tag;
                        imgCol.DataPropertyName = column.DataPropertyName;
                        imgCol.ReadOnly = column.ReadOnly;
                        imgCol.Visible = column.Visible;
                        imgCol.Width = column.Width;
                        imgCol.MinimumWidth = column.MinimumWidth;
                        imgCol.ValueType = column.ValueType;
                        imgCol.DefaultCellStyle.Alignment = column.DefaultCellStyle.Alignment;
                        imgCol.HeaderCell.Style.Alignment = column.HeaderCell.Style.Alignment;

                        if (isCheckBoxHeaderVisible) {
                            this.Columns.RemoveAt(1);
                            this.Columns.Insert(1, imgCol);
                        } else {
                            this.Columns.RemoveAt(0);
                            this.Columns.Insert(0, imgCol);
                        }
                    }
                }

                #endregion 이미지 컬럼 생성시 수행

                if (isContextMenuVisible) {
                    this.CellMouseUp += JWDataGridView_CellMouseUp;
                }

                this.MultiSelect = isMultiSelect;

                if (isSearchViewEnable) {
                    this.KeyUp += JWDataGridView_KeyUp;
                }

                this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                this.ScrollBars = ScrollBars.Both;
                this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            } catch (Exception e) {
                Debug.WriteLine("grid settng exception : " + e.Message);
            }

            //컬럼 사이즈 고정 (height)
            this.AllowUserToResizeColumns = false;

            //로우 사이즈 고정 (height)
            this.AllowUserToResizeRows = false;
        }

        public void SetGrid(
                   bool isCheckBoxHeaderVisible,
                   bool isImageColumnVisible,
                   bool isSearchViewEnable,
                   bool isContextMenuVisible,
                   bool isMultiSelect,
                   bool isIndexColumnVisible,
                   bool[] isColReadOnly,
                   bool[] isColVisible,
                   DataGridViewSelectionMode selectMode,
                   string[] colNames,
                   string[] colHeaderTexts,
                   int[] colWidths,
                   int[] colMinWidths,
                   int[] colFillWeights,
                   int[] displayIndexs,
                   DataGridViewContentAlignment[] rowCellStyleAlignments,
                   DataGridViewContentAlignment[] headerAlingments,
                   bool isDebugVisible = false) {

            #region 기본설정

            //다중 선택을 위한 체크박스 활성화
            this._isCheckBoxHeaderVisible = isCheckBoxHeaderVisible;

            //기본설정
            this.AutoGenerateColumns = false;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.RowHeadersVisible = false;

            int columnCnt = colNames.Length + 2;

            /*if (isIndexColumnVisible)
                columnCnt = columnCnt + 1;*/

            this.ColumnCount = columnCnt;
            int isdf = 0;
            try {
                for (int i = 0; i < columnCnt; i++) {
                    isdf = i;
                    /*if (i == 0)
                    {
                        /*DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                        chk.Name = "CHK";
                        chk.Visible = isCheckBoxHeaderVisible;
                        chk.Width = 40;
                        chk.ReadOnly = false;
                        this.Columns.Insert(0, chk);
                        /*this.Columns[i].Name = "CHK";
                        this.Columns[i].HeaderText = "";
                        this.Columns[i].Visible = isCheckBoxHeaderVisible;
                        this.Columns[i].Width = 40;                        */

                    /*Rectangle rect = this.GetCellDisplayRectangle(0, -1, true);
                    Size s = new System.Drawing.Size(rect.Width / 2, rect.Height / 2 + 2);
                    rect.X = (rect.Width / 4) + 3; //- rect //rect.Location.X + (rect.Width  / 4);
                    rect.Y = (rect.Height / 4);// +(rect.Height / 4);

                    CheckBox chkHeader = new CheckBox();
                    chkHeader.Name = "chkHeader";
                    chkHeader.Size = s;
                    chkHeader.Location = rect.Location;
                    chkHeader.CheckedChanged += chkHeader_CheckedChanged;
                    this.Controls.Add(chkHeader);
                    this.Columns[0].ReadOnly = false;*/
                    //}
                    if (i == 0) {
                        this.Columns[i].Name = "index";
                        this.Columns[i].HeaderText = "순번";
                        this.Columns[i].Visible = isIndexColumnVisible;
                        this.Columns[i].Width = 40;
                        this.Columns[i].ReadOnly = true;
                        this.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    } else if (i == this.ColumnCount - 1) {
                        this.Columns[i].Name = "EDITOBJECT"; //컬럼 이름
                        this.Columns[i].HeaderText = "EDITOBJECT"; //표시될 TEXT
                        this.Columns[i].DataPropertyName = "EDITOBJECT"; //바인딩될 이름 (컬럼명과 동일설정)
                        this.Columns[i].Width = 100; //컬럼 폭
                        this.Columns[i].Visible = isDebugVisible;
                    } else {
                        //기본설정
                        this.Columns[i].Name = colNames[i - 1];
                        this.Columns[i].HeaderText = colHeaderTexts[i - 1];
                        this.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        this.Columns[i].Tag = colNames[i - 1];
                        this.Columns[i].DataPropertyName = colNames[i - 1];

                        //읽기속성
                        if (isColReadOnly != null) this.Columns[i].ReadOnly = isColReadOnly[i - 1];
                        else this.Columns[i].ReadOnly = true;

                        //표시속성
                        if (isColVisible != null) this.Columns[i].Visible = isColVisible[i - 1];

                        //컬럼 폭
                        if (colWidths != null) this.Columns[i].Width = colWidths[i - 1];
                        //else this.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                        //컬럼 최소 폭
                        if (colMinWidths != null) this.Columns[i].MinimumWidth = colMinWidths[i - 1];

                        //컬럼 폭 비율 지정
                        //if (colFillWeights != null && this.Columns[i].Visible)
                        //this.Columns[i].FillWeight = colFillWeights[i];

                        //표시 순서 지정
                        if (displayIndexs != null && this.Columns[i].Visible)
                            this.Columns[i].DisplayIndex = displayIndexs[i - 1];

                        //컬럼 Cell 정렬상태
                        if (rowCellStyleAlignments != null)
                            this.Columns[i].DefaultCellStyle.Alignment = rowCellStyleAlignments[i - 1];
                        else
                            DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //컬럼 헤더 정렬 상태
                        if (headerAlingments != null)
                            this.Columns[i].HeaderCell.Style.Alignment = headerAlingments[i - 1];
                        else
                            this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    this.Columns[i].Resizable = DataGridViewTriState.True;
                    this.Columns[i].Frozen = false;
                    //this.Columns[i].SortMode = DataGridViewColumnSortMode.Programmatic;
                }

                this.BackgroundColor = Color.FromArgb(239, 243, 244);
                this.GridColor = Color.FromArgb(217, 218, 220);

                this.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(239, 243, 244);
                this.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                this.DefaultCellStyle.ForeColor = Color.FromArgb(65, 90, 140);
                this.DefaultCellStyle.SelectionForeColor = Color.FromArgb(255, 255, 255);
                this.DefaultCellStyle.SelectionBackColor = Color.FromArgb(104, 203, 240);

                this.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                //this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                //this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //this.ColumnHeadersHeight = 30;
                this.RowTemplate.Height = 25;
                this.RowTemplate.MinimumHeight = 25;

                //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                //this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                //this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.MultiSelect = isMultiSelect;
                this.BackgroundColor = Color.White;

                #endregion 기본설정

                #region 체크박스 컬럼 생성 시 수행

                if (isCheckBoxHeaderVisible) {
                    DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn();
                    chkColumn.Name = "CHK";
                    chkColumn.HeaderText = "";
                    chkColumn.Width = 30;
                    chkColumn.DataPropertyName = "CHK";
                    chkColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this.Columns.Insert(0, chkColumn);
                    this.Columns[0].ReadOnly = false;
                    chkColumn.TrueValue = true;
                    chkColumn.FalseValue = false;
                    //chkColumn.change += chkHeader_CheckedChanged;

                    /*Rectangle rect = this.GetCellDisplayRectangle(0, -1, true);
                    Size s = new System.Drawing.Size(rect.Width / 2, rect.Height / 2 + 2);
                    rect.X = (rect.Width / 4) + 3; //- rect //rect.Location.X + (rect.Width  / 4);
                    rect.Y = (rect.Height / 4);// +(rect.Height / 4);

                    CheckBox chkHeader = new CheckBox();
                    chkHeader.Name = "chkHeader";
                    chkHeader.Size = s;
                    chkHeader.Location = rect.Location;
                    chkHeader.CheckedChanged += chkHeader_CheckedChanged;
                    this.Controls.Add(chkHeader);
                    this.Columns[0].ReadOnly = false;*/
                }

                #endregion 체크박스 컬럼 생성 시 수행

                #region 이미지 컬럼 생성시 수행

                if (isImageColumnVisible) {
                    DataGridViewColumn column = null;

                    if (isCheckBoxHeaderVisible) column = this.Columns[1];
                    else column = this.Columns[0];

                    if (column != null) {
                        DataGridViewImageColumn imgCol = new DataGridViewImageColumn();

                        imgCol.Name = column.Name;
                        imgCol.HeaderText = column.HeaderText;
                        imgCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                        imgCol.Tag = column.Tag;
                        imgCol.DataPropertyName = column.DataPropertyName;
                        imgCol.ReadOnly = column.ReadOnly;
                        imgCol.Visible = column.Visible;
                        imgCol.Width = column.Width;
                        imgCol.MinimumWidth = column.MinimumWidth;
                        imgCol.ValueType = column.ValueType;
                        imgCol.DefaultCellStyle.Alignment = column.DefaultCellStyle.Alignment;
                        imgCol.HeaderCell.Style.Alignment = column.HeaderCell.Style.Alignment;

                        if (isCheckBoxHeaderVisible) {
                            this.Columns.RemoveAt(1);
                            this.Columns.Insert(1, imgCol);
                        } else {
                            this.Columns.RemoveAt(0);
                            this.Columns.Insert(0, imgCol);
                        }
                    }
                }

                #endregion 이미지 컬럼 생성시 수행

                if (isContextMenuVisible) {
                    this.CellMouseUp += JWDataGridView_CellMouseUp;
                }

                this.MultiSelect = isMultiSelect;

                if (isSearchViewEnable) {
                    this.KeyUp += JWDataGridView_KeyUp;
                }

                this.SelectionMode = selectMode;
                this.ScrollBars = ScrollBars.Both;
            } catch (Exception e) {
                Debug.WriteLine("grid settng exception : " + e.Message);
            }

            //컬럼 사이즈 고정 (height)
            this.AllowUserToResizeColumns = false;

            //로우 사이즈 고정 (height)
            this.AllowUserToResizeRows = false;
        }

        public void SetGrid(
                   bool isCheckBoxHeaderVisible,
                   bool isImageColumnVisible,
                   bool isSearchViewEnable,
                   bool isContextMenuVisible,
                   bool isMultiSelect,
                   bool isIndexColumnVisible,
                   bool[] isColReadOnly,
                   bool[] isColVisible,
                   string[] comboBoxColumn,
                   NameValueCollection comboBoxColumnItem,
                   string[] checkBoxColumn,
                   DataGridViewSelectionMode selectMode,
                   string[] colNames,
                   string[] colHeaderTexts,
                   int[] colWidths,
                   int[] colMinWidths,
                   int[] colFillWeights,
                   int[] displayIndexs,
                   DataGridViewContentAlignment[] rowCellStyleAlignments,
                   DataGridViewContentAlignment[] headerAlingments,
                   bool isDebugVisible = false) {

            #region 기본설정

            //다중 선택을 위한 체크박스 활성화
            this._isCheckBoxHeaderVisible = isCheckBoxHeaderVisible;

            //기본설정
            this.AutoGenerateColumns = false;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.RowHeadersVisible = false;

            int columnCnt = colNames.Length + 2;

            /*if (isIndexColumnVisible)
                columnCnt = columnCnt + 1;*/

            this.ColumnCount = columnCnt;
            int isdf = 0;
            try {
                try {
                    for (int i = 0; i < columnCnt; i++) {
                        isdf = i;
                        /*if (i == 0)
                        {
                            /*DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                            chk.Name = "CHK";
                            chk.Visible = isCheckBoxHeaderVisible;
                            chk.Width = 40;
                            chk.ReadOnly = false;
                            this.Columns.Insert(0, chk);
                            /*this.Columns[i].Name = "CHK";
                            this.Columns[i].HeaderText = "";
                            this.Columns[i].Visible = isCheckBoxHeaderVisible;
                            this.Columns[i].Width = 40;                        */

                        /*Rectangle rect = this.GetCellDisplayRectangle(0, -1, true);
                        Size s = new System.Drawing.Size(rect.Width / 2, rect.Height / 2 + 2);
                        rect.X = (rect.Width / 4) + 3; //- rect //rect.Location.X + (rect.Width  / 4);
                        rect.Y = (rect.Height / 4);// +(rect.Height / 4);

                        CheckBox chkHeader = new CheckBox();
                        chkHeader.Name = "chkHeader";
                        chkHeader.Size = s;
                        chkHeader.Location = rect.Location;
                        chkHeader.CheckedChanged += chkHeader_CheckedChanged;
                        this.Controls.Add(chkHeader);
                        this.Columns[0].ReadOnly = false;*/
                        //}
                        if (i == 0) {
                            this.Columns[i].Name = "index";
                            this.Columns[i].HeaderText = "순번";
                            this.Columns[i].Visible = isIndexColumnVisible;
                            this.Columns[i].Width = 40;
                            this.Columns[i].ReadOnly = true;
                            this.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        } else if (i == this.ColumnCount - 1) {
                            this.Columns[i].Name = "EDITOBJECT"; //컬럼 이름
                            this.Columns[i].HeaderText = "EDITOBJECT"; //표시될 TEXT
                            this.Columns[i].DataPropertyName = "EDITOBJECT"; //바인딩될 이름 (컬럼명과 동일설정)
                            this.Columns[i].Width = 100; //컬럼 폭
                            this.Columns[i].Visible = isDebugVisible;
                        } else {
                            //기본설정
                            this.Columns[i].Name = colNames[i - 1];
                            this.Columns[i].HeaderText = colHeaderTexts[i - 1];
                            this.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                            this.Columns[i].Tag = colNames[i - 1];
                            this.Columns[i].DataPropertyName = colNames[i - 1];

                            //읽기속성
                            if (isColReadOnly != null) this.Columns[i].ReadOnly = isColReadOnly[i - 1];
                            else this.Columns[i].ReadOnly = true;

                            //표시속성
                            if (isColVisible != null) this.Columns[i].Visible = isColVisible[i - 1];

                            //컬럼 폭
                            if (colWidths != null) this.Columns[i].Width = colWidths[i - 1];
                            //else this.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                            //컬럼 최소 폭
                            if (colMinWidths != null) this.Columns[i].MinimumWidth = colMinWidths[i - 1];

                            //컬럼 폭 비율 지정
                            //if (colFillWeights != null && this.Columns[i].Visible)
                            //this.Columns[i].FillWeight = colFillWeights[i];

                            //표시 순서 지정
                            if (displayIndexs != null && this.Columns[i].Visible)
                                this.Columns[i].DisplayIndex = displayIndexs[i - 1];

                            //컬럼 Cell 정렬상태
                            if (rowCellStyleAlignments != null)
                                this.Columns[i].DefaultCellStyle.Alignment = rowCellStyleAlignments[i - 1];
                            else
                                DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            //컬럼 헤더 정렬 상태
                            if (headerAlingments != null)
                                this.Columns[i].HeaderCell.Style.Alignment = headerAlingments[i - 1];
                            else
                                this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }

                        this.Columns[i].Resizable = DataGridViewTriState.True;
                        this.Columns[i].Frozen = false;
                        //this.Columns[i].SortMode = DataGridViewColumnSortMode.Programmatic;
                    }
                } catch (Exception ex) {
                    Console.WriteLine(" Excetopn SetGrid : " + ex.Message);
                }

                this.BackgroundColor = Color.FromArgb(239, 243, 244);
                this.GridColor = Color.FromArgb(217, 218, 220);

                this.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(239, 243, 244);
                this.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                this.DefaultCellStyle.ForeColor = Color.FromArgb(65, 90, 140);
                this.DefaultCellStyle.SelectionForeColor = Color.FromArgb(255, 255, 255);
                this.DefaultCellStyle.SelectionBackColor = Color.FromArgb(104, 203, 240);

                this.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                //this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                //this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //this.ColumnHeadersHeight = 30;
                this.RowTemplate.Height = 25;
                this.RowTemplate.MinimumHeight = 25;

                //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                //this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                //this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.MultiSelect = isMultiSelect;
                this.BackgroundColor = Color.White;

                #endregion 기본설정

                #region 체크박스 컬럼 생성 시 수행

                if (isCheckBoxHeaderVisible) {
                    DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn();
                    chkColumn.Name = "CHK";
                    chkColumn.HeaderText = "";
                    chkColumn.Width = 30;
                    chkColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    chkColumn.TrueValue = true;
                    chkColumn.FalseValue = false;
                    this.Columns.Insert(0, chkColumn);

                    /*Rectangle rect = this.GetCellDisplayRectangle(0, -1, true);
                    Size s = new System.Drawing.Size(rect.Width / 2, rect.Height / 2 + 2);
                    rect.X = (rect.Width / 4) + 3; //- rect //rect.Location.X + (rect.Width  / 4);
                    rect.Y = (rect.Height / 4);// +(rect.Height / 4);

                    CheckBox chkHeader = new CheckBox();
                    chkHeader.Name = "chkHeader";
                    chkHeader.Size = s;
                    chkHeader.Location = rect.Location;
                    chkHeader.CheckedChanged += chkHeader_CheckedChanged;
                    this.Controls.Add(chkHeader);
                    this.Columns[0].ReadOnly = false;*/
                }

                #endregion 체크박스 컬럼 생성 시 수행

                #region 이미지 컬럼 생성시 수행

                if (isImageColumnVisible) {
                    DataGridViewColumn column = null;

                    if (isCheckBoxHeaderVisible) column = this.Columns[1];
                    else column = this.Columns[0];

                    if (column != null) {
                        DataGridViewImageColumn imgCol = new DataGridViewImageColumn();

                        imgCol.Name = column.Name;
                        imgCol.HeaderText = column.HeaderText;
                        imgCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                        imgCol.Tag = column.Tag;
                        imgCol.DataPropertyName = column.DataPropertyName;
                        imgCol.ReadOnly = column.ReadOnly;
                        imgCol.Visible = column.Visible;
                        imgCol.Width = column.Width;
                        imgCol.MinimumWidth = column.MinimumWidth;
                        imgCol.ValueType = column.ValueType;
                        imgCol.DefaultCellStyle.Alignment = column.DefaultCellStyle.Alignment;
                        imgCol.HeaderCell.Style.Alignment = column.HeaderCell.Style.Alignment;

                        if (isCheckBoxHeaderVisible) {
                            this.Columns.RemoveAt(1);
                            this.Columns.Insert(1, imgCol);
                        } else {
                            this.Columns.RemoveAt(0);
                            this.Columns.Insert(0, imgCol);
                        }
                    }
                }

                #endregion 이미지 컬럼 생성시 수행

                if (isContextMenuVisible) {
                    this.CellMouseUp += JWDataGridView_CellMouseUp;
                }

                this.MultiSelect = isMultiSelect;

                if (isSearchViewEnable) {
                    this.KeyUp += JWDataGridView_KeyUp;
                }

                foreach (string item in comboBoxColumn) {
                    int n = this.Columns.IndexOf(this.Columns[item]);

                    DataGridViewComboBoxColumn cmb = new DataGridViewComboBoxColumn();
                    cmb.HeaderText = this.Columns[item].HeaderText;
                    cmb.Name = this.Columns[item].Name;
                    cmb.DataPropertyName = this.Columns[item].DataPropertyName;
                    cmb.Width = this.Columns[item].Width;
                    cmb.MinimumWidth = this.Columns[item].MinimumWidth;
                    cmb.DisplayIndex = this.Columns[item].DisplayIndex;
                    cmb.ReadOnly = this.Columns[item].ReadOnly;
                    /*cmb.MaxDropDownItems = comboBoxColumnItem[item].Split(new string[]{","}, StringSplitOptions.None).Length;

                    foreach (string key in comboBoxColumnItem.Keys)
                    {
                        if (key == item)
                        {
                            string[] values = comboBoxColumnItem.GetValues(key);

                            foreach (string value in values)
                            {
                                cmb.Items.Add(value);
                            }

                            break;
                        }
                    }*/

                    this.Columns.RemoveAt(n);
                    this.Columns.Insert(n, cmb);
                }

                this.SelectionMode = selectMode;
                this.ScrollBars = ScrollBars.Both;
            } catch (Exception e) {
                Console.WriteLine("grid settng exception : " + e.Message);
            }

            //컬럼 사이즈 고정 (height)
            this.AllowUserToResizeColumns = false;

            //로우 사이즈 고정 (height)
            this.AllowUserToResizeRows = false;
        }

        public void SetGrid(
                   bool isCheckBoxHeaderVisible,
                   bool isImageColumnVisible,
                   bool isSearchViewEnable,
                   bool isContextMenuVisible,
                   bool isMultiSelect,
                   bool isIndexColumnVisible,
                   bool[] isColReadOnly,
                   bool[] isColVisible,
                   string[] comboBoxColumn,
                   NameValueCollection comboBoxColumnItem,
                   string[] checkBoxColumn,
                   DataGridViewSelectionMode selectMode,
                   string[] colNames,
                   string[] colHeaderTexts,
                   int[] colWidths,
                   int[] colMinWidths,
                   int[] colFillWeights,
                   int[] displayIndexs,
                   DataGridViewContentAlignment[] rowCellStyleAlignments,
                   DataGridViewContentAlignment[] headerAlingments,
                   bool isDebugVisible = false,
                   bool isLineColor = false) {

            #region 기본설정

            //다중 선택을 위한 체크박스 활성화
            this._isCheckBoxHeaderVisible = isCheckBoxHeaderVisible;

            //기본설정
            this.AutoGenerateColumns = false;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.RowHeadersVisible = false;

            this._isLineColor = isLineColor;

            int columnCnt = colNames.Length + 2;

            /*if (isIndexColumnVisible)
                columnCnt = columnCnt + 1;*/

            this.ColumnCount = columnCnt;
            int isdf = 0;
            try {
                try {
                    for (int i = 0; i < columnCnt; i++) {
                        isdf = i;
                        /*if (i == 0)
                        {
                            /*DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                            chk.Name = "CHK";
                            chk.Visible = isCheckBoxHeaderVisible;
                            chk.Width = 40;
                            chk.ReadOnly = false;
                            this.Columns.Insert(0, chk);
                            /*this.Columns[i].Name = "CHK";
                            this.Columns[i].HeaderText = "";
                            this.Columns[i].Visible = isCheckBoxHeaderVisible;
                            this.Columns[i].Width = 40;                        */

                        /*Rectangle rect = this.GetCellDisplayRectangle(0, -1, true);
                        Size s = new System.Drawing.Size(rect.Width / 2, rect.Height / 2 + 2);
                        rect.X = (rect.Width / 4) + 3; //- rect //rect.Location.X + (rect.Width  / 4);
                        rect.Y = (rect.Height / 4);// +(rect.Height / 4);

                        CheckBox chkHeader = new CheckBox();
                        chkHeader.Name = "chkHeader";
                        chkHeader.Size = s;
                        chkHeader.Location = rect.Location;
                        chkHeader.CheckedChanged += chkHeader_CheckedChanged;
                        this.Controls.Add(chkHeader);
                        this.Columns[0].ReadOnly = false;*/
                        //}
                        if (i == 0) {
                            this.Columns[i].Name = "index";
                            this.Columns[i].HeaderText = "순번";
                            this.Columns[i].Visible = isIndexColumnVisible;
                            this.Columns[i].Width = 40;
                            this.Columns[i].ReadOnly = true;
                            this.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        } else if (i == this.ColumnCount - 1) {
                            this.Columns[i].Name = "EDITOBJECT"; //컬럼 이름
                            this.Columns[i].HeaderText = "EDITOBJECT"; //표시될 TEXT
                            this.Columns[i].DataPropertyName = "EDITOBJECT"; //바인딩될 이름 (컬럼명과 동일설정)
                            this.Columns[i].Width = 100; //컬럼 폭
                            this.Columns[i].Visible = isDebugVisible;
                        } else {
                            //기본설정
                            this.Columns[i].Name = colNames[i - 1];
                            this.Columns[i].HeaderText = colHeaderTexts[i - 1];
                            this.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                            this.Columns[i].Tag = colNames[i - 1];
                            this.Columns[i].DataPropertyName = colNames[i - 1];

                            //읽기속성
                            if (isColReadOnly != null) this.Columns[i].ReadOnly = isColReadOnly[i - 1];
                            else this.Columns[i].ReadOnly = true;

                            //표시속성
                            if (isColVisible != null) this.Columns[i].Visible = isColVisible[i - 1];

                            //컬럼 폭
                            if (colWidths != null) this.Columns[i].Width = colWidths[i - 1];
                            //else this.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                            //컬럼 최소 폭
                            if (colMinWidths != null) this.Columns[i].MinimumWidth = colMinWidths[i - 1];

                            //컬럼 폭 비율 지정
                            //if (colFillWeights != null && this.Columns[i].Visible)
                            //this.Columns[i].FillWeight = colFillWeights[i];

                            //표시 순서 지정
                            if (displayIndexs != null && this.Columns[i].Visible)
                                this.Columns[i].DisplayIndex = displayIndexs[i - 1];

                            //컬럼 Cell 정렬상태
                            if (rowCellStyleAlignments != null)
                                this.Columns[i].DefaultCellStyle.Alignment = rowCellStyleAlignments[i - 1];
                            else
                                DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            //컬럼 헤더 정렬 상태
                            if (headerAlingments != null)
                                this.Columns[i].HeaderCell.Style.Alignment = headerAlingments[i - 1];
                            else
                                this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }

                        this.Columns[i].Resizable = DataGridViewTriState.True;
                        this.Columns[i].Frozen = false;
                        //this.Columns[i].SortMode = DataGridViewColumnSortMode.Programmatic;
                    }
                } catch (Exception ex) {
                    Console.WriteLine(" Excetopn SetGrid : " + ex.Message);
                }

                this.BackgroundColor = Color.FromArgb(239, 243, 244);
                this.GridColor = Color.FromArgb(217, 218, 220);

                this.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(239, 243, 244);
                this.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                this.DefaultCellStyle.ForeColor = Color.FromArgb(65, 90, 140);
                this.DefaultCellStyle.SelectionForeColor = Color.FromArgb(255, 255, 255);
                this.DefaultCellStyle.SelectionBackColor = Color.FromArgb(104, 203, 240);

                this.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                //this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                //this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //this.ColumnHeadersHeight = 30;
                this.RowTemplate.Height = 25;
                this.RowTemplate.MinimumHeight = 25;

                //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                //this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                //this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.MultiSelect = isMultiSelect;
                this.BackgroundColor = Color.White;

                #endregion 기본설정

                #region 체크박스 컬럼 생성 시 수행

                if (isCheckBoxHeaderVisible) {
                    DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn();
                    chkColumn.Name = "CHK";
                    chkColumn.HeaderText = "";
                    chkColumn.Width = 30;
                    chkColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    chkColumn.TrueValue = true;
                    chkColumn.FalseValue = false;
                    this.Columns.Insert(0, chkColumn);

                    /*Rectangle rect = this.GetCellDisplayRectangle(0, -1, true);
                    Size s = new System.Drawing.Size(rect.Width / 2, rect.Height / 2 + 2);
                    rect.X = (rect.Width / 4) + 3; //- rect //rect.Location.X + (rect.Width  / 4);
                    rect.Y = (rect.Height / 4);// +(rect.Height / 4);

                    CheckBox chkHeader = new CheckBox();
                    chkHeader.Name = "chkHeader";
                    chkHeader.Size = s;
                    chkHeader.Location = rect.Location;
                    chkHeader.CheckedChanged += chkHeader_CheckedChanged;
                    this.Controls.Add(chkHeader);
                    this.Columns[0].ReadOnly = false;*/
                }

                #endregion 체크박스 컬럼 생성 시 수행

                #region 이미지 컬럼 생성시 수행

                if (isImageColumnVisible) {
                    DataGridViewColumn column = null;

                    if (isCheckBoxHeaderVisible) column = this.Columns[1];
                    else column = this.Columns[0];

                    if (column != null) {
                        DataGridViewImageColumn imgCol = new DataGridViewImageColumn();

                        imgCol.Name = column.Name;
                        imgCol.HeaderText = column.HeaderText;
                        imgCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                        imgCol.Tag = column.Tag;
                        imgCol.DataPropertyName = column.DataPropertyName;
                        imgCol.ReadOnly = column.ReadOnly;
                        imgCol.Visible = column.Visible;
                        imgCol.Width = column.Width;
                        imgCol.MinimumWidth = column.MinimumWidth;
                        imgCol.ValueType = column.ValueType;
                        imgCol.DefaultCellStyle.Alignment = column.DefaultCellStyle.Alignment;
                        imgCol.HeaderCell.Style.Alignment = column.HeaderCell.Style.Alignment;

                        if (isCheckBoxHeaderVisible) {
                            this.Columns.RemoveAt(1);
                            this.Columns.Insert(1, imgCol);
                        } else {
                            this.Columns.RemoveAt(0);
                            this.Columns.Insert(0, imgCol);
                        }
                    }
                }

                #endregion 이미지 컬럼 생성시 수행

                if (isContextMenuVisible) {
                    this.CellMouseUp += JWDataGridView_CellMouseUp;
                }

                this.MultiSelect = isMultiSelect;

                if (isSearchViewEnable) {
                    this.KeyUp += JWDataGridView_KeyUp;
                }

                foreach (string item in comboBoxColumn) {
                    int n = this.Columns.IndexOf(this.Columns[item]);

                    DataGridViewComboBoxColumn cmb = new DataGridViewComboBoxColumn();
                    cmb.HeaderText = this.Columns[item].HeaderText;
                    cmb.Name = this.Columns[item].Name;
                    cmb.DataPropertyName = this.Columns[item].DataPropertyName;
                    cmb.Width = this.Columns[item].Width;
                    cmb.MinimumWidth = this.Columns[item].MinimumWidth;
                    cmb.DisplayIndex = this.Columns[item].DisplayIndex;
                    cmb.ReadOnly = this.Columns[item].ReadOnly;
                    /*cmb.MaxDropDownItems = comboBoxColumnItem[item].Split(new string[]{","}, StringSplitOptions.None).Length;

                    foreach (string key in comboBoxColumnItem.Keys)
                    {
                        if (key == item)
                        {
                            string[] values = comboBoxColumnItem.GetValues(key);

                            foreach (string value in values)
                            {
                                cmb.Items.Add(value);
                            }

                            break;
                        }
                    }*/

                    this.Columns.RemoveAt(n);
                    this.Columns.Insert(n, cmb);
                }

                this.SelectionMode = selectMode;
                this.ScrollBars = ScrollBars.Both;
            } catch (Exception e) {
                Console.WriteLine("grid settng exception : " + e.Message);
            }

            //컬럼 사이즈 고정 (height)
            this.AllowUserToResizeColumns = false;

            //로우 사이즈 고정 (height)
            this.AllowUserToResizeRows = false;
        }

        #endregion setting function

        private void InitializeComponent() {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            //
            // JWDataGridView
            //
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeColumns = false;
            this.AllowUserToResizeRows = false;
            this.RowTemplate.Height = 23;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }
    }
}