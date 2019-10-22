using System;
using System.Data;
using System.Collections;
using System.Drawing;

namespace PIPS.PAGIS.Db.SQLite
{

    public delegate byte[] ImageRefHandler(string img);

    public class ImageRefDataColumn : StringDataColumn
    {
        private ImageRefHandler imgCallback;
        public ImageRefDataColumn(ImageRefHandler imgCallback, string name)
            : base(name)
        {
            this.imgCallback = imgCallback;
        }

        protected override string GetStringInternal(object val)
        {
            byte[] buf = this.imgCallback(base.GetStringInternal(val));
            if ((buf == null) || (buf.Length <= 0))
                return string.Empty;
            return Convert.ToBase64String(buf, 0, buf.Length);
        }

    }

    public class StringDataColumn : DataColumn
    {
        public StringDataColumn(string name) : base(name) { }

        public override string Type
        {
            get
            {
                return "text";
            }
        }

        public override object GetValue(IDataReader rdr, int index)
        {
            return rdr.GetString(index);
        }

        public override void SetValue(IDataParameter param, object val)
        {
            param.DbType = DbType.String;
            param.Value = val;

        }

        protected override string GetStringInternal(object val)
        {
            return val.ToString();
        }

    }
    public class BooleanDataColumn : StringDataColumn
    {
        public BooleanDataColumn(string name) : base(name) { }

        public override object GetValue(IDataReader rdr, int index)
        {
            return bool.Parse((string)base.GetValue(rdr, index));
        }

        public override void SetValue(IDataParameter param, object val)
        {
            base.SetValue(param, val.ToString());
        }
    }
    public class CharDataColumn : StringDataColumn
    {
        public CharDataColumn(string name) : base(name) { }

        public override object GetValue(IDataReader rdr, int index)
        {
            return ((string)base.GetValue(rdr, index))[0];
        }

        public override void SetValue(IDataParameter param, object val)
        {
            base.SetValue(param, val.ToString());
        }

    }
    public class LongDataColumn : DataColumn
    {
        public LongDataColumn(string name) : base(name) { }

        public override string Type
        {
            get
            {
                return "integer";
            }
        }

        public override object GetValue(IDataReader rdr, int index)
        {
            return rdr.GetInt64(index);
        }

        public override void SetValue(IDataParameter param, object val)
        {
            param.DbType = DbType.Int64;
            param.Value = val;
        }

        protected override string GetStringInternal(object val)
        {
            return val.ToString();
        }

    }
    public class DateTimeDataColumn : LongDataColumn
    {
        public DateTimeDataColumn(string name) : base(name) { }

        public override object GetValue(IDataReader rdr, int index)
        {
            return new DateTime((long)base.GetValue(rdr, index));
        }

        public override void SetValue(IDataParameter param, object val)
        {
            DateTime dt = (DateTime)val;
            base.SetValue(param, dt.Ticks);
        }

        protected override string GetStringInternal(object val)
        {
            return base.GetStringInternal(((DateTime)val).Ticks);
        }

    }

    public class DoubleDataColumn : DataColumn
    {
        public DoubleDataColumn(string name) : base(name) { }

        public override string Type
        {
            get
            {
                return "real";
            }
        }

        /*public override string ToXmlValue(object val) {
            return ((double)val).ToString(
        }*/


        public override object GetValue(IDataReader rdr, int index)
        {
            return rdr.GetDouble(index);
        }

        public override void SetValue(IDataParameter param, object val)
        {
            param.DbType = DbType.Double;
            param.Value = val;
        }

        protected override string GetStringInternal(object val)
        {
            return ((double)val).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

    }

    /*public delegate byte[] GetBytes
    public class ImageRefDataColumn : StringDataColumn {
        public ImageRefDataColumn(string name) : base(name) {}

    }*/

    public class ByteArrayDataColumn : DataColumn
    {
        public ByteArrayDataColumn(string name) : base(name) { }

        public override string Type
        {
            get
            {
                return "blob";
            }
        }

