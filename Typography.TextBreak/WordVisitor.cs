﻿//MIT, 2016-present, WinterDev
// some code from icu-project
// © 2016 and later: Unicode, Inc. and others.
// License & terms of use: http://www.unicode.org/copyright.html#License

using System;
using System.Collections.Generic;

namespace Typography.TextBreak
{

    public enum VisitorState
    {
        Init,
        Parsing,
        OutOfRangeChar,
        End,
    }



    public delegate void NewWordBreakHandlerDelegate(WordVisitor vistor);
    //
    public class DelegateBaseWordVisitor : WordVisitor
    {
        readonly NewWordBreakHandlerDelegate _newWordBreakHandler;
        internal DelegateBaseWordVisitor(NewWordBreakHandlerDelegate newWordBreakHandler)
        {
            _newWordBreakHandler = newWordBreakHandler;
        }
        protected override void OnBreak()
        {
            _newWordBreakHandler(this);
        }
    }

    public abstract class WordVisitor
    {

        //#if DEBUG
        //        List<BreakAtInfo> dbugBreakAtList = new List<BreakAtInfo>();
        //        bool dbugCollectBreakAtList;
        //#endif
        char[] _buffer;

        int _startIndex;
        int _endIndex;

        int _currentIndex;
        char _currentChar;
        int _latestBreakAt;

        Stack<int> _tempCandidateBreaks = new Stack<int>();


        public SpanBreakInfo SpanBreakInfo { get; set; }
        internal void LoadText(char[] buffer, int index)
        {
            LoadText(buffer, index, buffer.Length);
        }
        internal void LoadText(char[] buffer, int index, int len)
        {
            //check index < buffer

            //reset all
            _buffer = buffer;
            _endIndex = index + len;

            _startIndex = _currentIndex = index;
            LatestSpanStartAt = _startIndex;

            _currentChar = buffer[_currentIndex];


            _tempCandidateBreaks.Clear();
            _latestBreakAt = 0;

            //#if DEBUG
            //            dbugBreakAtList.Clear();
            //#endif
        }
        protected virtual void OnBreak() { }

        public VisitorState State { get; internal set; }
        //
        public int CurrentIndex => _currentIndex;
        //
        public char Char => _currentChar;
        //
        public bool IsEnd => _currentIndex >= _endIndex;

        public string CopyCurrentSpanString()
        {
            return new string(_buffer, LatestSpanStartAt, LatestSpanLen);
        }

#if DEBUG
        //int dbugAddSteps;
#endif

        internal void AddWordBreakAt(int index, WordKind wordKind)
        {

#if DEBUG
            //dbugAddSteps++;
            //if (dbugAddSteps >= 57)
            //{

            //}
            if (index == _latestBreakAt)
            {
                throw new NotSupportedException();
            }
#endif

            LatestSpanLen = (ushort)(index - LatestBreakAt);
            LatestSpanStartAt = _latestBreakAt;
            LatestWordKind = wordKind;

            _latestBreakAt = index;//**

            OnBreak();

            //#if DEBUG
            //            if (dbugCollectBreakAtList)
            //            {
            //                dbugBreakAtList.Add(new BreakAtInfo(index, wordKind));
            //            }

            //#endif
        }
        internal void AddWordBreakAtCurrentIndex(WordKind wordKind = WordKind.Text)
        {
            AddWordBreakAt(this.CurrentIndex, wordKind);
        }
        internal void AddWordBreak_AndSetCurrentIndex(int index, WordKind wordKind)
        {
            AddWordBreakAt(index, wordKind);
            SetCurrentIndex(LatestBreakAt);
        }
        //
        public int LatestSpanStartAt { get; private set; }
        public int LatestBreakAt => _latestBreakAt;
        public WordKind LatestWordKind { get; private set; }
        public ushort LatestSpanLen { get; private set; }
        //

        internal void SetCurrentIndex(int index)
        {
            _currentIndex = index;
            if (index < _endIndex)
            {
                _currentChar = _buffer[index];
            }
            else
            {
                //can't read next
                //the set state= end
                this.State = VisitorState.End;
            }
        }
        internal Stack<int> GetTempCandidateBreaks() => _tempCandidateBreaks;
    }



}