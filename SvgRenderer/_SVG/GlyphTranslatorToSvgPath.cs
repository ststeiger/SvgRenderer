﻿
using System.Drawing;
using Typography.OpenFont;


namespace SvgRenderer
{


    public class GlyphTranslatorToSvgPath 
        : IGlyphTranslator
    {
        SvgPath ps;
        float lastMoveX;
        float lastMoveY;
        float lastX;
        float lastY;
        bool contour_is_closed;


        public GlyphTranslatorToSvgPath(bool isContourClosed)
        {
            contour_is_closed = isContourClosed;
        }


        public GlyphTranslatorToSvgPath()
            : this(true)
        { }


        public void BeginRead(int contourCount)
        {
            ps = new SvgPath();
            ps.Reset();
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
                new PointF(lastX, lastY),
                new PointF(c1x, c1y),
                new PointF(c2x, c2y),
                new PointF(lastX = (float)x2, lastY = (float)y2));
        }

        public void Curve4(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            contour_is_closed = false;
            ps.AddBezier(
                new PointF(lastX, lastY),
                new PointF((float)x1, (float)y1),
                new PointF((float)x2, (float)y2),
                new PointF(lastX = (float)x3, lastY = (float)y3));
        }

        public void EndRead()
        { }


        public void LineTo(float x1, float y1)
        {
            contour_is_closed = false;
            ps.AddLine(
                 new PointF(lastX, lastY),
                 new PointF(lastX = (float)x1, lastY = (float)y1));
        }


        public void MoveTo(float x0, float y0)
        {
            if (!contour_is_closed)
            {
                CloseContour();
            }

            lastX = lastMoveX = (float)x0;
            lastY = lastMoveY = (float)y0;
        }


        // Extension to abstract class 
        public SvgPath ResultGraphicsPath
        {
            get { return this.ps; }
        }


        // Extension to abstract class 
        public void Reset()
        {
            ps = null;
            lastMoveX = lastMoveY = lastX = lastY = 0;
            contour_is_closed = true;
        }


    }

}
