using iTextSharp.text;
using iTextSharp.text.pdf;
using PDFManager.DAL.dbo;
using PDFManager.DO.dbo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFManager
{
    public class PDFFile
    {


        /// <summary>
        /// Creates a new extractor for a pdf document
        /// </summary>
        /// <param name="PDFPath">The full path to a PDF document that may contain child documents</param>
        public PDFFile(ExtractedFileDO Source)
        {
            SourceFile = Source;
        }

        public ExtractedFileDO SourceFile { get; set; }
        
        


        public void ExtractEmbeddedFiles()
        {
            // create a new reader object
            PdfReader reader = new PdfReader(SourceFile.FilePath);

            // get the names
            PdfDictionary names = reader.Catalog.GetAsDict(PdfName.NAMES);
            if (names == null)
                return;

            // get the files
            PdfDictionary embeddedFiles = names.GetAsDict(PdfName.EMBEDDEDFILES);
            if (embeddedFiles == null)
                return;

            // get the specs
            PdfArray fileSpecs = embeddedFiles.GetAsArray(PdfName.NAMES);
            if (fileSpecs == null)
                return;

            // how many embedded files
            int eFLength = fileSpecs.Size;


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
                            // save the file data to db
                            string fileName =fileSpec.GetAsString(key).ToString();
                            ExtractedFileDO extractedFile = new ExtractedFileDO()
                            {
                                FileName = fileName,
                                Extracted = false,
                                FilePath = ComputeChildFilePath(fileName),
                                ParentFileId = SourceFile.ExtractedFileId
                            };

                            ExtractedFile.Create(extractedFile);

                            // write extracted file to disk
                            File.WriteAllBytes(extractedFile.FilePath, PdfReader.GetStreamBytes(stream));
                        }
                    }
                }
            }

            reader.Close();
            reader.Dispose();

            // remove embedded files
            RemoveEmbeddedFiles(SourceFile.FilePath);

        }


        private string ComputeChildFilePath(string ChildFileName)
        {
            string folder = SourceFile.FilePath.Replace(SourceFile.FileName, "") + SourceFile.ExtractedFileId.ToString();
            
            Directory.CreateDirectory(folder);

            string path = folder + "\\" + ChildFileName;

            return path;


        }


        void RemoveEmbeddedFiles(string _documentPath)
        {

            // we want to overwrite the input file with one that has not embedded files
            string outputFile = _documentPath;

            // make a copy of the original for reading
            string Folder = Path.GetDirectoryName(_documentPath);
            string FileName = Path.GetFileNameWithoutExtension(_documentPath);
            string tempFile = Path.Combine(Folder, FileName) + ".temp" + Path.GetExtension(_documentPath);
            File.Move(_documentPath, tempFile);

            // get input document
            PdfReader inputPdf = new PdfReader(tempFile);


            // retrieve the total number of pages
            int pageCount = inputPdf.NumberOfPages;


            // load the input document
            Document inputDoc = new Document(inputPdf.GetPageSizeWithRotation(1));


            // create the filestream
            using (FileStream fs = new FileStream(outputFile, FileMode.Create))
            {
                // create the output writer
                PdfWriter outputWriter = PdfWriter.GetInstance(inputDoc, fs);
                inputDoc.Open();
                PdfContentByte cb1 = outputWriter.DirectContent;


                // copy pages from input to output document
                for (int i = 1; i <= pageCount; i++)
                {
                    inputDoc.SetPageSize(inputPdf.GetPageSizeWithRotation(i));
                    inputDoc.NewPage();


                    PdfImportedPage page = outputWriter.GetImportedPage(inputPdf, i);
                    int rotation = inputPdf.GetPageRotation(i);


                    if (rotation == 90 || rotation == 270)
                    {
                        cb1.AddTemplate(page, 0, -1f, 1f, 0, 0,
                            inputPdf.GetPageSizeWithRotation(i).Height);
                    }
                    else
                    {
                        cb1.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                    }
                }


                inputDoc.Close();
                inputDoc.Dispose();
            }

            inputPdf.Close();
            inputPdf.Dispose();

            // delete the temp file
            File.Delete(tempFile);
        }
        
    }
}
