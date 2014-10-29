using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFManager
{
    public class PDFExtractor
    {
        /// <summary>
        /// The folder where extracted files will go
        /// </summary>
        string _basePath = @"C:\Temp\CoalUranium\PDF\";

        /// <summary>
        /// The file name of the document
        /// </summary>
        public string FileName
        {
            get
            {
                return Path.GetFileName(OriginalPath);
            }
        }


        /// <summary>
        /// The file name without the extension
        /// </summary>
        public string FileNameWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(OriginalPath);
            }
        }


        /// <summary>
        /// The path to the original document
        /// </summary>
        public string OriginalPath { get; set; }


        /// <summary>
        /// The path to the extracted document
        /// </summary>
        public string ExtractedPath
        {
            get
            {
                return Path.Combine(DocumentFolder, FileName);
            }
        }


        /// <summary>
        /// The folder where the Extracted document will be written
        /// </summary>
        public string DocumentFolder
        {
            get
            {
                FileInfo fileInfo = new FileInfo(OriginalPath);
                string projectFolder = fileInfo.Directory.Name;
                return Path.Combine(_basePath, projectFolder);
            }
        }


        /// <summary>
        /// The path to the folder where child documents will be extracted into
        /// </summary>
        public string ChildDocumentFolder
        {
            get
            {
                return Path.Combine(DocumentFolder, FileNameWithoutExtension);
            }
        }




        /// <summary>
        /// Creates a new extractor for a pdf document
        /// </summary>
        /// <param name="PDFPath">The full path to a PDF document that may contain child documents</param>
        public PDFExtractor(string PDFPath, string OutputFolder)
        {
            OriginalPath = PDFPath;
            _basePath = OutputFolder;

            if (!Directory.Exists(DocumentFolder))
                Directory.CreateDirectory(DocumentFolder);
        }


        /// <summary>
        /// Extract the child pdf documents into the Child Document Folder
        /// </summary>
        public void Extract()
        {
            // get a list of the extracted files
            List<string> extractedFiles = ExtractEmbeddedFiles();

            // process each extracted file
            foreach (string extractedFile in extractedFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(extractedFile);
                string folder = string.Format(@"{0}{1}\", _basePath, fileName);
                PDFExtractor extractor = new PDFExtractor(extractedFile, folder);
                extractor.Extract();
            }

        }


        List<string> ExtractEmbeddedFiles()
        {
            // create a new reader object
            PdfReader reader = new PdfReader(OriginalPath);

            // container for the extracted files
            List<string> attachments = new List<string>();

            // get the names
            PdfDictionary names = reader.Catalog.GetAsDict(PdfName.NAMES);
            if (names == null)
                return attachments;

            // get the files
            PdfDictionary embeddedFiles = names.GetAsDict(PdfName.EMBEDDEDFILES);
            if (embeddedFiles == null)
                return attachments;

            // get the specs
            PdfArray fileSpecs = embeddedFiles.GetAsArray(PdfName.NAMES);
            if (fileSpecs == null)
                return attachments;

            // how many embedded files
            int eFLength = fileSpecs.Size;

            // ensure child directory exists
            if (eFLength > 0 && !Directory.Exists(ChildDocumentFolder))
                Directory.CreateDirectory(ChildDocumentFolder);
            

            // add all embedded files to the dictionary
            for (int i = 0; i < eFLength; i++)
            {
                i++; //objects are in pairs and only want odd objects (1,3,5...)
                PdfDictionary fileSpec = fileSpecs.GetAsDict(i); // may be null
                if (fileSpec != null)
                {
                    PdfDictionary refs = fileSpec.GetAsDict(PdfName.EF);
                    foreach (PdfName key in refs.Keys)
                    {
                        // get the name and bytes
                        PRStream stream = (PRStream)PdfReader.GetPdfObject(refs.GetAsIndirectObject(key));
                        if (stream != null)
                        {
                            // add a reference to the extracted file in the dictionary
                            string name = fileSpec.GetAsString(key).ToString();
                            string path = Path.Combine(ChildDocumentFolder, name);
                            attachments.Add(path);

                            // extract the file if it hasn't been extracted already
                            if (!File.Exists(path))
                            {
                                byte[] bytes = PdfReader.GetStreamBytes(stream);
                                File.WriteAllBytes(path, bytes);
                            }
                        }
                    }
                }
            }

            reader.Close();
            reader.Dispose();

            // return embedded files
            return attachments;

        }
        
    }
}
