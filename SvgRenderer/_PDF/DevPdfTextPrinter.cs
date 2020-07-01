//MIT, 2016-present, WinterDev

using Typography.OpenFont;
using Typography.OpenFont.Tables;
using Typography.TextLayout;
using Typography.Contours;


namespace SvgRenderer
{

    /// <summary>
    /// developer's version, Gdi+ text printer
    /// </summary>
    class DevPdfTextPrinter 
        : TextPrinterBase
    {
        protected Typeface _currentTypeface;
        protected GlyphOutlineBuilder _currentGlyphPathBuilder;
        protected GlyphTranslatorToPdfPath _txToGdiPath;
        protected GlyphLayout _glyphLayout = new GlyphLayout();
        protected PdfSharpCore.Drawing.XSolidBrush _fillBrush = new PdfSharpCore.Drawing.XSolidBrush(PdfSharpCore.Drawing.XColors.Black);
        protected PdfSharpCore.Drawing.XPen _outlinePen = new PdfSharpCore.Drawing.XPen(PdfSharpCore.Drawing.XColors.Green);

        //for optimization
        protected GlyphMeshCollection<PdfSharpCore.Drawing.XGraphicsPath> _glyphMeshCollections = new GlyphMeshCollection<PdfSharpCore.Drawing.XGraphicsPath>();
        protected UnscaledGlyphPlanList _reusableUnscaledGlyphPlanList = new UnscaledGlyphPlanList();
        
        
        public override GlyphLayout GlyphLayoutMan
        {
            get
            {
                return _glyphLayout;
            }
        } // End Property GlyphLayoutMan 


        public bool EnableColorGlyph { get; set; } = true;
        public PdfSharpCore.Drawing.XColor FillColor { get; set; }
        public PdfSharpCore.Drawing.XColor OutlineColor { get; set; }

        public PdfSharpCore.Drawing.XGraphics TargetGraphics { get; set; }
        
        
        public DevPdfTextPrinter()
        {
            FillBackground = true;
            FillColor = PdfSharpCore.Drawing.XColors.Black;
            OutlineColor = PdfSharpCore.Drawing.XColors.Green;
        } // End Constructor 
        
        
        public override Typeface Typeface
        {
            get => _currentTypeface;

            set
            {
                //check if we change it or not
                if (value == _currentTypeface) return;
                //change ...
                //check if we have used this typeface before?
                //if not, create a proper glyph builder for it
                //--------------------------------
                //reset 
                _currentTypeface = value;
                _currentGlyphPathBuilder = null;
                _glyphLayout.Typeface = value;
                //--------------------------------
                if (value == null) return;
                //--------------------------------

                //2. glyph builder
                _currentGlyphPathBuilder = new GlyphOutlineBuilder(_currentTypeface);
                //for gdi path***
                //3. glyph reader,output as Gdi+ GraphicsPath
                _txToGdiPath = new GlyphTranslatorToPdfPath();
                //4.
                OnFontSizeChanged();
            } // End Set 

        } // End Property Typeface 


        protected override void OnFontSizeChanged()
        {
            //update some font matrix property  
            if (Typeface != null)
            {
                float pointToPixelScale = Typeface.CalculateScaleToPixelFromPointSize(this.FontSizeInPoints);
                this.FontAscendingPx = Typeface.Ascender * pointToPixelScale;
                this.FontDescedingPx = Typeface.Descender * pointToPixelScale;
                this.FontLineGapPx = Typeface.LineGap * pointToPixelScale;
                this.FontLineSpacingPx = FontAscendingPx - FontDescedingPx + FontLineGapPx;
            } // End if (Typeface != null) 

        } // End Sub OnFontSizeChanged 


        public void UpdateGlyphLayoutSettings()
        {
            _glyphLayout.Typeface = this.Typeface;
            _glyphLayout.ScriptLang = this.ScriptLang;
            _glyphLayout.PositionTechnique = this.PositionTechnique;
            _glyphLayout.EnableLigature = this.EnableLigature;
        } // End Sub UpdateGlyphLayoutSettings 


        protected void UpdateVisualOutputSettings()
        {
            _currentGlyphPathBuilder.SetHintTechnique(this.HintTechnique);
            _fillBrush.Color = this.FillColor;
            _outlinePen.Color = this.OutlineColor;
        } // End Sub UpdateVisualOutputSettings 


        protected PdfSharpCore.Drawing.XGraphicsPath GetExistingOrCreateGraphicsPath(ushort glyphIndex)
        {
            if (!_glyphMeshCollections.TryGetCacheGlyph(glyphIndex, out PdfSharpCore.Drawing.XGraphicsPath path))
            {
                _txToGdiPath.Reset(); //clear

                //if not found then create a new one
                _currentGlyphPathBuilder.BuildFromGlyphIndex(glyphIndex, this.FontSizeInPoints, _txToGdiPath);
                path = _txToGdiPath.ResultGraphicsPath;

                //register
                _glyphMeshCollections.RegisterCachedGlyph(glyphIndex, path);
            } // End if (!_glyphMeshCollections.TryGetCacheGlyph(glyphIndex, out System.Drawing.Drawing2D.GraphicsPath path)) 

            return path;
        } // End Function GetExistingOrCreateGraphicsPath 


        // public override void DrawFromGlyphPlans(GlyphPlanSequence glyphPlanList, int startAt, int len, float left, float top)
        public override void DrawFromGlyphPlans(GlyphPlanSequence seq, int startAt, int len, float x, float y)
        {
            UpdateVisualOutputSettings();

            //draw data in glyph plan 
            //3. render each glyph

            float sizeInPoints = this.FontSizeInPoints;
            float pxscale = _currentTypeface.CalculateScaleToPixelFromPointSize(sizeInPoints);


            Typeface typeface = this.Typeface;
            _glyphMeshCollections.SetCacheInfo(typeface, sizeInPoints, this.HintTechnique);


            //this draw a single line text span*** 
            PdfSharpCore.Drawing.XGraphics g = this.TargetGraphics;
            float baseline = y;
            GlyphPlanSequenceSnapPixelScaleLayout snapToPxScale =
                new GlyphPlanSequenceSnapPixelScaleLayout(seq, startAt, len, pxscale);

            COLR colrTable = typeface.COLRTable;
            CPAL cpalTable = typeface.CPALTable;



            bool canUseColorGlyph = EnableColorGlyph && colrTable != null && cpalTable != null;

            while (snapToPxScale.Read())
            {
                ushort glyphIndex = snapToPxScale.CurrentGlyphIndex;

                if (canUseColorGlyph && colrTable.LayerIndices.TryGetValue(glyphIndex, out ushort colorLayerStart))
                {

                    ushort colorLayerCount = colrTable.LayerCounts[glyphIndex];

                    for (int c = colorLayerStart; c < colorLayerStart + colorLayerCount; ++c)
                    {
                        PdfSharpCore.Drawing.XGraphicsPath path = GetExistingOrCreateGraphicsPath(colrTable.GlyphLayers[c]);
                        if (path == null)
                        {
#if DEBUG
                            System.Diagnostics.Debug.WriteLine("gdi_printer: no path?");
#endif
                            continue;
                        }

                        //then move pen point to the position we want to draw a glyph
                        float cx = (float)System.Math.Round(snapToPxScale.ExactX + x);
                        float cy = (float)System.Math.Floor(snapToPxScale.ExactY + baseline);

                        int palette = 0; // FIXME: assume palette 0 for now 
                        cpalTable.GetColor(
                            cpalTable.Palettes[palette] + colrTable.GlyphPalettes[c], //index
                            out byte red, out byte green, out byte blue, out byte alpha);

                        g.TranslateTransform(cx, cy);
                        
                        
                        _fillBrush.Color = PdfSharpCore.Drawing.XColor.FromArgb(red, green, blue);//***
                        if (FillBackground)
                        {
                            // g.FillPath()
                            g.DrawPath(_fillBrush, path);
                        }
                        if (DrawOutline)
                        {
                            g.DrawPath(_outlinePen, path);
                        }

                        // and then we reset back
                        g.TranslateTransform(-cx, -cy);
                    } // Next c 
                }
                else
                {
                    PdfSharpCore.Drawing.XGraphicsPath path = GetExistingOrCreateGraphicsPath(glyphIndex);
                    
                    if (path == null)
                    {
                        //???
#if DEBUG
                        System.Diagnostics.Debug.WriteLine("gdi_printer: no path?");
#endif
                        continue;
                    }

                    //------
                    //then move pen point to the position we want to draw a glyph
                    float cx = (float)System.Math.Round(snapToPxScale.ExactX + x);
                    float cy = (float)System.Math.Floor(snapToPxScale.ExactY + baseline);

                    g.TranslateTransform(cx, cy);

                    if (FillBackground)
                    {
                        //g.FillPath(_fillBrush, path);
                        g.DrawPath(_fillBrush, path);
                    }
                    if (DrawOutline)
                    {
                        g.DrawPath(_outlinePen, path);
                    }
                    //and then we reset back ***
                    g.TranslateTransform(-cx, -cy);
                } // End else of if (canUseColorGlyph && colrTable.LayerIndices.TryGetValue(glyphIndex, out ushort colorLayerStart))

            } // Whend 

        } // End Sub DrawFromGlyphPlans 


        public override void DrawString(char[] textBuffer, int startAt, int len, float x, float y)
        {
            _reusableUnscaledGlyphPlanList.Clear();
            //1. unscale layout, in design unit
            _glyphLayout.Layout(textBuffer, startAt, len);
            _glyphLayout.GenerateUnscaledGlyphPlans(_reusableUnscaledGlyphPlanList);


            //draw from the glyph plan seq
            DrawFromGlyphPlans(new GlyphPlanSequence(_reusableUnscaledGlyphPlanList), x, y);
        } // End Sub DrawString 


    } // End Class DevGdiTextPrinter 


} // End Namespace SampleWinForms
