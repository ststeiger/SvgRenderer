
namespace libFontConfigSharp
{


	internal static class Native
	{
		// public const string LIB = "libfontconfig.so.1";

		public const string LIB = @"C:\Users\Stefan.Steiger\AppData\Local\Gtk\3.24\libfontconfig-1.dll";


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
