
namespace SvgRenderer.WindowsFonts
{
    
    // get_outline_text_metrics
    // https://github.com/alexhenrie/wine/blob/master/dlls/gdi32/freetype.c 
    // https://github.com/alexhenrie/wine/blob/master/dlls/gdi32/font.c
    // https://www.freetype.org/freetype2/docs/reference/ft2-basic_types.html
    // https://superuser.com/questions/760627/how-to-list-installed-font-families
    public class FontData
    {
        [System.Runtime.InteropServices.DllImport("GDI32", SetLastError = true)]
        internal static extern uint GetFontData(System.IntPtr hdc, int dwTable, int dwOffset, byte[] lpvBuffer,
            int cbData);

        // [System.Runtime.InteropServices.DllImport("GDI32", SetLastError = true)]
        // private static extern uint GetFontData(AspNetCore.ReportingServices.Rendering.RichText.Win32DCSafeHandle hdc, uint dwTable, uint dwOffset, System.IntPtr lpvBuffer, uint cbData);

        // https://docs.microsoft.com/en-us/windows/win32/winprog/windows-data-types   
        // https://docs.microsoft.com/en-us/windows/win32/api/wingdi/nf-wingdi-getfontdata
        // DWORD GetFontData(HDC hdc, DWORD dwTable, DWORD dwOffset, PVOID pvBuffer, DWORD cjBuffer);

        // hdc A handle to the device context.
        // dwOffset The offset from the beginning of the font metric table to the location where the function should begin retrieving information.
        // pvBuffer A pointer to a buffer that receives the font information.If this parameter is NULL, the function returns the size of the buffer required for the font data.
        // cjBuffer The length, in bytes, of the information to be retrieved.If this parameter is zero, GetFontData returns the size of the data specified in the dwTable parameter.
        [System.Runtime.InteropServices.DllImport("GDI32", SetLastError = true)]
        private static extern uint GetFontData(System.IntPtr hdc, uint dwTable, uint dwOffset, byte[] lpvBuffer,
            uint cbData);


        // [System.Runtime.InteropServices.DllImport("GDI32", SetLastError = true)]
        // internal static extern uint GetFontData(AspNetCore.ReportingServices.Rendering.RichText.Win32DCSafeHandle hdc, int dwTable, int dwOffset, byte[] lpvBuffer, int cbData);


        // [System.Runtime.InteropServices.DllImport("T2Embed")]
        // private static extern int TTGetEmbeddingType(AspNetCore.ReportingServices.Rendering.RichText.Win32DCSafeHandle hdc, ref uint status);


        // https://stackoverflow.com/questions/16769758/get-a-font-filename-based-on-the-font-handle-hfont
        public static bool GetFontFileData(System.IntPtr hdc, out byte[] buffer)
        {
            bool result = false;

            uint size = GetFontData(hdc, 0, 0, null, 0);
            if (size > 0)
            {
                // byte[] buffer = new byte[size];
                buffer = new byte[size];
                if (GetFontData(hdc, 0, 0, buffer, size) == size)
                {
                    result = true;
                }
                else
                    buffer = null;
            }
            else
                buffer = null;

            return result;
        }
        
        
    }
    
    
}