﻿//MIT, 2016-present, WinterDev
// some code from icu-project
// © 2016 and later: Unicode, Inc. and others.
// License & terms of use: http://www.unicode.org/copyright.html#License



using System;
using Typography.OpenFont;

namespace Typography.TextBreak
{
    public enum SurrogatePairBreakingOption
    {
        OnlySurrogatePair,
        ConsecutiveSurrogatePairs,
        ConsecutiveSurrogatePairsAndJoiner
    }
    public class EngBreakingEngine : BreakingEngine
    {
        enum LexState
        {
            Init,
            Whitespace,
            Text,
            Number,
            CollectSurrogatePair,
            CollectConsecutiveUnicode,
        }

        public bool BreakNumberAfterText { get; set; }
        public bool BreakPeroidInTextSpan { get; set; }
        public bool EnableCustomAbbrv { get; set; }
        public bool EnableUnicodeRangeBreaker { get; set; }


        public bool IncludeLatinExtended { get; set; } = true;


        public SurrogatePairBreakingOption SurrogatePairBreakingOption { get; set; } = SurrogatePairBreakingOption.ConsecutiveSurrogatePairsAndJoiner;

        public CustomAbbrvDic EngCustomAbbrvDic { get; set; }
        struct BreakBounds
        {
            public int startIndex;
            public int length;
            public WordKind kind;
        }

        static readonly SpanBreakInfo s_latin = new SpanBreakInfo(false, ScriptTagDefs.Latin.Tag);
        static readonly SpanBreakInfo s_unknown = new SpanBreakInfo(false, ScriptTagDefs.Latin.Tag);

        public EngBreakingEngine()
        {

        }
        internal override void BreakWord(WordVisitor visitor, char[] charBuff, int startAt, int len)
        {
            visitor.State = VisitorState.Parsing;
            visitor.SpanBreakInfo = s_latin;

            DoBreak(visitor, charBuff, startAt, len);

        }
        public override bool CanHandle(char c)
        {
            //this is basic eng + surrogate-pair( eg. emoji)
            return (c <= 255) ||
                   char.IsHighSurrogate(c) ||
                   char.IsPunctuation(c) ||
                   char.IsWhiteSpace(c) ||
                   char.IsControl(c) ||
                   char.IsSymbol(c) ||
                   (IncludeLatinExtended && (IsLatinExtendedA(c) || IsLatinExtendedB(c)));
        }
        static bool IsLatinExtendedA(char c) => c >= 0x100 & c <= 0x017F;
        static bool IsLatinExtendedB(char c) => c >= 0x0180 & c <= 0x024F;
        //
        public override bool CanBeStartChar(char c) => true;
        //

        static void OnBreak(WordVisitor vis, in BreakBounds bb) => vis.AddWordBreak_AndSetCurrentIndex(bb.startIndex + bb.length, bb.kind);


        static void CollectConsecutiveUnicodeRange(char[] input, ref int start, int len, out SpanBreakInfo spanBreakInfo)
        {

            char c1 = input[start];
            if (UnicodeRangeFinder.GetUniCodeRangeFor(c1, out int startCodePoint, out int endCodePoint, out spanBreakInfo))
            {
                int lim = start + len;
                for (int i = start; i < lim; ++i)
                {
                    c1 = input[i];
                    if (c1 < startCodePoint || c1 > endCodePoint)
                    {
                        //out of range again
                        //break here
                        start = i;
                        return;
                    }
                }
                start = lim;
            }
            else
            {
                //for unknown,
                //just collect until turn back to latin
#if DEBUG
                System.Diagnostics.Debug.WriteLine("unknown unicode range:");
#endif


                int lim = start + len;
                for (int i = start; i < lim; ++i)
                {
                    c1 = input[i];
                    if ((c1 >= 0 && c1 < 256) || //eng range
                        char.IsHighSurrogate(c1) || //surrogate pair
                        UnicodeRangeFinder.GetUniCodeRangeFor(c1, out startCodePoint, out endCodePoint, out spanBreakInfo)) //or found some wellknown range
                    {
                        //break here
                        start = i;
                        return;
                    }
                }

                start = lim;
                spanBreakInfo = s_unknown;
            }
        }
        static void CollectConsecutiveSurrogatePairs(char[] input, ref int start, int len, bool withZeroWidthJoiner)
        {

            int lim = start + len;
            for (int i = start; i < lim;) //start+1
            {
                char c = input[i];

                if ((i + 1 < lim) &&
                    char.IsHighSurrogate(c) &&
                    char.IsLowSurrogate(input[i + 1]))
                {
                    i += 2;//**
                    start = i;
                }
                else if (withZeroWidthJoiner && c == 8205)
                {
                    //https://en.wikipedia.org/wiki/Zero-width_joiner
                    i += 1;
                    start = i;
                }
                else
                {
                    //stop
                    start = i;
                    return;
                }
            }

        }

