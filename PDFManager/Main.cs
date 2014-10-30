using PDFManager.DAL.dbo;
using PDFManager.DO.dbo;
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
            PDFSourceFolder.Text = @"C:\Otter Creek\Permit Documents\"; // ProgramSettings.LastFolder;


        }

        private void Browse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Select the PDF source folder";
            //folderBrowserDialog1.SelectedPath = ProgramSettings.LastFolder;
            folderBrowserDialog1.ShowNewFolderButton = true;
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                //ProgramSettings.LastFolder = folderBrowserDialog1.SelectedPath;
                PDFSourceFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }




        private void ExtractPDFs_Click(object sender, EventArgs e)
        {
            // clear data and files to get ready to rerun
            Reset();

            // get a reference to all files
            LoadSourceFiles(PDFSourceFolder.Text);

            // run through each source file
            ProcessSourceFiles();

            // create the output document
            string html = GenerateHtml();
            string htmlPath = Path.Combine(OutputRoot, "files.html");
            File.WriteAllText(htmlPath, html);
        }


        /// <summary>
        /// Clears the database and output folder
        /// </summary>
        void Reset()
        {
            // clear the database and folder
            ExtractedFile.Truncate();
            
            foreach (string file in Directory.GetFiles(OutputRoot))
                File.Delete(file);
            foreach (string folder in Directory.GetDirectories(OutputRoot))
            {

                DirectoryInfo dir = new DirectoryInfo(folder);
                dir.Delete(true);
                //Directory.Delete(folder, true);
            }
        }


        /// <summary>
        /// Adds a reference to the source files to the database
        /// </summary>
        /// <param name="folder"></param>
        void LoadSourceFiles(String folder)
        {
            // a list of all the PDF files in the source folder
            string[] files = Directory.GetFiles(folder, "*.pdf");

            // loop through each file in the folder
            foreach (string file in files)
            {
                string destPath = file.Replace(PDFSourceFolder.Text, OutputRoot);
                File.Copy(file, destPath);

                // create a new database object for each file in the path
                ExtractedFileDO pdf = new ExtractedFileDO() { ParentFileId = null, FileName = Path.GetFileName(file), FilePath = destPath, Extracted = false };
                pdf.ExtractedFileId = ExtractedFile.Create(pdf);

                
            }

            // process each subfolder
            string[] dirs = Directory.GetDirectories(folder);
            foreach (string dir in dirs)
            {
                string destFolder = dir.Replace(PDFSourceFolder.Text, OutputRoot);
                Directory.CreateDirectory(destFolder);
                LoadSourceFiles(dir);
            }

        }


        /// <summary>
        /// Processes all source files that have not already been processed
        /// </summary>
        void ProcessSourceFiles()
        {
            // get a list of all files that have not been processed
            ExtractedFileDO[] SourceFiles = ExtractedFile.GetAll().Where(f => f.Extracted == false).ToArray();
            if (SourceFiles == null || SourceFiles.Count() == 0)
            {
                FinishedProcessing();
                return;
            }


            // process each one
            foreach (ExtractedFileDO SourceFile in SourceFiles)
            {
                ProcessSourceFile(SourceFile);
            }

            // rerun the process on all new files
            ProcessSourceFiles();
        }


        /// <summary>
        /// Processes an individual source file
        /// </summary>
        /// <param name="SourceFile"></param>
        void ProcessSourceFile(ExtractedFileDO SourceFile)
        {
            // extract all nested files
            PDFFile pdf = new PDFFile(SourceFile);
            pdf.ExtractEmbeddedFiles();

            SourceFile.Extracted = true;
            ExtractedFile.Update(SourceFile);

        }


        StringBuilder html = new StringBuilder();

        /// <summary>
        /// Generates the html file code
        /// </summary>
        /// <returns></returns>
        string GenerateHtml()
        {
            html = new StringBuilder();

            // get all the source files
            ExtractedFileDO[] sourceFiles = ExtractedFile.GetAll().Where(f => f.ParentFileId.HasValue == false).ToArray();
            
            html.AppendLine("<html>");
            html.AppendLine("<body>");
            html.AppendLine("<ul>");

            foreach (ExtractedFileDO file in sourceFiles)
            {
                WriteFile(file);
            }

            html.AppendLine("</ul>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return html.ToString();
        }

        void WriteFile(ExtractedFileDO file)
        {
            string href = file.FilePath.Replace(@"C:\Temp", "").Replace(@"\", "/");
            string name = Path.GetFileNameWithoutExtension(file.FileName);
            ExtractedFileDO[] children = ExtractedFile.GetAll().Where(f => f.ParentFileId == file.ExtractedFileId).ToArray();

            html.AppendLine("<li>");
            html.AppendLine(string.Format("<a href='{0}'>{1}</a>", href, name));
            if (children.Count() > 0)
            {
                html.AppendLine("<ul>");
                foreach (ExtractedFileDO child in children)
                {
                    WriteFile(child);
                }
                html.AppendLine("</ul>");
            }
            html.AppendLine("</li>");
        }


        void FinishedProcessing()
        {
            MessageBox.Show("Done");
        }


    }
}
