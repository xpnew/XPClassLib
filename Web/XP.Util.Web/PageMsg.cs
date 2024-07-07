using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace XP.Util.Web
{
    public class PageMsg
    {
        public static void xpnewAlert(string str)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            sb.Append("<script language=\"JavaScript\">\n");
            sb.Append("alert('");
            sb.Append(str);
            sb.Append("');\n");
            sb.Append("history.go(-1);\n");
            sb.Append("</script>\n");
            String sc = sb.ToString();

            Write(sc);
        }
        public static void xpnewAlert(string str, bool Refresh)
        {
            if (Refresh)
            {
                string url = HttpContext.Current.Request.UrlReferrer.ToString();
                xpnewAlert(str, url);
            }
            else
            {
                xpnewAlert(str);
            }

        }
        public static void xpnewAlert(string str, string url)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            sb.Append("<script language=\"JavaScript\">\n");
            sb.Append("alert('");
            sb.Append(str);
            sb.Append("');\n");
            sb.Append("location.replace('");
            sb.Append(url);
            sb.Append("');\n");
            sb.Append("</script>\n");
            String sc = sb.ToString();

            Write(sc);
        }

        public static void xpnewClose(string str)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            sb.Append("<script language=\"JavaScript\">\n");
            sb.Append("alert('");
            sb.Append(str);
            sb.Append("');\n");
            sb.Append("window.close();\n");
            sb.Append("</script>\n");
            String sc = sb.ToString();

            Write(sc);
        }


        public static void Alert(string str)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            sb.Append("<script language=\"JavaScript\">");
            sb.Append("alert('" + getMsgCHS(str) + "');");
            sb.Append("history.go(-1);");
            sb.Append("</script>");
            String sc = sb.ToString();

            Write(sc);
            //WriteClientScript(json, sc);

        }
        public static void Write(string str)
        {

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;

            HttpContext.Current.Response.Write(str);
            HttpContext.Current.Response.End();

            //try
            //{
            //    HttpContext.Current.Response.End();

            //}
            //catch (System.Threading.ThreadAbortException ex)
            //{
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
            //}
            //catch (Exception ex)
            //{

            //}


        }

       


        private static string getMsgCHS(string str)
        {
            string connstr = Conf.ConnStr;

            string sql = "Select Id,message from xpnewMessage where KeyWords like %" + str + "%";

            /* 原始代码为了支持多语言可以从数据库读取信息。
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                string tit = dr.GetString(1);
                string id = dr.GetString(0);
                return tit;
            }

            conn.Close();
             * 
             * */


            return str;
        }
    }
}
