using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mohammad.Globalization
{
    public class PersianAlphabetConverter
    {
        private static readonly List<byte> _IgnoreList = new List<byte>();
        private static readonly Dictionary<byte, PersianCharacter> _AlphabetList = new Dictionary<byte, PersianCharacter>();
        private static readonly Dictionary<byte, PersianCharacter> _NumberList = new Dictionary<byte, PersianCharacter>();

        static PersianAlphabetConverter()
        {
            InitIgnores();
            InitiAlphabets();
            InitiNumbers();
        }

        private static void InitIgnores()
        {
            _IgnoreList.Add(193); //Á
            _IgnoreList.Add(240); //Çð
            _IgnoreList.Add(241); //Çñ
            _IgnoreList.Add(242); //Çò
            _IgnoreList.Add(243); //Çó
            _IgnoreList.Add(245); //Çõ
            _IgnoreList.Add(246); //Çö
            _IgnoreList.Add(248); //Çø
        }

        private static void InitiAlphabets()
        {
            AddAlphabet(new PersianCharacter(32, 32, 32, 32, 32, false, false)); //space
            AddAlphabet(new PersianCharacter(194, 141, 141, 141, 141, false, false)); //Â
            AddAlphabet(new PersianCharacter(199, 144, 144, 145, 145, false, true)); //ÇáÝ
            AddAlphabet(new PersianCharacter(195, 144, 144, 145, 145, false, true)); //Ã
            AddAlphabet(new PersianCharacter(197, 144, 144, 145, 145, false, true)); //Å
            AddAlphabet(new PersianCharacter(200, 146, 147, 147, 146, true, true)); //È
            AddAlphabet(new PersianCharacter(129, 148, 149, 149, 148, true, true)); //
            AddAlphabet(new PersianCharacter(202, 150, 151, 151, 150, true, true)); //Ê
            AddAlphabet(new PersianCharacter(203, 152, 153, 153, 152, true, true)); //Ë
            AddAlphabet(new PersianCharacter(204, 154, 155, 155, 154, true, true)); //Ì
            AddAlphabet(new PersianCharacter(141, 156, 157, 157, 156, true, true)); //
            AddAlphabet(new PersianCharacter(205, 158, 159, 159, 158, true, true)); //Í
            AddAlphabet(new PersianCharacter(206, 160, 161, 161, 160, true, true)); //Î
            AddAlphabet(new PersianCharacter(207, 162, 162, 162, 162, false, true)); //Ï
            AddAlphabet(new PersianCharacter(208, 163, 163, 163, 163, false, true)); //Ð
            AddAlphabet(new PersianCharacter(209, 164, 164, 164, 164, false, true)); //Ñ
            AddAlphabet(new PersianCharacter(210, 165, 165, 165, 165, false, true)); //Ò
            AddAlphabet(new PersianCharacter(142, 166, 166, 166, 166, false, true)); //Ò
            AddAlphabet(new PersianCharacter(211, 167, 168, 168, 167, true, true)); //Ó
            AddAlphabet(new PersianCharacter(212, 169, 170, 170, 169, true, true)); //Ô
            AddAlphabet(new PersianCharacter(213, 171, 172, 172, 171, true, true)); //Õ
            AddAlphabet(new PersianCharacter(214, 173, 174, 174, 173, true, true)); //Ö
            AddAlphabet(new PersianCharacter(216, 175, 175, 175, 175, true, true)); //Ø
            AddAlphabet(new PersianCharacter(217, 224, 224, 224, 224, true, true)); //Ù
            AddAlphabet(new PersianCharacter(218, 225, 228, 227, 226, true, true)); //Ú
            AddAlphabet(new PersianCharacter(219, 229, 232, 231, 230, true, true)); //Û
            AddAlphabet(new PersianCharacter(221, 233, 234, 234, 233, true, true)); //Ý
            AddAlphabet(new PersianCharacter(222, 235, 236, 236, 235, true, true)); //Þ
            AddAlphabet(new PersianCharacter(223, 237, 238, 238, 237, true, true)); //˜
            AddAlphabet(new PersianCharacter(144, 239, 240, 240, 239, true, true)); //
            AddAlphabet(new PersianCharacter(225, 241, 243, 243, 241, true, true)); //á
            AddAlphabet(new PersianCharacter(227, 244, 245, 245, 244, true, true)); //ã
            AddAlphabet(new PersianCharacter(228, 246, 247, 247, 246, true, true)); //ä
            AddAlphabet(new PersianCharacter(230, 248, 248, 248, 248, false, true)); //æ
            AddAlphabet(new PersianCharacter(196, 248, 248, 248, 248, false, true)); //Ä
            AddAlphabet(new PersianCharacter(229, 249, 251, 250, 249, true, true)); //å
            AddAlphabet(new PersianCharacter(201, 249, 251, 250, 249, true, true)); //É
            AddAlphabet(new PersianCharacter(236, 252, 254, 254, 252, true, true)); //í
            AddAlphabet(new PersianCharacter(237, 252, 254, 254, 252, true, true)); //í
            AddAlphabet(new PersianCharacter(198, 252, 254, 254, 252, true, true)); //í ÈÇ åãÒå
        }

        private static void InitiNumbers()
        {
            const byte zero1256 = 48;
            byte diff = 80;
            for (var i = zero1256; i < zero1256 + 10; i++)
            {
                var newCode = System.Convert.ToByte(i + diff);
                _NumberList.Add(i, new PersianCharacter(i, newCode, newCode, newCode, newCode, false, false, false));
            }
        }

        private static void AddAlphabet(PersianCharacter c) { _AlphabetList.Add(c.Pc1256Code, c); }

        public static byte[] Convert(string input)
        {
            var pageCode1256 =
                Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(1256), Encoding.UTF8.GetBytes(input)).Where(b => !GetIgnoreCharacters().Contains(b)).ToList();
            var result = new List<ConvertedCharacter>();
            for (var i = 0; i < pageCode1256.Count; i++)
            {
                var currentPc1256Code = pageCode1256[i];
                var ch = FindChar(currentPc1256Code);
                var beforeChar = i > 0 ? FindChar(pageCode1256[i - 1]) : null;
                var afterChar = i < pageCode1256.Count - 1 ? FindChar(pageCode1256[i + 1]) : null;
                result.Add(ch != null ? ch.GetProperPrinterCode(beforeChar, afterChar) : new ConvertedCharacter(currentPc1256Code, false));
            }
            var reversedBytes = new List<byte>();
            if (result.Count > 0)
            {
                var i = result.Count - 1;
                var current = result[i--];
                while (i >= -1)
                {
                    var buffer = new List<byte>();
                    if (current.IsNeedToReverse)
                    {
                        while (current.IsNeedToReverse)
                        {
                            buffer.Add(current.ByteCode);
                            if (i < 0)
                            {
                                i--;
                                break;
                            }
                            current = result[i--];
                        }
                        reversedBytes.AddRange(buffer);
                    }
                    else
                    {
                        while (!current.IsNeedToReverse)
                        {
                            buffer.Add(current.ByteCode);
                            if (i < 0)
                            {
                                i--;
                                break;
                            }
                            current = result[i--];
                        }
                        for (var k = buffer.Count - 1; k >= 0; k--)
                            reversedBytes.Add(buffer[k]);
                    }
                }
            }
            return reversedBytes.ToArray();
        }

        private static IEnumerable<byte> GetIgnoreCharacters() { return _IgnoreList; }

        private static PersianCharacter FindChar(byte pc1256Code)
        {
            if (_AlphabetList.ContainsKey(pc1256Code))
                return _AlphabetList[pc1256Code];
            if (_NumberList.ContainsKey(pc1256Code))
                return _NumberList[pc1256Code];
            return null;
        }

        private class PersianCharacter
        {
            private readonly bool _IsNeedToReverse;
            private readonly bool _IsStickyToBeginOfNextChar;
            private readonly bool _IsStickyToEndOfPreviousChar;
            private readonly byte _PrinterBeginCode;
            private readonly byte _PrinterEndCode;
            private readonly byte _PrinterIsolatedCode;
            private readonly byte _PrinterMiddleCode;
            public readonly byte Pc1256Code;

            public PersianCharacter(byte pC1256Code, byte isolatedCode, byte beginCode, byte middleCode, byte endCode, bool isStickyToBegin, bool isStickyToEnd,
                bool isNeedToReverse = true)
            {
                this.Pc1256Code = pC1256Code;
                this._PrinterIsolatedCode = isolatedCode;
                this._PrinterBeginCode = beginCode;
                this._PrinterMiddleCode = middleCode;
                this._PrinterEndCode = endCode;
                this._IsStickyToBeginOfNextChar = isStickyToBegin;
                this._IsStickyToEndOfPreviousChar = isStickyToEnd;
                this._IsNeedToReverse = isNeedToReverse;
            }

            public ConvertedCharacter GetProperPrinterCode(PersianCharacter beforeChar, PersianCharacter afterChar)
            {
                byte properByteCode;
                var isStickyToPreviousChar = beforeChar != null && beforeChar._IsStickyToBeginOfNextChar;
                var isStickyToNextChar = afterChar != null && afterChar._IsStickyToEndOfPreviousChar;
                if (isStickyToPreviousChar && isStickyToNextChar) //Middle
                    properByteCode = this._PrinterMiddleCode;
                else if (!isStickyToPreviousChar && !isStickyToNextChar) //Isolated
                    properByteCode = this._PrinterIsolatedCode;
                else if (isStickyToNextChar) //Begin
                    properByteCode = this._PrinterBeginCode;
                else //End
                    properByteCode = this._PrinterEndCode;
                return new ConvertedCharacter(properByteCode, this._IsNeedToReverse);
            }
        }

        private class ConvertedCharacter
        {
            public byte ByteCode { get; }
            public bool IsNeedToReverse { get; }

            public ConvertedCharacter(byte code, bool isNeedToReverse)
            {
                this.ByteCode = code;
                this.IsNeedToReverse = isNeedToReverse;
            }
        }
    }
}