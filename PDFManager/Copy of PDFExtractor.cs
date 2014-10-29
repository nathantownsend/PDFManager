using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xPDFManager
{
    public class PDFExtractor
    {

        /// <summary>
        /// The full path of the document
        /// </summary>
        string _documentPath;

        /// <summary>
        /// The folder containing the document
        /// </summary>
        string _documentFolder;
        
        /// <summary>
        /// The folder for writing attached documents
        /// </summary>
        string _childFolder;

        string _html;

        /// <summary>
        /// The child documnets
        /// </summary>
        List<PDFExtractor> _children = new List<PDFExtractor>();


        /// <summary>
        /// Opens a PDF File for extraction in current location
        /// </summary>
        public PDFExtractor(string DocumentPath)
        {
            _documentPath = DocumentPath;
            _documentFolder = Path.GetDirectoryName(DocumentPath);
            _childFolder = Path.Combine(_documentFolder, Path.GetFileNameWithoutExtension(_documentPath));
        }

        /// <summary>
        /// Opens a PDF File for extraction in new location
        /// </summary>
        /// <param name="DocumentPath"></param>
        /// <param name="OutputFolder"></param>
        public PDFExtractor(string DocumentPath, string OutputFolder)
        {
            _documentPath = DocumentPath;
            _documentFolder = Path.GetDirectoryName(DocumentPath);
            _childFolder = OutputFolder;
        }


        /// <summary>
        /// Extracts attached PDF files and removes them from the document
        /// </summary>
        public string Extract(bool KeepOriginalPDF)
        {

            Dictionary<string, byte[]> attachments = GetEmbeddedFiles();

            // if no attachments, then done
            if (attachments.Count == 0)
            {
                _html = string.Format("<ul><li><a href='{0}'>{1}</a></li></ul>", DocumentPath, Path.GetFileNameWithoutExtension(DocumentPath));
                return _html;
            }

            // when the client calls this operation they may want to preserve the original with the embedded paths
            // as well as create one without the attachments. 
            if (KeepOriginalPDF)
            {
                string originalPath = Path.Combine(_documentFolder, Path.GetFileNameWithoutExtension(_documentPath) + ".original" + Path.GetExtension(_documentPath));
                File.Copy(_documentPath, originalPath);
            }


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<ul>");
            sb.AppendLine("  <li>");
            sb.AppendLine(string.Format("    <a href='{0}'>{1}</a>", DocumentPath, Path.GetFileNameWithoutExtension(DocumentPath)));

            // ensure child directory exists
            if (!Directory.Exists(_childFolder))
                Directory.CreateDirectory(_childFolder);


            // write each child file
            foreach (KeyValuePair<string, byte[]> attachment in attachments)
            {
                string childPath = Path.Combine(_childFolder, attachment.Key);
                File.WriteAllBytes(childPath, attachment.Value);
                PDFExtractor child = new PDFExtractor(childPath);
                _children.Add(child);
            }

            // extract each child pdf without keeping a copy of the original
            foreach (PDFExtractor child in _children)
            {
                sb.Append(child.Extract(false));
                sb.AppendLine();
            }

            

            // now that children are written to disk, remove them from this document
            RemoveEmbeddedFiles();


            sb.AppendLine("  </li>");
            sb.AppendLine("</ul>");
            _html = sb.ToString();
            return _html;

        }


        /// <summary>
        /// writes the generated html as a text file
        /// </summary>
        public void WriteTextIndex()
        {
            string replace = _documentFolder.Substring(0, _documentFolder.IndexOf("CoalUranium"));
            _html = _html.Replace(replace, "/");
            _html = _html.Replace("\\", "/");
            string htmlPath = Path.Combine(_documentFolder, Path.GetFileNameWithoutExtension(DocumentPath) + ".txt");
            File.WriteAllText(htmlPath, _html);
        }

        public void WriteHtmlIndex()
        {
            _html = _html.Replace(_documentFolder + "\\", "");
            _html = _html.Replace("\\", "/");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset=\"utf-8\">");
            sb.AppendLine(string.Format("<title>{0} Contents</title>", Path.GetFileNameWithoutExtension(DocumentPath)));
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.Append(_html);
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            string htmlPath = Path.Combine(_documentFolder, Path.GetFileNameWithoutExtension(DocumentPath) + ".htm");
            File.WriteAllText(htmlPath, sb.ToString());

        }

        /// <summary>
        /// The child documents (attachments)
        /// </summary>
        public List<PDFExtractor> Children { get { return _children; } }


        /// <summary>
        /// The full path of the document
        /// </summary>
        public string DocumentPath { get { return _documentPath; } }



        /// <summary>
        /// Returns the embedded pdf documents within a file
        /// </summary>
        /// <param name="writeReader"></param>
        /// <returns></returns>
        Dictionary<string, byte[]> GetEmbeddedFiles()
        {

            PdfReader reader = new PdfReader(DocumentPath);

            // container for the extracted files
            Dictionary<string, byte[]> attachments = new Dictionary<string, byte[]>();

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
                            string name = fileSpec.GetAsString(key).ToString();
                            byte[] bytes = PdfReader.GetStreamBytes(stream);
                            attachments.Add(name, bytes);
                        }

                    }
                }
            }

            reader.Close();
            reader.Dispose();

            // return embedded files
            return attachments;

        }



        void RemoveEmbeddedFiles()
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
