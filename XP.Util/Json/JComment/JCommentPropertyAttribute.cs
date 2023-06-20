using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Json.JComment
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class JCommentPropertyAttribute : JsonCommentAttribute
    {
        public JCommentPropertyAttribute(string comment) : base(comment)
        {
        }
    }
}
