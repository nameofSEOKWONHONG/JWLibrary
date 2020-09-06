using System.Windows.Forms;

namespace JWLibrary.Winform.DataViewControls {

    public class DataGridViewDobleLineColumn : DataGridViewColumn {

        public DataGridViewDobleLineColumn() {
            this.CellTemplate = new DataGridViewDobleLineCell();
            this.ReadOnly = true;
        }
    }
}