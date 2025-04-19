using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace XP.Compress.Rar.Schematrix
{

    // Token: 0x02000007 RID: 7
    // (Invoke) Token: 0x06000016 RID: 22
    public delegate void PasswordRequiredHandler(object sender, PasswordRequiredEventArgs e);


    public class Unrar : IDisposable
    {
        // Token: 0x06000019 RID: 25
        [DllImport("unrar64.dll")]
        private static extern IntPtr RAROpenArchive(ref Unrar.RAROpenArchiveData archiveData);

        // Token: 0x0600001A RID: 26
        [DllImport("unrar64.dll")]
        private static extern IntPtr RAROpenArchiveEx(ref Unrar.RAROpenArchiveDataEx archiveData);

        // Token: 0x0600001B RID: 27
        [DllImport("unrar64.dll")]
        private static extern int RARCloseArchive(IntPtr hArcData);

        // Token: 0x0600001C RID: 28
        [DllImport("unrar64.dll")]
        private static extern int RARReadHeader(IntPtr hArcData, ref Unrar.RARHeaderData headerData);

        // Token: 0x0600001D RID: 29
        [DllImport("unrar64.dll")]
        private static extern int RARReadHeaderEx(IntPtr hArcData, ref Unrar.RARHeaderDataEx headerData);

        // Token: 0x0600001E RID: 30
        [DllImport("unrar64.dll")]
        private static extern int RARProcessFile(IntPtr hArcData, int operation, [MarshalAs(UnmanagedType.LPStr)] string destPath, [MarshalAs(UnmanagedType.LPStr)] string destName);

        // Token: 0x0600001F RID: 31
        [DllImport("unrar64.dll")]
        private static extern void RARSetCallback(IntPtr hArcData, Unrar.UNRARCallback callback, int userData);

        // Token: 0x06000020 RID: 32
        [DllImport("unrar64.dll")]
        private static extern void RARSetPassword(IntPtr hArcData, [MarshalAs(UnmanagedType.LPStr)] string password);

        // Token: 0x14000001 RID: 1
        // (add) Token: 0x06000021 RID: 33 RVA: 0x00002048 File Offset: 0x00000248
        // (remove) Token: 0x06000022 RID: 34 RVA: 0x00002080 File Offset: 0x00000280
        [field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event DataAvailableHandler DataAvailable;

        // Token: 0x14000002 RID: 2
        // (add) Token: 0x06000023 RID: 35 RVA: 0x000020B8 File Offset: 0x000002B8
        // (remove) Token: 0x06000024 RID: 36 RVA: 0x000020F0 File Offset: 0x000002F0
        [field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event ExtractionProgressHandler ExtractionProgress;

        // Token: 0x14000003 RID: 3
        // (add) Token: 0x06000025 RID: 37 RVA: 0x00002128 File Offset: 0x00000328
        // (remove) Token: 0x06000026 RID: 38 RVA: 0x00002160 File Offset: 0x00000360
        [field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event MissingVolumeHandler MissingVolume;

        // Token: 0x14000004 RID: 4
        // (add) Token: 0x06000027 RID: 39 RVA: 0x00002198 File Offset: 0x00000398
        // (remove) Token: 0x06000028 RID: 40 RVA: 0x000021D0 File Offset: 0x000003D0
        [field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event NewFileHandler NewFile;

        // Token: 0x14000005 RID: 5
        // (add) Token: 0x06000029 RID: 41 RVA: 0x00002208 File Offset: 0x00000408
        // (remove) Token: 0x0600002A RID: 42 RVA: 0x00002240 File Offset: 0x00000440
        [field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event NewVolumeHandler NewVolume;

        // Token: 0x14000006 RID: 6
        // (add) Token: 0x0600002B RID: 43 RVA: 0x00002278 File Offset: 0x00000478
        // (remove) Token: 0x0600002C RID: 44 RVA: 0x000022B0 File Offset: 0x000004B0
        [field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event PasswordRequiredHandler PasswordRequired;

        // Token: 0x0600002D RID: 45 RVA: 0x000022E8 File Offset: 0x000004E8
        public Unrar()
        {
            this.callback = new Unrar.UNRARCallback(this.RARCallback);
        }

        // Token: 0x0600002E RID: 46 RVA: 0x0000237A File Offset: 0x0000057A
        public Unrar(string archivePathName)
            : this()
        {
            this.archivePathName = archivePathName;
        }

        //// Token: 0x0600002F RID: 47 RVA: 0x0000238C File Offset: 0x0000058C
        //protected override void Finalize()
        //{
        //    try
        //    {
        //        bool flag = this.archiveHandle != IntPtr.Zero;
        //        if (flag)
        //        {
        //            Unrar.RARCloseArchive(this.archiveHandle);
        //            this.archiveHandle = IntPtr.Zero;
        //        }
        //    }
        //    finally
        //    {
        //        base.Finalize();
        //    }
        //}


        ~Unrar()
        {
            try
            {
                bool flag = this.archiveHandle != IntPtr.Zero;
                if (flag)
                {
                    Unrar.RARCloseArchive(this.archiveHandle);
                    this.archiveHandle = IntPtr.Zero;
                }
            }
            finally
            {
              
            }
        }
        // Token: 0x06000030 RID: 48 RVA: 0x000023E4 File Offset: 0x000005E4
        public void Dispose()
        {
            bool flag = this.archiveHandle != IntPtr.Zero;
            if (flag)
            {
                Unrar.RARCloseArchive(this.archiveHandle);
                this.archiveHandle = IntPtr.Zero;
            }
        }

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000031 RID: 49 RVA: 0x00002420 File Offset: 0x00000620
        // (set) Token: 0x06000032 RID: 50 RVA: 0x00002438 File Offset: 0x00000638
        public string ArchivePathName
        {
            get
            {
                return this.archivePathName;
            }
            set
            {
                this.archivePathName = value;
            }
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000033 RID: 51 RVA: 0x00002444 File Offset: 0x00000644
        public string Comment
        {
            get
            {
                return this.comment;
            }
        }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000034 RID: 52 RVA: 0x0000245C File Offset: 0x0000065C
        public RARFileInfo CurrentFile
        {
            get
            {
                return this.currentFile;
            }
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000035 RID: 53 RVA: 0x00002474 File Offset: 0x00000674
        // (set) Token: 0x06000036 RID: 54 RVA: 0x0000248C File Offset: 0x0000068C
        public string DestinationPath
        {
            get
            {
                return this.destinationPath;
            }
            set
            {
                this.destinationPath = value;
            }
        }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x06000037 RID: 55 RVA: 0x00002498 File Offset: 0x00000698
        // (set) Token: 0x06000038 RID: 56 RVA: 0x000024B0 File Offset: 0x000006B0
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
                bool flag = this.archiveHandle != IntPtr.Zero;
                if (flag)
                {
                    Unrar.RARSetPassword(this.archiveHandle, value);
                }
            }
        }

        // Token: 0x06000039 RID: 57 RVA: 0x000024E8 File Offset: 0x000006E8
        public void Close()
        {
            bool flag = this.archiveHandle == IntPtr.Zero;
            if (!flag)
            {
                int result = Unrar.RARCloseArchive(this.archiveHandle);
                bool flag2 = result != 0;
                if (flag2)
                {
                    this.ProcessFileError(result);
                }
                else
                {
                    this.archiveHandle = IntPtr.Zero;
                }
            }
        }

        // Token: 0x0600003A RID: 58 RVA: 0x0000253C File Offset: 0x0000073C
        public void Open()
        {
            bool flag = this.ArchivePathName.Length == 0;
            if (flag)
            {
                throw new IOException("Archive name has not been set.");
            }
            this.Open(this.ArchivePathName, Unrar.OpenMode.Extract);
        }

        // Token: 0x0600003B RID: 59 RVA: 0x00002578 File Offset: 0x00000778
        public void Open(Unrar.OpenMode openMode)
        {
            bool flag = this.ArchivePathName.Length == 0;
            if (flag)
            {
                throw new IOException("Archive name has not been set.");
            }
            this.Open(this.ArchivePathName, openMode);
        }

        // Token: 0x0600003C RID: 60 RVA: 0x000025B4 File Offset: 0x000007B4
        public void Open(string archivePathName, Unrar.OpenMode openMode)
        {
            IntPtr handle = IntPtr.Zero;
            bool flag = this.archiveHandle != IntPtr.Zero;
            if (flag)
            {
                this.Close();
            }
            this.ArchivePathName = archivePathName;
            Unrar.RAROpenArchiveDataEx openStruct = default(Unrar.RAROpenArchiveDataEx);
            openStruct.Initialize();
            openStruct.ArcName = this.archivePathName + "\0";
            openStruct.ArcNameW = this.archivePathName + "\0";
            openStruct.OpenMode = (uint)openMode;
            bool flag2 = this.retrieveComment;
            if (flag2)
            {
                openStruct.CmtBuf = new string('\0', 65536);
                openStruct.CmtBufSize = 65536U;
            }
            else
            {
                openStruct.CmtBuf = null;
                openStruct.CmtBufSize = 0U;
            }
            handle = Unrar.RAROpenArchiveEx(ref openStruct);
            bool flag3 = openStruct.OpenResult > 0U;
            if (flag3)
            {
                switch (openStruct.OpenResult)
                {
                    case 11U:
                        throw new OutOfMemoryException("Insufficient memory to perform operation.");
                    case 12U:
                        throw new IOException("Archive header broken");
                    case 13U:
                        throw new IOException("File is not a valid archive.");
                    case 15U:
                        throw new IOException("File could not be opened.");
                }
            }
            this.archiveHandle = handle;
            this.archiveFlags = (Unrar.ArchiveFlags)openStruct.Flags;
            Unrar.RARSetCallback(this.archiveHandle, this.callback, this.GetHashCode());
            bool flag4 = openStruct.CmtState == 1U;
            if (flag4)
            {
                this.comment = openStruct.CmtBuf.ToString();
            }
            bool flag5 = this.password.Length != 0;
            if (flag5)
            {
                Unrar.RARSetPassword(this.archiveHandle, this.password);
            }
            this.OnNewVolume(this.archivePathName);
        }

        // Token: 0x0600003D RID: 61 RVA: 0x0000275C File Offset: 0x0000095C
        public bool ReadHeader()
        {
            bool flag = this.archiveHandle == IntPtr.Zero;
            if (flag)
            {
                throw new IOException("Archive is not open.");
            }
            this.header = default(Unrar.RARHeaderDataEx);
            this.header.Initialize();
            this.currentFile = null;
            int result = Unrar.RARReadHeaderEx(this.archiveHandle, ref this.header);
            bool flag2 = result == 10;
            bool flag3;
            if (flag2)
            {
                flag3 = false;
            }
            else
            {
                bool flag4 = result == 12;
                if (flag4)
                {
                    throw new IOException("Archive data is corrupt.");
                }
                bool flag5 = (this.header.Flags & 1U) != 0U && this.currentFile != null;
                if (flag5)
                {
                    this.currentFile.ContinuedFromPrevious = true;
                }
                else
                {
                    this.currentFile = new RARFileInfo();
                    this.currentFile.FileName = this.header.FileNameW.ToString();
                    bool flag6 = (this.header.Flags & 2U) > 0U;
                    if (flag6)
                    {
                        this.currentFile.ContinuedOnNext = true;
                    }
                    bool flag7 = this.header.PackSizeHigh > 0U;
                    if (flag7)
                    {
                        this.currentFile.PackedSize = (long)((ulong)this.header.PackSizeHigh * 4294967296UL + (ulong)this.header.PackSize);
                    }
                    else
                    {
                        this.currentFile.PackedSize = (long)((ulong)this.header.PackSize);
                    }
                    bool flag8 = this.header.UnpSizeHigh > 0U;
                    if (flag8)
                    {
                        this.currentFile.UnpackedSize = (long)((ulong)this.header.UnpSizeHigh * 4294967296UL + (ulong)this.header.UnpSize);
                    }
                    else
                    {
                        this.currentFile.UnpackedSize = (long)((ulong)this.header.UnpSize);
                    }
                    this.currentFile.HostOS = (int)this.header.HostOS;
                    this.currentFile.FileCRC = (long)((ulong)this.header.FileCRC);
                    this.currentFile.FileTime = this.FromMSDOSTime(this.header.FileTime);
                    this.currentFile.VersionToUnpack = (int)this.header.UnpVer;
                    this.currentFile.Method = (int)this.header.Method;
                    this.currentFile.FileAttributes = (int)this.header.FileAttr;
                    this.currentFile.BytesExtracted = 0L;
                    bool flag9 = (this.header.Flags & 32U) == 32U;
                    if (flag9)
                    {
                        this.currentFile.IsDirectory = true;
                    }
                    this.OnNewFile();
                }
                flag3 = true;
            }
            return flag3;
        }

        // Token: 0x0600003E RID: 62 RVA: 0x000029DC File Offset: 0x00000BDC
        public string[] ListFiles()
        {
            ArrayList fileNames = new ArrayList();
            while (this.ReadHeader())
            {
                bool flag = !this.currentFile.IsDirectory;
                if (flag)
                {
                    fileNames.Add(this.currentFile.FileName);
                }
                this.Skip();
            }
            string[] files = new string[fileNames.Count];
            fileNames.CopyTo(files);
            return files;
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00002A44 File Offset: 0x00000C44
        public void Skip()
        {
            int result = Unrar.RARProcessFile(this.archiveHandle, 0, string.Empty, string.Empty);
            bool flag = result != 0;
            if (flag)
            {
                this.ProcessFileError(result);
            }
        }

        // Token: 0x06000040 RID: 64 RVA: 0x00002A7C File Offset: 0x00000C7C
        public void Test()
        {
            int result = Unrar.RARProcessFile(this.archiveHandle, 1, string.Empty, string.Empty);
            bool flag = result != 0;
            if (flag)
            {
                this.ProcessFileError(result);
            }
        }

        // Token: 0x06000041 RID: 65 RVA: 0x00002AB4 File Offset: 0x00000CB4
        protected void OnPasswordRequired(object sender, PasswordRequiredEventArgs e)
        {
            bool flag = string.IsNullOrEmpty(this._Pwd);
            if (!flag)
            {
                bool flag2 = this._Pwd == null || this._Pwd.Length == 0;
                if (flag2)
                {
                    e.ContinueOperation = false;
                }
                else
                {
                    e.Password = this._Pwd;
                    e.ContinueOperation = true;
                }
            }
        }

        // Token: 0x06000042 RID: 66 RVA: 0x00002B10 File Offset: 0x00000D10
        public bool TestPassWord(string pwd)
        {
            this.PasswordRequired += this.OnPasswordRequired;
            this._Pwd = pwd;
            int result = Unrar.RARProcessFile(this.archiveHandle, 1, string.Empty, string.Empty);
            this.PasswordRequired -= this.OnPasswordRequired;
            bool flag = result != 0;
            bool flag2;
            if (flag)
            {
                this.ProcessFileError(result);
                flag2 = false;
            }
            else
            {
                flag2 = true;
            }
            return flag2;
        }

        // Token: 0x06000043 RID: 67 RVA: 0x00002B7C File Offset: 0x00000D7C
        public void Extract()
        {
            this.Extract(this.destinationPath, string.Empty);
        }

        // Token: 0x06000044 RID: 68 RVA: 0x00002B91 File Offset: 0x00000D91
        public void Extract(string destinationName)
        {
            this.Extract(string.Empty, destinationName);
        }

        // Token: 0x06000045 RID: 69 RVA: 0x00002BA1 File Offset: 0x00000DA1
        public void ExtractToDirectory(string destinationPath)
        {
            this.Extract(destinationPath, string.Empty);
        }

        // Token: 0x06000046 RID: 70 RVA: 0x00002BB4 File Offset: 0x00000DB4
        private void Extract(string destinationPath, string destinationName)
        {
            int result = Unrar.RARProcessFile(this.archiveHandle, 2, destinationPath, destinationName);
            bool flag = result != 0;
            if (flag)
            {
                this.ProcessFileError(result);
            }
        }

        // Token: 0x06000047 RID: 71 RVA: 0x00002BE4 File Offset: 0x00000DE4
        private DateTime FromMSDOSTime(uint dosTime)
        {
            ushort hiWord = (ushort)((dosTime & 4294901760U) >> 16);
            ushort loWord = (ushort)(dosTime & 65535U);
            int year = ((hiWord & 65024) >> 9) + 1980;
            int month = (hiWord & 480) >> 5;
            int day = (int)(hiWord & 31);
            int hour = (loWord & 63488) >> 11;
            int minute = (loWord & 2016) >> 5;
            int second = (int)(loWord & 31) << 1;
            return new DateTime(year, month, day, hour, minute, second);
        }

        // Token: 0x06000048 RID: 72 RVA: 0x00002C70 File Offset: 0x00000E70
        private void ProcessFileError(int result)
        {
            switch (result)
            {
                case 12:
                    throw new IOException("File CRC Error");
                case 13:
                    throw new IOException("File is not a valid archive.");
                case 14:
                    throw new OutOfMemoryException("Unknown archive format.");
                case 15:
                    throw new IOException("File could not be opened.");
                case 16:
                    throw new IOException("File could not be created.");
                case 17:
                    throw new IOException("File close error.");
                case 18:
                    throw new IOException("File read error.");
                case 19:
                    throw new IOException("File write error.");
                default:
                    return;
            }
        }

        // Token: 0x06000049 RID: 73 RVA: 0x00002D04 File Offset: 0x00000F04
        private int RARCallback(uint msg, int UserData, IntPtr p1, int p2)
        {
            string volume = string.Empty;
            string newVolume = string.Empty;
            int result = -1;
            switch (msg)
            {
                case 0U:
                    {
                        volume = Marshal.PtrToStringAnsi(p1);
                        bool flag = p2 == 1;
                        if (flag)
                        {
                            result = this.OnNewVolume(volume);
                        }
                        else
                        {
                            bool flag2 = p2 == 0;
                            if (flag2)
                            {
                                newVolume = this.OnMissingVolume(volume);
                                bool flag3 = newVolume.Length == 0;
                                if (flag3)
                                {
                                    result = -1;
                                }
                                else
                                {
                                    bool flag4 = newVolume != volume;
                                    if (flag4)
                                    {
                                        for (int i = 0; i < newVolume.Length; i++)
                                        {
                                            Marshal.WriteByte(p1, i, (byte)newVolume[i]);
                                        }
                                        Marshal.WriteByte(p1, newVolume.Length, 0);
                                    }
                                    result = 1;
                                }
                            }
                        }
                        break;
                    }
                case 1U:
                    result = this.OnDataAvailable(p1, p2);
                    break;
                case 2U:
                    result = this.OnPasswordRequired(p1, p2);
                    break;
            }
            return result;
        }

        // Token: 0x0600004A RID: 74 RVA: 0x00002DF0 File Offset: 0x00000FF0
        protected virtual void OnNewFile()
        {
            bool flag = this.NewFile != null;
            if (flag)
            {
                NewFileEventArgs e = new NewFileEventArgs(this.currentFile);
                this.NewFile(this, e);
            }
        }

        // Token: 0x0600004B RID: 75 RVA: 0x00002E28 File Offset: 0x00001028
        protected virtual int OnPasswordRequired(IntPtr p1, int p2)
        {
            int result = -1;
            bool flag = this.PasswordRequired != null;
            if (flag)
            {
                PasswordRequiredEventArgs e = new PasswordRequiredEventArgs();
                this.PasswordRequired(this, e);
                bool flag2 = e.ContinueOperation && e.Password.Length > 0;
                if (flag2)
                {
                    int i = 0;
                    while (i < e.Password.Length && i < p2)
                    {
                        Marshal.WriteByte(p1, i, (byte)e.Password[i]);
                        i++;
                    }
                    Marshal.WriteByte(p1, e.Password.Length, 0);
                    result = 1;
                }
                return result;
            }
            throw new IOException("Password is required for extraction.");
        }

        // Token: 0x0600004C RID: 76 RVA: 0x00002EE8 File Offset: 0x000010E8
        protected virtual int OnDataAvailable(IntPtr p1, int p2)
        {
            int result = 1;
            bool flag = this.currentFile != null;
            if (flag)
            {
                this.currentFile.BytesExtracted += (long)p2;
            }
            bool flag2 = this.DataAvailable != null;
            if (flag2)
            {
                byte[] data = new byte[p2];
                Marshal.Copy(p1, data, 0, p2);
                DataAvailableEventArgs e = new DataAvailableEventArgs(data);
                this.DataAvailable(this, e);
                bool flag3 = !e.ContinueOperation;
                if (flag3)
                {
                    result = -1;
                }
            }
            bool flag4 = this.ExtractionProgress != null && this.currentFile != null;
            if (flag4)
            {
                ExtractionProgressEventArgs e2 = new ExtractionProgressEventArgs();
                e2.FileName = this.currentFile.FileName;
                e2.FileSize = this.currentFile.UnpackedSize;
                e2.BytesExtracted = this.currentFile.BytesExtracted;
                e2.PercentComplete = this.currentFile.PercentComplete;
                this.ExtractionProgress(this, e2);
                bool flag5 = !e2.ContinueOperation;
                if (flag5)
                {
                    result = -1;
                }
            }
            return result;
        }

        // Token: 0x0600004D RID: 77 RVA: 0x00002FF8 File Offset: 0x000011F8
        protected virtual int OnNewVolume(string volume)
        {
            int result = 1;
            bool flag = this.NewVolume != null;
            if (flag)
            {
                NewVolumeEventArgs e = new NewVolumeEventArgs(volume);
                this.NewVolume(this, e);
                bool flag2 = !e.ContinueOperation;
                if (flag2)
                {
                    result = -1;
                }
            }
            return result;
        }

        // Token: 0x0600004E RID: 78 RVA: 0x00003044 File Offset: 0x00001244
        protected virtual string OnMissingVolume(string volume)
        {
            string result = string.Empty;
            bool flag = this.MissingVolume != null;
            if (flag)
            {
                MissingVolumeEventArgs e = new MissingVolumeEventArgs(volume);
                this.MissingVolume(this, e);
                bool continueOperation = e.ContinueOperation;
                if (continueOperation)
                {
                    result = e.VolumeName;
                }
            }
            return result;
        }

        // Token: 0x04000007 RID: 7
        private string archivePathName = string.Empty;

        // Token: 0x04000008 RID: 8
        private IntPtr archiveHandle = new IntPtr(0);

        // Token: 0x04000009 RID: 9
        private bool retrieveComment = true;

        // Token: 0x0400000A RID: 10
        private string password = string.Empty;

        // Token: 0x0400000B RID: 11
        private string comment = string.Empty;

        // Token: 0x0400000C RID: 12
        private Unrar.ArchiveFlags archiveFlags = (Unrar.ArchiveFlags)0U;

        // Token: 0x0400000D RID: 13
        private Unrar.RARHeaderDataEx header = default(Unrar.RARHeaderDataEx);

        // Token: 0x0400000E RID: 14
        private string destinationPath = string.Empty;

        // Token: 0x0400000F RID: 15
        private RARFileInfo currentFile = null;

        // Token: 0x04000010 RID: 16
        private Unrar.UNRARCallback callback = null;

        // Token: 0x04000011 RID: 17
        private string _Pwd = string.Empty;

        // Token: 0x02000011 RID: 17
        public enum OpenMode
        {
            // Token: 0x04000034 RID: 52
            List,
            // Token: 0x04000035 RID: 53
            Extract
        }

        // Token: 0x02000012 RID: 18
        private enum RarError : uint
        {
            // Token: 0x04000037 RID: 55
            EndOfArchive = 10U,
            // Token: 0x04000038 RID: 56
            InsufficientMemory,
            // Token: 0x04000039 RID: 57
            BadData,
            // Token: 0x0400003A RID: 58
            BadArchive,
            // Token: 0x0400003B RID: 59
            UnknownFormat,
            // Token: 0x0400003C RID: 60
            OpenError,
            // Token: 0x0400003D RID: 61
            CreateError,
            // Token: 0x0400003E RID: 62
            CloseError,
            // Token: 0x0400003F RID: 63
            ReadError,
            // Token: 0x04000040 RID: 64
            WriteError,
            // Token: 0x04000041 RID: 65
            BufferTooSmall,
            // Token: 0x04000042 RID: 66
            UnknownError
        }

        // Token: 0x02000013 RID: 19
        private enum Operation : uint
        {
            // Token: 0x04000044 RID: 68
            Skip,
            // Token: 0x04000045 RID: 69
            Test,
            // Token: 0x04000046 RID: 70
            Extract
        }

        // Token: 0x02000014 RID: 20
        private enum VolumeMessage : uint
        {
            // Token: 0x04000048 RID: 72
            Ask,
            // Token: 0x04000049 RID: 73
            Notify
        }

        // Token: 0x02000015 RID: 21
        [Flags]
        private enum ArchiveFlags : uint
        {
            // Token: 0x0400004B RID: 75
            Volume = 1U,
            // Token: 0x0400004C RID: 76
            CommentPresent = 2U,
            // Token: 0x0400004D RID: 77
            Lock = 4U,
            // Token: 0x0400004E RID: 78
            SolidArchive = 8U,
            // Token: 0x0400004F RID: 79
            NewNamingScheme = 16U,
            // Token: 0x04000050 RID: 80
            AuthenticityPresent = 32U,
            // Token: 0x04000051 RID: 81
            RecoveryRecordPresent = 64U,
            // Token: 0x04000052 RID: 82
            EncryptedHeaders = 128U,
            // Token: 0x04000053 RID: 83
            FirstVolume = 256U
        }

        // Token: 0x02000016 RID: 22
        private enum CallbackMessages : uint
        {
            // Token: 0x04000055 RID: 85
            VolumeChange,
            // Token: 0x04000056 RID: 86
            ProcessData,
            // Token: 0x04000057 RID: 87
            NeedPassword
        }

        // Token: 0x02000017 RID: 23
        private struct RARHeaderData
        {
            // Token: 0x06000066 RID: 102 RVA: 0x000034AF File Offset: 0x000016AF
            public void Initialize()
            {
                this.CmtBuf = new string('\0', 65536);
                this.CmtBufSize = 65536U;
            }

            // Token: 0x04000058 RID: 88
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string ArcName;

            // Token: 0x04000059 RID: 89
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string FileName;

            // Token: 0x0400005A RID: 90
            public uint Flags;

            // Token: 0x0400005B RID: 91
            public uint PackSize;

            // Token: 0x0400005C RID: 92
            public uint UnpSize;

            // Token: 0x0400005D RID: 93
            public uint HostOS;

            // Token: 0x0400005E RID: 94
            public uint FileCRC;

            // Token: 0x0400005F RID: 95
            public uint FileTime;

            // Token: 0x04000060 RID: 96
            public uint UnpVer;

            // Token: 0x04000061 RID: 97
            public uint Method;

            // Token: 0x04000062 RID: 98
            public uint FileAttr;

            // Token: 0x04000063 RID: 99
            [MarshalAs(UnmanagedType.LPStr)]
            public string CmtBuf;

            // Token: 0x04000064 RID: 100
            public uint CmtBufSize;

            // Token: 0x04000065 RID: 101
            public uint CmtSize;

            // Token: 0x04000066 RID: 102
            public uint CmtState;
        }

        // Token: 0x02000018 RID: 24
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct RARHeaderDataEx
        {
            // Token: 0x06000067 RID: 103 RVA: 0x000034CE File Offset: 0x000016CE
            public void Initialize()
            {
                this.CmtBuf = new string('\0', 65536);
                this.CmtBufSize = 65536U;
            }

            // Token: 0x04000067 RID: 103
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string ArcName;

            // Token: 0x04000068 RID: 104
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string ArcNameW;

            // Token: 0x04000069 RID: 105
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string FileName;

            // Token: 0x0400006A RID: 106
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string FileNameW;

            // Token: 0x0400006B RID: 107
            public uint Flags;

            // Token: 0x0400006C RID: 108
            public uint PackSize;

            // Token: 0x0400006D RID: 109
            public uint PackSizeHigh;

            // Token: 0x0400006E RID: 110
            public uint UnpSize;

            // Token: 0x0400006F RID: 111
            public uint UnpSizeHigh;

            // Token: 0x04000070 RID: 112
            public uint HostOS;

            // Token: 0x04000071 RID: 113
            public uint FileCRC;

            // Token: 0x04000072 RID: 114
            public uint FileTime;

            // Token: 0x04000073 RID: 115
            public uint UnpVer;

            // Token: 0x04000074 RID: 116
            public uint Method;

            // Token: 0x04000075 RID: 117
            public uint FileAttr;

            // Token: 0x04000076 RID: 118
            [MarshalAs(UnmanagedType.LPStr)]
            public string CmtBuf;

            // Token: 0x04000077 RID: 119
            public uint CmtBufSize;

            // Token: 0x04000078 RID: 120
            public uint CmtSize;

            // Token: 0x04000079 RID: 121
            public uint CmtState;

            // Token: 0x0400007A RID: 122
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public uint[] Reserved;
        }

        // Token: 0x02000019 RID: 25
        public struct RAROpenArchiveData
        {
            // Token: 0x06000068 RID: 104 RVA: 0x000034ED File Offset: 0x000016ED
            public void Initialize()
            {
                this.CmtBuf = new string('\0', 65536);
                this.CmtBufSize = 65536U;
            }

            // Token: 0x0400007B RID: 123
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string ArcName;

            // Token: 0x0400007C RID: 124
            public uint OpenMode;

            // Token: 0x0400007D RID: 125
            public uint OpenResult;

            // Token: 0x0400007E RID: 126
            [MarshalAs(UnmanagedType.LPStr)]
            public string CmtBuf;

            // Token: 0x0400007F RID: 127
            public uint CmtBufSize;

            // Token: 0x04000080 RID: 128
            public uint CmtSize;

            // Token: 0x04000081 RID: 129
            public uint CmtState;
        }

        // Token: 0x0200001A RID: 26
        public struct RAROpenArchiveDataEx
        {
            // Token: 0x06000069 RID: 105 RVA: 0x0000350C File Offset: 0x0000170C
            public void Initialize()
            {
                this.CmtBuf = new string('\0', 65536);
                this.CmtBufSize = 65536U;
                this.Reserved = new uint[32];
            }

            // Token: 0x04000082 RID: 130
            [MarshalAs(UnmanagedType.LPStr)]
            public string ArcName;

            // Token: 0x04000083 RID: 131
            [MarshalAs(UnmanagedType.LPWStr)]
            public string ArcNameW;

            // Token: 0x04000084 RID: 132
            public uint OpenMode;

            // Token: 0x04000085 RID: 133
            public uint OpenResult;

            // Token: 0x04000086 RID: 134
            [MarshalAs(UnmanagedType.LPStr)]
            public string CmtBuf;

            // Token: 0x04000087 RID: 135
            public uint CmtBufSize;

            // Token: 0x04000088 RID: 136
            public uint CmtSize;

            // Token: 0x04000089 RID: 137
            public uint CmtState;

            // Token: 0x0400008A RID: 138
            public uint Flags;

            // Token: 0x0400008B RID: 139
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public uint[] Reserved;
        }

        // Token: 0x0200001B RID: 27
        // (Invoke) Token: 0x0600006B RID: 107
        private delegate int UNRARCallback(uint msg, int UserData, IntPtr p1, int p2);
    }
}
