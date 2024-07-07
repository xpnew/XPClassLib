using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Win.Caching
{
    internal sealed class FileChangeEvent : EventArgs
    {
        internal FileAction Action;
        internal string FileName;
        internal FileChangeEvent(FileAction action, string fileName)
        {
            this.Action = action;
            this.FileName = fileName;
        }
    }

    internal enum FileAction
    {
        Dispose = -2,
        Error = -1,
        Overwhelming = 0,
        Added = 1,
        Removed = 2,
        Modified = 3,
        RenamedOldName = 4,
        RenamedNewName = 5,
    }
}
