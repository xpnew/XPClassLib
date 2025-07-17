using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljy.Comm.ReturnEntities
{
    /// <summary>
    /// 返回数据 基类， IReturn的基本实现
    /// </summary>
	public class BaseReturn:IReturn
    {
        /// <summary>
        /// 结果:就绪
        /// </summary>
        public bool IsReady { get; set; }
        /// <summary>
        /// 结果：成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 消息，错误提示
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回的内容主体
        /// </summary>
        public string Body { get; set; }


        ///public string Rmk { get; set; }

    }
}
