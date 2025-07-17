using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljy.Comm.ReturnEntities
{

    /// <summary>
    /// 泛型数据返回
    /// </summary>
    public class GenericModelReturn<T> : BaseReturn
    {

        public T DataInfo { get; set; }
    }
}
