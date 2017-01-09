namespace JWLibrary.FFmpeg.Test
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
            this.btnRecStart = new System.Windows.Forms.Button();
            this.btnRecStop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFps = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblFrame = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRecStart
            // 
            this.btnRecStart.Location = new System.Drawing.Point(106, 12);
            this.btnRecStart.Name = "btnRecStart";
            this.btnRecStart.Size = new System.Drawing.Size(296, 23);
            this.btnRecStart.TabIndex = 0;
            this.btnRecStart.Text = "Recording Start";
            this.btnRecStart.UseVisualStyleBackColor = true;
            this.btnRecStart.Click += new System.EventHandler(this.btnRecStart_Click);
            // 
            // btnRecStop
            // 
            this.btnRecStop.Location = new System.Drawing.Point(106, 41);
            this.btnRecStop.Name = "btnRecStop";
            this.btnRecStop.Size = new System.Drawing.Size(296, 23);
            this.btnRecStop.TabIndex = 1;
            this.btnRecStop.Text = "Recording Stop";
            this.btnRecStop.UseVisualStyleBackColor = true;
            this.btnRecStop.Click += new System.EventHandler(this.btnRecStop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "FPS : ";
            // 
            // lblFps
            // 
            this.lblFps.AutoSize = true;
            this.lblFps.Location = new System.Drawing.Point(71, 103);
            this.lblFps.Name = "lblFps";
            this.lblFps.Size = new System.Drawing.Size(38, 12);
            this.lblFps.TabIndex = 3;
            this.lblFps.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "FRAME : ";
            // 
            // lblFrame
            // 
            this.lblFrame.AutoSize = true;
            this.lblFrame.Location = new System.Drawing.Point(92, 127);
            this.lblFrame.Name = "lblFrame";
            this.lblFrame.Size = new System.Drawing.Size(38, 12);
            this.lblFrame.TabIndex = 5;
            this.lblFrame.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "TIME : ";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(80, 150);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(38, 12);
            this.lblTime.TabIndex = 7;
            this.lblTime.Text = "label6";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 189);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblFrame);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblFps);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRecStop);
            this.Controls.Add(this.btnRecStart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRecStart;
        private System.Windows.Forms.Button btnRecStop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFps;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblFrame;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTime;
    }
}

