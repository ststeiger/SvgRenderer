
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


        static void Main(string[] args)
        {
            string outputDirectory = System.IO.Path.GetDirectoryName( typeof(Program).Assembly.Location);
            outputDirectory = System.IO.Path.Combine(outputDirectory, "..", "..", "..");
            outputDirectory = System.IO.Path.GetFullPath(outputDirectory);
            string fontDirectory = System.IO.Path.Combine(outputDirectory, "TestFonts");
            // Helpers.FontHelper.ListInstalledTypefaces(fontDirectory);


            string textToPrint = "Hello World";
            textToPrint = "H";
            textToPrint = "A";
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

            */
            
            textToPrint = ".NET Core";
            textToPrint = "1. Obergeschoss";
            textToPrint = "1st Floor";


            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                System.Console.WriteLine(System.Environment.NewLine);
                System.Console.WriteLine("The GDI-renderer needs libgdiplus.so/libgdiplus.dylib from the mono-project.");
                System.Console.WriteLine("sudo apt-get install -y libgdiplus");
                System.Console.WriteLine(System.Environment.NewLine);
            }
            
            GdiTextRenderingTest.Test(textToPrint, fontDirectory, outputDirectory);
            SvgRenderingTest.Test(textToPrint, fontDirectory, outputDirectory);
            // SkiaRenderer.Test(outputDirectory);

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine("Raster and vector-image created in: ");
            System.Console.WriteLine(outputDirectory);
            System.Console.WriteLine("(FontRendering.png, FontRendering.svg)");
            System.Console.WriteLine(System.Environment.NewLine);


            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            // System.Console.ReadKey();
        } // End Sub Main(string[] args)


    } // End Class Program 


} // End Namespace SvgRenderer 
