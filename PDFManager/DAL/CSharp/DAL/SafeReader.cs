using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PDFManager
{
    public class SafeReader : IDataReader
    {
        int i = -1;
        int _fieldCount;
        int _depth;
        bool _closed = false;

        List<Dictionary<string, object>> _data = new List<Dictionary<string, object>>();
        Dictionary<string, object> _currentRow;

        public SafeReader(SqlDataReader dr)
        {
            _fieldCount = dr.FieldCount;
            _depth = dr.Depth;

            while (dr.Read())
            {

                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    string name = dr.GetName(i);
                    object value = dr.GetValue(i);
                    row.Add(name, value);
                }
                _data.Add(row);
            }
        }

        /// <summary>
        /// Moves the reader to the previous row
        /// </summary>
        public void PreviousRow()
        {
            i--;
        }

        /// <summary>
        /// Advances the SafeReader to the next record
        /// </summary>
        /// <returns>False if the last record has been returned</returns>
        public bool Read()
        {
            if (_closed)
                throw new DataException("Attempt to read when reader is closed");
            i++;
            return (i < _data.Count);
        }

        /// <summary>
        /// Returns the value of the column
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public object GetValue(string ColumnName)
        {
            _currentRow = _data[i];
            return _currentRow[ColumnName];
        }

        /// <summary>
        /// Returns the value of the column
        /// </summary>
        /// <param name="ColumnIndex"></param>
        /// <returns></returns>
        public object GetValue(int ColumnIndex)
        {
            _currentRow = _data[i];
            object[] objs = _currentRow.Values.ToArray();
            return objs[ColumnIndex];
        }

        /// <summary>
        /// Returns the name of the column
        /// </summary>
        /// <param name="ColumnIndex"></param>
        /// <returns></returns>
        public string GetName(int ColumnIndex)
        {
            _currentRow = _data[i];
            string[] objs = _currentRow.Keys.ToArray();
            return objs[ColumnIndex];
        }

        public int RowCount
        {
            get
            {
                return _data.Count;
            }
        }


        #region IDataReader Members

        public void Close()
        {
            _closed = true;
        }

        public int Depth
        {
            get { return _depth; }
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool IsClosed
        {
            get { return _closed; }
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public int RecordsAffected
        {
            get { return _data.Count; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _data = null;
            _currentRow = null;
        }

        #endregion

        #region IDataRecord Members

        public int FieldCount
        {
            get { return _fieldCount; }
        }

        public bool GetBoolean(int i)
        {
            return Convert.ToBoolean(GetValue(i));
        }

        public byte GetByte(int i)
        {
            return Convert.ToByte(GetValue(i));
        }

        public byte[] GetBytes(int i)
        {
            byte[] value = (byte[])GetValue(i);
            return value;
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            return Convert.ToChar(GetValue(i));
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            return Convert.ToDateTime(GetValue(i));
        }

        public decimal GetDecimal(int i)
        {
            return Convert.ToDecimal(GetValue(i));
        }

        public double GetDouble(int i)
        {
            return Convert.ToDouble(GetValue(i));
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            return new Guid(this.GetValue(i).ToString());

        }

        public short GetInt16(int i)
        {
            return Convert.ToInt16(GetValue(i));
        }

        public int GetInt32(int i)
        {
            return Convert.ToInt32(GetValue(i));
        }

        public long GetInt64(int i)
        {
            return Convert.ToInt64(GetValue(i));
        }

        public int GetOrdinal(string name)
        {
            _currentRow = _data[i];
            string[] objs = _currentRow.Keys.ToArray();
            return Array.IndexOf(objs, name);
        }

        public string GetString(int i)
        {
            return Convert.ToString(GetValue(i));
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            return Convert.IsDBNull(GetValue(i));
        }

        public object this[string name]
        {
            get { return GetValue(name); }
        }

        public object this[int i]
        {
            get { return GetValue(i); }
        }

        #endregion
    }

}