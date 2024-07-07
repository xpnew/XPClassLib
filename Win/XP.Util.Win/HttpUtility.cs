using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Win
{

    /// <summary>
    /// 解决不方便引入System.Web.dll而搬运了微软的代码
    /// </summary>
    /// <remarks>
    /// https://referencesource.microsoft.com/#System.Web/httpserverutility.cs,d8e36c79851b91a8
    /// </remarks>
    public class HttpUtility
    {



        /// <devdoc>
        ///    <para>[To be supplied.]</para>
        /// </devdoc>
        public static string UrlEncode(string str, Encoding e)
        {
            if (str == null)
                return null;
            return System.Uri.EscapeDataString(str);
        }
                /// <devdoc>
        ///    <para>[To be supplied.]</para>
        /// </devdoc>
        public static string UrlDecode(string str)
        {
            if (str == null)
                return null;
            return System.Uri.UnescapeDataString(str);
        }


        ///// <devdoc>
        /////    <para>[To be supplied.]</para>
        ///// </devdoc>
        //public static byte[] UrlEncodeToBytes(string str, Encoding e)
        //{
        //    //if (str == null)
        //    //    return null;
        //    //byte[] bytes = e.GetBytes(str);
        //    //return HttpEncoder.Current.UrlEncode(bytes, 0, bytes.Length, false /* alwaysCreateNewReturnValue */);

        //   return System.Uri.EscapeDataString(str);
        //}
    }
}
