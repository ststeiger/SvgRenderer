
namespace AspNetCore.ReportingServices.Rendering.ExcelRenderer.Excel
{

	internal enum CharSet : byte
	{
		ANSI_CHARSET = 0,
		DEFAULT_CHARSET = 1,
		SYMBOL_CHARSET = 2,
		MAC_CHARSET = 77,
		SHIFTJIS_CHARSET = 0x80,
		HANGEUL_CHARSET = 129,
		HANGUL_CHARSET = 129,
		JOHAB_CHARSET = 130,
		GB2312_CHARSET = 134,
		CHINESEBIG5_CHARSET = 136,
		GREEK_CHARSET = 161,
		TURKISH_CHARSET = 162,
		VIETNAMESE_CHARSET = 163,
		HEBREW_CHARSET = 177,
		ARABIC_CHARSET = 178,
		BALTIC_CHARSET = 186,
		RUSSIAN_CHARSET = 204,
		THAI_CHARSET = 222,
		EASTEUROPE_CHARSET = 238,
		OEM_CHARSET = byte.MaxValue
	}


	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
	internal class LOGFONT
	{
		public int lfHeight;

		public int lfWidth;

		public int lfEscapement;

		public int lfOrientation;

		public int lfWeight;

		public byte lfItalic;

		public byte lfUnderline;

		public byte lfStrikeOut;

		public byte lfCharSet;

		public byte lfOutPrecision;

		public byte lfClipPrecision;

		public byte lfQuality;

		public byte lfPitchAndFamily;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
		public string lfFaceName;

		public override string ToString()
		{
			return
				"lfHeight=" + lfHeight + ", " +
				"lfWidth=" + lfWidth + ", " +
				"lfEscapement=" + lfEscapement + ", " +
				"lfOrientation=" + lfOrientation + ", " +
				"lfWeight=" + lfWeight + ", " +
				"lfItalic=" + lfItalic + ", " +
				"lfUnderline=" + lfUnderline + ", " +
				"lfStrikeOut=" + lfStrikeOut + ", " +
				"lfCharSet=" + lfCharSet + ", " +
				"lfOutPrecision=" + lfOutPrecision + ", " +
				"lfClipPrecision=" + lfClipPrecision + ", " +
				"lfQuality=" + lfQuality + ", " +
				"lfPitchAndFamily=" + lfPitchAndFamily + ", " +
				"lfFaceName=" + lfFaceName.ToString();
		}
		
		
	}
}
