
//Apache2, 2017-present, WinterDev
//Apache2, 2014-2016, Samuel Carlsson, WinterDev

using Typography.OpenFont;


namespace SvgRenderer
{
    //------------------
    //this is Gdi+ version ***
    //render with System.Drawing.Drawing2D.GraphicsPath
    //------------------
    /// <summary>
    /// read result as Gdi+ GraphicsPath
    /// </summary>
    public class GlyphTranslatorToPdfPath 
        : IGlyphTranslator
    {
        //this gdi+ version
        PdfSharpCore.Drawing.XGraphicsPath ps;
        float lastMoveX;
        float lastMoveY;
        float lastX;
        float lastY;

        bool contour_is_closed = true;
        public GlyphTranslatorToPdfPath()
        { }
        
        public void BeginRead(int countourCount)
        {
            ps = new PdfSharpCore.Drawing.XGraphicsPath();
            // ps.Reset();
        }
        
        
        public void EndRead()
        { }
        
        
        public void MoveTo(float x0, float y0)
        {
            if (!contour_is_closed)
            {
                CloseContour();
            }

            lastX = lastMoveX = (float)x0;
            lastY = lastMoveY = (float)y0;
        }
        
        
        public void CloseContour()
        {
            contour_is_closed = true;
            ps.CloseFigure();

            lastX = lastMoveX;
            lastY = lastMoveY;
        }
        
        
        public void Curve3(float x1, float y1, float x2, float y2)
        {
            //from http://stackoverflow.com/questions/9485788/convert-quadratic-curve-to-cubic-curve
            //Control1X = StartX + (.66 * (ControlX - StartX))
            //Control2X = EndX + (.66 * (ControlX - EndX)) 
            contour_is_closed = false;
            float c1x = lastX + (float)((2f / 3f) * (x1 - lastX));
            float c1y = lastY + (float)((2f / 3f) * (y1 - lastY));
            //---------------------------------------------------------------------
            float c2x = (float)(x2 + ((2f / 3f) * (x1 - x2)));
            float c2y = (float)(y2 + ((2f / 3f) * (y1 - y2)));
            //---------------------------------------------------------------------
            ps.AddBezier(
                new PdfSharpCore.Drawing.XPoint(lastX, lastY),
                new PdfSharpCore.Drawing.XPoint(c1x, c1y),
                new PdfSharpCore.Drawing.XPoint(c2x, c2y),
                new PdfSharpCore.Drawing.XPoint(lastX = (float)x2, lastY = (float)y2));

        }
        
        
        public void Curve4(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            contour_is_closed = false;
            ps.AddBezier(
                new PdfSharpCore.Drawing.XPoint(lastX, lastY), 
                new PdfSharpCore.Drawing.XPoint((float)x1, (float)y1),
                new PdfSharpCore.Drawing.XPoint((float)x2, (float)y2),
                new PdfSharpCore.Drawing.XPoint(lastX = (float)x3, lastY = (float)y3));
        }
        
        
        public void LineTo(float x1, float y1)
        {
            contour_is_closed = false;
            ps.AddLine(
                 new PdfSharpCore.Drawing.XPoint(lastX, lastY),
                 new PdfSharpCore.Drawing.XPoint(lastX = (float)x1, lastY = (float)y1));
        }
        
        
        public void Reset()
        {
            ps = null;
            lastMoveX = lastMoveY = lastX = lastY = 0;
            contour_is_closed = true;
        }
        public PdfSharpCore.Drawing.XGraphicsPath ResultGraphicsPath => this.ps;
    }
    
    
}
