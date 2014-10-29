using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFManager
{
    public class PdfFile
    {
        PdfReader reader;

        /// <summary>
        /// Creates a new PDFDocument object from a pdf file on disk
        /// </summary>
        /// <param name="FilePath"></param>
        public PdfFile(string FilePath) : this(new PdfReader(FilePath)) { }
        

        /// <summary>
        /// Creates a new PDFDocument object from a pdf file in memory
        /// </summary>
        /// <param name="Reader"></param>
        public PdfFile(PdfReader Reader)
        {
            reader = Reader;
        }



    }
}
