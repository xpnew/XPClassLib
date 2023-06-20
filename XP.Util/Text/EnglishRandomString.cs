using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util.Text
{
    public class EnglishRandomString : RandomStringByRangBase
    {

        public bool CharSmallLettersEnable { get; set; }
        public bool CharCapitalLettersEnable { get; set; }
        public bool CharNumberEnable { get; set; }
        public string CharPunctuation { get; set; }


        public EnglishRandomString():base()
        {
        }

        public EnglishRandomString(bool enableNumber, bool enableCapital, bool enableSmall, string punctuation): base()
        {
            CharNumberEnable = enableNumber;
            CharCapitalLettersEnable = enableCapital;
            CharSmallLettersEnable = enableSmall;
            CharPunctuation = punctuation;
            _InitRange();
        }


        protected override void _InitRange()
        {
            //base._InitRange();
            StringBuilder sb = new StringBuilder();

            int idx = 0;
            if (CharNumberEnable)
            {
                idx = 0;
                char[] Chars = new char[10];
                for (int k = 48; k <= 57; k++)
                {
                    sb.Append((char)k);
                }

            }
            if (CharCapitalLettersEnable)
            {
                for (int k = 65; k <= 90; k++)
                {
                    sb.Append((char)k);
                }
            }
            if (CharSmallLettersEnable)
            {
                for (int k = 97; k <= 122; k++)
                {
                    sb.Append((char)k);
                }
            }
            sb.Append(CharPunctuation);
            Range = sb.ToString();
        }
    }
}
