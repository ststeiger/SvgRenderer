using PdfSharpCore.Drawing;
using Typography.Contours;
using Typography.OpenFont;
using Typography.TextLayout;


namespace SvgRenderer
{
    class PdfTextRenderingTest
    {
        public static void Test(string textToPrint, string fontDirectory, string outputDirectory)
        {
            char[] textBuffer = textToPrint.ToCharArray();

            PdfSharpCore.Pdf.PdfDocument s_document = null;

            // Create a temporary file
            s_document = new PdfSharpCore.Pdf.PdfDocument();
            s_document.Info.Title = "PDFsharp XGraphic Sample";
            s_document.Info.Author = "Stefan Lange";
            s_document.Info.Subject = "Created with code snippets that show the use of graphical functions";
            s_document.Info.Keywords = "PDFsharp, XGraphics";

            PdfSharpCore.Pdf.PdfPage page = s_document.AddPage();
            
            using (PdfSharpCore.Drawing.XGraphics g = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page))
            {
                g.SmoothingMode = PdfSharpCore.Drawing.XSmoothingMode.HighQuality;
                // g.Clear(System.Drawing.Color.White);
                g.ScaleTransform(1.0F, -1.0F); // Flip the Y-Axis 
                g.TranslateTransform(0.0F, -(float) 500); // Translate the drawing area accordingly   


                DevPdfTextPrinter _currentTextPrinter = new DevPdfTextPrinter();
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
                int lineSpacingPx = (int) System.Math.Ceiling(_currentTextPrinter.FontLineSpacingPx);
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
                g.ScaleTransform(1.0F, -1.0F); // Flip the Y-Axis 
                g.TranslateTransform(0.0F, -(float) 500); // Translate the drawing area accordingly            
            } // End Using g 


            outputDirectory = System.IO.Path.Combine(outputDirectory, "FontRendering.pdf");
            s_document.Save(outputDirectory);
        } // End Sub Test 
    } // End Class GdiTextRenderingTest 
} // End Namespace SvgRenderer 