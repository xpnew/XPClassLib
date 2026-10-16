using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XP.Browser.Cef109.Handlers
{
    /// <summary>
    ///  所有的浏览器 键盘快捷键处理器 GlobalKeyboardHandler 的摘要说明
    /// </summary>
    /// <remarks>
    /// 作    者：Administrator
    /// 创建时间：2026/10/10 4:23:44	
    /// 
    /// </remarks>
    public class GlobalKeyboardHandler : IKeyboardHandler
    {

        public bool OnPreKeyEvent(
       IWebBrowser browserControl,
       IBrowser browser,
       KeyType type,
       int windowsKeyCode,
       int nativeKeyCode,
       CefEventFlags modifiers,
       bool isSystemKey,
       ref bool isKeyboardShortcut)
        {
            if (type == KeyType.KeyUp)
            {
                // Ctrl+W 关闭窗口
                if (windowsKeyCode == (int)Keys.W &&
                    modifiers.HasFlag(CefEventFlags.ControlDown))
                {
                    var control = browserControl as ChromiumWebBrowser;
                    control?.Invoke(new Action(() =>
                    {
                        var form = control.FindForm();
                        form?.Close();
                    }));
                    return true;
                }

                // F12 打开开发者工具
                if (windowsKeyCode == (int)Keys.F12)
                {
                    var control = browserControl as ChromiumWebBrowser;
                    control?.ShowDevTools();
                    return true;
                }
                // 空格翻页
                //注意：加了 !ControlDown 判断，避免和 Ctrl+Space 冲突。如果页面有输入框，空格会被吃掉，需要的话可以再加个 isInputFocused 判断跳过。
                if (windowsKeyCode == (int)Keys.Space && !modifiers.HasFlag(CefEventFlags.ControlDown))
                {
                    browser.MainFrame.ExecuteJavaScriptAsync("window.scrollBy(0, window.innerHeight)");
                    return true;
                }

            }

            return false;
        }
        public bool OnKeyEvent(
       IWebBrowser browserControl,
       IBrowser browser,
       KeyType type,
       int windowsKeyCode,
       int nativeKeyCode,
       CefEventFlags modifiers,
       bool isSystemKey)
        {
            return false;
        }
    }
}
