using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Finders
{
    public class TextFileFoundResult:FindResultBase
    {

        public bool IsFind { get; set; }

        public int TotalFoundLine { get; set; }

        public List<TextLineFoundResult> FindLines { get; set; }


    }
}
