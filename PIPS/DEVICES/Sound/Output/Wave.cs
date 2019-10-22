using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace PIPS.Devices.Sound.Output {

	internal delegate void BufferFillEventHandler(ref WaveOutBuffer buf);

	#region WavePlayback

	internal class WavePlayback : IDisposable {
		private WaveOutPlayer player;
		private WaveFormat format;
		private WaveStream stream;
		private bool disposed = false;

		public WavePlayback(Stream s) {
			stream = new WaveStream(s);
			format = stream.Format;
			if (format.wFormatTag != (short)WaveFormats.Pcm && format.wFormatTag != (short)WaveFormats.Float)
				throw new Exception("Not a PCM stream.");
		}

		~WavePlayback() {
			Dispose(false);
		}

		public void Dispose() {
			Dispose(true);
		}

		public void Dispose(bool disposing) {
			if (!disposed) {
				disposed = true;
				if (player != null)
					try {
						player.Dispose();
					}
					finally {
						player = null;
					}
				if (stream != null)
					try {
						stream.Close();
					}
					finally {
						stream = null;
					}
			}
		}

		public void Play() {
			player = new WaveOutPlayer(-1, format, 16384, 3, new BufferFillEventHandler(FillBuffer));
		}

		private void FillBuffer(ref WaveOutBuffer buf) {
			int size = buf.Size;
			byte[] b = new byte[size];
			if (stream != null) {
				int pos = 0;
				while (pos < size) {
					int toget = size - pos;
					int got = stream.Read(b, pos, toget);
					pos += got;
					if (got < toget)
						break;
				}
				if (pos == 0) {
					buf.Finished = true;
					return;
				}
			}
			else {
				System.Array.Clear(b, 0, b.Length);
			}
			System.Runtime.InteropServices.Marshal.Copy(b, 0, buf.Data, size);
		}
	}

	#endregion

	#region WaveStream

	internal class WaveStream : Stream, IDisposable {
		private Stream m_Stream;
		private long m_DataPos;
		private long m_Length;

		private WaveFormat m_Format;

		public WaveFormat Format {
			get { return m_Format; }
		}

		private string ReadChunk(BinaryReader reader) {
			byte[] ch = new byte[4];
			reader.Read(ch, 0, ch.Length);
			return System.Text.Encoding.ASCII.GetString(ch);
		}

		private void ReadHeader() {
			BinaryReader Reader = new BinaryReader(m_Stream);
			if (ReadChunk(Reader) != "RIFF")
				throw new Exception("Invalid file format");

			Reader.ReadInt32(); // File length minus first 8 bytes of RIFF description, we don't use it

			if (ReadChunk(Reader) != "WAVE")
				throw new Exception("Invalid file format");

			if (ReadChunk(Reader) != "fmt ")
				throw new Exception("Invalid file format");

			int len = Reader.ReadInt32();
			if (len < 16) // bad format chunk length
				throw new Exception("Invalid file format");

			m_Format = new WaveFormat(22050, 16, 2); // initialize to any format
			m_Format.wFormatTag = Reader.ReadInt16();
			m_Format.nChannels = Reader.ReadInt16();
			m_Format.nSamplesPerSec = Reader.ReadInt32();
			m_Format.nAvgBytesPerSec = Reader.ReadInt32();
			m_Format.nBlockAlign = Reader.ReadInt16();
			m_Format.wBitsPerSample = Reader.ReadInt16(); 

			// advance in the stream to skip the wave format block 
			len -= 16; // minimum format size
			while (len > 0) {
				Reader.ReadByte();
				len--;
			}

			// assume the data chunk is aligned
			while(m_Stream.Position < m_Stream.Length && ReadChunk(Reader) != "data")
				;

			if (m_Stream.Position >= m_Stream.Length)
				throw new Exception("Invalid file format");

			m_Length = Reader.ReadInt32();
			m_DataPos = m_Stream.Position;

			Position = 0;
		}

		public WaveStream(string fileName) : this(new FileStream(fileName, FileMode.Open)) {
		}
		public WaveStream(Stream S) {
			m_Stream = S;
			ReadHeader();
		}
		~WaveStream() {
			Dispose();
		}
		public void Dispose() {
			if (m_Stream != null)
				m_Stream.Close();
			GC.SuppressFinalize(this);
		}

		public override bool CanRead {
			get { return true; }
		}
		public override bool CanSeek {
			get { return true; }
		}
		public override bool CanWrite {
			get { return false; }
		}
		public override long Length {
			get { return m_Length; }
		}
		public override long Position {
			get { return m_Stream.Position - m_DataPos; }
			set { Seek(value, SeekOrigin.Begin); }
		}
		public override void Close() {
			Dispose();
		}
		public override void Flush() {
		}
		public override void SetLength(long len) {
			throw new InvalidOperationException();
		}
		public override long Seek(long pos, SeekOrigin o) {
			switch(o) {
				case SeekOrigin.Begin:
					m_Stream.Position = pos + m_DataPos;
					break;
				case SeekOrigin.Current:
					m_Stream.Seek(pos, SeekOrigin.Current);
					break;
				case SeekOrigin.End:
					m_Stream.Position = m_DataPos + m_Length - pos;
					break;
			}
			return this.Position;
		}
		public override int Read(byte[] buf, int ofs, int count) {
			int toread = (int)Math.Min(count, m_Length - Position);
			return m_Stream.Read(buf, ofs, toread);
		}
		public override void Write(byte[] buf, int ofs, int count) {
			throw new InvalidOperationException();
		}
	}

	#endregion

	#region WaveFormats enum

	internal enum WaveFormats {
		Pcm = 1,
		Float = 3
	}

	#endregion

	#region WaveFormat struct

	[StructLayout(LayoutKind.Sequential)] 
	internal class WaveFormat {
		public short wFormatTag;
		public short nChannels;
		public int nSamplesPerSec;
		public int nAvgBytesPerSec;
		public short nBlockAlign;
		public short wBitsPerSample;
		public short cbSize;

		public WaveFormat(int rate, int bits, int channels) {
			wFormatTag = (short)WaveFormats.Pcm;
			nChannels = (short)channels;
			nSamplesPerSec = rate;
			wBitsPerSample = (short)bits;
			cbSize = 0;
               
			nBlockAlign = (short)(channels * (bits / 8));
			nAvgBytesPerSec = nSamplesPerSec * nBlockAlign;
		}
	}

	#endregion

	#region WaveNative interop

	internal class WaveNative {
		// consts
		public const int MMSYSERR_NOERROR = 0; // no error

		public const int MM_WOM_OPEN = 0x3BB;
		public const int MM_WOM_CLOSE = 0x3BC;
		public const int MM_WOM_DONE = 0x3BD;

		public const int CALLBACK_FUNCTION = 0x00030000;    // dwCallback is a FARPROC 

		public const int TIME_MS = 0x0001;  // time in milliseconds 
		public const int TIME_SAMPLES = 0x0002;  // number of wave samples 
		public const int TIME_BYTES = 0x0004;  // current byte offset 

		// callbacks
		public delegate void WaveDelegate(IntPtr hdrvr, int uMsg, int dwUser, ref WaveHdr wavhdr, int dwParam2);

		// structs 

		[StructLayout(LayoutKind.Sequential)] public struct WaveHdr {
			public IntPtr lpData; // pointer to locked data buffer
			public int dwBufferLength; // length of data buffer
			public int dwBytesRecorded; // used for input only
			public IntPtr dwUser; // for client's use
			public int dwFlags; // assorted flags (see defines)
			public int dwLoops; // loop control counter
			public IntPtr lpNext; // PWaveHdr, reserved for driver
			public int reserved; // reserved for driver
		}

		private const string mmdll = "winmm.dll";

		// native calls
		[DllImport(mmdll)]
		public static extern int waveOutGetNumDevs();
		[DllImport(mmdll)]
		public static extern int waveOutPrepareHeader(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, int uSize);
		[DllImport(mmdll)]
		public static extern int waveOutUnprepareHeader(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, int uSize);
		[DllImport(mmdll)]
		public static extern int waveOutWrite(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, int uSize);
		[DllImport(mmdll)]
		public static extern int waveOutOpen(out IntPtr hWaveOut, int uDeviceID, WaveFormat lpFormat, WaveDelegate dwCallback, int dwInstance, int dwFlags);
		[DllImport(mmdll)]
		public static extern int waveOutReset(IntPtr hWaveOut);
		[DllImport(mmdll)]
		public static extern int waveOutClose(IntPtr hWaveOut);
		[DllImport(mmdll)]
		public static extern int waveOutPause(IntPtr hWaveOut);
		[DllImport(mmdll)]
		public static extern int waveOutRestart(IntPtr hWaveOut);
		[DllImport(mmdll)]
		public static extern int waveOutGetPosition(IntPtr hWaveOut, out int lpInfo, int uSize);
		[DllImport(mmdll)]
		public static extern int waveOutSetVolume(IntPtr hWaveOut, int dwVolume);
		[DllImport(mmdll)]
		public static extern int waveOutGetVolume(IntPtr hWaveOut, out int dwVolume);
	}

	#endregion

	#region WaveOutHelper

	internal class WaveOutHelper {
		public static void Try(int err) {
			if (err != WaveNative.MMSYSERR_NOERROR)
				throw new Exception(err.ToString());
		}
	}

	#endregion

	#region WaveOutBuffer

	internal class WaveOutBuffer : IDisposable {
		public WaveOutBuffer NextBuffer;

		private AutoResetEvent m_PlayEvent = new AutoResetEvent(false);
		private IntPtr m_WaveOut;

		private WaveNative.WaveHdr m_Header;
		private byte[] m_HeaderData;
		private GCHandle m_HeaderHandle;
		private GCHandle m_HeaderDataHandle;

		private bool m_Playing;
		private bool m_Finished = false;

		internal static void WaveOutProc(IntPtr hdrvr, int uMsg, int dwUser, ref WaveNative.WaveHdr wavhdr, int dwParam2) {
			if (uMsg == WaveNative.MM_WOM_DONE) {
				try {
					GCHandle h = (GCHandle)wavhdr.dwUser;
					WaveOutBuffer buf = (WaveOutBuffer)h.Target;
					buf.OnCompleted();
				}
				catch {
				}
			}
		}

		public WaveOutBuffer(IntPtr waveOutHandle, int size) {
			m_WaveOut = waveOutHandle;

			m_HeaderHandle = GCHandle.Alloc(m_Header, GCHandleType.Pinned);
			m_Header.dwUser = (IntPtr)GCHandle.Alloc(this, GCHandleType.Weak);
			m_HeaderData = new byte[size];
			m_HeaderDataHandle = GCHandle.Alloc(m_HeaderData, GCHandleType.Pinned);
			m_Header.lpData = m_HeaderDataHandle.AddrOfPinnedObject();
			m_Header.dwBufferLength = size;
			WaveOutHelper.Try(WaveNative.waveOutPrepareHeader(m_WaveOut, ref m_Header, Marshal.SizeOf(m_Header)));
		}
		~WaveOutBuffer() {
			Dispose();
		}
		public void Dispose() {
			if (m_Header.lpData != IntPtr.Zero) {
				WaveNative.waveOutUnprepareHeader(m_WaveOut, ref m_Header, Marshal.SizeOf(m_Header));
				m_HeaderHandle.Free();
				m_Header.lpData = IntPtr.Zero;
			}
			m_PlayEvent.Close();
			if (m_HeaderDataHandle.IsAllocated)
				m_HeaderDataHandle.Free();
			GC.SuppressFinalize(this);
		}

		public int Size {
			get { return m_Header.dwBufferLength; }
		}

		public IntPtr Data {
			get { return m_Header.lpData; }
			set { m_Header.lpData = value; }
		}

		public bool Finished {
			get { return m_Finished; }
			set { m_Finished = value; }
		}

		public bool Play() {
			if (m_Header.lpData == IntPtr.Zero) {
				m_Finished = true;
				return false;
			}
			lock(this) {
				m_PlayEvent.Reset();
				m_Playing = WaveNative.waveOutWrite(m_WaveOut, ref m_Header, Marshal.SizeOf(m_Header)) == WaveNative.MMSYSERR_NOERROR;
				return m_Playing;
			}
		}
		public void WaitFor() {
			if (m_Playing) {
				m_Playing = m_PlayEvent.WaitOne();
			}
			else {
				Thread.Sleep(0);
			}
		}
		public void OnCompleted() {
			m_PlayEvent.Set();
			m_Playing = false;
		}
	}

	#endregion

	#region WaveOutPlayer

	internal class WaveOutPlayer : IDisposable {
		private IntPtr m_WaveOut;
		private WaveOutBuffer m_Buffers; // linked list
		private WaveOutBuffer m_CurrentBuffer;
		private Thread m_Thread;
		private BufferFillEventHandler m_FillProc;
		private bool m_Finished;
		private byte m_zero;

		private WaveNative.WaveDelegate m_BufferProc = new WaveNative.WaveDelegate(WaveOutBuffer.WaveOutProc);

		public static int DeviceCount {
			get { return WaveNative.waveOutGetNumDevs(); }
		}

		public WaveOutPlayer(int device, WaveFormat format, int bufferSize, int bufferCount, BufferFillEventHandler fillProc) {
			m_zero = format.wBitsPerSample == 8 ? (byte)128 : (byte)0;
			m_FillProc = fillProc;
			WaveOutHelper.Try(WaveNative.waveOutOpen(out m_WaveOut, device, format, m_BufferProc, 0, WaveNative.CALLBACK_FUNCTION));
			AllocateBuffers(bufferSize, bufferCount);
			m_Thread = new Thread(new ThreadStart(ThreadProc));
			m_Thread.IsBackground = true;
			m_Thread.Start();
		}
		~WaveOutPlayer() {
			Dispose();
		}
		public void Dispose() {
			if (m_Thread != null)
				try {
					m_Finished = true;
					if (m_WaveOut != IntPtr.Zero)
						WaveNative.waveOutReset(m_WaveOut);
					m_FillProc = null;
					FreeBuffers();
					if (m_WaveOut != IntPtr.Zero)
						WaveNative.waveOutClose(m_WaveOut);
					try {
						if (m_Thread.IsAlive && m_Thread.ThreadState == ThreadState.Running) {
							try {
								m_Thread.Abort();
							} finally {}
						}
					} catch (ThreadStateException) {
					}
					GC.KeepAlive(m_Thread);
				}
				finally {
					m_Thread = null;
					m_WaveOut = IntPtr.Zero;
				}
			GC.SuppressFinalize(this);
		}
		private void ThreadProc() {
			while (!m_Finished) {
				Advance();
				if (m_FillProc != null && !m_Finished)
					m_FillProc(ref m_CurrentBuffer);
				else {
					// zero out buffer
					byte v = m_zero;
					byte[] b = new byte[m_CurrentBuffer.Size];
					//for (int i = 0; i < b.Length; i++)
					//	b[i] = v;
					System.Array.Clear(b, 0, m_CurrentBuffer.Size);
					Marshal.Copy(b, 0, m_CurrentBuffer.Data, b.Length);

				}
				if (!m_CurrentBuffer.Finished) {
					m_CurrentBuffer.Play();
				} else {
					m_CurrentBuffer.OnCompleted();
				}
				m_Finished = AllFinished();
			}
			WaitForAllBuffers();
		}

		private void AllocateBuffers(int bufferSize, int bufferCount) {
			FreeBuffers();
			if (bufferCount > 0) {
				m_Buffers = new WaveOutBuffer(m_WaveOut, bufferSize);
				WaveOutBuffer Prev = m_Buffers;
				try {
					for (int i = 1; i < bufferCount; i++) {
						WaveOutBuffer Buf = new WaveOutBuffer(m_WaveOut, bufferSize);
						Prev.NextBuffer = Buf;
						Prev = Buf;
					}
				}
				finally {
					Prev.NextBuffer = m_Buffers;
				}
			}
		}
		private void FreeBuffers() {
			m_CurrentBuffer = null;
			if (m_Buffers != null) {
				WaveOutBuffer First = m_Buffers;
				m_Buffers = null;

				WaveOutBuffer Current = First;
				do {
					WaveOutBuffer Next = Current.NextBuffer;
					Current.Dispose();
					Current = Next;
				} while(Current != First);
			}
		}
		private bool AllFinished() {
			WaveOutBuffer Buf = m_Buffers;
			while (Buf.NextBuffer != m_Buffers) {
				if (!Buf.Finished) return false;
				Buf = Buf.NextBuffer;
			}
			return true;
		}

		private void Advance() {
			m_CurrentBuffer = m_CurrentBuffer == null ? m_Buffers : m_CurrentBuffer.NextBuffer;
			m_CurrentBuffer.WaitFor();
		}
		private void WaitForAllBuffers() {
			WaveOutBuffer Buf = m_Buffers;
			while (Buf.NextBuffer != m_Buffers) {
				Buf.WaitFor();
				Buf = Buf.NextBuffer;
			}
		}
	}

	#endregion

}
