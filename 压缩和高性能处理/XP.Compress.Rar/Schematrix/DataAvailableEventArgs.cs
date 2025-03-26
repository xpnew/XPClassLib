using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Compress.Rar.Schematrix
{
    public delegate void DataAvailableHandler(object sender, DataAvailableEventArgs e);


    // Token: 0x0200000B RID: 11
    public class DataAvailableEventArgs
    {
        // Token: 0x06000051 RID: 81 RVA: 0x000030C4 File Offset: 0x000012C4
        public DataAvailableEventArgs(byte[] data)
        {
            this.Data = data;
        }

        // Token: 0x04000016 RID: 22
        public readonly byte[] Data;

        // Token: 0x04000017 RID: 23
        public bool ContinueOperation = true;
    }
}
