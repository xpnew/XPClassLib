using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util.Text
{

    /// <summary>
    /// 随机字符串，基类
    /// </summary>
    public class RandomStringBase
    {


        public string Result { get; set; }

        public Random Rnd
        {
            get
            {
                if (null == _Rnd)
                {
                    return InnterRandom;
                }
                return _Rnd;
            }
            set { _Rnd = value; }
        }

        private Random _Rnd = null;


        //protected  Random InnterRandom = new Random();

        internal static Random InnterRandom = new Random();

    }
}
