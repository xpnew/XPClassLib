using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XP.Text.Tags
{

    /// <summary>
    /// 标签查找
    /// </summary>
    public class TagFinder<TEntity> where TEntity : BaseTag, new()
    {
        public string InputText { get; set; }
        public List<TEntity> Result { get; set; }

        public TagFinder()
            : this(null)
        {


        }
        public TagFinder(string input)
        {
            InputText = input;

            _Init();

        }
        protected void _Init()
        {
            Result = new List<TEntity>();
        }



        public void Analyze()
        {
            TEntity DefaultTag = new TEntity();

            //\{Loop:([^}]+)}
            string RegParten = DefaultTag.PrefixChar + DefaultTag.TagPrefix + ":([^}]+)" + DefaultTag.SuffixChar;

            if ('{' == DefaultTag.PrefixChar[0])
            {
                RegParten = "\\" + RegParten;
            }
            var m = Regex.Match(InputText, RegParten,
       RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.Compiled);

            while (m.Success)
            {
                

            }


        }



    }
}
