
namespace SvgRenderer
{
    

    // https://stackoverflow.com/questions/2587751/an-algorithm-to-find-bounding-box-of-closed-bezier-curves
    // https://stackoverflow.com/questions/24809978/calculating-the-bounding-box-of-cubic-bezier-curve
    public class BezierBoundsComputation
    {
        
        
        public static BoundingBox GetBoundsOfBezierCurve(float x0, float y0, float x1, float y1, float x2, float y2, float x3, float y3)
        {
            float[] tvalues = new float[4];
            PointF[] points = new PointF[4];
            float[][] bounds = new float[][]
            {
                new float[] {0, 0, 0},
                new float[] {0, 0, 0}
            };


            float a, b, c, t, t1, t2, b2ac, sqrtb2ac;
            int valuesIndex = 0;
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
                        // tvalues.Add(t);
                        tvalues[valuesIndex] = t;
                        valuesIndex++;
                    }

                    continue;
                } // End if (System.Math.Abs(a) < float.Epsilon) // Numerical robustness 

                b2ac = b * b - 4 * c * a;
                if (b2ac < 0)
                {
                    continue;
                }

                sqrtb2ac = (float) System.Math.Sqrt(b2ac);

                t1 = (-b + sqrtb2ac) / (2 * a);
                if (0 < t1 && t1 < 1)
                {
                    // tvalues.Add(t1);
                    tvalues[valuesIndex] = t1;
                    valuesIndex++;
                }

