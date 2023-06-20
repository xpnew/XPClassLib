using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Web.Permission
{
    public class ControllerUtility
    {

        #region 随机、文件名
        public static string GetRandomId()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffffff");
        }

        public static string GetGUID()
        {
            return System.Guid.NewGuid().ToString();
        }


        /// <summary>获得随机文件名，如果已经存在则重新生成</summary>
        /// <param name="physicalPath">物理路径</param>
        /// <returns></returns>
        public static string GetRandomFileName(string physicalPath, string extName)
        {
            if (!physicalPath.EndsWith("\\"))
            {
                physicalPath += "\\";
            }
            bool NotExistFile = true;
            string NewFileName;
            NewFileName = GetRandomId() + extName;
            while (NotExistFile)
            {
                if (System.IO.File.Exists(physicalPath + NewFileName))
                {
                    NewFileName = GetRandomId() + extName;
                }
                else
                {
                    NotExistFile = false;
                }
            }
            return NewFileName;
        }
        #endregion

        #region <<时间类型格式化方法>>
        /// <summary>
        /// 格式化时间查询条件
        /// </summary>
        /// <param name="dt">格式时间</param>
        /// <param name="daySpan">加减天数</param>
        /// <returns></returns>
        public static string FormatDateTimeCondition(DateTime dt, double daySpan)
        {
            string ret = "";
            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() == "th-th")
            {
                ret = DateTime.Now.AddDays(daySpan).AddYears(-543).ToString("MM/dd/yyyy");
            }
            else
            {
                ret = DateTime.Now.AddDays(daySpan).ToString("MM/dd/yyyy");
            }

            return ret;
        }

        /// <summary>
        /// 格式化时间查询条件(获取前一个月的日期)
        /// </summary>
        /// <param name="dt">格式时间</param>
        /// <param name="daySpan">加减天数</param>
        /// <returns></returns>
        public static string FormatDateTimeCondition1(DateTime dt, int daySpan)
        {
            string ret = "";
            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() == "th-th")
            {
                ret = DateTime.Now.AddMonths(daySpan).AddYears(-543).ToString("MM/dd/yyyy");
            }
            else
            {
                ret = DateTime.Now.AddMonths(daySpan).ToString("MM/dd/yyyy");
            }

            return ret;
        }

        /// <summary>
        /// 格式化时间查询条件(获取上个月的第一天)
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string FirstDayOfPreviousMonth(DateTime datetime)
        {
            string ret = "";
            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() == "th-th")
            {
                ret = datetime.AddDays(1 - datetime.Day).AddMonths(-1).AddYears(-543).ToString("MM/dd/yyyy");
            }
            else
            {
                ret = datetime.AddDays(1 - datetime.Day).AddMonths(-1).ToString("MM/dd/yyyy");

            }
            return ret;
        }


        /// <summary>
        /// 格式化时间查询数据
        /// </summary>
        /// <param name="dt">格式时间</param>
        /// <param name="showTime">是否显示时分秒</param>
        /// <returns></returns>
        public static string FormatDateTimeData(DateTime dt, bool showTime)
        {
            string ret = "";
            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() == "th-th")
            {
                if (showTime)
                {
                    ret = dt.AddYears(-543).ToString("MM/dd/yyyy HH:mm:ss");
                }
                else
                {
                    ret = dt.AddYears(-543).ToString("MM/dd/yyyy");
                }
            }
            else
            {
                if (showTime)
                {
                    ret = dt.ToString("MM/dd/yyyy HH:mm:ss");
                }
                else
                {
                    ret = dt.ToString("MM/dd/yyyy");
                }
            }

            return ret;
        }

        /// <summary>
        /// 格式化时间查询数据
        /// </summary>
        /// <param name="dt">格式时间(可以空的Nullable《DateTime》)</param>
        /// <param name="showTime">是否显示时分秒</param>
        /// <returns></returns>
        public static string FormatDateTimeData(Nullable<DateTime> dt, bool showTime)
        {
            if (!dt.HasValue)
            {
                return String.Empty;
            }
            return FormatDateTimeData(dt.Value, showTime);

        }



        /// <summary>
        /// 格式化时间字符串
        /// </summary>
        /// <param name="datetimeString"></param>
        /// <returns></returns>
        public static DateTime FormatDateTimeParse(string datetimeString)
        {
            return DateTime.Parse(datetimeString, CultureInfo.InvariantCulture);
        }
        #endregion
    }
}
