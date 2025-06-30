using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Finders
{
    public class FindResultBase
    {

        [DisplayName("文件名")]
        [Display(Name = "文件名", Order = 1000)]
        public string FileName { get; set; }


        [DisplayName("路径")]
        [Display(Name = "路径", Order = 2000)]
        public string PathName { get; set; }

        [DisplayName("全名（带路径）")]
        [Display(Name = "全名", Order = 2000)]
        public string FullName { get; set; }



    }
}

