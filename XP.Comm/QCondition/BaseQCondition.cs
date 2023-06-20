using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.QCondition
{
    [Serializable]
    public class BaseQCondition
    {

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }


        /// <summary>
        /// 创建时间范围-起点
        /// </summary>
        public long? BeginTS { get; set; }
        /// <summary>
        /// 创建时间范围-终点
        /// </summary>
        public long? EndTS { get; set; }



        /// <summary>
        /// 开始时间年月日，类似20080808这种数字
        /// </summary>
        public int? BeginYMD { get; set; }


        /// <summary>
        /// 结束时间年月日，类似20080808这种数字
        /// </summary>
        public int? EndYMD { get; set; }


        /// <summary>
        /// 最大值
        /// </summary>
        public int? MaxVal { get; set; }

        /// <summary>
        /// 最小值 
        /// </summary>
        public int? MinVal { get; set; }


        /// <summary>
        /// 最高价
        /// </summary>
        public decimal? MaxPrice { get; set; }

        /// <summary>
        /// 最低价
        /// </summary>
        public decimal? MinPrice { get; set; }





        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageTotal = 0;

        public int RecordTotal = 0;

        /// <summary>
        /// 是否使用了分页。条件对象可以用在不分页的场合
        /// </summary>
        public bool UsePaging { get; set; }


        /// <summary>
        /// 当前企业id
        /// </summary>
        public int? CurrentStoreId { get; set; }
        /// <summary>
        /// 当前是系统商户
        /// </summary>
        /// <remarks>
        /// 在BLL层或者应用层赋值，如果为空在DAL层只会检查是不是1
        /// </remarks>
        public bool? CurrentIsSysStoreId { get; set; }
  

        public List<int> SelfStoreIdList { get; set; }
        /// <summary>
        /// 排序方式 
        /// </summary>
        public OrderTypeDef OrderBy { get; set; }



        public BaseQCondition()
        {
            PageSize = 20;
            PageIndex = 1;
            PageTotal = 0;
            RecordTotal = 0;
            UsePaging = false;
            OrderBy = OrderTypeDef.Default;
        }
    }
}
