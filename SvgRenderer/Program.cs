
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
            */
            textToPrint = "COR-Basic";
            textToPrint = ".NET Core";
            // textToPrint = "Internet Exploder";
            
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
        }


    }


}
