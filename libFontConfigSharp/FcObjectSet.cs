
namespace libFontConfigSharp
{


	public class FcObjectSet 
		: System.IDisposable
	{

		internal System.IntPtr Handle;

		public FcObjectSet()
		{
			Handle = Native.FcObjectSetCreate ();
		}


		internal FcObjectSet (System.IntPtr handle)
		{
			Handle = handle;
		}


		public void Add(string obj)
		{
			Native.FcObjectSetAdd (Handle, obj);
		}


		public void Dispose()
		{
			Native.FcObjectSetDestroy (Handle);
		}


	}


}
