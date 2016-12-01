namespace JWLibrary.Winform.DataGridViewControls
{
    partial class SearchForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.cbo_Condition = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Search = new System.Windows.Forms.TextBox();
            this.btnPre = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "검색어";
            // 
            // cbo_Condition
            // 
            this.cbo_Condition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_Condition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbo_Condition.FormattingEnabled = true;
            this.cbo_Condition.Location = new System.Drawing.Point(85, 12);
            this.cbo_Condition.Name = "cbo_Condition";
            this.cbo_Condition.Size = new System.Drawing.Size(242, 20);
            this.cbo_Condition.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "조건";
            // 
            // tb_Search
            // 
            this.tb_Search.Location = new System.Drawing.Point(85, 49);
            this.tb_Search.Name = "tb_Search";
            this.tb_Search.Size = new System.Drawing.Size(242, 21);
            this.tb_Search.TabIndex = 6;
            // 
            // btnPre
            // 
            this.btnPre.Location = new System.Drawing.Point(237, 76);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(42, 23);
            this.btnPre.TabIndex = 7;
            this.btnPre.Text = "<<";
            this.btnPre.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(285, 76);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(42, 23);
            this.btnNext.TabIndex = 8;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(333, 47);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(42, 23);
            this.btnFind.TabIndex = 9;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 108);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPre);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbo_Condition);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_Search);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SearchForm";
            this.Text = "SearchForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbo_Condition;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_Search;
        private System.Windows.Forms.Button btnPre;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnFind;
    }
}