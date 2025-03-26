using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XP.Compress.Rar.Schematrix;
using XP.Util.Loger;

namespace XP.Compress.Rar
{
    /// <summary>
    /// 旧版，直接从Dll当中获取的，存在bug
    /// </summary>
    public class GuestPassword
    {
        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000057 RID: 87 RVA: 0x000031C5 File Offset: 0x000013C5
        // (set) Token: 0x06000058 RID: 88 RVA: 0x000031CD File Offset: 0x000013CD
        public string PhysicalFilePath { get; set; }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000059 RID: 89 RVA: 0x000031D6 File Offset: 0x000013D6
        // (set) Token: 0x0600005A RID: 90 RVA: 0x000031DE File Offset: 0x000013DE
        public string PhysicalExtractDir { get; set; }

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x0600005B RID: 91 RVA: 0x000031E7 File Offset: 0x000013E7
        // (set) Token: 0x0600005C RID: 92 RVA: 0x000031EF File Offset: 0x000013EF
        public string[] Passwords { get; set; }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x0600005D RID: 93 RVA: 0x000031F8 File Offset: 0x000013F8
        // (set) Token: 0x0600005E RID: 94 RVA: 0x00003200 File Offset: 0x00001400
        public bool HasFind { get; set; }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x0600005F RID: 95 RVA: 0x00003209 File Offset: 0x00001409
        // (set) Token: 0x06000060 RID: 96 RVA: 0x00003211 File Offset: 0x00001411
        public string RealPwd { get; set; }

        // Token: 0x06000061 RID: 97 RVA: 0x0000321A File Offset: 0x0000141A
        public GuestPassword(string phyFilePath, string phyDir, string[] pwds)
        {
            this.PhysicalFilePath = phyFilePath;
            this.PhysicalExtractDir = phyDir;
            this.Passwords = pwds;
            this.HasFind = false;
            this.RealPwd = string.Empty;
        }

        /// <summary>
        /// 对外传递消息
        /// </summary>
        public Action<string> SayMsg;

        // Token: 0x06000062 RID: 98 RVA: 0x00003257 File Offset: 0x00001457
        public void Start()
        {
            this.UnRar(this.PhysicalFilePath, this.PhysicalExtractDir);
        }

        // Token: 0x06000063 RID: 99 RVA: 0x00003270 File Offset: 0x00001470
        private bool UnRar(string zipFile, string destFolder)
        {
            bool flag = !File.Exists(zipFile);
            bool flag2;
            if (flag)
            {
                flag2 = false;
            }
            else
            {
                bool flag3 = !Directory.Exists(destFolder);
                if (flag3)
                {
                    Directory.CreateDirectory(destFolder);
                }
                Unrar unRar = new Unrar();
                unRar.PasswordRequired += this.UnRar_PasswordRequired;
                try
                {
                    unRar.DestinationPath = destFolder;
                    unRar.Open(zipFile, Unrar.OpenMode.Extract);
                    while (unRar.ReadHeader())
                    {
                        unRar.Extract();
                    }
                    flag2 = true;
                }
                catch (Exception ex)
                {
                    LogHelper.Instance.Debuglog(ex.ToString(), "_Debuglog.txt");
                    flag2 = false;
                }
                finally
                {
                    bool flag4 = unRar != null;
                    if (flag4)
                    {
                        unRar.Close();
                        unRar.Dispose();
                    }
                }
            }
            return flag2;
        }