        /*public override string ToXmlValue(object val) {
            return Convert.ToBase64String((byte[])val, 0, ((byte[])val).Length);
        }

        public override object FromXmlValue(string val) {
            return Convert.FromBase64String(val);
        }*/


        public override object GetValue(IDataReader rdr, int index)
        {
            int count;
            try
            {
                count = (int) rdr.GetBytes(index, 0, null, 0, 0);
            }
            catch (InvalidCastException ex)
            {
                count = 0;
            }
            if (count == 0)
                return null;
            var buffer = new byte[count];
            rdr.GetBytes(index, 0, buffer, 0, count);
            return buffer;
        }

        public override void SetValue(IDataParameter param, object val)
        {
            param.DbType = DbType.Binary;
            param.Value = val;
            if ((val != null) && (((byte[])val).Length <= 0))
            {
                param.Value = null;
            }
        }

        protected override string GetStringInternal(object val)
        {
            return Convert.ToBase64String((byte[])val, 0, ((byte[])val).Length);
        }

    }

    public class IntDataColumn : DataColumn
    {
        public IntDataColumn(string name) : base(name) { }

        public override string Type
        {
            get
            {
                return "integer";
            }
        }

        /*	public override object FromXmlValue(string val) {
                return int.Parse(val);
            }

            public override string ToXmlValue(object val) {
                return val.ToString();
            }*/


        public override object GetValue(IDataReader rdr, int index)
        {
            return rdr.GetInt32(index);
        }

        public override void SetValue(IDataParameter param, object val)
        {
            param.DbType = DbType.Int32;
            param.Value = val;
        }

        protected override string GetStringInternal(object val)
        {
            return val.ToString();
        }

    }
    public class ColorDataColumn : IntDataColumn
    {
        public ColorDataColumn(string name) : base(name) { }

        /*	public override string ToXmlValue(object val) {
                return base.ToXmlValue(((Color)val).ToArgb());
            }

            public override object FromXmlValue(string val) {
                return Color.FromArgb((int)base.FromXmlValue(val));
            }*/


        public override void SetValue(IDataParameter param, object val)
        {
            Color col = (Color)val;
            base.SetValue(param, col.ToArgb());
        }

        public override object GetValue(IDataReader rdr, int index)
        {
            return Color.FromArgb((int)base.GetValue(rdr, index));
        }

        protected override string GetStringInternal(object val)
        {
            return base.GetStringInternal(((Color)val).ToArgb());
        }

    }



    /// <summary>
    /// Summary description for DataTableColumn.
    /// </summary>
    public abstract class DataColumn
    {
        private string name;

        public DataColumn(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
        //public abstract string ToXmlValue(object val);
        //public abstract object FromXmlValue(string val);
        public abstract string Type { get; }
        public abstract object GetValue(IDataReader rdr, int index);
        public abstract void SetValue(IDataParameter param, object val);
        public string GetString(object val)
        {
            if (val == null)
                return string.Empty;
            return this.GetStringInternal(val);
        }
        protected abstract string GetStringInternal(object val);
    }

    public class DataColumnCollection : CollectionBase
    {
        public DataColumnCollection() { }

        public DataColumn this[int index]
        {
            get
            {
                return ((DataColumn)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        public int Add(DataColumn value)
        {
            return (List.Add(value));
        }

        public int IndexOf(DataColumn value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, DataColumn value)
        {
            List.Insert(index, value);
        }

        public void Remove(DataColumn value)
        {
            List.Remove(value);
        }

        public bool Contains(DataColumn value)
        {
            return (List.Contains(value));
        }

        protected override void OnInsert(int index, Object value)
        {
            if (!(value is DataColumn))
                throw new ArgumentException("value must be of type DataColumn.", "value");
        }

        protected override void OnRemove(int index, Object value)
        {
            if (!(value is DataColumn))
                throw new ArgumentException("value must be of type DataColumn.", "value");
        }

        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (!(newValue is DataColumn))
                throw new ArgumentException("newValue must be of type DataColumn.", "newValue");
        }

        protected override void OnValidate(Object value)
        {
            if (!(value is DataColumn))
                throw new ArgumentException("value must be of type DataColumn.");
        }

    }
}
