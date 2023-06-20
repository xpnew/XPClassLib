using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Text.Tags.TMTags
{
    public class TMBase : BaseTag
    {

        protected override void _Init()
        {
            base._Init();

            PrefixChar = "{";

            SuffixChar = "}";
        }


       
    }
}
