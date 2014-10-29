namespace PDFManager
{
    partial class Main
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.PDFSourceFolder = new System.Windows.Forms.TextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ExtractPDFs = new System.Windows.Forms.Button();
            this.OverallProgress = new System.Windows.Forms.ProgressBar();
            this.IndividualProgress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // PDFSourceFolder
            // 
            this.PDFSourceFolder.Location = new System.Drawing.Point(51, 103);
            this.PDFSourceFolder.Name = "PDFSourceFolder";
            this.PDFSourceFolder.Size = new System.Drawing.Size(458, 20);
            this.PDFSourceFolder.TabIndex = 0;
            // 
            // Browse
            // 
            this.Browse.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Browse.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Browse.Location = new System.Drawing.Point(515, 101);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(75, 23);
            this.Browse.TabIndex = 1;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = false;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "PDF Source Folder";
            // 
            // ExtractPDFs
            // 
            this.ExtractPDFs.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ExtractPDFs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExtractPDFs.ForeColor = System.Drawing.Color.White;
            this.ExtractPDFs.Location = new System.Drawing.Point(51, 149);
            this.ExtractPDFs.Name = "ExtractPDFs";
            this.ExtractPDFs.Size = new System.Drawing.Size(458, 73);
            this.ExtractPDFs.TabIndex = 3;
            this.ExtractPDFs.Text = "Extract PDFs and Build Index";
            this.ExtractPDFs.UseVisualStyleBackColor = false;
            this.ExtractPDFs.Click += new System.EventHandler(this.ExtractPDFs_Click);
            // 
            // OverallProgress
            // 
            this.OverallProgress.Location = new System.Drawing.Point(51, 259);
            this.OverallProgress.Name = "OverallProgress";
            this.OverallProgress.Size = new System.Drawing.Size(458, 23);
            this.OverallProgress.TabIndex = 4;
            this.OverallProgress.Visible = false;
            // 
            // IndividualProgress
            // 
            this.IndividualProgress.Location = new System.Drawing.Point(51, 288);
            this.IndividualProgress.Name = "IndividualProgress";
            this.IndividualProgress.Size = new System.Drawing.Size(458, 23);
            this.IndividualProgress.TabIndex = 5;
            this.IndividualProgress.Visible = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(614, 377);
            this.Controls.Add(this.IndividualProgress);
            this.Controls.Add(this.OverallProgress);
            this.Controls.Add(this.ExtractPDFs);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.PDFSourceFolder);
            this.Name = "Main";
            this.Text = "PDF Extractor";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox PDFSourceFolder;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ExtractPDFs;
        private System.Windows.Forms.ProgressBar OverallProgress;
        private System.Windows.Forms.ProgressBar IndividualProgress;



    }
}

