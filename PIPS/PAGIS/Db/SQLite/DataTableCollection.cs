using System;
using System.Collections;

namespace PIPS.PAGIS.Db.SQLite
{
    public class DataTableCollection : CollectionBase
    {
        public DataTableBase this[int index]
        {
            get { return ((DataTableBase) List[index]); }
            set { List[index] = value; }
        }

        public int Add(DataTableBase value)
        {
            return (List.Add(value));
        }

        public int IndexOf(DataTableBase value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, DataTableBase value)
        {
            List.Insert(index, value);
        }

        public void Remove(DataTableBase value)
        {
            List.Remove(value);
        }

        public bool Contains(DataTableBase value)
        {
            return (List.Contains(value));
        }

        protected override void OnInsert(int index, Object value)
        {
            if (!(value is DataTableBase))
                throw new ArgumentException("value must be of type DataTableBase.", "value");
        }

        protected override void OnRemove(int index, Object value)
        {
            if (!(value is DataTableBase))
                throw new ArgumentException("value must be of type DataTableBase.", "value");
        }

        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (!(newValue is DataTableBase))
                throw new ArgumentException("newValue must be of type DataTableBase.", "newValue");
        }

        protected override void OnValidate(Object value)
        {
            if (!(value is DataTableBase))
                throw new ArgumentException("value must be of type DataTableBase.");
        }
    }
}