using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.ReturnEntities
{
    /// <summary>
    /// 输入类<HttpReturn>的说明 
    /// </summary>
	public class HttpReturn:BaseReturn
    {


        /// <summary>
        /// 返回的http StatusCode: 200正常，500错误等等
        /// </summary>
        public string HttpCode { get; set; }
    }
}
