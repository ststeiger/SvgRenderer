
namespace libFontConfigSharp
{


	internal static class Native
	{
		// public const string LIB = "libfontconfig.so.1";

		public const string LIB = @"C:\Users\Stefan.Steiger\AppData\Local\Gtk\3.24\libfontconfig-1.dll";

		// ldd /usr/lib/x86_64-linux-gnu/libfontconfig.so
		// 	linux-vdso.so.1 (0x00007ffc383fb000)
		// 	libfreetype.so.6 => /usr/lib/x86_64-linux-gnu/libfreetype.so.6 (0x00007f92bb87b000)
		// 	libexpat.so.1 => /lib/x86_64-linux-gnu/libexpat.so.1 (0x00007f92bb649000)
		// 	libpthread.so.0 => /lib/x86_64-linux-gnu/libpthread.so.0 (0x00007f92bb42a000)
		// 	libc.so.6 => /lib/x86_64-linux-gnu/libc.so.6 (0x00007f92bb039000)
		// 	libpng16.so.16 => /usr/lib/x86_64-linux-gnu/libpng16.so.16 (0x00007f92bae07000)
		// 	libz.so.1 => /lib/x86_64-linux-gnu/libz.so.1 (0x00007f92babea000)
		// 	/lib64/ld-linux-x86-64.so.2 (0x00007f92bbd74000)
		// 	libm.so.6 => /lib/x86_64-linux-gnu/libm.so.6 (0x00007f92ba84c000)
		

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern System.IntPtr FcInitLoadConfigAndFonts();

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern System.IntPtr FcPatternCreate();

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern System.IntPtr FcFontList (System.IntPtr config, System.IntPtr p, System.IntPtr os);

		//this is varargs
		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern System.IntPtr FcObjectSetCreate();

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern void FcFontSetDestroy (System.IntPtr fs);

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern int FcObjectSetAdd (System.IntPtr os, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStr)]string obj);

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern FcResult FcPatternGetString (
			System.IntPtr p,
			[System.Runtime.InteropServices.MarshalAs (System.Runtime.InteropServices.UnmanagedType.LPStr)]string obj,
			int n,
			ref System.IntPtr s);

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern void FcPatternDestroy (System.IntPtr p);

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern void FcObjectSetDestroy (System.IntPtr os);

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern System.IntPtr FcNameParse ([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStr)]string name);

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern void FcDefaultSubstitute (System.IntPtr pattern);

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern int FcConfigSubstitute (System.IntPtr config, System.IntPtr p, FcMatchKind kind);

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern System.IntPtr FcFontMatch (System.IntPtr config, System.IntPtr p, out FcResult result);

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern System.IntPtr FcCharSetCreate();

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern System.IntPtr FcCharSetAddChar(System.IntPtr fcs, uint ucs4);

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern bool FcPatternAddCharSet(System.IntPtr p, string obj, System.IntPtr c);

		[System.Runtime.InteropServices.DllImport(LIB)]
		public static extern void FcCharSetDestroy(System.IntPtr fcs);

        [System.Runtime.InteropServices.DllImport(LIB)]
        public static extern bool FcConfigSetCurrent(System.IntPtr fcconfig);


	}


}
