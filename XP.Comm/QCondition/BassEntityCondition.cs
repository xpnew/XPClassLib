using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.QCondition
{
    /// <summary>
    /// 基础条件类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BassEntityCondition<T> : BaseQCondition
    {


        /// <summary>
        /// 实体本身属性条件
        /// </summary>
        public T EntitySelf { get; set; }



        public BassEntityCondition()
            : base()
        {

        }


    }
}
