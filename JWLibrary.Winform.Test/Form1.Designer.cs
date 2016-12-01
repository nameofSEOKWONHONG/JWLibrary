namespace JWLibrary.Winform.Test
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.jwFlowLayoutPanel1 = new JWLibrary.Winform.Container.JWFlowLayoutPanel();
            this.jwTextBox1 = new JWLibrary.Winform.CommonControls.JWTextBox();
            this.jWaterMarkTextBox1 = new JWLibrary.Winform.CommonControls.JWaterMarkTextBox();
            this.jwLabel1 = new JWLibrary.Winform.CommonControls.JWLabel();
            this.jwDateTimePicker1 = new JWLibrary.Winform.CommonControls.JWDateTimePicker();
            this.button2 = new System.Windows.Forms.Button();
            this.jwFlowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(64, 241);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // jwFlowLayoutPanel1
            // 
            this.jwFlowLayoutPanel1.Controls.Add(this.jwTextBox1);
            this.jwFlowLayoutPanel1.Controls.Add(this.jWaterMarkTextBox1);
            this.jwFlowLayoutPanel1.Controls.Add(this.jwLabel1);
            this.jwFlowLayoutPanel1.Controls.Add(this.jwDateTimePicker1);
            this.jwFlowLayoutPanel1.DataSource = null;
            this.jwFlowLayoutPanel1.Location = new System.Drawing.Point(64, 106);
            this.jwFlowLayoutPanel1.Name = "jwFlowLayoutPanel1";
            this.jwFlowLayoutPanel1.Size = new System.Drawing.Size(378, 92);
            this.jwFlowLayoutPanel1.TabIndex = 0;
            // 
            // jwTextBox1
            // 
            this.jwTextBox1.BindingName = "Id";
            this.jwTextBox1.InputMode = JWLibrary.Winform.CommonControls.JWTextBox.INPUT_MODE.GENERAL;
            this.jwTextBox1.Location = new System.Drawing.Point(3, 3);
            this.jwTextBox1.Name = "jwTextBox1";
            this.jwTextBox1.NumberFormat = null;
            this.jwTextBox1.Size = new System.Drawing.Size(100, 21);
            this.jwTextBox1.TabIndex = 1;
            // 
            // jWaterMarkTextBox1
            // 
            this.jWaterMarkTextBox1.BindingName = "Name";
            this.jWaterMarkTextBox1.InputMode = JWLibrary.Winform.CommonControls.JWTextBox.INPUT_MODE.GENERAL;
            this.jWaterMarkTextBox1.Location = new System.Drawing.Point(109, 3);
            this.jWaterMarkTextBox1.Name = "jWaterMarkTextBox1";
            this.jWaterMarkTextBox1.NumberFormat = null;
            this.jWaterMarkTextBox1.Size = new System.Drawing.Size(100, 21);
            this.jWaterMarkTextBox1.TabIndex = 2;
            this.jWaterMarkTextBox1.WaterMark = "Default Watermark...";
            this.jWaterMarkTextBox1.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.jWaterMarkTextBox1.WaterMarkFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.jWaterMarkTextBox1.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // jwLabel1
            // 
            this.jwLabel1.AutoSize = true;
            this.jwLabel1.BindingName = "Phone";
            this.jwLabel1.Location = new System.Drawing.Point(215, 0);
            this.jwLabel1.Name = "jwLabel1";
            this.jwLabel1.Size = new System.Drawing.Size(55, 12);
            this.jwLabel1.TabIndex = 3;
            this.jwLabel1.Text = "jwLabel1";
            // 
            // jwDateTimePicker1
            // 
            this.jwDateTimePicker1.BindingName = "EDate";
            this.jwDateTimePicker1.Location = new System.Drawing.Point(3, 30);
            this.jwDateTimePicker1.Name = "jwDateTimePicker1";
            this.jwDateTimePicker1.Size = new System.Drawing.Size(200, 21);
            this.jwDateTimePicker1.TabIndex = 4;
            this.jwDateTimePicker1.Value = new System.DateTime(2016, 12, 2, 3, 13, 49, 329);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(145, 241);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 450);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.jwFlowLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.jwFlowLayoutPanel1.ResumeLayout(false);
            this.jwFlowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Container.JWFlowLayoutPanel jwFlowLayoutPanel1;
        private CommonControls.JWTextBox jwTextBox1;
        private System.Windows.Forms.Button button1;
        private CommonControls.JWaterMarkTextBox jWaterMarkTextBox1;
        private CommonControls.JWLabel jwLabel1;
        private CommonControls.JWDateTimePicker jwDateTimePicker1;
        private System.Windows.Forms.Button button2;
    }
}

