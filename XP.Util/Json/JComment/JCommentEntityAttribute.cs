using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Json.JComment
{
    [AttributeUsage(AttributeTargets.Class , AllowMultiple = true)]
    public class JCommentEntityAttribute : JsonCommentAttribute
    {
        public JCommentEntityAttribute(string comment) : base(comment)
        {
        }
    }
}
