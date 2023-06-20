using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Text.Tags.TMTags
{
    /// <summary>
    /// 循环标签
    /// </summary>
    public class TMLoop:TMBase
    {


        protected override void _Init()
        {
            base._Init();

            TagPrefix = "Loop";
            Type = TagType.Double;
        }


    }
}
