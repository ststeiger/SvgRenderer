﻿
namespace libFontConfigSharp
{


	public class FcPattern 
		: System.IDisposable
	{
		internal System.IntPtr Handle;


		public FcPattern()
		{
			Handle = Native.FcPatternCreate ();
		}


		internal FcPattern(System.IntPtr handle)
		{
			Handle = handle;
		}


		public FcResult GetString(string obj, int n, ref string val)
		{
			var ptr = System.IntPtr.Zero;
			var result =  Native.FcPatternGetString (Handle, obj, n, ref ptr);
			if (result == FcResult.Match)
				val = System.Runtime.InteropServices.Marshal.PtrToStringAnsi (ptr);
			return result;
		}


		public void AddCharSet(string name, FcCharSet fcs)
		{
			Native.FcPatternAddCharSet(Handle, name, fcs.Handle);
		}


		public FcPattern Match(FcConfig config, out FcResult result)
		{
			return new FcPattern (Native.FcFontMatch (config.Handle, Handle, out result));
		}


		public void DefaultSubstitute()
		{
			Native.FcDefaultSubstitute (Handle);
		}


		public bool ConfigSubstitute(FcConfig config, FcMatchKind kind)
		{
			return Native.FcConfigSubstitute (config.Handle, Handle, kind) != 0;
		}


		public static FcPattern FromFamilyName(string name)
		{
			return new FcPattern (Native.FcNameParse (name));
		}


		public void Dispose()
		{
			Native.FcPatternDestroy (Handle);
		}


	}


}
