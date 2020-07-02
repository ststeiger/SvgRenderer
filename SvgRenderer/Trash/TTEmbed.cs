
namespace SvgRenderer.Trash
{


    class TTEmbed
    {


        public static void TRACE(string format, params object[] args)
        {
            System.Console.Write("Trace: ");
            System.Console.WriteLine(string.Format(format, args));
        }

        public static void WARN(string format, params object[] args)
        {
            System.Console.Write("WARN: ");
            System.Console.WriteLine(string.Format(format, args));
        }


        // D:\Stefan.Steiger\Documents\Visual Studio 2017\Projects\wine\include\verrsrc.h
        // #ifndef __MSABI_LONG
        //    # if defined(_MSC_VER) || defined(__MINGW32__) || defined(__CYGWIN__)
        //        #  define __MSABI_LONG(x)         x ## l
        //    # else
        //        #  define __MSABI_LONG(x)         x
        //    # endif
        // #endif


        // https://github.com/wine-mirror/wine/blob/master/include/t2embapi.h
        [System.Flags()]
        public enum LicenseFlags
         : int  
        {
            LICENSE_INSTALLABLE = 0x0000,
            LICENSE_DEFAULT = 0x0000,
            LICENSE_NOEMBEDDING = 0x0002,
            LICENSE_PREVIEWPRINT = 0x0004,
            LICENSE_EDITABLE = 0x0008,
        }


        // Possible return values.
        public enum ErrorCodes
           : int
        {
            E_NONE = 0x0000,
            E_API_NOTIMPL = 0x0001,
            E_HDCINVALID = 0x0006,
            E_NOFREEMEMORY = 0x0007,
            E_NOTATRUETYPEFONT = 0x000a,
            E_ERRORACCESSINGFONTDATA = 0x000c,
            E_ERRORACCESSINGFACENAME = 0x000d,
            E_FACENAMEINVALID = 0x0113,
            E_PERMISSIONSINVALID = 0x0117,
            E_PBENABLEDINVALID = 0x0118,
        }



        // embedding privileges
        public enum Embeddability
            : int
        {
            EMBED_PREVIEWPRINT = 1,
            EMBED_EDITABLE = 2,
            EMBED_INSTALLABLE = 3,
            EMBED_NOEMBEDDING = 4
        }


        // https://www.youtube.com/watch?v=6wLlWWp8Vcg&t=564
        public static Embeddability TTGetEmbeddingType() //HDC hDC, ULONG* status)
        {
            // WORD	A 16 - bit unsigned integer = uint16 = ushort
            // WORD fsType;
            ushort fsType = 12; // From OS/2-Table
            int otmfsType = (fsType & 0xf);


            // if (TODO: IsTrueTypeFont) throw new System.InvalidOperationException("Not a TrueType-fon.");

            if (otmfsType == (int)LicenseFlags.LICENSE_INSTALLABLE)
                return Embeddability.EMBED_INSTALLABLE;
            else if ((otmfsType & (int)LicenseFlags.LICENSE_EDITABLE) != 0)
                return Embeddability.EMBED_EDITABLE;
            else if ((otmfsType & (int)LicenseFlags.LICENSE_PREVIEWPRINT) != 0)
                return Embeddability.EMBED_PREVIEWPRINT;
            else if ((otmfsType & (int)LicenseFlags.LICENSE_NOEMBEDDING) != 0)
                return Embeddability.EMBED_NOEMBEDDING;

            WARN("unrecognized flags, %#x\n", otmfsType);
            return Embeddability.EMBED_INSTALLABLE;
        } // End Function TTGetEmbeddingType 


        // https://github.com/wine-mirror/wine/blob/master/dlls/t2embed/main.c
        // LONG	A 32-bit signed integer.
        // LONG WINAPI 
        public static ErrorCodes TTGetEmbeddingType(System.IntPtr hDC, out Embeddability status) //HDC hDC, ULONG* status)
        {
            // https://docs.microsoft.com/en-us/windows/win32/winprog/windows-data-types
            // WORD	A 16 - bit unsigned integer = uint16 = ushort
            // WORD fsType;
            ushort fsType = 12; // TODO: Get From font-file

            if (hDC == System.IntPtr.Zero)
            {
                status = Embeddability.EMBED_NOEMBEDDING;
                return ErrorCodes.E_HDCINVALID;
            }

            // OUTLINETEXTMETRICW otm;
            // otm.otmSize = sizeof(otm);
            // if (!GetOutlineTextMetricsW(hDC, otm.otmSize, &otm))
            // return ErrorCodes.E_NOTATRUETYPEFONT;

            // if (!status)
            // return ErrorCodes.E_PERMISSIONSINVALID;

            int otmfsType = (fsType & 0xf);


            if (otmfsType == (int) LicenseFlags.LICENSE_INSTALLABLE)
                status = Embeddability.EMBED_INSTALLABLE;
            else if ((otmfsType & (int)LicenseFlags.LICENSE_EDITABLE) != 0)
                status = Embeddability.EMBED_EDITABLE;
            else if ((otmfsType & (int)LicenseFlags.LICENSE_PREVIEWPRINT) != 0)
                status = Embeddability.EMBED_PREVIEWPRINT;
            else if ((otmfsType & (int)LicenseFlags.LICENSE_NOEMBEDDING) != 0)
                status = Embeddability.EMBED_NOEMBEDDING;
            else
            {
                WARN("unrecognized flags, %#x\n", otmfsType);
                status = Embeddability.EMBED_INSTALLABLE;
            }

            TRACE("fsType 0x%04x, status %u\n", fsType, status);
            return ErrorCodes.E_NONE;
        } // End Function TTGetEmbeddingType 


        /*
        // Check if file is TrueType-file 
        static class KnownFontFiles
        {
            public static bool IsTtcf(ushort u1, ushort u2)
            ...

        public PreviewFontInfo ReadPreview(Stream stream)
        {
            //var little = BitConverter.IsLittleEndian;
            using (var input = new ByteOrderSwappingBinaryReader(stream))
            {
                ushort majorVersion = input.ReadUInt16();
                ushort minorVersion = input.ReadUInt16();

                if (KnownFontFiles.IsTtcf(majorVersion, minorVersion))    
         */


    } // End Class TTEmbed 


} // End Namespace SvgRenderer.Trash 
