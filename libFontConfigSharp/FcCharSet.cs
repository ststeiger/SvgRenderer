
namespace libFontConfigSharp
{


	public class FcCharSet 
		: System.IDisposable
	{
		internal System.IntPtr Handle;


		public FcCharSet()
		{
			Handle = Native.FcCharSetCreate();
		}

		public void AddCharacter(uint codepoint)
		{
			Native.FcCharSetAddChar(Handle, codepoint);
		}


		public void Dispose()
		{
			Native.FcCharSetDestroy(Handle);
		}


	}


}
