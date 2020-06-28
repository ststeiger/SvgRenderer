using Microsoft.VisualBasic;
using SampleWinForms;
using System;
using System.Collections.Generic;
using System.Text;

using Typography.Contours;
using Typography.FontManagement;
using Typography.OpenFont;
using Typography.TextLayout;

namespace SvgRenderer
{
    class GdiTextRenderingTest
    {

        public static void Test(string fontDirectory, string outputDirectory)
        {
            char[] textBuffer = "Hello World".ToCharArray();
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(1000, 1000);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(System.Drawing.Color.White);
                g.ScaleTransform(1.0F, -1.0F);// Flip the Y-Axis 
                g.TranslateTransform(0.0F, -(float)500);// Translate the drawing area accordingly   


                //1. create font collection             
                InstalledTypefaceCollection _installedFontCollection = new InstalledTypefaceCollection();

                //2. set some essential handler
                _installedFontCollection.SetFontNameDuplicatedHandler((f1, f2) => FontNameDuplicatedDecision.Skip);

                _installedFontCollection.LoadFontsFromFolder(fontDirectory);
                //installedFontCollection.LoadSystemFonts();


                InstalledTypeface ff = null;

                foreach (InstalledTypeface thisTypeface in _installedFontCollection.GetInstalledFontIter())
                {
                    System.Console.WriteLine(thisTypeface.FontName);
                    ff = thisTypeface;
                    break;
                }




                TypefaceStore _typefaceStore = new TypefaceStore();
                _typefaceStore.FontCollection = _installedFontCollection;
                _installedFontCollection.UpdateUnicodeRanges();


                DevGdiTextPrinter _currentTextPrinter = new DevGdiTextPrinter();
                _currentTextPrinter.ScriptLang = new ScriptLang(ScriptTagDefs.Thai.Tag);
                _currentTextPrinter.Typeface = _typefaceStore.GetTypeface(ff);
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


    }
}
