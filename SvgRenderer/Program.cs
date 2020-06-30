﻿
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
            string fontDirectory = @"C:\Users\Administrator\Documents\Visual Studio 2019\Projects\Typography\Demo\Windows\TestFonts";
            string outputDirectory = System.IO.Path.GetDirectoryName( typeof(Program).Assembly.Location);
            outputDirectory = System.IO.Path.Combine(outputDirectory, "..", "..", "..");
            outputDirectory = System.IO.Path.GetFullPath(outputDirectory);
            
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                fontDirectory = "/usr/share/fonts/truetype/ubuntu/";
            }

            if ("COR".Equals(System.Environment.UserDomainName, System.StringComparison.InvariantCultureIgnoreCase))
            {
                fontDirectory = @"D:\username\Documents\Visual Studio 2017\Projects\Typography\Demo\Windows\TestFonts";
            }


            string textToPrint = "Hello World";
            textToPrint = "H";
            textToPrint = "A";
            textToPrint = "HELLO";
            textToPrint = "HELiOS";
            textToPrint = "hello";
            
            
            GdiTextRenderingTest.Test(textToPrint, fontDirectory, outputDirectory);
            SvgRenderingTest.Test(textToPrint, fontDirectory, outputDirectory);
            // SkiaRenderer.Test(outputDirectory);
            
            
            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            // System.Console.ReadKey();
        }


    }


}
