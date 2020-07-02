
namespace SvgRenderer
{


    // https://docs.microsoft.com/en-us/typography/opentype/spec/otff
    // https://en.wikipedia.org/wiki/TrueType
    // https://formats.kaitai.io/ttf/index.html
    // https://formats.kaitai.io/ttf/ttf.svg
    // https://github.com/PaintLab/SaveAsPdf
    // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/graphics/skiasharp/curves/text-paths
    class Program
    {


        public static void TestGdiFont()
        {
            // System.Drawing.Text.PrivateFontCollection myFonts = new System.Drawing.Text.PrivateFontCollection();
            // myFonts.AddFontFile(@"C:\path\to\BLACKR.TTF");
            // System.Drawing.Font oFont = new System.Drawing.Font(myFonts.Families[0], 20);

            // https://stackoverflow.com/questions/16769758/get-a-font-filename-based-on-the-font-handle-hfont
            // Get font as stream


            System.Drawing.FontFamily fontFamily = new System.Drawing.FontFamily("Arial");
            System.Drawing.Font font = new System.Drawing.Font(
               fontFamily,
               16,
               System.Drawing.FontStyle.Regular,
               System.Drawing.GraphicsUnit.Pixel);

            System.IntPtr handleToFont = font.ToHfont();



            AspNetCore.ReportingServices.Rendering.ExcelRenderer.Excel.LOGFONT lOGFONT = new AspNetCore.ReportingServices.Rendering.ExcelRenderer.Excel.LOGFONT();
            font.ToLogFont(lOGFONT);

            AspNetCore.ReportingServices.Rendering.ExcelRenderer.Excel.CharSet cs = (AspNetCore.ReportingServices.Rendering.ExcelRenderer.Excel.CharSet)lOGFONT.lfCharSet;

            System.Console.WriteLine(lOGFONT.lfFaceName);


            // font.IsSystemFont
            System.Console.WriteLine(font.Name);
            System.Console.WriteLine(font.OriginalFontName);
            System.Console.WriteLine(font.SystemFontName);
            System.Console.WriteLine(font.FontFamily.Name);
        } // End Sub TestGdiFont 


        // https://superuser.com/questions/760627/how-to-list-installed-font-families
        public static void ListWindowsFonts()
        {
            // reg query "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /s

            System.Drawing.Text.InstalledFontCollection installedFonts = new System.Drawing.Text.InstalledFontCollection();
            foreach (System.Drawing.FontFamily thisFontFamily in installedFonts.Families)
            {
                System.Console.Write(thisFontFamily.Name);
                // https://www.codeproject.com/Articles/4190/XFont-Get-font-name-and-file-information
                // https://stackoverflow.com/questions/7408024/how-to-get-a-font-file-name/7408230

                System.Drawing.Font font = new System.Drawing.Font(
                   thisFontFamily,
                   16,
                   System.Drawing.FontStyle.Regular,
                   System.Drawing.GraphicsUnit.Pixel
                );

                System.Console.WriteLine(font.Name);
                System.Console.WriteLine(font.OriginalFontName);
                System.Console.WriteLine(font.SystemFontName);
            } // Next thisFontFamily 

        } // End Sub ListWindowsFonts 


        public static void ListWindowsFonts2()
        {
            // Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts");

            foreach (string value in key.GetValueNames())
            {
                System.Console.Write("Font: ");
                System.Console.WriteLine(value);

                // Check for the publisher to ensure it's our product
                string fileName = System.Convert.ToString(key.GetValue(value));

                System.Console.Write("Filename: ");
                System.Console.WriteLine(fileName);
            } // Next value 

        } // End Sub ListWindowsFonts2 


        public static void ListFontsOnOSX()
        {
            // https:// github.com/xamarin/xamarin-macios
            // https:// github.com/xamarin/xamarin-macios/tree/main/src/AppKit
            // https:// github.com/mono/xwt/blob/master/Xwt.XamMac/Xwt.Mac/Util.cs
            // https:// github.com/mono/cocoa-sharp


            // NSFontManager.SharedFontManager

            // manager = Cocoa.NSFontManager.sharedFontManager()
            // font_families = list(manager.availableFontFamilies())

            // //  This returns an array of NSStrings that gives you each font installed on the system
            // NSArray* fonts = [[NSFontManager sharedFontManager] availableFontFamilies];

            // //  Does the same as the above, but includes each available font style (e.g. you get
            // //  Verdana, "Verdana-Bold", "Verdana-BoldItalic", and "Verdana-Italic" for Verdana).
            // NSArray* fonts = [[NSFontManager sharedFontManager] availableFonts];
        }


