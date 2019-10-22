using System;
using System.Runtime.InteropServices;
	
namespace PIPS.Utilities {
	

	/// <summary>
	/// Summary description for WaveBasic.
	/// </summary>
	public class WaveBasic {
		/// <summary>
		/// Constructor
		/// </summary>
		public WaveBasic() {}
			
		[DllImport("winmm.dll")]
		private static extern long PlaySound(String lpszName, IntPtr hModule, SoundFlags dwFlags);

		[Flags]
			private enum SoundFlags : int {
			Synchronous = 0x0000,            // play synchronously (default)
			Asynchronous = 0x0001,        // play asynchronously
			NoDefaultSound = 0x0002,        // silence (!default) if sound not found
			//by default a beep is played when the sound cannot be played
			MemoryFile = 0x0004,        // pszSound points to a memory file
			LoopUntilNext = 0x0008,            // loop the sound until next sndPlaySound
			DontStopCurrentSound = 0x0010,        // don't stop any currently playing sound
			StopCurrentSound = 0x40,
			DontWaitIfBusy = 0x00002000,        // don't wait if the driver is busy
			RegistryAlias = 0x00010000,        // name is a registry alias
			RegistryAliasId = 0x00110000,        // alias is a predefined id
			Filename = 0x00020000
		}

		//
		public static void PlaySoundSync(string file) {
			PlaySound(file, SoundFlags.Synchronous | SoundFlags.Filename);
		}

		public static void PlaySoundAsync( string file ) {
			PlaySound(file, SoundFlags.Asynchronous | SoundFlags.Filename);
		}

		private static void PlaySound(string file, SoundFlags flags) {
			PlaySound(file, IntPtr.Zero, flags);
		}
	}
}