                t2 = (-b - sqrtb2ac) / (2 * a);
                if (0 < t2 && t2 < 1)
                {
                    // tvalues.Add(t2);
                    tvalues[valuesIndex] = t2;
                    valuesIndex++;
                }

            } // Next i 

            float x, y, mt;
            int j = valuesIndex;//-1; // tvalues.Count;
            int jlen = j;
            while ((j--) != 0)
            {
                t = tvalues[j];
                mt = 1 - t;
                x = (mt * mt * mt * x0) + (3 * mt * mt * t * x1) + (3 * mt * t * t * x2) + (t * t * t * x3);
                bounds[0][j] = x;

                y = (mt * mt * mt * y0) + (3 * mt * mt * t * y1) + (3 * mt * t * t * y2) + (t * t * t * y3);
                bounds[1][j] = y;
                points[j] = new PointF(x, y);
            } // Whend 

            tvalues[jlen] = 0;
            tvalues[jlen + 1] = 1;
            points[jlen] = new PointF(x0, y0);
            points[jlen + 1] = new PointF(x3, y3);
            bounds[0][jlen] = x0;
            bounds[1][jlen] = y0;
            bounds[0][jlen + 1] = x3;
            bounds[1][jlen + 1] = y3;

            // tvalues.Count = bounds[0].length = bounds[1].length = points.Count = jlen + 2;
            int newLen = jlen + 2;
            System.Array.Resize<PointF>(ref points, newLen);
            System.Array.Resize<float>(ref bounds[1], newLen);
            System.Array.Resize<float>(ref bounds[0], newLen);
            System.Array.Resize<float>(ref tvalues, newLen);


            BoundingBox bb = new BoundingBox()
            {
                Left = Min(bounds[0]),
                Top = Min(bounds[1]),
                Right = Max(bounds[0]),
                Bottom = Max(bounds[1]),
                Points = points,
                TValues = tvalues
            };
            
            return bb;
        } // End Function GetBoundsOfBezierCurve 


        private static T Min<T>(params T[] source) 
            where T: struct, System.IComparable<T>
        {
            if (source == null) 
                throw new System.ArgumentNullException("source");

            T value = default(T);
            bool hasValue = false;
            foreach (T x in source)
            {
                if (hasValue)
                {
                    // if (x < value) // https://docs.microsoft.com/en-us/dotnet/api/system.icomparable-1?view=netcore-3.1
                    // Less than zero This object precedes the object specified by the CompareTo method in the sort order.
                    // Zero This current instance occurs in the same position in the sort order as the object specified by the CompareTo method argument.
                    // Greater than zero
                    if (x.CompareTo(value) < 0)
                        value = x;
                }
                else
                {
                    value = x;
                    hasValue = true;
                }
            } // Next x 

            if (hasValue) 
                return value;

            throw new System.InvalidOperationException("Sequence contains no elements");
        } // End Function Min 


        private static T Max<T>(params T[] source) 
            where T : struct, System.IComparable<T>
        {
            if (source == null) 
                throw new System.ArgumentNullException("source");

            T value = default(T);
            bool hasValue = false;
            foreach (T x in source)
            {
                if (hasValue)
                {
                    // if (x > value) // https://docs.microsoft.com/en-us/dotnet/api/system.icomparable-1?view=netcore-3.1
                    // Less than zero This object precedes the object specified by the CompareTo method in the sort order.
                    // Zero This current instance occurs in the same position in the sort order as the object specified by the CompareTo method argument.
                    // Greater than zero
                    if (x.CompareTo(value) > 0)
                        value = x;
                }
                else
                {
                    value = x;
                    hasValue = true;
                }
            } // Next x 

            if (hasValue) 
                return value;

            throw new System.InvalidOperationException("Sequence contains no elements");
        } // End Function Max 


        public static void Test()
        {
            // Usage:
            BoundingBox bounds = GetBoundsOfBezierCurve(532,333,117,305,28,93,265,42);
            System.Console.WriteLine(bounds);

            // Prints: {"left":135.77684049079755,"top":42,"right":532,"bottom":333
            // ,"points":[{"X":135.77684049079755,"Y":144.86387466397255},{"X":532,"Y":333},{"X":265,"Y":42}]
            // ,"tvalues":[0.6365030674846626,0,1]}
        } // End Sub Test 


    } // End Class BezierBoundsComputation 


    public class BoundingBox
    {
        public float Left;
        public float Top;
        public float Right;
        public float Bottom;

        // public System.Collections.Generic.List<PointF> Points;
        public PointF[] Points;

        // public System.Collections.Generic.List<float> TValues; // t values of local extremes  
        public float[] TValues;
        

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("{ ");
            sb.Append("     \"left\":   ");
            sb.AppendLine(this.Left.ToString(System.Globalization.CultureInfo.InvariantCulture));
            sb.Append("    ,\"top\":    ");
            sb.AppendLine(this.Top.ToString(System.Globalization.CultureInfo.InvariantCulture));
            sb.Append("    ,\"right\":  ");
            sb.AppendLine(this.Right.ToString(System.Globalization.CultureInfo.InvariantCulture));
            sb.Append("    ,\"bottom\": ");
            sb.AppendLine(this.Bottom.ToString(System.Globalization.CultureInfo.InvariantCulture));

            sb.AppendLine("    ,\"points\":[");
            for (int i = 0; i < this.Points.Length; ++i)
            {
                if (i != 0)
                    sb.Append("        ,");
                else
                    sb.Append("         ");

                sb.Append("{ \"X\": ");
                sb.Append(this.Points[i].X.ToString(System.Globalization.CultureInfo.InvariantCulture) );
                sb.Append(", \"Y\": ");
                sb.Append(this.Points[i].Y.ToString(System.Globalization.CultureInfo.InvariantCulture));
                sb.AppendLine(" }");
            } // Next i 

            sb.AppendLine("    ]");
            sb.AppendLine("    ,\"tvalues\":[ ");

            for (int i = 0; i < this.TValues.Length; ++i)
            {
                if (i != 0)
                    sb.Append("        ,");
                else
                    sb.Append("         ");

                sb.AppendLine(this.TValues[i].ToString(System.Globalization.CultureInfo.InvariantCulture));
            }

            sb.AppendLine("    ] ");
            sb.AppendLine(" } ");
            
            return sb.ToString();
        } // End Function ToString 


    } // End Class BoundingBox 


}