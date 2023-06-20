using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using XP.Util.WebUtils;




namespace XP.Web.ControllerBase
{
    public class BassWeb : Controller
    {



        #region 路由和视图管理
        private string mDefaultPageAct = "query";

        public string DefaultPageAct
        {
            get { return mDefaultPageAct; }
            set { mDefaultPageAct = value; }
        }

        private string mControllerName;
        public string ControllerName
        {
            get
            {
                if (String.IsNullOrEmpty(mControllerName))
                {
                    mControllerName = RouteData.Values["controller"].ToString();
                }
                return mControllerName;
            }
            set
            {
                mControllerName = value;
            }
        }
        private string _ActionName;

        public string ActionName
        {
            get
            {
                if (String.IsNullOrEmpty(_ActionName))
                {
                    if (null == RouteData.Values["action"])
                        return String.Empty;
                    _ActionName = RouteData.Values["action"].ToString();
                }

                return _ActionName;
            }
            set { _ActionName = value; }
        }

        private string mViewPageUrl;
        /// <summary>根据内页名称规范生成的view地址</summary>
        /// <remarks>
        /// 通过GetViewsUrl()方法生成"~/Views/" + controller + "/" + action + "/" + pageact + ".cshtml"格式的地址  
        /// </remarks>
        public string ViewPageUrl
        {
            get
            {
                if (String.IsNullOrEmpty(mViewPageUrl))
                {
                    mViewPageUrl = GetViewsUrl();
                }
                return mViewPageUrl;
            }
        }
        [NonAction]
        public string GetActionUrl()
        {
            //连接字符串 目标 View("~/Views/" + controller + "/" + action + "/" + pageact + ".cshtml");
            StringBuilder sb = new StringBuilder();
            if (null != RouteData.Values["menugroup"])
            {
                sb.Append("~/");
                sb.Append(RouteData.Values["menugroup"].ToString());
                sb.Append("/");
                if (null != RouteData.Values["controller"])
                {
                    sb.Append(RouteData.Values["controller"].ToString());
                    sb.Append("/");
                    if (null != RouteData.Values["action"])
                    {
                        sb.Append(RouteData.Values["action"].ToString());
                        sb.Append("/");
                    }
                }

            }
            else
            {
                sb.Append("/");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 指定一个视图文件名字，然后制作一个视图的路经（控制器目录下）
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [NonAction]
        protected string BuildingViewUrl(string filename)
        {
            //连接字符串 目标 View("~/Views/" + controller + "/" + action + "/" + pageact + ".cshtml");
            StringBuilder sb = new StringBuilder();
            if (null != RouteData.Values["menugroup"])
            {
                sb.Append("~/Views/");
                sb.Append(RouteData.Values["menugroup"].ToString());
                sb.Append("/");
                if (null != RouteData.Values["controller"])
                {
                    sb.Append(RouteData.Values["controller"].ToString());
                    sb.Append("/");
                    if (null != filename && 0 < filename.Length)
                    {
                        sb.Append(filename);
                        sb.Append(".cshtml");
                    }
                }

            }
            else
            {
                sb.Append("/");
            }
            return sb.ToString();

        }


        [NonAction]
        public string GetViewsUrl()
        {
            //连接字符串 目标 View("~/Views/" + controller + "/" + action + "/" + pageact + ".cshtml");
            StringBuilder sb = new StringBuilder();
            if (null != RouteData.Values["menugroup"])
            {
                sb.Append("~/Views/");
                sb.Append(RouteData.Values["menugroup"].ToString());
                sb.Append("/");
                if (null != RouteData.Values["controller"])
                {
                    sb.Append(RouteData.Values["controller"].ToString());
                    sb.Append("/");
                    if (null != RouteData.Values["action"])
                    {
                        sb.Append(RouteData.Values["action"].ToString());
                        sb.Append(".cshtml");
                    }
                }

            }
            else
            {
                sb.Append("/");
            }
            return sb.ToString();
        }

        #endregion



        #region 工具方法

        public ActionResult SendHtml(string html)
        {
            ContentResult result = new ContentResult();
            result.ContentType = "";
            result.ContentEncoding = UTF8Encoding.UTF8;
            result.Content = html;
            return result;


        }


        /// <summary>
        /// 获取客户端提供的参数，假定提交的类型是文本，并且会过滤非法字符
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rq"></param>
        /// <returns></returns>
        public string GetQurey(string key, NameValueCollection rq = null)
        {
            string Result = null;
            if (null == rq)
            {
                rq = Request.Params;
            }
            string input = rq[key];

            if (!String.IsNullOrEmpty(input))
            {
                Result = CleanHtml.DropHTML(input);
            }
            return Result;
        }
        /// <summary>
        /// 获取一个可空整数类型的参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rq"></param>
        /// <returns></returns>
        public int? GetNullInt(string key, NameValueCollection rq = null)
        {
            int? Result = null;
            if (null == rq)
            {
                rq = Request.Params;
            }
            string input = rq[key];

            if (!String.IsNullOrEmpty(input))
            {
                if (vbs.IsInt(input))
                {
                    Result = int.Parse(input);
                }
            }
            return Result;
        }

        /// <summary>
        /// 获取一个整数类型的参数，空值为0并且也可以自由指定
        /// </summary>
        /// <param name="key"></param>
        /// <param name="_default"></param>
        /// <param name="rq"></param>
        /// <returns></returns>
        public int GetInt(string key, int _default = 0, NameValueCollection rq = null)
        {
            int Result = _default;
            if (null == rq)
            {
                rq = Request.Params;
            }

            string input = rq[key];
            if (!String.IsNullOrEmpty(input))
            {
                if (vbs.IsInt(input))
                {
                    Result = int.Parse(input);
                }
            }
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rq"></param>
        /// <returns></returns>
        public List<int> GetIntList(string key, NameValueCollection rq = null)
        {
            List<int> Result = null;
            if (null == rq)
            {
                rq = Request.Params;
            }
            string input = rq[key];
            if (!String.IsNullOrEmpty(input))
            {
                Result = String2IdList(input);
            }
            return Result;
        }

        /// <summary>字符串解析成int list</summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<int> String2IdList(string str)
        {
            if (null == str)
            {
                return new List<int>();
            }
            string[] IdArray = str.Split(new char[] { ',', '|' });
            List<int> IdList = new List<int>();
            foreach (string idStr in IdArray)
            {
                IdList.Add(Convert.ToInt32(idStr));
            }

            return IdList;
        }

        #endregion


    }
}
