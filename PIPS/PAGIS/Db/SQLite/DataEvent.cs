using System;
using System.Drawing;
using System.Collections;
using System.IO;

namespace PIPS.PAGIS.Db.SQLite
{
	public delegate void DataEventHandler(DataEvent ev);
	
	/// <summary>
	/// Summary description for DataEvent.
	/// </summary>
	public abstract class DataEvent {
		public static Image CreateImage(byte[] data) {
			if((data == null) || (data.Length <= 0))
				return null;
			MemoryStream ms = new MemoryStream(data);
			return Image.FromStream(ms);
		}

		private object[] data;
		private long id;
		protected DataEventCollectionCollection linkedEventCollections;
		private DataTableBase table;
		private bool abortTransaction = false;
		
		public DataEvent(DataTableBase table) {
			this.id = -1;
			this.table = table;

			this.data = new object[this.table.Columns.Count];
			for(int i = 0; i < this.table.Columns.Count; i++)
				this.data[i] = null;

			this.linkedEventCollections = new DataEventCollectionCollection();
			foreach(DataTableBase linkedTable in this.table.LinkedTables)
				this.linkedEventCollections.Add(new DataEventCollection());
		}

		public DataTableBase Table {
			get { return this.table; }
		}

		public void Delete() {
			this.Table.Delete(this);
		}

		public virtual void Save() {
			this.Table.Save(this);
		}

		public DataEventCollection LinkedDataEvents(DataTableBase linkedTable) {
			return this.LinkedDataEvents(this.table.LinkedTables.IndexOf(linkedTable));
		}
		public virtual DataEventCollection LinkedDataEvents(int index) {
			return this.linkedEventCollections[index];
		}

		public long ID {
			get {
				return this.id;
			}
			set {
				this.id = value;
			}
		}

		public object this[int index] {
			get {
				return this.data[index];
			}
			set {
				this.data[index] = value;
			}
		}

		public int ColumnCount {
			get {
				return this.table.Columns.Count;
			}
		}

		public bool AbortTransaction {
			get {
				return this.abortTransaction;
			}
			set {
				this.abortTransaction = value;
			}
		}
	}

	public class DataEventCollection : CollectionBase {
		public DataEventCollection() {}

		public DataEvent this[ int index ]  {
			get  {
				return( (DataEvent) List[index] );
			}
			set  {
				List[index] = value;
			}
		}

		public int Add( DataEvent value )  {
			return( List.Add( value ) );
		}

		public int IndexOf( DataEvent value )  {
			return( List.IndexOf( value ) );
		}

		public void Insert( int index, DataEvent value )  {
			List.Insert( index, value );
		}

		public void Remove( DataEvent value )  {
			List.Remove( value );
		}

		public bool Contains( DataEvent value )  {
			return( List.Contains( value ) );
		}

		protected override void OnInsert( int index, Object value )  {
			if ( !(value is DataEvent) )
				throw new ArgumentException( "value must be of type DataEvent.", "value" );
		}

		protected override void OnRemove( int index, Object value )  {
			if ( !(value is DataEvent) )
				throw new ArgumentException( "value must be of type DataEvent.", "value" );
		}

		protected override void OnSet( int index, Object oldValue, Object newValue )  {
			if ( !(newValue is DataEvent) )
				throw new ArgumentException( "newValue must be of type DataEvent.", "newValue" );
		}

		protected override void OnValidate( Object value )  {
			if ( !(value is DataEvent) )
				throw new ArgumentException( "value must be of type DataEvent." );
		}

	}

	public class DataEventCollectionCollection : CollectionBase {
		public DataEventCollectionCollection() {}

		public DataEventCollection this[ int index ]  {
			get  {
				return( (DataEventCollection) List[index] );
			}
			set  {
				List[index] = value;
			}
		}

		public int Add( DataEventCollection value )  {
			return( List.Add( value ) );
		}

		public int IndexOf( DataEventCollection value )  {
			return( List.IndexOf( value ) );
		}

		public void Insert( int index, DataEventCollection value )  {
			List.Insert( index, value );
		}

		public void Remove( DataEventCollection value )  {
			List.Remove( value );
		}

		public bool Contains( DataEventCollection value )  {
			return( List.Contains( value ) );
		}

		protected override void OnInsert( int index, Object value )  {
			if ( !(value is DataEventCollection) )
				throw new ArgumentException( "value must be of type DataEventCollection.", "value" );
		}

		protected override void OnRemove( int index, Object value )  {
			if ( !(value is DataEventCollection) )
				throw new ArgumentException( "value must be of type DataEventCollection.", "value" );
		}

		protected override void OnSet( int index, Object oldValue, Object newValue )  {
			if ( !(newValue is DataEventCollection) )
				throw new ArgumentException( "newValue must be of type DataEventCollection.", "newValue" );
		}

		protected override void OnValidate( Object value )  {
			if ( !(value is DataEventCollection) )
				throw new ArgumentException( "value must be of type DataEventCollection." );
		}

	}
}
