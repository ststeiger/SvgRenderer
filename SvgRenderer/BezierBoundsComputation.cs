
namespace SvgRenderer
{
    
    // https://stackoverflow.com/questions/2587751/an-algorithm-to-find-bounding-box-of-closed-bezier-curves
    // https://stackoverflow.com/questions/24809978/calculating-the-bounding-box-of-cubic-bezier-curve
    public class BezierBoundsComputation
    {
        
        
        public static BoundingBox GetBoundsOfBezierCurve(float x0, float y0, float x1, float y1, float x2, float y2, float x3, float y3)
        {
            // System.Collections.Generic.List<float> tvalues = new System.Collections.Generic.List<float>();
            float[] tvalues = new float[4];
            PointF[] points = new PointF[4];
            // System.Collections.Generic.List<PointF> points = new System.Collections.Generic.List<PointF>();
            
            float[][] bounds = new float[][]
            {
                new float[] {0, 0},
                new float[] {0, 0}
            };


            float a, b, c, t, t1, t2, b2ac, sqrtb2ac;
            for (int i = 0; i < 2; ++i)
            {
                if (i == 0)
                {
                    a = -3 * x0 + 9 * x1 - 9 * x2 + 3 * x3;
                    b = 6 * x0 - 12 * x1 + 6 * x2;
                    c = 3 * x1 - 3 * x0;
                }
                else
                {
                    a = -3 * y0 + 9 * y1 - 9 * y2 + 3 * y3;
                    b = 6 * y0 - 12 * y1 + 6 * y2;
                    c = 3 * y1 - 3 * y0;
                }


                if (System.Math.Abs(a) < float.Epsilon) // Numerical robustness
                {
                    if (System.Math.Abs(b) < float.Epsilon) // Numerical robustness
                    {
                        continue;
                    }

                    t = -c / b;
                    if (0 < t && t < 1)
                    {
                        tvalues.Add(t);
                    }

                    continue;
                }

                b2ac = b * b - 4 * c * a;
                if (b2ac < 0)
                {
                    continue;
                }

                sqrtb2ac = (float) System.Math.Sqrt(b2ac);

                t1 = (-b + sqrtb2ac) / (2 * a);
                if (0 < t1 && t1 < 1)
                {
                    tvalues.Add(t1);
                }

                t2 = (-b - sqrtb2ac) / (2 * a);
                if (0 < t2 && t2 < 1)
                {
                    tvalues.Add(t2);
                }
            }

            float x, y, mt;
            int j = tvalues.Count;
            int jlen = j;
            while ((j--) != 0)
            {
                t = tvalues[j];
                mt = 1 - t;
                x = (mt * mt * mt * x0) + (3 * mt * mt * t * x1) + (3 * mt * t * t * x2) + (t * t * t * x3);
                bounds[0][j] = x;

                y = (mt * mt * mt * y0) + (3 * mt * mt * t * y1) + (3 * mt * t * t * y2) + (t * t * t * y3);
                bounds[1][j] = y;
                //points[j] = new PointF(x, y);
                
                if(points.Count == 0)
                    points.Add(new PointF(x, y));
                else
                    points.Insert(0, new PointF(x, y));
            }
            
            tvalues[jlen] = 0;
            tvalues[jlen + 1] = 1;
            points[jlen] = new PointF(x0, y0);
            points[jlen + 1] = new PointF(x3, y3);
            bounds[0][jlen] = x0;
            bounds[1][jlen] = y0;
            bounds[0][jlen + 1] = x3;
            bounds[1][jlen + 1] = y3;

            // tvalues.Count = bounds[0].length = bounds[1].length = points.Count = jlen + 2;


            BoundingBox bb = new BoundingBox()
            {
                Left = System.Math.Min(bounds[0][0], bounds[0][1]),
                Top = System.Math.Min(bounds[1][0], bounds[1][1]),
                Right = System.Math.Max(bounds[0][0], bounds[0][1]),
                Bottom = System.Math.Max(bounds[1][0], bounds[1][1]),
                Points = points,
                TValues = tvalues
            };
            
            return bb;
        }
        
        
        public static void Test()
        {
            // Usage:
            BoundingBox bounds = GetBoundsOfBezierCurve(532,333,117,305,28,93,265,42);
            System.Console.WriteLine(bounds);
            
            // Prints: {"left":135.77684049079755,"top":42,"right":532,"bottom":333
            // ,"points":[{"X":135.77684049079755,"Y":144.86387466397255},{"X":532,"Y":333},{"X":265,"Y":42}]
            // ,"tvalues":[0.6365030674846626,0,1]}
        }
        
        
    }


    public class BoundingBox
    {
        public float Left;
        public float Top;
        public float Right;
        public float Bottom;

        public System.Collections.Generic.List<PointF> Points;
        public System.Collections.Generic.List<float> TValues; // t values of local extremes  

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("{\"left\": ");
            sb.Append(this.Left);
            sb.Append(",\"top\": ");
            sb.Append(this.Top);
            sb.Append(",\"right\": ");
            sb.Append(this.Right);
            sb.Append("\"bottom\": ");
            sb.AppendLine(this.Bottom.ToString(System.Globalization.CultureInfo.InvariantCulture));

            sb.Append("\"points\":[");
            for (int i = 0; i < this.Points.Count; ++i)
            {
                if (i != 0)
                    sb.Append(",");
                
                sb.Append("{\"X\": ");
                sb.Append(this.Points[i].X.ToString(System.Globalization.CultureInfo.InvariantCulture) );
                sb.Append(", \"Y\": ");
                sb.Append(this.Points[i].Y.ToString(System.Globalization.CultureInfo.InvariantCulture));
                sb.Append("}");
            }

            sb.AppendLine();
            sb.AppendLine(" ]");
            sb.Append(",\"tvalues\":[");

            for (int i = 0; i < this.TValues.Count; ++i)
            {
                if (i != 0)
                    sb.Append(",");
                sb.Append(this.TValues[i].ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            sb.AppendLine(" ] ");
            sb.AppendLine(" } ");
            
            return base.ToString();
        }
    }
}