using System;
using System.IO;
using System.Collections;
using System.Text;

namespace PIPS
{
	public delegate void CsvLineReadHandler(string[] values);

	public interface CommaDelimitedFile {
		string[] ReadLine();
		void Close();
	}

	/// <summary>
	/// Summary description for CSV.
	/// </summary>
	/// <summary>
	/// CSV file parser.
	/// </summary>
	public class CSV : CommaDelimitedFile {

		private CsvReader cr;
		public CSV() {}
		public CSV(string filename) : this(new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None)) {}
		public CSV(Stream s) {
			cr = new CsvReader(s, 1024);
		}
		public CSV(StreamReader sr) {
			cr = new CsvReader(sr, 1024);
		}

		public string[] ReadLine() {
			if(!cr.Read())
				return null;
			string[] values = new string[cr.FieldCount];
			for(int i = 0; i < values.Length; i++)
				values[i] = cr[i];
			return values;
		}

		public void Close() {
			if(cr != null) {
				cr.Close();
				cr = null;
			}
		}
		public void FromFile(string path, CsvLineReadHandler LineRead) { 
			if(!File.Exists(path)) return;
			FileStream fs = File.OpenRead(path);
			FromStream(fs, LineRead);
		}

		public void FromStream(Stream stream, CsvLineReadHandler LineRead) {  
			StreamReader sr = new StreamReader(stream);
			FromStreamReader(sr, LineRead);
		}

		public void FromStreamReader(StreamReader sr, CsvLineReadHandler LineRead) {
			CsvReader csv = new CsvReader(sr, 1024);
			try {
				int cnt = 1;
				while (csv.Read()) {
					int len = csv.FieldCount;
					string[] values = new string[len];
					for (int i = 0; i < len; i++) values[i] = csv[i];
					if(LineRead != null)
						LineRead(values);
					cnt++;
				}
			} finally {
				csv.Close();
			}
		}

		#region CsvReader

		class CsvReader {
			TextReader _r;
			char[] _buffer;
			int _pos;
			int _used;

			// assumes end of record delimiter is {CR}{LF}, {CR}, or {LF}
			// possible values are {CR}{LF}, {CR}, {LF}, ';', ',', '\t'
			// char _recDelim;

			char _colDelim; // possible values ',', ';', '\t', '|'
			char _quoteChar;

			ArrayList _values;
			int _fields;

			public CsvReader(Stream stm, int bufsize) {  // the location of the .csv file
				_r = new StreamReader(stm, true);
				_buffer = new char[bufsize];
				_values = new ArrayList();
			}     
			public CsvReader(TextReader stm, int bufsize) {  // the location of the .csv file
				_r = stm;
				_buffer = new char[bufsize];
				_values = new ArrayList();
			}     

			public TextReader Reader {
				get { return _r; }
			}

			const int EOF = 0xffff;

			public bool Read() { // read a record.
				_fields = 0;
				char ch = ReadChar();
				if (ch == 0) {
					return false;
				}
				while (ch != 0 && ch == '\r' || ch == '\n' || ch == ' ')
					ch = ReadChar();
				if (ch == 0) return false;

				while (ch != 0 && ch != '\r' && ch != '\n') {
					StringBuilder sb = AddField();
					if (ch == '\'' || ch == '"') {
						_quoteChar= ch;         
						char c = ReadChar();
						bool done = false;
						while (!done && c != 0) {
							while (c != 0 && c != ch) { // scan literal.
								sb.Append(c);
								c = ReadChar();
							}
							if (c == ch) {
								done = true;
								char next = ReadChar(); // consume end quote
								if (next == ch ) {
									// it was an escaped quote sequence "" inside the literal
									// so append a single " and consume the second end quote.
									done = false;
									sb.Append(next);
									c = ReadChar();
								} else if (_colDelim != 0 && next != _colDelim && next != 0 && next != '\n' && next != '\r') {
									// it was an un-escaped quote embedded inside a string literal
									// in this case the quote is probably just part of the text so ignore it.
									done = false;
									sb.Append(c);
									sb.Append(next);
									c = ReadChar();
								} else {
									c = next;
								}
							}
						}
						ch = c;         
					} 
					else {        
						// scan number, date, time, float, etc.
						while (ch != 0 && ch != '\n' && ch != '\r') {
							if (ch == _colDelim || (_colDelim == '\0' && (ch == ',' || ch == ';' || ch == '\t' || ch == '|')))
								break;
							sb.Append(ch);
							ch = ReadChar();
						}
					} 
					if (ch == _colDelim || (_colDelim == '\0' && (ch == ',' || ch == ';' || ch == '\t' || ch == '|'))){
						_colDelim = ch;
						ch = ReadChar();
						if (ch == '\n' || ch == '\r') {
							sb=AddField(); // blank field.
						}
					}
				}
				return true;
			}

			public char QuoteChar { get { return _quoteChar; } }
			public char Delimiter { get { return _colDelim; } set { _colDelim = value ; }}

			public int FieldCount { get { return _fields; } }

			public string this[int i] { get { return ((StringBuilder)_values[i]).ToString().Trim(); } }

			char ReadChar() {
				if (_pos == _used) {
					_pos = 0;
					_used = _r.Read(_buffer, 0, _buffer.Length);
				}
				if (_pos == _used) {
					return (char)0;
				}
				return _buffer[_pos++];
			}

			StringBuilder AddField() {
				if (_fields == _values.Count) {
					_values.Add(new StringBuilder());
				}
				StringBuilder sb = (StringBuilder)_values[_fields++];
				sb.Length = 0;
				return sb;
			}

			public void Close() {
				_r.Close();
			}
		}

		#endregion

	}

	
	/// <summary>
	/// Summary description for CSV.
	/// </summary>
	/// <summary>
	/// CSV file parser.
	/// </summary>
	public class Txt : CommaDelimitedFile {

		static char[] _colDelim =  { ',', ';', '\t', '|' };
		private StreamReader txt;
		public Txt() {}
		public Txt(string filename) : this(new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None)) {}
		public Txt(Stream s) {
			txt = new StreamReader(s);
		}
		public Txt(StreamReader sr) {
			txt = sr;
		}

		public string[] ReadLine() {
			string line = txt.ReadLine();
			return null != line ? line.Split(_colDelim) : null;
		}

		public void Close() {
			if(txt != null) {
				txt.Close();
				txt = null;
			}
		}

		public void FromFile(string path, CsvLineReadHandler LineRead) { 
			if(!File.Exists(path)) return;
			FileStream fs = File.OpenRead(path);
			FromStream(fs, LineRead);
		}

		public void FromStream(Stream stream, CsvLineReadHandler LineRead) {  
			StreamReader sr = new StreamReader(stream);
			FromStreamReader(sr, LineRead);
		}

		public void FromStreamReader(StreamReader sr, CsvLineReadHandler LineRead) {
			try {
				Txt txt = new Txt(sr);
				int cnt = 1;
				string[] values;
				while ((values = txt.ReadLine()) != null) {
					if(LineRead != null)
						LineRead(values);
					cnt++;
				}
			} finally {
				txt.Close();
			}
		}
	}
}
