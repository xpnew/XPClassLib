using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Finders
{
    public class TextFileFinderResult:FindResultBase
    {

        public bool IsFind { get; set; }

        public int TotalFoundLine { get; set; }

        public List<TextLineFinderResult> FindLines { get; set; }


    }
}
