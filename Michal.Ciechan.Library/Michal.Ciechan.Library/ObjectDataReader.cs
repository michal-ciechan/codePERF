// <copyright project="Salient.Reflection" file="DynamicProperties.cs" company="Sky Sanders">
// This source is a Public Domain Dedication.
// Please see http://spikes.codeplex.com/ for details.   
// Attribution is appreciated
// </copyright> 
// <version>1.0</version>

using System;
using System.Collections.Generic;
using System.Data;

namespace Michal.Ciechan.Library
{
    public abstract class ObjectDataReader : IDataReader
        {
            protected bool Closed;
            protected IList<DynamicProperties.Property> Fields;

            protected ObjectDataReader()
            {
            }

            protected ObjectDataReader(Type elementType)
            {
                SetFields(elementType);
                Closed = false;
            }

            #region IDataReader Members

            public abstract object GetValue(int i);
            public abstract bool Read();

            #endregion

            #region Implementation of IDataRecord

            public int FieldCount
            {
                get { return Fields.Count; }
            }

            public virtual int GetOrdinal(string name)
            {
                for (int i = 0; i < Fields.Count; i++)
                {
                    if (Fields[i].Info.Name == name)
                    {
                        return i;
                    }
                }

                throw new IndexOutOfRangeException("name");
            }

            object IDataRecord.this[int i]
            {
                get { return GetValue(i); }
            }

            public virtual bool GetBoolean(int i)
            {
                return (Boolean)GetValue(i);
            }

            public virtual byte GetByte(int i)
            {
                return (Byte)GetValue(i);
            }

            public virtual char GetChar(int i)
            {
                return (Char)GetValue(i);
            }

            public virtual DateTime GetDateTime(int i)
            {
                return (DateTime)GetValue(i);
            }

            public virtual decimal GetDecimal(int i)
            {
                return (Decimal)GetValue(i);
            }

            public virtual double GetDouble(int i)
            {
                return (Double)GetValue(i);
            }

            public virtual Type GetFieldType(int i)
            {
                return Fields[i].Info.PropertyType;
            }

            public virtual float GetFloat(int i)
            {
                return (float)GetValue(i);
            }

            public virtual Guid GetGuid(int i)
            {
                return (Guid)GetValue(i);
            }

            public virtual short GetInt16(int i)
            {
                return (Int16)GetValue(i);
            }

            public virtual int GetInt32(int i)
            {
                return (Int32)GetValue(i);
            }

            public virtual long GetInt64(int i)
            {
                return (Int64)GetValue(i);
            }

            public virtual string GetString(int i)
            {
                return (string)GetValue(i);
            }

            public virtual bool IsDBNull(int i)
            {
                return GetValue(i) == null;
            }

            object IDataRecord.this[string name]
            {
                get { return GetValue(GetOrdinal(name)); }
            }


            public virtual string GetDataTypeName(int i)
            {
                return GetFieldType(i).Name;
            }


            public virtual string GetName(int i)
            {
                if (i < 0 || i >= Fields.Count)
                {
                    throw new IndexOutOfRangeException("name");
                }
                return Fields[i].Info.Name;
            }

            public virtual int GetValues(object[] values)
            {
                int i = 0;
                for (; i < Fields.Count; i++)
                {
                    if (values.Length <= i)
                    {
                        return i;
                    }
                    values[i] = GetValue(i);
                }
                return i;
            }

            public virtual IDataReader GetData(int i)
            {
                // need to think about this one
                throw new NotImplementedException();
            }

            public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
            {
                // need to keep track of the bytes got for each record - more work than i want to do right now
                // http://msdn.microsoft.com/en-us/library/system.data.idatarecord.getbytes.aspx
                throw new NotImplementedException();
            }

            public virtual long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
            {
                // need to keep track of the bytes got for each record - more work than i want to do right now
                // http://msdn.microsoft.com/en-us/library/system.data.idatarecord.getchars.aspx
                throw new NotImplementedException();
            }

            #endregion

            #region Implementation of IDataReader

            public virtual void Close()
            {
                Closed = true;
            }


            public virtual DataTable GetSchemaTable()
            {
                var dt = new DataTable();
                foreach (DynamicProperties.Property field in Fields)
                {
                    dt.Columns.Add(new DataColumn(field.Info.Name, field.Info.PropertyType));
                }
                return dt;
            }

            public virtual bool NextResult()
            {
                throw new NotImplementedException();
            }


            public virtual int Depth
            {
                get { throw new NotImplementedException(); }
            }

            public virtual bool IsClosed
            {
                get { return Closed; }
            }

            public virtual int RecordsAffected
            {
                get
                {
                    // assuming select only?
                    return -1;
                }
            }

            #endregion

            #region Implementation of IDisposable

            public virtual void Dispose()
            {
                Fields = null;
            }

            #endregion

            protected void SetFields(Type elementType)
            {
                Fields = DynamicProperties.CreatePropertyMethods(elementType);
            }
        }
}