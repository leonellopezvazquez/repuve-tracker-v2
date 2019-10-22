using System;
using System.Collections;

namespace PIPS.PAGIS.Db.SQLite
{
    public class DataFileCollection : CollectionBase {
        public DataFileCollection() {}

        public DataFileBase this[ int index ]  {
            get  {
                return( (DataFileBase) List[index] );
            }
            set  {
                List[index] = value;
            }
        }

        public DataFileBase this[ Type type ] {
            get {
                foreach(DataFileBase file in List)
                    if(file.GetType() == type)
                        return file;
                return null;
            }
        }

        public int Add( DataFileBase value )  {
            return( List.Add( value ) );
        }

        public int IndexOf( DataFileBase value )  {
            return( List.IndexOf( value ) );
        }

        public void Insert( int index, DataFileBase value )  {
            List.Insert( index, value );
        }

        public void Remove( DataFileBase value )  {
            List.Remove( value );
        }

        public bool Contains( DataFileBase value )  {
            return( List.Contains( value ) );
        }

        protected override void OnInsert( int index, Object value )  {
            if ( !(value is DataFileBase) )
                throw new ArgumentException( "value must be of type DataFileBase.", "value" );
        }

        protected override void OnRemove( int index, Object value )  {
            if ( !(value is DataFileBase) )
                throw new ArgumentException( "value must be of type DataFileBase.", "value" );
        }

        protected override void OnSet( int index, Object oldValue, Object newValue )  {
            if ( !(newValue is DataFileBase) )
                throw new ArgumentException( "newValue must be of type DataFileBase.", "newValue" );
        }

        protected override void OnValidate( Object value )  {
            if ( !(value is DataFileBase) )
                throw new ArgumentException( "value must be of type DataFileBase." );
        }

    }
}