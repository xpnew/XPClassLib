using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XP.Browser.Cef109.BrowserWins;

namespace XP.Browser.Cef109.Handlers
{
    /// <summary>
    /// 简单生命周期处理器，为弹出的新窗口添加快捷键绑定  SimplyLifeSpanHandler 的摘要说明
    /// </summary>
    /// <remarks>
    /// 作    者：Administrator
    /// 创建时间：2026/10/10 4:25:40	
    /// 
    /// </remarks>
    public class SimplyLifeSpanHandler : ILifeSpanHandler
    {


        public Form BorderForm { get; set; }


        public bool OnBeforePopup(
        IWebBrowser browserControl, IBrowser browser, IFrame frame,
        string targetUrl, string targetFrameName,
        WindowOpenDisposition targetDisposition, bool userGesture,
        IPopupFeatures popupFeatures, IWindowInfo windowInfo,
        IBrowserSettings browserSettings, ref bool noJavascriptAccess,
        out IWebBrowser newBrowser)
        {
            if(null == BorderForm || BorderForm.IsDisposed)
            {
                //BorderForm = null;
                var  form = new BaseBrowserWin(targetUrl)
                {
                    Width = 800,
                    Height = 600,
                    StartPosition = FormStartPosition.CenterScreen
                };

                var brs = form.MainBrowser;
                brs.LifeSpanHandler = this; // 关键：将生命周期处理器设置为当前实例
                brs.KeyboardHandler = new GlobalKeyboardHandler(); // 关键：设置键盘处理器

                BorderForm = form;
            }
               
           
            BorderForm.Show();

            newBrowser = null; // 关键：返回 true 时必须为 null
            return true; // 取消默认弹出
        }

        public void OnAfterCreated(IWebBrowser b, IBrowser br) { }
        public bool DoClose(IWebBrowser b, IBrowser br) => false;
        public void OnBeforeClose(IWebBrowser b, IBrowser br) { }
    }
}
