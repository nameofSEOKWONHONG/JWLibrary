using System.Drawing;
using System.Windows.Forms;

namespace JWLibrary.Winform.DataViewControls {

    public class DataGridViewDoubleLineCellValue {
        public string UpperLabelText;
        public string UpperValueText;
        public string LowerLabelText;
        public Image[] LowerValueImg;
    }

    //하나의 셀을 아래위로 2줄로 나누어 정보를 표시하는 셀
    // 표시 형식:
    // 글자 [공백] 글자
    // 글자 [공백] 그림
    public class DataGridViewDobleLineCell : DataGridViewTextBoxCell {

        protected override void Paint(
                                 Graphics graphics,
                                 Rectangle clipBounds,
                                 Rectangle cellBounds,
                                 int rowIndex,
                                 DataGridViewElementStates cellState,
                                 object value,
                                 object formattedValue,
                                 string errorText,
                                 DataGridViewCellStyle cellStyle,
                                 DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                 DataGridViewPaintParts paintParts) {
            string upperLabelText = "";
            string upperValueText = "";
            string lowerLabelText = "";
            Image[] lowerValueImg = null;

            DataGridViewDoubleLineCellValue cellValue =
                formattedValue as DataGridViewDoubleLineCellValue;
            if (null != cellValue) {
                upperLabelText = cellValue.UpperLabelText;
                upperValueText = cellValue.UpperValueText;
                lowerLabelText = cellValue.LowerLabelText;
                lowerValueImg = cellValue.LowerValueImg;
            }

            base.Paint(graphics, clipBounds, cellBounds,
                        rowIndex, cellState,
                        value, "", errorText, cellStyle,
                        advancedBorderStyle, paintParts);

            const int HORSPACE = 32;             // 높이
            const int HORITEMSPACE = 10;         // 아이템 끝나는 좌표 y
            const int COLNAMESPACE = 46;         // 칼럼 이름 시작 좌표 x
            const int COLITTEMSPACE = 350;       // 아이템 끝나는 좌표 x

            const int ADDCOLITEMSPACE = 50;      // 아이템 추가되는 좌표 X

            const int ICONHOR = 16;              // 아이콘 크기 x
            const int ICONVER = 16;              // 아이콘 크기 y

            //const string strKeyword2 = "True";

            StringFormat format = new StringFormat(StringFormatFlags.DirectionRightToLeft);

            DataGridViewColumn parent = this.OwningColumn;

            //parent.GridAttributeValues(rowIndex);
            Font fnt = parent.InheritedStyle.Font;
            Font cellfnt = cellStyle.Font;

            if (lowerValueImg != null) {
                RectangleF newRect = RectangleF.Empty;

                int addItemspace = 0;

                for (int i = 0; i < lowerValueImg.Length; i++) {
                    if (i == 0) {
                        newRect = new RectangleF(
                                             cellBounds.Left + 315,
                                             cellBounds.Y + (HORSPACE) + HORITEMSPACE,
                                             ICONHOR,
                                             ICONVER);
                    } else {
                        addItemspace += ADDCOLITEMSPACE;

                        newRect = new RectangleF(
                                             cellBounds.Left + 315 + addItemspace,
                                             cellBounds.Y + (HORSPACE) + HORITEMSPACE,
                                             ICONHOR,
                                             ICONVER);
                    }

                    graphics.DrawImage(lowerValueImg[i], newRect);
                }
            }
            /*
            Image img = null;
            if (parent.str3Value == strKeyword2)
            {
                img = ManagementConsoleWin32.Properties.Resources.icon_account_on;
                //graphics.DrawImage(
                //ManagementConsoleWin32.Properties.Resources.icon_account_on, newRect);
            }
            else
            {
                img = ManagementConsoleWin32.Properties.Resources.icon_account_off;
                //graphics.DrawImage(
                //ManagementConsoleWin32.Properties.Resources.icon_account_off, newRect);
            }
            graphics.DrawImage(img, newRect);
            /**/

            string cellText = formattedValue.ToString();
            SizeF textSize = graphics.MeasureString(cellText, fnt);

            Color textColor = parent.InheritedStyle.ForeColor;
            if ((cellState & DataGridViewElementStates.Selected) ==
            DataGridViewElementStates.Selected) {
                textColor = parent.InheritedStyle.SelectionForeColor;
            }

            Pen myPen = new Pen(Color.FromArgb(239, 243, 244));

            Point first1pt = new Point(cellBounds.Left, cellBounds.Y + HORSPACE);
            Point first2pt = new Point(cellBounds.Right, cellBounds.Y + HORSPACE);
            Point second1pt = new Point(cellBounds.Left, cellBounds.Y + (HORSPACE * 2));
            Point second2pt = new Point(cellBounds.Right, cellBounds.Y + (HORSPACE * 2));

            graphics.DrawLine(myPen, first1pt, first2pt);
            graphics.DrawLine(myPen, second1pt, second2pt);

            // Draw the text:
            using (SolidBrush brush = new SolidBrush(textColor)) {
                graphics.DrawString(
                    upperLabelText, //parent.TransLang(parent.str2ColName),
                    fnt,
                    brush,
                    cellBounds.X + COLNAMESPACE,
                    cellBounds.Y + HORITEMSPACE);
                graphics.DrawString(
                    upperValueText, //parent.str2Value,
                    cellfnt,
                    brush,
                    cellBounds.Left + COLITTEMSPACE + ICONVER,
                    cellBounds.Y + HORITEMSPACE,
                    format);

                graphics.DrawString(
                    lowerLabelText, //parent.TransLang(parent.str3ColName),
                    fnt,
                    brush,
                    cellBounds.X + COLNAMESPACE,
                    cellBounds.Y + (HORSPACE) + HORITEMSPACE);
            }
        }
    }
}