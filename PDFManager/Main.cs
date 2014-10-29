using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFManager
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        string OutputRoot = @"C:\Temp\CoalUranium\PDF\";


        private void Main_Load(object sender, EventArgs e)
        {
            PDFSourceFolder.Text = ProgramSettings.LastFolder;


        }

        private void Browse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Select the PDF source folder";
            folderBrowserDialog1.SelectedPath = ProgramSettings.LastFolder;
            folderBrowserDialog1.ShowNewFolderButton = true;
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ProgramSettings.LastFolder = folderBrowserDialog1.SelectedPath;
                PDFSourceFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void ExtractPDFs_Click(object sender, EventArgs e)
        {
            // ensure source folder exists
            if (!Directory.Exists(PDFSourceFolder.Text))
            {
                MessageBox.Show(string.Format("The folder '{0}' could not be found", PDFSourceFolder.Text), "Folder not found");
                return;
            }


            // a list of all the PDF files in the source folder
            string[] files = Directory.GetFiles(PDFSourceFolder.Text);

            // setup progress bar based on files
            OverallProgress.Maximum = files.Length;
            OverallProgress.Minimum = 0;
            OverallProgress.Value = 0;
            OverallProgress.Show();

            // extract each file
            foreach (string file in files)
            {

                PDFExtractor extractor = new PDFExtractor(file, OutputRoot);
                extractor.Extract();
                
                // update progress bar
                OverallProgress.Value += 1;
                OverallProgress.Show();
                OverallProgress.Refresh();
                this.Refresh();
                Application.DoEvents();
            }

            // Open the output folder
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = "explorer";
            process.StartInfo.Arguments = OutputRoot;
            process.Start();

        }

        /*

        void PDFDropZone_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }

        void PDFDropZone_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {

                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

                ExtractorProgress.Maximum = files.Length;
                ExtractorProgress.Value = 0;
                ExtractorProgress.Visible = true;
                Application.DoEvents();

                foreach (string file in files)
                {
                    PDFExtractor extractor = new PDFExtractor(file);
                    extractor.Extract(false);
                    //extractor.WriteHtmlIndex();
                    extractor.WriteTextIndex();
                    ExtractorProgress.Value += 1;
                    ExtractorProgress.Show();
                    Application.DoEvents();
                }

                string message = string.Format("Finished extracting {0} {1}", files.Length, (files.Length > 1 ? "files" : "file"));

                MessageBox.Show(message);

                ExtractorProgress.Hide();
            }
        }
         
         
        private void Instructions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Instructions help = new Instructions();
            help.Show();
        }
         
        */



    }
}
