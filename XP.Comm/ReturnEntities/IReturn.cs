using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljy.Comm.ReturnEntities
{
    /// <summary>
    /// 返回数据的接口定义
    /// </summary>
	public interface IReturn
    {
        /// <summary>
        /// 结果:就绪
        /// </summary>
        bool IsReady { get; set; }
        /// <summary>
        /// 结果：成功
        /// </summary>
        bool IsSuccess { get; set; }    

        /// <summary>
        /// 消息，错误提示
        /// </summary>
        string Msg { get; set; }

        /// <summary>
        /// 返回的内容主体
        /// </summary>
        string Body { get; set; }




     
    }
}
