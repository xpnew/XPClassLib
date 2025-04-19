using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XP.Text.Tags.TMTags;

namespace XP.Text.Tags.Utils
{
    public static class LoopTagUtil
    {


        public static List<TMLoop> FindLoops(string input)
        {

            List<TMLoop> Result = new List<TMLoop>();


            //var LoopReg = new Regex("\\{Loop:([\\w-]+)}", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            //var m = LoopReg.Match(input);
            var LoopTagPattern = "\\{Loop:([\\w-]+)}";
            var m = Regex.Match(input, LoopTagPattern,
                   RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.Compiled);
            var Tm1 = "\\{Loop:LoopName}(.+)\\{/Loop:LoopName}";
            while (m.Success)
            {
                Console.WriteLine("Find loop tag: " + m.Groups[1] + " at "
                   + m.Groups[1].Index);
                //x.Say("Found href " + m.Groups[1] + " at " + m.Groups[1].Index);
                string SubLoopPartPattern = Tm1.Replace("LoopName", m.Groups[1].Value);
                var Match1 = Regex.Match(input, SubLoopPartPattern,
                   RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.Compiled);
                if (Match1.Success)
                {
                    TMLoop NewTag = new TMLoop()
                    {
                        TagName = m.Groups[1].Value,
                        TagContent = Match1.Groups[1].Value,
                        NoteText = Match1.Groups[0].Value,
                    };
                    Result.Add(NewTag);
                }
                m = m.NextMatch();
            }
            return Result;
        }
    }
}
