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
            string outputDirectory = @"D:\";

            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                fontDirectory = "";
                outputDirectory = "/root/Desktop";
            }



            GdiTextRenderingTest.Test(fontDirectory, outputDirectory);
            SvgRenderingTest.Test(fontDirectory, outputDirectory);
            SkiaRenderer.Test(outputDirectory);


            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        }


    }


}
