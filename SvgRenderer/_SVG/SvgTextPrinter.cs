﻿
using Typography.OpenFont;
using Typography.OpenFont.Tables;
using Typography.TextLayout;
using Typography.Contours;


namespace SvgRenderer
{


    class SvgTextPrinter
        : TextPrinterBase
    {


        protected Typeface _currentTypeface;
        protected GlyphOutlineBuilder _currentGlyphPathBuilder;
        protected GlyphTranslatorToSvgPath _txToGdiPath;
        protected GlyphLayout _glyphLayout = new GlyphLayout();
        protected SvgSolidBrush _fillBrush = new SvgSolidBrush(SvgColor.Black);
        protected SvgPen _outlinePen = new SvgPen(SvgColor.Green);

        //for optimization
        protected GlyphMeshCollection<SvgPath> _glyphMeshCollections = new GlyphMeshCollection<SvgPath>();
        protected UnscaledGlyphPlanList _reusableUnscaledGlyphPlanList = new UnscaledGlyphPlanList();


        public override GlyphLayout GlyphLayoutMan
        {
            get
            {
                return _glyphLayout;
            }
        } // End Property GlyphLayoutMan 


        public bool EnableColorGlyph { get; set; } = true;
        public string FillColor { get; set; }
        public string OutlineColor { get; set; }

        public SvgGraphics TargetGraphics { get; set; }



        public SvgTextPrinter()
        {
            FillBackground = true;
            FillColor = SvgColor.Black;
            OutlineColor = SvgColor.Green;
        } // End Constructor 


        public override Typeface Typeface
        {
            get
            {
                return _currentTypeface;
            }
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
                _txToGdiPath = new GlyphTranslatorToSvgPath();
                //4.
                OnFontSizeChanged();
            }

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
        } // End Function UpdateVisualOutputSettings 


        protected SvgPath GetExistingOrCreateGraphicsPath(ushort glyphIndex)
        {
            if (!_glyphMeshCollections.TryGetCacheGlyph(glyphIndex, out SvgPath path))
            {
                _txToGdiPath.Reset(); //clear

                //if not found then create a new one
                _currentGlyphPathBuilder.BuildFromGlyphIndex(glyphIndex, this.FontSizeInPoints, _txToGdiPath);
                path = _txToGdiPath.ResultGraphicsPath;

                //register
                _glyphMeshCollections.RegisterCachedGlyph(glyphIndex, path);
            }

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
            SvgGraphics g = this.TargetGraphics;
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

                        SvgPath path = GetExistingOrCreateGraphicsPath(colrTable.GlyphLayers[c]);
                        if (path == null)
                        {
#if DEBUG
                            System.Diagnostics.Debug.WriteLine("gdi_printer: no path?");
#endif
                            continue;
                        } // End if (path == null) 

                        //then move pen point to the position we want to draw a glyph
                        float cx = (float)System.Math.Round(snapToPxScale.ExactX + x);
                        float cy = (float)System.Math.Floor(snapToPxScale.ExactY + baseline);

                        int palette = 0; // FIXME: assume palette 0 for now 
                        cpalTable.GetColor(
                            cpalTable.Palettes[palette] + colrTable.GlyphPalettes[c], //index
                            out byte red, out byte green, out byte blue, out byte alpha);


                        g.TranslateTransform(cx, cy);

                        _fillBrush.Color = SvgColor.ToWebRgb(red, green, blue);

                        if (FillBackground)
                        {
                            g.FillPath(_fillBrush, path);
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
                    SvgPath path = GetExistingOrCreateGraphicsPath(glyphIndex);

                    if (path == null)
                    {
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
                        g.FillPath(_fillBrush, path);
                    }

                    if (DrawOutline)
                    {
                        g.DrawPath(_outlinePen, path);
                    }

                    // and then we reset back
                    g.TranslateTransform(-cx, -cy);
                } // End else

            } // Whend 

        } // End Sub DrawFromGlyphPlans 


        public override void DrawString(char[] textBuffer, int startAt, int len, float x, float y)
        {
            this.TargetGraphics.SetTextAndFont(new string(textBuffer), this.Typeface.Name, this.FontSizeInPoints);
            this.TargetGraphics.OpenGroup();

            _reusableUnscaledGlyphPlanList.Clear();
            //1. unscale layout, in design unit
            _glyphLayout.Layout(textBuffer, startAt, len);
            _glyphLayout.GenerateUnscaledGlyphPlans(_reusableUnscaledGlyphPlanList);

            //draw from the glyph plan seq
            DrawFromGlyphPlans(new GlyphPlanSequence(_reusableUnscaledGlyphPlanList), x, y);

            this.TargetGraphics.CloseGroup();
        } // End Sub DrawString 


    } // End Class SvgTextPrinter 


} // End Namespace SvgRenderer 
