
namespace SvgRenderer
{


    public class SvgGraphics
        : System.IDisposable
    {

        protected System.Text.StringBuilder m_stringBuilder;

        public SmoothingMode SmoothingMode
        {
            get; set;
        }


        public SvgGraphics()
            : this(new System.Text.StringBuilder())
        { }


        public SvgGraphics(System.Text.StringBuilder sb)
        {
            this.m_stringBuilder = sb;
        } // End Constructor 


        // Clears the entire drawing surface and fills it with the specified background color.
        //   color: System.Drawing.Color structure that represents the background color of the drawing surface.
        public void Clear(System.Drawing.Color color)
        {
            this.m_stringBuilder.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:svg=""http://www.w3.org/2000/svg"">");
            m_stringBuilder.AppendLine();

            // id="svg2" version="1.1"
            // viewBox="0 0 1000 1187.198"
            // width="1000" height="1187.198"

            this.m_stringBuilder.AppendLine("<!--");
            this.m_stringBuilder.AppendFormat("    Clear: R: {0}, G: {1}, B: {2}", color.R, color.G, color.B);
            this.m_stringBuilder.AppendLine();
            this.m_stringBuilder.AppendLine("-->");
            this.m_stringBuilder.AppendLine();
        } // End Sub Clear 


        public void WriteEndFile()
        {
            this.m_stringBuilder.AppendLine("</svg>");
        } // End Sub WriteEndFile 



        public void OpenGroup()
        {
            this.m_stringBuilder.Append("<g>");
        }

        public void CloseGroup()
        {
            this.m_stringBuilder.Append("</g>");
        }


        protected string m_rotate;



        // Applies the specified rotation to the transformation matrix of this System.Drawing.Graphics.
        //   angle: Angle of rotation in degrees.
        public void RotateTransform(float angle)
        {
            // rotate(30 0 0)
            this.m_translate = "rotate(" + angle.ToString(System.Globalization.CultureInfo.InvariantCulture) + " 0 0)";
        }



        protected string m_scale;

        // Applies the specified scaling operation to the transformation matrix of this
        // System.Drawing.Graphics by prepending it to the object's transformation matrix.
        //   sx: Scale factor in the x direction.
        //   sy: Scale factor in the y direction.
        public void ScaleTransform(float sx, float sy)
        {
            // scale(sx sy)
            this.m_scale = "scale(" + sx.ToString(System.Globalization.CultureInfo.InvariantCulture)
                + " " + sy.ToString(System.Globalization.CultureInfo.InvariantCulture)
                + ")"
            ;
        } // End Sub ScaleTransform 



        protected string m_translate;

        // Changes the origin of the coordinate system by prepending the specified translation
        // to the transformation matrix of this System.Drawing.Graphics.
        //   dx: The x-coordinate of the translation.
        //   dy: The y-coordinate of the translation.
        public void TranslateTransform(float dx, float dy)
        {
            // translate(dx dy)
            this.m_translate = "translate(" + dx.ToString(System.Globalization.CultureInfo.InvariantCulture)
                + " " + dy.ToString(System.Globalization.CultureInfo.InvariantCulture)
                + ")"
            ;
        } // End Sub TranslateTransform 


        // Fills the interior of a System.Drawing.Drawing2D.GraphicsPath.
        //   brush: System.Drawing.Brush that determines the characteristics of the fill.
        //   path: System.Drawing.Drawing2D.GraphicsPath that represents the path to fill.
        // Ausnahmen: T:System.ArgumentNullException: brush is null. -or- path is null.
        public void FillPath(SvgSolidBrush brush, SvgPath path)
        {
            DrawOrFillPath($"\" style=\"fill: {brush.Color}; stroke: none;\"", path);
            
            // this.m_stringBuilder.AppendLine("<!--");
            // this.m_stringBuilder.AppendLine("FillPath: ");
            // this.m_stringBuilder.Append(path.sb);
            // this.m_stringBuilder.AppendLine();

            // this.m_stringBuilder.AppendLine("Brush: " + brush.ToString());
            // this.m_stringBuilder.AppendLine("-->");
            // this.m_stringBuilder.AppendLine();
        } // End Sub FillPath 


        // Draws a System.Drawing.Drawing2D.GraphicsPath.
        //   pen: System.Drawing.Pen that determines the color, width, and style of the path.
        //   path: System.Drawing.Drawing2D.GraphicsPath to draw.
        // Ausnahmen: T:System.ArgumentNullException: pen is null. -or- path is null.
        public void DrawPath(SvgPen pen, SvgPath path)
        {
            DrawOrFillPath($"\" style=\"stroke: {pen.Color}; stroke-width:1px;\"", path);
        }
        
        
        public void DrawOrFillPath(string style, SvgPath path)
        {
            this.m_stringBuilder.Append("<path d=\"");
            this.m_stringBuilder.Append(path.sb);
            
            // stroke-width:0.26px
            this.m_stringBuilder.Append(style);
            
            if (this.m_scale != null || this.m_translate != null)
            {
                this.m_stringBuilder.Append(" transform=\"");

                if (this.m_translate != null)
                {
                    this.m_stringBuilder.Append(this.m_translate);
                    if (this.m_scale != null)
                        this.m_stringBuilder.Append(" ");
                } // End if(this.m_translate != null) 

                if (this.m_scale != null)
                    this.m_stringBuilder.Append(this.m_scale);

                this.m_stringBuilder.Append("\"");
            } // End if (this.m_scale != null || this.m_translate != null) 

            this.m_stringBuilder.AppendLine(" />");
        } // End Sub DrawPath 
        
        
        public void Dispose()
        {
            // Don't Clear the stringBuilder, classes are passed by reference.
            // if (this.m_stringBuilder != null) this.m_stringBuilder.Length = 0;
            this.m_stringBuilder = null;
            this.m_rotate = null;
            this.m_scale = null;
            this.m_translate = null;
        } // End Sub Dispose 


    } // End Class SvgGraphics 


} // End Namespace SvgRenderer 
