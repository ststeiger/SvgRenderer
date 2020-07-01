
namespace SvgRenderer.FontConfig
{

    public class foo
    {
        [System.Runtime.InteropServices.DllImport("GDI32", SetLastError = true)]
        internal static extern uint GetFontData(System.IntPtr hdc, int dwTable, int dwOffset, byte[] lpvBuffer, int cbData);

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
        private static extern uint GetFontData(System.IntPtr hdc, uint dwTable, uint dwOffset, byte[] lpvBuffer, uint cbData);


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




    // https://github.com/idkiller
    // https://gist.github.com/idkiller/338b8346fb7fd41ebef1dfd1dd8de794
    public class FontQuery
    {



        // https://stackoverflow.com/questions/544972/load-fonts-from-file-on-a-c-sharp-application
        // System.Drawing.Text.PrivateFontCollection
        // https://web.archive.org/web/20170313145219/https://blog.andreloker.de/post/2008/07/03/Load-a-font-from-disk-stream-or-byte-array.aspx
        // https://stackoverflow.com/questions/7742148/how-to-convert-text-to-svg-paths

        public static void Test(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }

            string fontFamily = args[0];
            System.IntPtr config = FontConfig.FcInitLoadConfigAndFonts();
            System.IntPtr pattern = FontConfig.FcPatternCreate();
            FontConfig.FcPatternAddString(pattern, FontConfig.FC_FAMILY, fontFamily);

            FontConfig.FcConfigSubstitute(config, pattern, FontConfig.FcMatchKind.FcMatchPattern);
            FontConfig.FcDefaultSubstitute(pattern);

            System.IntPtr font = FontConfig.FcFontMatch(config, pattern, out FontConfig.FcResult result);
            if (font != System.IntPtr.Zero)
            {
                if (FontConfig.FcPatternGetString(font, FontConfig.FC_FILE, 0, out string file) == FontConfig.FcResult.FcResultMatch)
                {
                    System.Console.Write($"{file} ");
                }

                if (FontConfig.FcPatternGetString(font, FontConfig.FC_FAMILY, 0, out string family) == FontConfig.FcResult.FcResultMatch)
                {
                    System.Console.Write($"[{family}] ");
                }

                if (FontConfig.FcPatternGetString(font, FontConfig.FC_POSTSCRIPT_NAME, 0, out string postscriptname) == FontConfig.FcResult.FcResultMatch)
                {
                    System.Console.Write($"[{postscriptname}] ");
                }
                System.Console.WriteLine();
            }

            FontConfig.FcPatternDestroy(pattern);
        }


    }


    static class FontConfig
    {
        const string libFontConfig = "libfontconfig.so";

