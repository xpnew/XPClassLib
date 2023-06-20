using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.QCondition;
using XP.Web.ControllerBase;


namespace XP.Web.Permission.Controllers
{

    /// <summary>
    /// 后台项目基类
    /// </summary>
    public class BMPageBase: BMControllerRoot
    {

        #region 查询条件

        protected bool _hasInitCondition = false;

        protected BaseQCondition _QCondition;

        public BaseQCondition QCondition
        {
            get
            {
                if (_hasInitCondition)
                {
                    return _QCondition;
                }
                else
                {
                    InitQCondition();
                    return _QCondition;
                }
            }
            set
            {
                _QCondition = value;
            }
        }
        protected void InitQCondition()
        {

            if (null == _QCondition)
                return;

            if (!String.IsNullOrEmpty(Request["PageIndex"]))
            {
                _QCondition.PageIndex = int.Parse(Request["PageIndex"]);
            }
            if (!String.IsNullOrEmpty(Request["PageSize"]))
            {
                _QCondition.PageSize = int.Parse(Request["PageSize"]);
            }
            //if (!String.IsNullOrEmpty(Request["sort"]))
            //{
            //    _QCondition.Sort = (Common.SORT)Enum.Parse(typeof(Common.SORT), Request["sort"].ToString(), true);
            //}
            //if (!String.IsNullOrEmpty(Request["OrderFiled"]))
            //{
            //    _QCondition.OrderFiled = Request["OrderFiled"];
            //}

            if (!String.IsNullOrEmpty(Request["BeginTime"]))
            {
                _QCondition.BeginTime = ControllerUtility.FormatDateTimeParse(Request["BeginTime"]);
            }
            if (!String.IsNullOrEmpty(Request["EndTime"]))
            {
                _QCondition.EndTime = ControllerUtility.FormatDateTimeParse(Request["EndTime"]);
            }


            _hasInitCondition = true;

        }
        protected virtual void InitQCondition<T>(T tCondition) where T : BaseQCondition
        {

            if (null == tCondition)
                return;

            if (!String.IsNullOrEmpty(Request["PageIndex"]))
            {
                tCondition.PageIndex = int.Parse(Request["PageIndex"]);
            }
            else
            {
                tCondition.PageIndex = 1;
            }
            if (!String.IsNullOrEmpty(Request["PageSize"]))
            {
                tCondition.PageSize = int.Parse(Request["PageSize"]);
            }
            else
            {
                tCondition.PageSize = 10;
            }

            //不用用GetType()来比较Type，因为派生类肯定不能等于QSortCondition
            //SA.Model.QCondition.QSortCondition sortQ = tCondition as SA.Model.QCondition.QSortCondition;
            //if (null != sortQ)
            //{

            //    if (!String.IsNullOrEmpty(Request["sort"]))
            //    {
            //        sortQ.Sort = (Common.SORT)Enum.Parse(typeof(Common.SORT), Request["sort"].ToString(), true);
            //    }
            //    else
            //    {
            //        sortQ.Sort = Common.SORT.DOWN;
            //    }
            //    if (!String.IsNullOrEmpty(Request["OrderFiled"]))
            //    {
            //        sortQ.OrderFiled = Request["OrderFiled"];
            //    }
            //}

            if (!String.IsNullOrEmpty(Request["BeginTime"]))
            {
                tCondition.BeginTime = ControllerUtility.FormatDateTimeParse(Request["BeginTime"]);
                ViewBag.BeginTime = tCondition.BeginTime;
                ViewBag.BeginTimeStr = Request["BeginTime"].Trim();

                int Ynd = 0;
                var dt = tCondition.BeginTime.Value;
                tCondition.BeginYMD = BuildYmd(dt);
            }
            if (!String.IsNullOrEmpty(Request["EndTime"]))
            {
                tCondition.EndTime = ControllerUtility.FormatDateTimeParse(Request["EndTime"]);
                tCondition.EndYMD = BuildYmd(tCondition.EndTime.Value);
                //不带时间的话，会生成 0:00:00，所以要延后一天。
                if (1 > Request["EndTime"].IndexOf(':'))
                {
                    tCondition.EndTime = tCondition.EndTime.Value.AddDays(1).AddMilliseconds(-1);
                }
                ViewBag.EndTime = tCondition.EndTime;
                ViewBag.EndTimeStr = Request["EndTime"].Trim();
            }
            else
            {
                int Ynd = 0;
                var dt = DateTime.Now.AddDays(-1);
                tCondition.EndYMD = BuildYmd(dt);
            }



            tCondition.CurrentStoreId = GetStoreId();
            tCondition.CurrentIsSysStoreId = CheckSysStore();
            tCondition.SelfStoreIdList = GetSelfStoreIdList();
            tCondition.StatisticStoreNames = GetStatisticStoreNames();
        }


 




        protected int BuildYmd(DateTime dt)
        {
            int Ynd = 0;
            Ynd = dt.Year * 10000 + dt.Month * 100 + dt.Day;
            return Ynd;
        }

        /// <summary>
        /// 获取商户Id，需要派生类来具体实现
        /// </summary>
        /// <returns></returns>
        protected virtual int GetStoreId()
        {
            throw new ArgumentNullException();
        }


        protected virtual bool CheckSysStore()
        {
            throw new ArgumentNullException();

        }

        protected virtual List<int> GetSelfStoreIdList()
        {
            throw new ArgumentNullException();
        }

        protected virtual string GetStatisticStoreNames()
        {
            throw new ArgumentNullException();
        }

        protected virtual void InitQConditionNoDateTime<T>(T tCondition) where T : BaseQCondition
        {

            if (null == tCondition)
                return;

            if (!String.IsNullOrEmpty(Request["PageIndex"]))
            {
                tCondition.PageIndex = int.Parse(Request["PageIndex"]);
            }
            else
            {
                tCondition.PageIndex = 1;
            }
            if (!String.IsNullOrEmpty(Request["PageSize"]))
            {
                tCondition.PageSize = int.Parse(Request["PageSize"]);
            }
            else
            {
                tCondition.PageSize = 10;
            }
            
            tCondition.CurrentStoreId = GetStoreId();
            tCondition.CurrentIsSysStoreId = CheckSysStore();
            tCondition.SelfStoreIdList = GetSelfStoreIdList();
            tCondition.StatisticStoreNames = GetStatisticStoreNames();
        }

        #endregion

    }


}
