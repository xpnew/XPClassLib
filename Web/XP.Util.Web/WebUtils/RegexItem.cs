using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XP.Util.WebUtils
{
    public class RegexItem
    {
        public Regex Reg { get; set; }
        public string PlaceText { get; set; }

        public RegexItem(string pattern, string text, RegexOptions options)
        {
            Reg = new Regex(pattern, options);
            PlaceText = text;
        }

        public string Replace(string input)
        {
            return Reg.Replace(input, PlaceText);
        }
    }
}
