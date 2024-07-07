using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Web
{
    /// <summary>
    /// 由控制器名、动作名组成的MVC名称组
    /// </summary>
    public struct MvcNameGroup
    {

        public string AreaName { get; set; }

        public string ControllerName { get; set; }


        public string ActionName { get; set; }


        public string RealActionName { get; set; }


        public string AreaName2Lower { get; set; }
        public string ControllerName2Lower { get; set; }
        public string ActionName2Lower { get; set; }



    }
}
