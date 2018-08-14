namespace FingerPrintControlTest
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.fingerPrintControl2 = new FingerPrintControl.FingerPrintControl();
            this.SuspendLayout();
            // 
            // fingerPrintControl2
            // 
            this.fingerPrintControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fingerPrintControl2.Location = new System.Drawing.Point(261, 90);
            this.fingerPrintControl2.Name = "fingerPrintControl2";
            this.fingerPrintControl2.Size = new System.Drawing.Size(260, 243);
            this.fingerPrintControl2.TabIndex = 0;
            this.fingerPrintControl2.OnFingerPrintCompleted += new FingerPrintControl.FingerPrintCompleted(this.FingerPrintControl2_OnFingerPrintCompleted);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(763, 425);
            this.Controls.Add(this.fingerPrintControl2);
            this.Name = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private FingerPrintControl.FingerPrintControl fingerPrintControl1;
        private FingerPrintControl.FingerPrintControl fingerPrintControl2;
    }
}

