using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Text.Tags
{

    /// <summary>
    /// 属性分析器
    /// </summary>
    /// <remarks>
    /// 注意：{Loop:ColumnsDict  propertyname=""  comparevalue="String"} 
    /// 这种 需要先把Loop 作为标签前缀、 ColumnsDict 作为标签名先给挑选出来
    /// 
    /// </remarks>
    public class AttributesFinder
    {
        /// <summary>
        /// 边界符号,一般情况下就是单双引号（关于值）
        /// </summary>
        public List<char> BorderChars { get; set; }
        /// <summary>
        /// 空格符，包括：空格 tab(\t) 换行（\n \r）
        /// </summary>
        public List<char> SpaceChars { get; set; }

        public string InputText { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public List<TagAttribute> ResultAttrs { get; set; }

        public List<AttributLocations> LocationsList { get; set; }



        /// <summary>
        /// 当前游标
        /// </summary>
        private int _WorkCharIndex = 0;

        public AttributesFinder()
            : this(null)
        {

        }

        public AttributesFinder(string input)
        {
            InputText = input;
            if (!String.IsNullOrEmpty(InputText))
            {
                InputText = InputText.Trim();
            }
            ResultAttrs = new List<TagAttribute>();
            LocationsList = new List<AttributLocations>();
            BorderChars = new List<char>() { '\'', '\"' };
            SpaceChars = new List<char>() { ' ', '\t', '\r', '\n' };
        }


        public void Analizy()
        {

            if (String.IsNullOrEmpty(InputText))
            {
                return;
            }


        }


        public TagAttribute Next()
        {
            TagAttribute Result = new TagAttribute();

            Result.Type = AttributeType.NONE;
            AttributLocations Location = new AttributLocations();
            Result.Location = Location;

            Location.Start = -1;
            bool NotFindOther = false;
            bool NotEnd = false;
            bool FindName = false;
            bool FindBorder = false;

            int BorderIndex = -1;

            #region 处理属名称
            // 先找到属性的开始
            while (_WorkCharIndex < InputText.Length)
            {
                char chr = InputText[_WorkCharIndex];
                if (SpaceChars.Contains(chr))
                {

                }
                else
                {
                    if ('=' == chr)
                    {
                        return FindErrorAttr();
                    }
                    else if (BorderChars.Contains(chr))
                    {
                        return FindErrorAttr();
                    }
                    else
                    {
                        FindName = true;
                        Location.Start = _WorkCharIndex;
                        Location.NameStart = _WorkCharIndex;
                        break;
                    }
                }
                _WorkCharIndex++;
            }

            _WorkCharIndex++;
            //找到属性名称的结束 
            while (_WorkCharIndex < InputText.Length)
            {
                char chr = InputText[_WorkCharIndex];
                if (SpaceChars.Contains(chr) || '=' == chr || BorderChars.Contains(chr))
                {
                    Location.NameEnd = _WorkCharIndex - 1;
                    break;
                }
                _WorkCharIndex++;
            }
            //没找到属性的结束，也就是这是一个单属性，并且是最后一个
            if (0 == Location.NameEnd)
            {
                Location.ValueStart = Location.NameStart;
                Location.End = Location.ValueEnd = Location.NameEnd = InputText.Length - 1;

                Result.Name = InputText.Substring(Location.NameStart, Location.NameEnd - Location.NameStart +1 );
                Result.Value = InputText.Substring(Location.NameStart, Location.NameEnd - Location.NameStart+1);
                Result.Type = AttributeType.Single;
                return Result;
            }
            Result.Name = InputText.Substring(Location.NameStart, Location.NameEnd - Location.NameStart + 1);

            #endregion
            //寻找属性的值
            while (_WorkCharIndex < InputText.Length)
            {
                char chr = InputText[_WorkCharIndex];
                if (SpaceChars.Contains(chr))
                {

                }
                else
                {
                    if ('=' == chr)
                    {
                        BorderIndex = FindBorderIndex();
                        break;
                    }
                    else if (BorderChars.Contains(chr))
                    {
                        BorderIndex = _WorkCharIndex;
                        break;
                    }
                    else // 单属性
                    {
                        Location.End = Location.ValueEnd = Location.NameEnd;
                        Result.Value = InputText.Substring(Location.NameStart, Location.NameEnd - Location.NameStart +1);
                        Result.Type = AttributeType.Single;
                        return Result;
                    }
                }
                _WorkCharIndex++;
            }

            if (-1 == BorderIndex)
            {
                Location.End = _WorkCharIndex;
                Result.Value = String.Empty;
                Result.Type = AttributeType.Error;
                _WorkCharIndex++;
                return Result;
            }
            _WorkCharIndex++;
            int NextBorderIndex = FindNextBorderChar(InputText[BorderIndex]);

            if (-1 == NextBorderIndex)
            {
                Location.End = BorderIndex;
                Result.Value = String.Empty;
                Result.Type = AttributeType.Error;
                return Result;
            }
            Location.End =  NextBorderIndex;
            Location.ValueEnd = NextBorderIndex - 1;
            Location.ValueStart = BorderIndex + 1;
            Result.Value = InputText.Substring(Location.ValueStart, Location.ValueEnd - Location.ValueStart +1);
            Result.Type = AttributeType.Normal;
            return Result;
        }
        private int FindBorderIndex()
        {
            int Result = -1;
            _WorkCharIndex++;
            while (_WorkCharIndex < InputText.Length)
            {
                char chr = InputText[_WorkCharIndex];
                if (SpaceChars.Contains(chr))
                {
                    _WorkCharIndex++;
                    continue;
                }
                //找到了 引号
                if (BorderChars.Contains(chr))
                {
                    Result = _WorkCharIndex;
                    break;
                }
                else
                {
                    break;
                }
            }
            return Result;
        }

        private int FindNextBorderChar(char borderChr)
        {
            int Result = -1;
            while (_WorkCharIndex < InputText.Length)
            {
                char chr = InputText[_WorkCharIndex];
                //找到了 引号
                if (chr == borderChr)
                {
                    char PrevChr = InputText[_WorkCharIndex - 1];
                    if ('\\' == PrevChr)
                    {
                        continue;
                    }
                    Result = _WorkCharIndex;
                    _WorkCharIndex++;
                    break;
                }
                _WorkCharIndex++;
            }
            return Result;
        }

        /// <summary>
        /// 从当前位置开始查找一个格式错误的属性（本来已经出现属性名的地方，出现了=号或者引号）
        /// </summary>
        /// <returns></returns>
        private TagAttribute FindErrorAttr()
        {
            TagAttribute Result = new TagAttribute();
            Result.Name = String.Empty;
            Result.Value = String.Empty;
            Result.Type = AttributeType.Error;
            AttributLocations Location = new AttributLocations();
            Result.Location = Location;

            Location.Start = _WorkCharIndex;
            Location.NameStart = _WorkCharIndex;
            Location.ValueStart = _WorkCharIndex;

            char StartChr = InputText[_WorkCharIndex];

            int FirstBorderIndex = _WorkCharIndex;
            int LastBorderIndex = -1;
            if ('=' == StartChr)
            {
                _WorkCharIndex++;
                while (_WorkCharIndex < InputText.Length)
                {
                    char chr = InputText[_WorkCharIndex];
                    if (SpaceChars.Contains(chr))
                    {

                    }
                    else
                    {
                        //找到了 引号
                        if (BorderChars.Contains(chr))
                        {
                            FirstBorderIndex = _WorkCharIndex;
                            break;
                        }
                        else // 找到了其它的字符,也就是说这个错误的属性是只有一个意外的=号
                        {
                            _WorkCharIndex--;
                            Location.End = _WorkCharIndex;
                            Location.NameEnd = _WorkCharIndex;
                            Location.ValueEnd = _WorkCharIndex;
                            _WorkCharIndex++;
                            return Result;
                        }
                    }
                    _WorkCharIndex++;
                }
            }
            else
            {

            }
            //虽然程序兼容属性值使用单引号和双引号，但是在这里已经是确认了是具体的哪一个。
            char BorderChar = InputText[FirstBorderIndex];

            _WorkCharIndex++;
            while (_WorkCharIndex < InputText.Length)
            {
                char chr = InputText[_WorkCharIndex];
                //找到了 引号
                if (chr == BorderChar)
                {
                    char PrevChr = InputText[_WorkCharIndex - 1];
                    if ('\\' == PrevChr)
                    {
                        continue;
                    }
                    LastBorderIndex = _WorkCharIndex;
                    _WorkCharIndex++;
                    break;
                }
                _WorkCharIndex++;
            }
            if (-1 == LastBorderIndex)
            {
                Location.End = _WorkCharIndex;
                Location.NameEnd = _WorkCharIndex;
                Location.ValueEnd = _WorkCharIndex;
            }
            else
            {
                Location.End = LastBorderIndex;
                Location.NameEnd = LastBorderIndex;
                Location.ValueEnd = LastBorderIndex;

            }
            Result.Value = InputText.Substring(Location.Start, Location.End - Location.End);
            return Result;
        }


        #region 作废的代码
        //protected TagAttribute GetAttr(string input)
        //{

        //}

        private void ssss()
        {

            //if (FindName)
            //{
            //    if (FindBorder)
            //    {
            //        if (BorderChars.Contains(chr))
            //        {
            //            Location.ValueEnd = _WorkCharIndex - 1;
            //            Location.End = _WorkCharIndex;
            //            break;
            //        }
            //    }
            //    else
            //    {
            //        if ('=' == chr)
            //        {
            //            Location.NameEnd = _WorkCharIndex - 1;
            //        }
            //        else if (BorderChars.Contains(chr))
            //        {
            //            Location.ValueStart = _WorkCharIndex + 1;
            //            FindBorder = true;
            //        }
            //        else if (SpaceChars.Contains(chr))
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (' ' != InputText[_WorkCharIndex] && '\t' == InputText[_WorkCharIndex] && '\r' == InputText[_WorkCharIndex] && '\n' == InputText[_WorkCharIndex])
            //    {
            //        FindName = true;
            //        Location.Start = _WorkCharIndex;
            //        Location.NameStart = _WorkCharIndex;
            //    }
            //}

        }

        #endregion



    }

}
