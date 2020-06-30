
using SixLabors.Fonts; // For CreateFont 
using SixLabors.ImageSharp.Processing; // For Mutate 
using SixLabors.ImageSharp.Drawing.Processing; // For DrawText

// using SixLabors.ImageSharp;
// using SixLabors.ImageSharp.PixelFormats;
// using SixLabors.ImageSharp.Formats.Png;

namespace SvgRenderer
{


    class TestSixLabors
    {

        public static System.IO.Stream WriteTextToImage(string text)
        {
            using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>.Load("Assets/share-bg.png"))
            {
                SixLabors.Fonts.FontCollection fontCollection = new SixLabors.Fonts.FontCollection();
                SixLabors.Fonts.Font regularFont = fontCollection.Install("Assets/TitilliumWeb-SemiBold.ttf").CreateFont(24, SixLabors.Fonts.FontStyle.Regular);
                SixLabors.Fonts.Font italicFont = fontCollection.Install("Assets/TitilliumWeb-BoldItalic.ttf").CreateFont(24, SixLabors.Fonts.FontStyle.Italic);

                image.Mutate(x => x.DrawText(text, regularFont, SixLabors.ImageSharp.Color.White, new SixLabors.ImageSharp.PointF(100, 100)));

                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                image.Save(stream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
                stream.Position = 0;
                return stream;
            }
        }


    }


}
