
using SampleWinForms;

using Typography.Contours;
using Typography.OpenFont;
using Typography.TextLayout;


namespace SvgRenderer
{


    class GdiTextRenderingTest
    {


        public static void Test(string textToPrint, string fontDirectory, string outputDirectory)
        {
            char[] textBuffer = textToPrint.ToCharArray();
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(1000, 1000);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(System.Drawing.Color.White);
                g.ScaleTransform(1.0F, -1.0F);// Flip the Y-Axis 
                g.TranslateTransform(0.0F, -(float)500);// Translate the drawing area accordingly   


                DevGdiTextPrinter _currentTextPrinter = new DevGdiTextPrinter();
                _currentTextPrinter.ScriptLang = new ScriptLang(ScriptTagDefs.Thai.Tag);
                _currentTextPrinter.Typeface = Helpers.FontHelper.GetTestTypeface(fontDirectory);
                _currentTextPrinter.FontSizeInPoints = 32;
                _currentTextPrinter.FillBackground = true;
                _currentTextPrinter.DrawOutline = false;

                //-----------------------  
                _currentTextPrinter.HintTechnique = HintTechnique.None;
                _currentTextPrinter.PositionTechnique = PositionTechnique.None;
                _currentTextPrinter.TargetGraphics = g;
                //render at specific pos
                int lineSpacingPx = (int)System.Math.Ceiling(_currentTextPrinter.FontLineSpacingPx);
                float x_pos = 0, y_pos = y_pos = lineSpacingPx * 2; //start 1st line


                //test draw multiple lines

                for (int i = 0; i < 3; ++i)
                {
                    _currentTextPrinter.DrawString(
                     textBuffer,
                     0,
                     textBuffer.Length,
                     x_pos,
                     y_pos
                    );
                    //draw top to bottom 
                    y_pos -= lineSpacingPx;
                }

                //transform back
                g.ScaleTransform(1.0F, -1.0F);// Flip the Y-Axis 
                g.TranslateTransform(0.0F, -(float)500);// Translate the drawing area accordingly            
            } // End Using g 


            outputDirectory = System.IO.Path.Combine(outputDirectory, "FontRendering.png");
            bmp.Save(outputDirectory, System.Drawing.Imaging.ImageFormat.Png);
        } // End Sub Test 


    } // End Class GdiTextRenderingTest 


} // End Namespace SvgRenderer 
