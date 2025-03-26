using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Compress.Rar.Schematrix
{
    // Token: 0x0200000F RID: 15
    public class RARFileInfo
    {
        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000055 RID: 85 RVA: 0x00003118 File Offset: 0x00001318
        public double PercentComplete
        {
            get
            {
                bool flag = this.UnpackedSize != 0L;
                double num;
                if (flag)
                {
                    num = (double)this.BytesExtracted / (double)this.UnpackedSize * 100.0;
                }
                else
                {
                    num = 0.0;
                }
                return num;
            }
        }

        // Token: 0x04000020 RID: 32
        public string FileName;

        // Token: 0x04000021 RID: 33
        public bool ContinuedFromPrevious = false;

        // Token: 0x04000022 RID: 34
        public bool ContinuedOnNext = false;

        // Token: 0x04000023 RID: 35
        public bool IsDirectory = false;

        // Token: 0x04000024 RID: 36
        public long PackedSize = 0L;

        // Token: 0x04000025 RID: 37
        public long UnpackedSize = 0L;

        // Token: 0x04000026 RID: 38
        public int HostOS = 0;

        // Token: 0x04000027 RID: 39
        public long FileCRC = 0L;

        // Token: 0x04000028 RID: 40
        public DateTime FileTime;

        // Token: 0x04000029 RID: 41
        public int VersionToUnpack = 0;

        // Token: 0x0400002A RID: 42
        public int Method = 0;

        // Token: 0x0400002B RID: 43
        public int FileAttributes = 0;

        // Token: 0x0400002C RID: 44
        public long BytesExtracted = 0L;
    }
}
