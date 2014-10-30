// CREATED BY: Nathan Townsend
// CREATED DATE: 10/29/2014
// DO NOT MODIFY THIS CODE
// CHANGES WILL BE LOST WHEN THE GENERATOR IS RUN AGAIN
// GENERATION TOOL: Dalapi Code Generator (DalapiPro.com)



using System;
using System.ComponentModel.DataAnnotations;

namespace PDFManager.DO.dbo
{
    /// <summary>
    /// Encapsulates a row of data in the ExtractedFile table
    /// </summary>
    public partial class ExtractedFileDO
    {

        public virtual Int32 ExtractedFileId {get; set;}
        public virtual Int32? ParentFileId {get; set;}
        public virtual String FileName {get; set;}
        public virtual String FilePath {get; set;}
        public virtual Boolean Extracted {get; set;}

    }
}