
namespace libFontConfigSharp
{


    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public class FcFontSet 
        : System.IDisposable
    {


        _FcFontSet fset;
        System.IntPtr handle;
        struct _FcFontSet
        {
            public int nfont;
            public int sfont;
            public System.IntPtr fonts;
        }


        internal FcFontSet(System.IntPtr handle)
        {
            this.handle = handle;
            fset = (_FcFontSet)System.Runtime.InteropServices.Marshal.PtrToStructure(handle, typeof(_FcFontSet));
        }


        public FcPattern this[int index]
        {
            get
            {
                if (index >= fset.nfont)
                    throw new System.IndexOutOfRangeException();
                return new FcPattern(System.Runtime.InteropServices.Marshal.ReadIntPtr(fset.fonts, index * System.Runtime.InteropServices.Marshal.SizeOf(typeof(System.IntPtr))));
            }
        }


        public int Count
        {
            get
            {
                return fset.nfont;
            }
        }


        public static FcFontSet FromList(FcConfig config, FcPattern pat, FcObjectSet os)
        {
            return new FcFontSet(Native.FcFontList(config.Handle, pat.Handle, os.Handle));
        }


        public void Dispose()
        {
            Native.FcFontSetDestroy(handle);
        }


    }


}
