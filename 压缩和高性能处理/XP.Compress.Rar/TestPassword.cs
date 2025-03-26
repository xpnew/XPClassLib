using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Compress.Rar.Schematrix;

namespace XP.Compress.Rar
{
    /// <summary>
    /// 测试密码
    /// </summary>
    public class TestPassword
    {
        public string PhysicalFilePath { get; set; }
        public string PhysicalExtractDir { get; set; }
        public bool HasFind { get; set; }
        public string RealPwd { get; set; }

        protected int _PwdIndex = 0;

        public TestPassword(string phyFilePath, string phyDir)
        {
            this.PhysicalFilePath = phyFilePath;
            this.PhysicalExtractDir = phyDir;
            this.HasFind = false;
            this.RealPwd = string.Empty;
        }

        public void StartTest(string[] pwds)
        {
            bool flag = !File.Exists(this.PhysicalFilePath);
            if (!flag)
            {
                bool flag2 = pwds == null || pwds.Length == 0;
                if (!flag2)
                {
                    Unrar unRar = null;
                    bool IsFine = true;
                    try
                    {
                        unRar = new Unrar();
                        unRar.Open(this.PhysicalFilePath, Unrar.OpenMode.Extract);
                        if (unRar.ReadHeader() && IsFine)
                        {
                            while (true)
                            {
                                string CurrentPwd = pwds[this._PwdIndex];
                                bool PwdRight = unRar.TestPassWord(CurrentPwd);
                                x.Say("正在验证密码： " + CurrentPwd);
                                bool flag3 = PwdRight;
                                if (flag3)
                                {
                                    this.HasFind = true;
                                    this.RealPwd = CurrentPwd;
                                    x.Say("找到密码：" + CurrentPwd);
                                    break;
                                }
                                this._PwdIndex++;
                                bool flag4 = this._PwdIndex >= pwds.Length;
                                if (flag4)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        x.SayError("Rar 出现了异常（可能是需要使用64位模式）：" + ex);
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
                }
            }
        }



    }
}
