using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Compress.Rar.Schematrix
{
    public delegate void MissingVolumeHandler(object sender, MissingVolumeEventArgs e);
    public class MissingVolumeEventArgs
    {


        // Token: 0x06000050 RID: 80 RVA: 0x000030AC File Offset: 0x000012AC
        public MissingVolumeEventArgs(string volumeName)
        {
            this.VolumeName = volumeName;
        }

        // Token: 0x04000014 RID: 20
        public string VolumeName;

        // Token: 0x04000015 RID: 21
        public bool ContinueOperation = false;
    }
}
