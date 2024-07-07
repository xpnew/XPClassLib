using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Util.Configs;

namespace XP.Util
{
    /// <summary>
    /// 视图页面工具
    /// </summary>
    public static class ViewsHelper
    {



        #region 资源地址
        /// <summary>
        /// 资源地址，可以通过版本号控制浏览器更新
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ResourceUrl(string path)
        {

            var cr = ConfigReader.Self;

            string contentPath = cr.GetSet("ContentPath");//AppSettings["ContentPath"];//静态文件地址
            string contentVersion = cr.GetSet("ContentVersion");//ConfigurationManager.AppSettings["ContentVersion"];
            string url = contentPath.TrimEnd('/') + "/" + path.TrimStart('/') + "?" + contentVersion;
            return url;
        }

        #endregion

        #region 图片站地址


        public static string FullPicPath(string path)
        {
            string Root = ConfigReader.Self.GetSet("ImgSiteWebPathRoot");
            //string FullPath = Root + path;
            return Root + path;
        }


        #endregion


        /// <summary>
        /// 站点名称属性，来自于配置文件
        /// </summary>
        public static string SiteName
        {
            get
            {
                var cr = ConfigReader.Self;
                string SiteName = cr.GetSet("SiteName");
                return SiteName;
            }
        }

        /// <summary>
        /// 出错提示模板
        /// </summary>
        /// <returns></returns>
        public static string ErrorMsgTm
        {
            get { return "{0} \n ErrorCode:{1}"; }
        }



    }
}
