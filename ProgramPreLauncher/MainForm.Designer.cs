
namespace ProgramPreLauncher
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.statusbar = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(816, 257);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // progress
            // 
            this.progress.BackColor = System.Drawing.SystemColors.Control;
            this.progress.ForeColor = System.Drawing.Color.Orange;
            this.progress.Location = new System.Drawing.Point(12, 219);
            this.progress.Maximum = 1000;
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(792, 23);
            this.progress.TabIndex = 1;
            // 
            // statusbar
            // 
            this.statusbar.AutoSize = true;
            this.statusbar.BackColor = System.Drawing.SystemColors.Control;
            this.statusbar.Location = new System.Drawing.Point(13, 242);
            this.statusbar.Name = "statusbar";
            this.statusbar.Size = new System.Drawing.Size(95, 12);
            this.statusbar.TabIndex = 2;
            this.statusbar.Text = "正在检查更新...";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 257);
            this.ControlBox = false;
            this.Controls.Add(this.statusbar);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.pictureBox1);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(816, 257);
            this.MinimumSize = new System.Drawing.Size(816, 257);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "正在检查更新";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Label statusbar;
    }
}