        // https://stackoverflow.com/questions/24809978/calculating-the-bounding-box-of-cubic-bezier-curve        
        // https://en.wikipedia.org/wiki/Zero_to_the_power_of_zero
        public static void ComputeBezierBounds(float A, float B, float C, float D)
        {
            float a = 3.0f * D - 9.0f * C + 9.0f * B - 3.0f * A;
            float b = 6.0f * A - 12.0f * B + 6.0f * C;
            float c = 3.0f * B - A;

            // solve for a t^2 + b t + c 

            float sqrtexp = b * b - 4 * a * c;
            //float two_a = 2 * a;

            if (sqrtexp < 0)
            {
                // No real solution (=collinear?) 
                // return points A and D
            }


            // at a=0 ==> t = -c/b;
            if (a == 0.0f)//if (two_a == 0.0f)
            {
                if (b == 0.0f)
                {

                }
                else
                {
                    // One solution
                    //  t = -c/b;
                }

            } // End if a == 0 

        } // End Sub ComputeBezierBounds 


        static void Main(string[] args)
        {
            ListWindowsFonts();
            BezierBoundsComputation.Test();
            FontConfig.FontQuery.Test((new string[] { "Verdana" }));

            // TestGdiFont();

            string outputDirectory = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);
            outputDirectory = System.IO.Path.Combine(outputDirectory, "..", "..", "..");
            outputDirectory = System.IO.Path.GetFullPath(outputDirectory);
            string fontDirectory = System.IO.Path.Combine(outputDirectory, "TestFonts");
            // string fontDirectory = System.IO.Path.Combine(outputDirectory, "SystemFonts");
            // Helpers.FontHelper.ListInstalledTypefaces(fontDirectory);


            string textToPrint = "Hello World";

            /*
            textToPrint = "HELLO";
            textToPrint = "HELiOS";
            textToPrint = "hello";
            textToPrint = "Halloween";
            textToPrint = "Aloha oe";

            // https://www.google.com/get/noto/#sans-hant
            textToPrint = "Привет мир";
            textToPrint = "你好，世界"; // Mandarin
            textToPrint = "مرحبا بالعالم"; // Arabic
            textToPrint = "Chào thế giới"; // Viet 
            textToPrint = "สวัสดีชาวโลก"; // Thai 
            textToPrint = "ওহে বিশ্ব"; // Bengali 
            textToPrint = "សួស្តី​ពិភពលោក"; // Khmer
            textToPrint = "こんにちは世界"; // Japanese 
            textToPrint = "안녕 세상"; // Korean
            textToPrint = "Γειά σου Κόσμε"; // Greek
            */

            textToPrint = ".NET Core";
            textToPrint = "1st Floor";
            textToPrint = "1ος όροφος";
            // textToPrint = "1-й этаж";
            textToPrint = "1. Obergeschoss äöüÄÖÜ";


            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                System.Console.WriteLine(System.Environment.NewLine);
                System.Console.WriteLine("The GDI-renderer needs libgdiplus.so/libgdiplus.dylib from the mono-project.");
                System.Console.WriteLine("sudo apt-get install -y libgdiplus");
                System.Console.WriteLine(System.Environment.NewLine);
            } // End if (System.Environment.OSVersion.Platform == System.PlatformID.Unix) 

            GdiTextRenderingTest.Test(textToPrint, fontDirectory, outputDirectory);
            SvgRenderingTest.Test(textToPrint, fontDirectory, outputDirectory);
            PdfTextRenderingTest.Test(textToPrint, fontDirectory, outputDirectory);
            // SkiaRenderer.Test(outputDirectory);

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine("Raster and vector-image created in: ");
            System.Console.WriteLine(outputDirectory);
            System.Console.WriteLine("(FontRendering.png, FontRendering.svg, FontRendering.pdf)");
            System.Console.WriteLine(System.Environment.NewLine);


            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            // System.Console.ReadKey();
        } // End Sub Main(string[] args)


    } // End Class Program 


} // End Namespace SvgRenderer 
