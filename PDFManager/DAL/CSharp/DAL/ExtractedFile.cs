// CREATED BY: Nathan Townsend
// CREATED DATE: 10/29/2014
// DO NOT MODIFY THIS CODE
// CHANGES WILL BE LOST WHEN THE GENERATOR IS RUN AGAIN
// GENERATION TOOL: Dalapi Code Generator (DalapiPro.com)



using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using PDFManager.DAL;
using PDFManager.DO.dbo;

namespace PDFManager.DAL.dbo
{
    /// <summary>
    /// Provides data access methods for the ExtractedFile database table
    /// </summary>
    /// <remarks>
    public partial class ExtractedFile
    {
        
        /// <summary>
        /// Creates a new ExtractedFile record
        /// </summary>
        public static int Create(ExtractedFileDO DO)
        {
            SqlParameter _ParentFileId = new SqlParameter("ParentFileId", SqlDbType.Int);
            SqlParameter _FileName = new SqlParameter("FileName", SqlDbType.VarChar);
            SqlParameter _FilePath = new SqlParameter("FilePath", SqlDbType.VarChar);
            SqlParameter _Extracted = new SqlParameter("Extracted", SqlDbType.Bit);
            
            _ParentFileId.Value = DO.ParentFileId;
            _FileName.Value = DO.FileName;
            _FilePath.Value = DO.FilePath;
            _Extracted.Value = DO.Extracted;
            
            SqlParameter[] _params = new SqlParameter[] {
                _ParentFileId,
                _FileName,
                _FilePath,
                _Extracted
            };

            return DataCommon.ExecuteScalar("[dbo].[ExtractedFile_Insert]", _params, "dbo");
            
        }


        /// <summary>
        /// Updates a ExtractedFile record and returns the number of records affected
        /// </summary>
        public static int Update(ExtractedFileDO DO)
        {
            SqlParameter _ExtractedFileId = new SqlParameter("ExtractedFileId", SqlDbType.Int);
            SqlParameter _ParentFileId = new SqlParameter("ParentFileId", SqlDbType.Int);
            SqlParameter _FileName = new SqlParameter("FileName", SqlDbType.VarChar);
            SqlParameter _FilePath = new SqlParameter("FilePath", SqlDbType.VarChar);
            SqlParameter _Extracted = new SqlParameter("Extracted", SqlDbType.Bit);
            
            _ExtractedFileId.Value = DO.ExtractedFileId;
            _ParentFileId.Value = DO.ParentFileId;
            _FileName.Value = DO.FileName;
            _FilePath.Value = DO.FilePath;
            _Extracted.Value = DO.Extracted;
            
            SqlParameter[] _params = new SqlParameter[] {
                _ExtractedFileId,
                _ParentFileId,
                _FileName,
                _FilePath,
                _Extracted
            };

            return DataCommon.ExecuteScalar("[dbo].[ExtractedFile_Update]", _params, "dbo");
        }


        /// <summary>
        /// Deletes a ExtractedFile record
        /// </summary>
        public static int Delete(ExtractedFileDO DO)
        {
            SqlParameter _ExtractedFileId = new SqlParameter("ExtractedFileId", SqlDbType.Int);
            
            _ExtractedFileId.Value = DO.ExtractedFileId;
            
            SqlParameter[] _params = new SqlParameter[] {
                _ExtractedFileId
            };

            return DataCommon.ExecuteScalar("[dbo].[ExtractedFile_Delete]", _params, "dbo");
        }


        /// <summary>
        /// Gets all ExtractedFile records
        /// </summary>
        public static ExtractedFileDO[] GetAll()
        {
            SafeReader sr = DataCommon.ExecuteSafeReader("[dbo].[ExtractedFile_GetAll]", new SqlParameter[] { }, "dbo");
            
            List<ExtractedFileDO> objs = new List<ExtractedFileDO>();
            
            while(sr.Read()){

                ExtractedFileDO obj = new ExtractedFileDO();
                
                obj.ExtractedFileId = sr.GetInt32(sr.GetOrdinal("ExtractedFileId"));
                obj.FileName = sr.GetString(sr.GetOrdinal("FileName"));
                obj.FilePath = sr.GetString(sr.GetOrdinal("FilePath"));
                obj.Extracted = sr.GetBoolean(sr.GetOrdinal("Extracted"));
                if (sr.IsDBNull(sr.GetOrdinal("ParentFileId"))) { obj.ParentFileId = null; } else { obj.ParentFileId = sr.GetInt32(sr.GetOrdinal("ParentFileId")); }


                objs.Add(obj);
            }

            return objs.ToArray();
        }


        /// <summary>
        /// Selects ExtractedFile records by PK
        /// </summary>
        public static ExtractedFileDO[] GetByPK(Int32 ExtractedFileId)
        {

            SqlParameter _ExtractedFileId = new SqlParameter("ExtractedFileId", SqlDbType.Int);
			
            _ExtractedFileId.Value = ExtractedFileId;
			
            SqlParameter[] _params = new SqlParameter[] {
                _ExtractedFileId
            };

            SafeReader sr = DataCommon.ExecuteSafeReader("[dbo].[ExtractedFile_GetByPK]", _params, "dbo");

            List<ExtractedFileDO> objs = new List<ExtractedFileDO>();
			
            while(sr.Read())
            {
                ExtractedFileDO obj = new ExtractedFileDO();
				
                obj.ExtractedFileId = sr.GetInt32(sr.GetOrdinal("ExtractedFileId"));
                obj.FileName = sr.GetString(sr.GetOrdinal("FileName"));
                obj.FilePath = sr.GetString(sr.GetOrdinal("FilePath"));
                obj.Extracted = sr.GetBoolean(sr.GetOrdinal("Extracted"));
                if (sr.IsDBNull(sr.GetOrdinal("ParentFileId"))) { obj.ParentFileId = null; } else { obj.ParentFileId = sr.GetInt32(sr.GetOrdinal("ParentFileId")); }

                objs.Add(obj);
            }

            return objs.ToArray();
        }




        /// <summary>
        /// Truncates the ExtractedFile Table
        /// </summary>
        public static void Truncate()
        {
            DataCommon.TruncateTable("dbo", "ExtractedFile");
        }

    }
}