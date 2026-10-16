using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Browser.Cef109.Handlers;

namespace XP.Browser.Cef109.BrowserWins
{
    /// <summary>
    /// 带快捷键支持的窗体WithKeyBorwser 的摘要说明
    /// </summary>
    /// <remarks>
    /// 作    者：Administrator
    /// 创建时间：2026/10/9 11:47:28	
    /// 
    /// </remarks>
    public class WithKeyBorwser: BaseBrowserWin
    {
        public WithKeyBorwser(string url) : base(url)
        {

        }
        public WithKeyBorwser() : base()
        {
        }

        protected override void _Init()
        {
            base._Init();

            // 添加键盘处理器

            chromeBrowser.KeyboardHandler = new GlobalKeyboardHandler();
            chromeBrowser.LifeSpanHandler = new SimplyLifeSpanHandler(); // 处理弹出窗口
            _AfterInitKeys();
        }

        protected virtual void _AfterInitKeys()
        {
            // 子类可以重写这个方法，添加额外的键盘处理逻辑   
        }
    }
}
