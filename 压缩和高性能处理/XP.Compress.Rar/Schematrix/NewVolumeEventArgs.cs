using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Compress.Rar.Schematrix
{

    public delegate void NewVolumeHandler(object sender, NewVolumeEventArgs e);


    public class NewVolumeEventArgs
    {
        // Token: 0x0600004F RID: 79 RVA: 0x00003094 File Offset: 0x00001294
        public NewVolumeEventArgs(string volumeName)
        {
            this.VolumeName = volumeName;
        }

        // Token: 0x04000012 RID: 18
        public string VolumeName;

        // Token: 0x04000013 RID: 19
        public bool ContinueOperation = true;
    }
}
