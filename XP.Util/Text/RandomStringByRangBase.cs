using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util.Text
{
    /// <summary>
    /// 随机字符串，在给定范围内，基类
    /// </summary>
    public class RandomStringByRangBase: RandomStringBase
    {
        public string Range { get; set; }

        public RandomStringByRangBase():base()
        {
            _Init();
        }

        protected void _Init()
        {



        }

        protected virtual void _InitRange()
        {

        }


        public string Start(int length)
        {
            if (null == Range || 0 == Range.Length)
            {
                return String.Empty;
            }
            int TotalCharNum = Range.Length;
            //Random rnd = InnterRandom;
            string Result = null;
            StringBuilder sb = new StringBuilder();
            for (int l = 0; l < length; l++)
            {
                sb.Append(Range[Rnd.Next(TotalCharNum)]);
            }


            Result = sb.ToString();

            return Result;

        }

        


    }
}
