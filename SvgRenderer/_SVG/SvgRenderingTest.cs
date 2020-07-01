
using Typography.Contours;
using Typography.OpenFont;
using Typography.TextLayout;


namespace SvgRenderer
{


    class SvgRenderingTest
    {


        public static void Test(string textToPrint, string fontDirectory, string outputDirectory)
        {
            char[] textBuffer = textToPrint.ToCharArray();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            using (SvgGraphics g = new SvgGraphics(sb))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(System.Drawing.Color.White);
                g.ScaleTransform(1.0F, -1.0F); // Flip the Y-Axis 
                g.TranslateTransform(0.0F, -(float)500); // Translate the drawing area accordingly   


                SvgTextPrinter _currentTextPrinter = new SvgTextPrinter();
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


                // test draw multiple lines

                /*
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
                */

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
                g.ScaleTransform(1.0F, -1.0F); // Flip the Y-Axis 
                g.TranslateTransform(0.0F, -(float)500); // Translate the drawing area accordingly 

                g.WriteEndFile();
            } // End Using g 

            string svg = sb.ToString();
            outputDirectory = System.IO.Path.Combine(outputDirectory, "FontRendering.svg");
            System.IO.File.WriteAllText(outputDirectory, svg, System.Text.Encoding.UTF8);
        } // End Sub Test 


    } // End Class SvgRenderingTest 


} // End Namespace SvgRenderer 