        internal const string FC_FAMILY = "family";    /* String */
        internal const string FC_STYLE = "style";   /* String */
        internal const string FC_SLANT = "slant";   /* Int */
        internal const string FC_WEIGHT = "weight";    /* Int */
        internal const string FC_SIZE = "size";    /* Range (double) */
        internal const string FC_ASPECT = "aspect";    /* Double */
        internal const string FC_PIXEL_SIZE = "pixelsize";   /* Double */
        internal const string FC_SPACING = "spacing";   /* Int */
        internal const string FC_FOUNDRY = "foundry";   /* String */
        internal const string FC_ANTIALIAS = "antialias";   /* Bool (depends) */
        internal const string FC_HINTING = "hinting";   /* Bool (true) */
        internal const string FC_HINT_STYLE = "hintstyle";   /* Int */
        internal const string FC_VERTICAL_LAYOUT = "verticallayout";  /* Bool (false) */
        internal const string FC_AUTOHINT = "autohint";    /* Bool (false) */
        internal const string FC_GLOBAL_ADVANCE = "globaladvance"; /* Bool (true) */
        internal const string FC_WIDTH = "width";   /* Int */
        internal const string FC_FILE = "file";    /* String */
        internal const string FC_INDEX = "index";   /* Int */
        internal const string FC_FT_FACE = "ftface";    /* FT_Face */
        internal const string FC_RASTERIZER = "rasterizer";  /* String (deprecated) */
        internal const string FC_OUTLINE = "outline";   /* Bool */
        internal const string FC_SCALABLE = "scalable";    /* Bool */
        internal const string FC_COLOR = "color";   /* Bool */
        internal const string FC_SCALE = "scale";   /* double (deprecated) */
        internal const string FC_SYMBOL = "symbol";    /* Bool */
        internal const string FC_DPI = "dpi";   /* double */
        internal const string FC_RGBA = "rgba";    /* Int */
        internal const string FC_MINSPACE = "minspace";    /* Bool use minimum line spacing */
        internal const string FC_SOURCE = "source";    /* String (deprecated) */
        internal const string FC_CHARSET = "charset";   /* CharSet */
        internal const string FC_LANG = "lang";    /* String RFC 3066 langs */
        internal const string FC_FONTVERSION = "fontversion"; /* Int from 'head' table */
        internal const string FC_FULLNAME = "fullname";    /* String */
        internal const string FC_FAMILYLANG = "familylang";  /* String RFC 3066 langs */
        internal const string FC_STYLELANG = "stylelang";   /* String RFC 3066 langs */
        internal const string FC_FULLNAMELANG = "fullnamelang";  /* String RFC 3066 langs */
        internal const string FC_CAPABILITY = "capability";  /* String */
        internal const string FC_FONTFORMAT = "fontformat";  /* String */
        internal const string FC_EMBOLDEN = "embolden";    /* Bool - true if emboldening needed*/
        internal const string FC_EMBEDDED_BITMAP = "embeddedbitmap";  /* Bool - true to enable embedded bitmaps */
        internal const string FC_DECORATIVE = "decorative";  /* Bool - true if style is a decorative variant */
        internal const string FC_LCD_FILTER = "lcdfilter";   /* Int */
        internal const string FC_FONT_FEATURES = "fontfeatures";  /* String */
        internal const string FC_NAMELANG = "namelang";    /* String RFC 3866 langs */
        internal const string FC_PRGNAME = "prgname";   /* String */
        internal const string FC_HASH = "hash";    /* String (deprecated) */
        internal const string FC_POSTSCRIPT_NAME = "postscriptname";  /* String */


        internal enum FcMatchKind
        {
            FcMatchPattern, FcMatchFont, FcMatchScan
        }

        internal enum FcResult
        {
            FcResultMatch, FcResultNoMatch, FcResultTypeMismatch, FcResultNoId, FcResultOutOfMemory
        }

        internal enum FcBool
        {
            FcFalse, FcTrue
        }

        [System.Runtime.InteropServices.DllImport(libFontConfig)]
        internal static extern System.IntPtr FcInitLoadConfigAndFonts();


        [System.Runtime.InteropServices.DllImport(libFontConfig)]
        internal static extern System.IntPtr FcPatternCreate();

        [System.Runtime.InteropServices.DllImport(libFontConfig)]
        internal static extern FcBool FcPatternAddString(System.IntPtr p, string obj, string s);

        [System.Runtime.InteropServices.DllImport(libFontConfig)]
        internal static extern FcResult FcPatternGetString(System.IntPtr p, string obj, int n, out string s);

        [System.Runtime.InteropServices.DllImport(libFontConfig)]
        internal static extern System.IntPtr FcNameParse(string name);

        [System.Runtime.InteropServices.DllImport(libFontConfig)]
        internal static extern FcBool FcConfigSubstitute(System.IntPtr config, System.IntPtr p, FcMatchKind kind);

        [System.Runtime.InteropServices.DllImport(libFontConfig)]
        internal static extern void FcDefaultSubstitute(System.IntPtr p);

        [System.Runtime.InteropServices.DllImport(libFontConfig)]
        internal static extern System.IntPtr FcFontMatch(System.IntPtr config, System.IntPtr p, out FcResult result);

        [System.Runtime.InteropServices.DllImport(libFontConfig)]
        internal static extern void FcPatternDestroy(System.IntPtr p);
    }


}