        const char FIRST_CHAR = (char)0;
        const char LAST_CHAR = (char)255;
        bool IsInOurLetterRange(char c) => (c >= FIRST_CHAR && c <= LAST_CHAR) || (IncludeLatinExtended && (IsLatinExtendedA(c) || IsLatinExtendedB(c)));


        void DoBreak(WordVisitor visitor, char[] input, int start, int len)
        {

            //----------------------------------------
            //simple break word/ num/ punc / space
            //similar to lexer function            
            //----------------------------------------
            int endBefore = start + len;
            if (endBefore > input.Length)
                throw new System.ArgumentOutOfRangeException(nameof(len), len, "The range provided was partially out of bounds.");
            else if (start < 0)
                throw new System.ArgumentOutOfRangeException(nameof(start), start, "The starting index was negative.");
            //throw instead of skipping the entire for loop
            else if (len < 0)
                throw new System.ArgumentOutOfRangeException(nameof(len), len, "The length provided was negative.");
            //----------------------------------------

            LexState lexState = LexState.Init;
            BreakBounds bb = new BreakBounds();
            bb.startIndex = start;

            bool enableUnicodeRangeBreaker = EnableUnicodeRangeBreaker;
            bool breakPeroidInTextSpan = BreakPeroidInTextSpan; 

            visitor.SpanBreakInfo = s_latin;
             
            for (int i = start; i < endBefore; ++i)
            {
                char c = input[i];
                switch (lexState)
                {
                    case LexState.Init:
                        {
                            //check char
                            if (c == '\r' && i < endBefore - 1 && input[i + 1] == '\n')
                            {
                                //this is '\r\n' linebreak
                                bb.startIndex = i;
                                bb.length = 2;
                                bb.kind = WordKind.NewLine;
                                //
                                OnBreak(visitor, bb);

                                //
                                bb.startIndex += 2;//***
                                bb.length = 0;
                                bb.kind = WordKind.Unknown;
                                lexState = LexState.Init;

                                i++;
                                continue;
                            }
                            else if (c == '\r' || c == '\n' || c == 0x85) //U+0085 NEXT LINE
                            {
                                bb.startIndex = i;
                                bb.length = 1;
                                bb.kind = WordKind.NewLine;
                                //
                                OnBreak(visitor, bb);
                                //
                                bb.length = 0;
                                bb.startIndex++;//***
                                bb.kind = WordKind.Unknown;
                                lexState = LexState.Init;
                                continue;
                            }
                            else if (char.IsLetter(c))
                            {
                                if (!IsInOurLetterRange(c))
                                {

                                    //letter is OUT_OF_RANGE

                                    if (i > bb.startIndex)
                                    {
                                        //flush
                                        bb.length = i - bb.startIndex;
                                        //
                                        OnBreak(visitor, bb);
                                        bb.startIndex += bb.length;//***
                                    }

                                    if (char.IsHighSurrogate(c))
                                    {
                                        lexState = LexState.CollectSurrogatePair;
                                        goto case LexState.CollectSurrogatePair;
                                    }

                                    if (enableUnicodeRangeBreaker)
                                    {
                                        //collect text until end for specific unicode range eng
                                        //find a proper unicode engine and collect until end of its range 
                                        lexState = LexState.CollectConsecutiveUnicode;
                                        goto case LexState.CollectConsecutiveUnicode;
                                    }
                                    else
                                    {
                                        visitor.State = VisitorState.OutOfRangeChar;
                                        return;
                                    }
                                }

                                //------------------
                                //just collect
                                bb.startIndex = i;
                                bb.kind = WordKind.Text;
                                lexState = LexState.Text;
                            }
                            else if (char.IsNumber(c))
                            {
                                bb.startIndex = i;
                                bb.kind = WordKind.Number;
                                lexState = LexState.Number;
                            }
                            else if (char.IsWhiteSpace(c))
                            {
                                //we collect whitespace
                                bb.startIndex = i;
                                bb.kind = WordKind.Whitespace;
                                lexState = LexState.Whitespace;
                            }
                            else if (char.IsPunctuation(c) || char.IsSymbol(c))
                            {

                                //for eng -
                                if (c == '-')
                                {
                                    //check next char is number or not
                                    if (i < endBefore - 1 &&
                                       char.IsNumber(input[i + 1]))
                                    {
                                        bb.startIndex = i;
                                        bb.kind = WordKind.Number;
                                        lexState = LexState.Number;
                                        continue;
                                    }
                                }

                                bb.startIndex = i;
                                bb.length = 1;
                                bb.kind = WordKind.Punc;

                                //we not collect punc
                                OnBreak(visitor, bb);
                                //
                                bb.startIndex += 1;
                                bb.length = 0;
                                bb.kind = WordKind.Unknown;
                                lexState = LexState.Init;
                                continue;
                            }
                            else if (char.IsControl(c))
                            {
                                bb.startIndex = i;
                                bb.length = 1;
                                bb.kind = WordKind.Control;
                                //
                                OnBreak(visitor, bb);
                                //
                                bb.length = 0;
                                bb.startIndex++;
                                lexState = LexState.Init;
                                continue;
                            }
                            else if (char.IsHighSurrogate(c))
                            {
                                lexState = LexState.CollectSurrogatePair;
                                goto case LexState.CollectSurrogatePair;
                            }
                            else if (!IsInOurLetterRange(c))
                            {
                                //letter is out-of-range or not 
                                //clear accum state
                                if (i > bb.startIndex)
                                {
                                    //some remaining data
                                    bb.length = i - bb.startIndex;
                                    //flush
                                    OnBreak(visitor, bb);
                                    //
                                    //
                                    //TODO: check if we should set startIndex and length
                                    //      like other 'after' onBreak()
                                }
                                if (char.IsHighSurrogate(c))
                                {
                                    lexState = LexState.CollectSurrogatePair;
                                    goto case LexState.CollectSurrogatePair;
                                }

                                if (enableUnicodeRangeBreaker)
                                {
                                    lexState = LexState.CollectConsecutiveUnicode;
                                    goto case LexState.CollectConsecutiveUnicode;
                                }
                                else
                                {
                                    visitor.State = VisitorState.OutOfRangeChar;
                                    return;
                                }
                            }
                            else
                            {
                                throw new System.NotSupportedException($"The character {c} (U+{((ushort)c).ToString("X4")}) was unhandled.");
                            }
                        }
                        break;
                    case LexState.Number:
                        {
                            //in number state
                            if (!char.IsNumber(c) && c != '.')
                            {
                                //if number then continue collect
                                //if not

                                //flush current state 
                                bb.length = i - bb.startIndex;
                                bb.kind = WordKind.Number;
                                //
                                OnBreak(visitor, bb);
                                //
                                bb.length = 0;
                                bb.startIndex = i;
                                bb.kind = WordKind.Unknown;
                                lexState = LexState.Init;
                                goto case LexState.Init;
                            }
                        }
                        break;
                    case LexState.Text:
                        {
                            bool is_number = char.IsNumber(c);
                            if (char.IsLetter(c) || is_number || (c == '.' && !breakPeroidInTextSpan))
                            {
                                //
                                //c may be out-of-range letter
                                //letter is out-of-range or not 
                                //clear accum state   

                                if (!IsInOurLetterRange(c))
                                {
                                    if (i > bb.startIndex)
                                    {
                                        //flush
                                        bb.length = i - bb.startIndex;
                                        bb.kind = WordKind.Text;
                                        //
                                        OnBreak(visitor, bb);
                                        //TODO: check if we should set startIndex and length
                                        //      like other 'after' onBreak()
                                    }

                                    if (char.IsHighSurrogate(c))
                                    {
                                        lexState = LexState.CollectSurrogatePair;
                                        goto case LexState.CollectSurrogatePair;
                                    }

                                    if (enableUnicodeRangeBreaker)
                                    {
                                        lexState = LexState.CollectConsecutiveUnicode;
                                        goto case LexState.CollectConsecutiveUnicode;
                                    }
                                    else
                                    {
                                        visitor.State = VisitorState.OutOfRangeChar;
                                        return;
                                    }
                                }

                                if (is_number && BreakNumberAfterText)
                                {
                                    //flush 
                                    bb.length = i - bb.startIndex;
                                    bb.kind = WordKind.Text;
                                    //
                                    OnBreak(visitor, bb);
                                    //
                                    bb.length = 1;
                                    bb.startIndex = i;
                                    lexState = LexState.Number;
                                }
                            }
                            else
                            {
                                //flush existing text ***
                                bb.length = i - bb.startIndex;
                                bb.kind = WordKind.Text;
                                //
                                OnBreak(visitor, bb);
                                //
                                bb.length = 0;
                                bb.startIndex = i;
                                bb.kind = WordKind.Unknown;
                                lexState = LexState.Init;
                                goto case LexState.Init;
                            }

                        }
                        break;
                    case LexState.Whitespace:
                        {
                            if (!char.IsWhiteSpace(c))
                            {
                                bb.length = i - bb.startIndex;
                                bb.kind = WordKind.Whitespace;
                                //
                                OnBreak(visitor, bb);
                                //
                                bb.length = 0;
                                bb.startIndex = i;
                                bb.kind = WordKind.Unknown;
                                lexState = LexState.Init;
                                goto case LexState.Init;
                            }
                        }
                        break;
                    case LexState.CollectSurrogatePair:
                        {

                            if (i < endBefore - 1 && //not the last one
                               char.IsLowSurrogate(input[i + 1]))
                            {
                                //surrogate pair 
                                //clear accum state
                                if (i > bb.startIndex)
                                {
                                    //some remaining data
                                    bb.length = i - bb.startIndex;
                                    //
                                    OnBreak(visitor, bb);
                                }
                                //-------------------------------
                                //surrogate pair

                                if (SurrogatePairBreakingOption == SurrogatePairBreakingOption.OnlySurrogatePair)
                                {
                                    bb.startIndex = i;
                                    bb.length = 2;
                                    bb.kind = WordKind.SurrogatePair;
                                    OnBreak(visitor, bb);
                                    i++;//consume next***
                                    bb.startIndex += 2;//reset 
                                }
                                else
                                {
                                    //see https://github.com/LayoutFarm/Typography/issues/18#issuecomment-345480185
                                    int begin = i + 2;
                                    CollectConsecutiveSurrogatePairs(input, ref begin, endBefore - begin, SurrogatePairBreakingOption == SurrogatePairBreakingOption.ConsecutiveSurrogatePairsAndJoiner);

                                    bb.startIndex = i;
                                    bb.length = begin - i;
                                    bb.kind = WordKind.SurrogatePair;
                                    OnBreak(visitor, bb);
                                    i += bb.length - 1;//consume 
                                    bb.startIndex += bb.length;//reset 
                                }
                                bb.length = 0; //reset
                                lexState = LexState.Init;
                                continue; //***
                            }
                            else
                            {
                                //error
                                throw new System.FormatException($"A high surrogate (U+{((ushort)c).ToString("X4")}) was not followed by a low surrogate.");
                            }
                        }
                    case LexState.CollectConsecutiveUnicode:
                        {
                            int begin = i;
                            bb.startIndex = i;
                            bb.kind = WordKind.Text;
                            CollectConsecutiveUnicodeRange(input, ref begin, len - i, out SpanBreakInfo spBreakInfo);
                            bb.length = begin - i;
                            if (bb.length > 0)
                            {
                                visitor.SpanBreakInfo = spBreakInfo;

                                OnBreak(visitor, bb); //flush

                                visitor.SpanBreakInfo = s_latin;//switch back
                                bb.length = 0;
                                bb.startIndex = begin;
                            }
                            else
                            {
                                throw new NotSupportedException();///???
                            }

                            i = begin - 1;
                            lexState = LexState.Init;
                        }
                        break;
                }
            }

            if (lexState != LexState.Init &&
                bb.startIndex < start + len)
            {
                //some remaining data

                bb.length = (start + len) - bb.startIndex;
                //
                OnBreak(visitor, bb);
                //
            }
            visitor.State = VisitorState.End;
        }
    }
}
