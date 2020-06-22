using System;
using System.Collections.Generic;

namespace Mohammad.Helpers.Html
{
    public sealed class HtmlParser
    {
        private string _Html;
        private int _Pos;
        private bool _ScriptBegin;
        public bool Eof { get { return this._Pos >= this._Html.Length; } }
        public HtmlParser(string html) { this.Reset(html); }
        public void Reset() { this._Pos = 0; }

        public void Reset(string html)
        {
            this._Html = html;
            this._Pos = 0;
        }

        private bool ParseNext(string name, out HtmlTag tag)
        {
            tag = null;
            if (string.IsNullOrEmpty(name))
                return false;

            while (this.MoveToNextTag())
            {
                this.Move();

                var c = this.Peek();
                if (c == '!' && this.Peek(1) == '-' && this.Peek(2) == '-')
                {
                    const string endComment = "-->";
                    this._Pos = this._Html.IndexOf(endComment, this._Pos);
                    this.NormalizePosition();
                    this.Move(endComment.Length);
                }
                else if (c == '/')
                {
                    this._Pos = this._Html.IndexOf('>', this._Pos);
                    this.NormalizePosition();
                    this.Move();
                }
                else
                {
                    var result = this.ParseTag(name, ref tag);

                    if (this._ScriptBegin)
                    {
                        const string endScript = "</script";
                        this._Pos = this._Html.IndexOf(endScript, this._Pos, StringComparison.OrdinalIgnoreCase);
                        this.NormalizePosition();
                        this.Move(endScript.Length);
                        this.SkipWhitespace();
                        if (this.Peek() == '>')
                            this.Move();
                    }

                    if (result)
                        return true;
                }
            }
            return false;
        }

        private bool ParseTag(string name, ref HtmlTag tag)
        {
            var s = this.ParseTagName();

            var doctype = this._ScriptBegin = false;
            if (string.Compare(s, "!DOCTYPE", StringComparison.OrdinalIgnoreCase) == 0)
                doctype = true;
            else if (string.Compare(s, "script", StringComparison.OrdinalIgnoreCase) == 0)
                this._ScriptBegin = true;

            var requested = false;
            if (name == "*" || string.Compare(s, name, StringComparison.OrdinalIgnoreCase) == 0)
            {
                tag = new HtmlTag(s);
                requested = true;
            }

            this.SkipWhitespace();
            while (this.Peek() != '>')
                if (this.Peek() == '/')
                {
                    if (requested)
                        tag.TrailingSlash = true;
                    this.Move();
                    this.SkipWhitespace();
                    this._ScriptBegin = false;
                }
                else
                {
                    s = !doctype ? this.ParseAttributeName() : this.ParseAttributeValue();
                    this.SkipWhitespace();
                    var value = string.Empty;
                    if (this.Peek() == '=')
                    {
                        this.Move();
                        this.SkipWhitespace();
                        value = this.ParseAttributeValue();
                        this.SkipWhitespace();
                    }
                    if (requested)
                    {
                        if (tag.Contains(s))
                            tag.Remove(s);
                        tag.Add(s, value);
                    }
                }
            this.Move();

            return requested;
        }

        private string ParseTagName()
        {
            var start = this._Pos;
            while (!this.Eof && !char.IsWhiteSpace(this.Peek()) && this.Peek() != '>')
                this.Move();
            return this._Html.Substring(start, this._Pos - start);
        }

        private string ParseAttributeName()
        {
            var start = this._Pos;
            while (!this.Eof && !char.IsWhiteSpace(this.Peek()) && this.Peek() != '>' && this.Peek() != '=')
                this.Move();
            return this._Html.Substring(start, this._Pos - start);
        }

        private string ParseAttributeValue()
        {
            int start, end;
            var c = this.Peek();
            if (c == '"' || c == '\'')
            {
                this.Move();
                start = this._Pos;
                this._Pos = this._Html.IndexOfAny(new[] {c, '\r', '\n'}, start);
                this.NormalizePosition();
                end = this._Pos;
                if (this.Peek() == c)
                    this.Move();
            }
            else
            {
                start = this._Pos;
                while (!this.Eof && !char.IsWhiteSpace(c) && c != '>')
                {
                    this.Move();
                    c = this.Peek();
                }
                end = this._Pos;
            }
            return this._Html.Substring(start, end - start);
        }

        private bool MoveToNextTag()
        {
            this._Pos = this._Html.IndexOf('<', this._Pos);
            this.NormalizePosition();
            return !this.Eof;
        }

        public char Peek() { return this.Peek(0); }

        public char Peek(int ahead)
        {
            var pos = this._Pos + ahead;
            if (pos < this._Html.Length)
                return this._Html[pos];
            return (char) 0;
        }

        private void Move(int ahead = 1) { this._Pos = Math.Min(this._Pos + ahead, this._Html.Length); }

        private void SkipWhitespace()
        {
            while (!this.Eof && char.IsWhiteSpace(this.Peek()))
                this.Move();
        }

        private void NormalizePosition()
        {
            if (this._Pos < 0)
                this._Pos = this._Html.Length;
        }

        public IEnumerable<dynamic> GetTagsByName(string tagName)
        {
            HtmlTag tag;
            while (this.ParseNext(tagName, out tag))
                yield return tag;
            this.Reset();
        }
    }
}