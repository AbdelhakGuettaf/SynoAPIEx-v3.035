namespace FingerPrintDEMO
{
    partial class Test
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
            this.btn_finger = new System.Windows.Forms.Button();
            this.btngenchar1 = new System.Windows.Forms.Button();
            this.richtxt_log = new System.Windows.Forms.RichTextBox();
            this.btnmach = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_finger
            // 
            this.btn_finger.Location = new System.Drawing.Point(36, 272);
            this.btn_finger.Name = "btn_finger";
            this.btn_finger.Size = new System.Drawing.Size(75, 23);
            this.btn_finger.TabIndex = 1;
            this.btn_finger.Text = "Enroll fingerprint.";
            this.btn_finger.UseVisualStyleBackColor = true;
            this.btn_finger.Click += new System.EventHandler(this.btn_finger_Click);
            // 
            // btngenchar1
            // 
            this.btngenchar1.Location = new System.Drawing.Point(196, 272);
            this.btngenchar1.Name = "btngenchar1";
            this.btngenchar1.Size = new System.Drawing.Size(75, 23);
            this.btngenchar1.TabIndex = 3;
            this.btngenchar1.Text = "Synthesize template";
            this.btngenchar1.UseVisualStyleBackColor = true;
            this.btngenchar1.Click += new System.EventHandler(this.btngenchar1_Click);
            // 
            // richtxt_log
            // 
            this.richtxt_log.Location = new System.Drawing.Point(12, 12);
            this.richtxt_log.Name = "richtxt_log";
            this.richtxt_log.Size = new System.Drawing.Size(475, 238);
            this.richtxt_log.TabIndex = 4;
            this.richtxt_log.Text = "";
            // 
            // btnmach
            // 
            this.btnmach.Location = new System.Drawing.Point(365, 272);
            this.btnmach.Name = "btnmach";
            this.btnmach.Size = new System.Drawing.Size(75, 23);
            this.btnmach.TabIndex = 3;
            this.btnmach.Text = "Match";
            this.btnmach.UseVisualStyleBackColor = true;
            this.btnmach.Click += new System.EventHandler(this.btnmach_Click);
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 328);
            this.Controls.Add(this.richtxt_log);
            this.Controls.Add(this.btnmach);
            this.Controls.Add(this.btngenchar1);
            this.Controls.Add(this.btn_finger);
            this.Name = "Test";
            this.Text = "Test";
            this.Load += new System.EventHandler(this.Test_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_finger;
        private System.Windows.Forms.Button btngenchar1;
        private System.Windows.Forms.RichTextBox richtxt_log;
        private System.Windows.Forms.Button btnmach;
    }
}