        // Token: 0x06000064 RID: 100 RVA: 0x00003344 File Offset: 0x00001544
        public void StartTest()
        {
            bool flag = !File.Exists(this.PhysicalFilePath);
            if (!flag)
            {
                bool flag2 = this.Passwords == null || this.Passwords.Length == 0;
                if (!flag2)
                {
                    Unrar unRar = null;
                    bool IsFine = true;
                    try
                    {
                        unRar = new Unrar();
                        unRar.Open(this.PhysicalFilePath, Unrar.OpenMode.Extract);
                        while (unRar.ReadHeader() && IsFine)
                        {
                            if (IsBroken) break;
                            string CurrentPwd = this.Passwords[this._PwdIndex];
                            bool PwdRight = unRar.TestPassWord(CurrentPwd);
                            SayMsg?.Invoke("正在验证密码： " + CurrentPwd);
                            x.Say("正在验证密码： " + CurrentPwd);
                            //bool flag3 = PwdRight;
                            if (PwdRight)
                            {
                                this.HasFind = true;
                                this.RealPwd = CurrentPwd;
                                x.Say("找到密码："  +  CurrentPwd);
                                IsFine = false;
                                break;
                            }
                            this._PwdIndex++;
                            bool flag4 = this._PwdIndex >= this.Passwords.Length;
                            if (flag4)
                            {
                                IsFine = false;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        x.SayError("Rar 出现了异常（可能是需要使用64位模式）："+ex);
                        Loger.Error("RAR 出现了异常（可能是需要使用64位模式）", ex);
                    }
                    finally
                    {
                        bool flag5 = unRar != null;
                        if (flag5)
                        {
                            unRar.Close();
                        }
                    }
                    //多次读取 RAR.ReadHeader()会返回异常，异常的时候TestPassWord()会返回错误的结果，需要重新开始
                    if (IsFine)
                    {
                        StartTest();
                    }
                }
            }
        }      
        
        public void StartTest(CancellationToken token)
        {
            bool flag = !File.Exists(this.PhysicalFilePath);
            if (!flag)
            {
                bool flag2 = this.Passwords == null || this.Passwords.Length == 0;
                if (!flag2)
                {
                    Unrar unRar = null;
                    bool IsFine = true;
                    try
                    {
                        unRar = new Unrar();
                        unRar.Open(this.PhysicalFilePath, Unrar.OpenMode.Extract);
                        while (unRar.ReadHeader() && IsFine)
                        {   
                            if(token.IsCancellationRequested) { break; }
                            string CurrentPwd = this.Passwords[this._PwdIndex];
                            bool PwdRight = unRar.TestPassWord(CurrentPwd);
                            x.Say("正在验证密码： " + CurrentPwd);
                            //bool flag3 = PwdRight;
                            if (PwdRight)
                            {
                                this.HasFind = true;
                                this.RealPwd = CurrentPwd;
                                x.Say("找到密码："  +  CurrentPwd);
                                IsFine = false;
                                break;
                            }
                            this._PwdIndex++;
                            bool flag4 = this._PwdIndex >= this.Passwords.Length;
                            if (flag4)
                            {
                                IsFine = false;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        x.SayError("Rar 出现了异常（可能是需要使用64位模式）："+ex);
                        Loger.Error("RAR 出现了异常（可能是需要使用64位模式）", ex);
                    }
                    finally
                    {
                        bool flag5 = unRar != null;
                        if (flag5)
                        {
                            unRar.Close();
                        }
                    }
                    //多次读取 RAR.ReadHeader()会返回异常，异常的时候TestPassWord()会返回错误的结果，需要重新开始
                    if (IsBroken) { return; }
                    if (IsFine)
                    {
                        StartTest(token);
                    }
                }
            }
        }

        public bool IsBroken { get; set; } = false;


        public void OnBroken()
        {
            IsBroken = true;
        }


        // Token: 0x06000065 RID: 101 RVA: 0x00003464 File Offset: 0x00001664
        private void UnRar_PasswordRequired(object sender, PasswordRequiredEventArgs e)
        {
            bool flag = this.Passwords == null || this.Passwords.Length == 0;
            if (flag)
            {
                e.ContinueOperation = false;
            }
            else
            {
                e.Password = this.Passwords[this._PwdIndex];
                e.ContinueOperation = true;
            }
        }

        // Token: 0x04000030 RID: 48
        protected int _PwdIndex = 0;
    }
}
