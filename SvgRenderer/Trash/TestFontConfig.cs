
namespace FontConfigTest
{


    class TestFontConfig
    {



		// Gets the path to a font
		public static void GetFontPath(string fontFamilyName)
		{
			libFontConfigSharp.FcConfig config = libFontConfigSharp.Fc.InitLoadConfigAndFonts();
			libFontConfigSharp.FcPattern pat = libFontConfigSharp.FcPattern.FromFamilyName(fontFamilyName);
			pat.ConfigSubstitute(config, libFontConfigSharp.FcMatchKind.Pattern);
			pat.DefaultSubstitute();
			libFontConfigSharp.FcResult result;
			libFontConfigSharp.FcPattern font = pat.Match(config, out result);

			string file = null;
			if (font.GetString(libFontConfigSharp.Fc.FC_FILE, 0, ref file) == libFontConfigSharp.FcResult.Match)
			{
				System.Console.WriteLine(file);
			}

			pat.Dispose();
		} // End Sub GetFontPath 


		// Lists all .ttf and .otf fonts provided by FontConfig
		public static void ListFonts()
		{
			libFontConfigSharp.FcConfig config = libFontConfigSharp.Fc.InitLoadConfigAndFonts();

			libFontConfigSharp.FcPattern pat = new libFontConfigSharp.FcPattern();

			libFontConfigSharp.FcObjectSet os = new libFontConfigSharp.FcObjectSet();
			os.Add(libFontConfigSharp.Fc.FC_FAMILY);
			os.Add(libFontConfigSharp.Fc.FC_STYLE);
			os.Add(libFontConfigSharp.Fc.FC_LANG);
			os.Add(libFontConfigSharp.Fc.FC_FILE);
			os.Add(string.Empty);

			libFontConfigSharp.FcFontSet fs = libFontConfigSharp.FcFontSet.FromList(config, pat, os);
			for (int i = 0; i < fs.Count; i++)
			{
				libFontConfigSharp.FcPattern font = fs[i];
				string file = "", family = "", style = "";

				if (font.GetString(libFontConfigSharp.Fc.FC_FILE, 0, ref file) == libFontConfigSharp.FcResult.Match &&
				   font.GetString(libFontConfigSharp.Fc.FC_FAMILY, 0, ref family) == libFontConfigSharp.FcResult.Match &&
				   font.GetString(libFontConfigSharp.Fc.FC_STYLE, 0, ref style) == libFontConfigSharp.FcResult.Match)
				{
					if (file.EndsWith(".ttf") || file.EndsWith(".otf"))
					{
						System.Console.WriteLine("{0}, {1}: {2}", family, style, file);
					}
				}

			} // Next i 

			os.Dispose();
			pat.Dispose();
			fs.Dispose();
		} // End Sub ListFonts 


		public static void Test()
		{
			ListFonts();
			GetFontPath("sans");
		}


	}


}
