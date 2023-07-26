using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace XP.Util.Debug
{
    /// <summary>
    /// 堆栈工具
    /// </summary>
    public static class StackTraceUtil
    {

        /// <summary>
        /// 将堆栈内容输出成文字
        /// </summary>
        /// <param name="st">输入的堆栈</param>
        /// <param name="maxPlies">最大层数</param>
        /// <param name="newLine">换行标记：\n或者<br />，默认是\n</param>
        /// <returns></returns>
        public static string BuildStackStr(StackTrace st, int maxPlies = 5, string newLine = "\n")
        {
            if(st.FrameCount == 0) {
                return "";
            }
            //得到当前的所有堆栈
            StackFrame[] sf = st.GetFrames();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < sf.Length; ++i)
            {
                var frame = sf[i];
                sb.Append(" 文件名： " + frame.GetFileName());
                sb.Append(" 行号： " + frame.GetFileLineNumber());
                var m = frame.GetMethod();
                if (null == m)
                {
                    sb.Append("匿名类");
                    continue;
                }
                var dt = m.DeclaringType;
                //这两个基本一样
                //var rt = sf[i].GetMethod().ReflectedType;

                if (null != dt)
                {
                    sb.Append(" 对象： " + dt.FullName);
                }
                sb.Append(" 方法名： " + m.Name);
                sb.Append(newLine);
            }
            return sb.ToString();
        }



    }
}
