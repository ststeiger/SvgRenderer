
using SkiaSharp;


namespace SvgRenderer
{


    class SkiaRenderer
    {


        public static void Test(string outputDirectory)
        {
            int width = 500;
            int height = 500;


            SKRect svgBounds = SKRect.Create(0, 0, 100, 100);

            outputDirectory = System.IO.Path.Combine(outputDirectory, "SkiaTestFile.svg");

            using (SKFileWStream stream = new SKFileWStream(outputDirectory)) // there are a few types of streams
            {
                using (SKCanvas canvas = SKSvgCanvas.Create(svgBounds, stream))

                // SKBitmap bitmap = new SKBitmap(width, height);
                // using (SKCanvas canvas = new SKCanvas(bitmap))
                {

                    using (SKPaint paint = new SKPaint())
                    {
                        paint.Typeface = SKTypeface.FromFamilyName(null, SKTypefaceStyle.Bold);
                        paint.TextSize = 10;

                        // paint.Style = SKPaintStyle.Stroke;
                        // paint.StrokeWidth = 1;
                        // paint.Color = SKColors.Red;

                        using (SKPath textPath = paint.GetTextPath("CODE", 0, 0))
                        {
                            // Set transform to center and enlarge clip path to window height
                            SKRect bounds;
                            textPath.GetTightBounds(out bounds);

                            // canvas.Translate(width / 2, height/ 2);
                            // canvas.Scale(width / bounds.Width, height / bounds.Height);
                            // canvas.Translate(-bounds.MidX, -bounds.MidY);

                            canvas.Translate(-bounds.Left, -bounds.Top);

                            // Set the clip path
                            // canvas.ClipPath(textPath);
                            canvas.DrawPath(textPath, paint);
                        } // End Using textPath 

                    } // End Using paint 

                } // End Using canvas 

            } // End Using stream 

            // string foo = bitmap.ToString();
            //System.Console.WriteLine(foo);
            System.Console.WriteLine("text");
        }


    }
}
