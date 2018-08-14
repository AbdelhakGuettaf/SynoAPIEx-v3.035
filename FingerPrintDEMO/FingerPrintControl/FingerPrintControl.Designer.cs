namespace FingerPrintControl
{
    partial class FingerPrintControl
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel_pic = new System.Windows.Forms.Panel();
            this.pic_fingerprint = new System.Windows.Forms.PictureBox();
            this.panel_txt = new System.Windows.Forms.Panel();
            this.lab_log = new System.Windows.Forms.Label();
            this.panel_pic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_fingerprint)).BeginInit();
            this.panel_txt.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_pic
            // 
            this.panel_pic.BackColor = System.Drawing.SystemColors.Window;
            this.panel_pic.Controls.Add(this.pic_fingerprint);
            this.panel_pic.Location = new System.Drawing.Point(0, 0);
            this.panel_pic.Name = "panel_pic";
            this.panel_pic.Size = new System.Drawing.Size(260, 197);
            this.panel_pic.TabIndex = 0;
            this.panel_pic.Click += new System.EventHandler(this.panel_pic_Click);
            // 
            // pic_fingerprint
            // 
            this.pic_fingerprint.Location = new System.Drawing.Point(78, 46);
            this.pic_fingerprint.Name = "pic_fingerprint";
            this.pic_fingerprint.Size = new System.Drawing.Size(92, 92);
            this.pic_fingerprint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pic_fingerprint.TabIndex = 0;
            this.pic_fingerprint.TabStop = false;
            this.pic_fingerprint.Click += new System.EventHandler(this.panel_pic_Click);
            // 
            // panel_txt
            // 
            this.panel_txt.Controls.Add(this.lab_log);
            this.panel_txt.Location = new System.Drawing.Point(0, 197);
            this.panel_txt.Name = "panel_txt";
            this.panel_txt.Size = new System.Drawing.Size(260, 46);
            this.panel_txt.TabIndex = 1;
            // 
            // lab_log
            // 
            this.lab_log.BackColor = System.Drawing.SystemColors.Window;
            this.lab_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_log.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_log.Location = new System.Drawing.Point(0, 0);
            this.lab_log.Name = "lab_log";
            this.lab_log.Size = new System.Drawing.Size(260, 46);
            this.lab_log.TabIndex = 0;
            this.lab_log.Text = "点击开始录入指纹";
            this.lab_log.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lab_log.Click += new System.EventHandler(this.panel_pic_Click);
            // 
            // FingerPrintControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_pic);
            this.Controls.Add(this.panel_txt);
            this.Name = "FingerPrintControl";
            this.Size = new System.Drawing.Size(260, 243);
            this.Load += new System.EventHandler(this.FingerPrintControl_Load);
            this.panel_pic.ResumeLayout(false);
            this.panel_pic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_fingerprint)).EndInit();
            this.panel_txt.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_pic;
        private System.Windows.Forms.Panel panel_txt;
        public System.Windows.Forms.Label lab_log;
        private System.Windows.Forms.PictureBox pic_fingerprint;
    }
}
