
namespace libFontConfigSharp
{


	public class FcConfig
	{
		internal System.IntPtr Handle;
		internal FcConfig(System.IntPtr handle)
		{
			Handle = handle;
		}


        public void SetCurrent()
        {
            Native.FcConfigSetCurrent(Handle);
        }


    }


}

