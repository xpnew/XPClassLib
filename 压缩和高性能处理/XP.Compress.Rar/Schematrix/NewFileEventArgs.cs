using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Compress.Rar.Schematrix
{

    public delegate void NewFileHandler(object sender, NewFileEventArgs e);


    public class NewFileEventArgs
    {
        // Token: 0x06000053 RID: 83 RVA: 0x000030F7 File Offset: 0x000012F7
        public NewFileEventArgs(RARFileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        // Token: 0x0400001A RID: 26
        public RARFileInfo fileInfo;
    }
}
