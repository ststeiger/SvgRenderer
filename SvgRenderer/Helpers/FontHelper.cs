
namespace SvgRenderer.Helpers
{


    public class FontHelper
    {


        public static Typography.OpenFont.Typeface TypefaceFromFile(string fontFile)
        {
            Typography.OpenFont.Typeface tf;

            using (System.IO.FileStream fs = new System.IO.FileStream(fontFile, System.IO.FileMode.Open))
            {
                Typography.OpenFont.OpenFontReader fontReader = new Typography.OpenFont.OpenFontReader();
                tf = fontReader.Read(fs);
            }

            return tf;
        } // End Function TypefaceFromFile 


        public static Typography.FontManagement.TypefaceStore GetTypeFaceStore(string fontDirectory)
        {
            //1. create font collection             
            Typography.FontManagement.InstalledTypefaceCollection installedFontCollection = new Typography.FontManagement.InstalledTypefaceCollection();

            //2. set some essential handler
            installedFontCollection.SetFontNameDuplicatedHandler((f1, f2) => Typography.FontManagement.FontNameDuplicatedDecision.Skip);

            // installedFontCollection.LoadFontsFromFolder(fontDirectory);
            Typography.FontManagement.InstalledTypefaceCollectionExtensions.LoadFontsFromFolder(installedFontCollection, fontDirectory);

            // // installedFontCollection.LoadSystemFonts();
            // Typography.FontManagement.InstalledTypefaceCollectionExtensions.LoadSystemFonts(installedFontCollection);

            installedFontCollection.UpdateUnicodeRanges();


            Typography.FontManagement.TypefaceStore typefaceStore = new Typography.FontManagement.TypefaceStore();
            typefaceStore.FontCollection = installedFontCollection;

            return typefaceStore;
        } // End Function GetTypeFaceStore 


        public static Typography.OpenFont.Typeface GetFirstTypeFaceInDirectory(string fontDirectory)
        {
            Typography.FontManagement.TypefaceStore tfs = GetTypeFaceStore(fontDirectory);

            Typography.FontManagement.InstalledTypeface firstTypeFace = null;

            foreach (Typography.FontManagement.InstalledTypeface thisTypeface in tfs.FontCollection.GetInstalledFontIter())
            {
                System.Console.Write(thisTypeface.FontName);
                System.Console.Write(" | (");
                System.Console.Write(thisTypeface.PostScriptName);
                System.Console.WriteLine(")");
                System.Console.WriteLine(thisTypeface.FontPath);
                firstTypeFace = thisTypeface;
                break;
            } // Next thisTypeface 

            return tfs.GetTypeface(firstTypeFace);
        } // End Function GetFirstTypeFaceInDirectory 


        public static Typography.OpenFont.Typeface GetFontByPostScriptName(string fontDirectory, string postScriptName = "Asana-Math")
        {
            Typography.FontManagement.TypefaceStore store = GetTypeFaceStore(fontDirectory);
            Typography.FontManagement.InstalledTypeface itf = store.FontCollection.GetFontByPostScriptName(postScriptName);

            return store.GetTypeface(itf);
        } // End Function GetFontByPostScriptName 


        public static Typography.OpenFont.Typeface GetFontByFontName(string fontDirectory, string fontName = "Asana Math", Typography.FontManagement.TypefaceStyle tfs = Typography.FontManagement.TypefaceStyle.Regular)
        {
            Typography.FontManagement.TypefaceStore store = GetTypeFaceStore(fontDirectory);
            Typography.FontManagement.InstalledTypeface itf = store.FontCollection.GetInstalledTypeface(fontName, tfs);

            return store.GetTypeface(itf);
        } // End Function GetFontByFontName 


        public static Typography.OpenFont.Typeface GetSntAnouvong(string fontDirectory)
        {
            return GetFontByFontName(fontDirectory, "SNT Anouvong", Typography.FontManagement.TypefaceStyle.Regular);
        } // End Function GetSntAnouvong 


        private static bool s_not_printed = true;

        public static Typography.OpenFont.Typeface GetTestTypeface(string fontDirectory)
        {
            Typography.FontManagement.TypefaceStore store = GetTypeFaceStore(fontDirectory);
            Typography.FontManagement.InstalledTypeface itf = store.FontCollection.GetInstalledTypeface("Asana Math", Typography.FontManagement.TypefaceStyle.Regular);

            if (s_not_printed)
            {
                System.Console.Write("FontPath: ");
                System.Console.WriteLine(itf.FontPath);
                System.Console.Write("FontName: ");
                System.Console.WriteLine(itf.FontName);
                System.Console.Write("PostScriptName: ");
                System.Console.WriteLine(itf.PostScriptName);
                System.Console.Write("TypographicFamilyName: ");
                System.Console.WriteLine(itf.TypographicFamilyName);
                System.Console.Write("TypographicFontSubFamily: ");
                System.Console.WriteLine(itf.TypographicFontSubFamily);
                System.Console.Write("FontSubFamily: ");
                System.Console.WriteLine(itf.FontSubFamily);
                System.Console.Write("TypefaceStyle: ");
                System.Console.WriteLine(itf.TypefaceStyle);

                if (itf.Languages != null && itf.Languages.SupportedLangs != null)
                    System.Console.WriteLine(string.Join(", ", itf.Languages.SupportedLangs));

                s_not_printed = false;
            }

            return store.GetTypeface(itf);
        } // End Function GetTestTypeface 


    } // End Class FontHelper 


} // End Namespace SvgRenderer.Helpers 
