using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Compress.Rar.Schematrix
{

    public delegate void ExtractionProgressHandler(object sender, ExtractionProgressEventArgs e);


    public class ExtractionProgressEventArgs
    {
        // Token: 0x0400001B RID: 27
        public string FileName;

        // Token: 0x0400001C RID: 28
        public long FileSize;

        // Token: 0x0400001D RID: 29
        public long BytesExtracted;

        // Token: 0x0400001E RID: 30
        public double PercentComplete;

        // Token: 0x0400001F RID: 31
        public bool ContinueOperation = true;
    }
}
