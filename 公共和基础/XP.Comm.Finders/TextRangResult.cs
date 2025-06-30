using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Finders
{
    public class TextRangResult : FindResultBase
    {

        [DisplayName("找到区域数量")]
        [Display(Name = "找到区域数量")]
        public int ReferenceCounter { get; set; }

        [DisplayName("目标文本数量")]
        [Display(Name = "目标文本数量")]
        public int TargetCounter { get; set; }
        [DisplayName("存在遗漏")]
        [Display(Name = "存在遗漏")]
        public bool HasOmit { get; set; }


        [DisplayName("文本区域")]
        [Display(Name = "文本区域")]
        public List<string> RangList { get; set; }
    }
}
