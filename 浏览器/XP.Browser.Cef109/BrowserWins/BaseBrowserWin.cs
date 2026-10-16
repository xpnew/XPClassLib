using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XP.Browser.Cef109.Filters;
using XP.Browser.Cef109.Handlers;


namespace XP.Browser.Cef109.BrowserWins
{
    public partial class BaseBrowserWin : Form
    {
        public string Url { get; set; }
        protected ChromiumWebBrowser chromeBrowser;

        protected TextBox txtAddress;

        public ChromiumWebBrowser MainBrowser
        {
            get { return chromeBrowser; }
        }
        public BaseBrowserWin(string url):this()
        {
            Url = url;
            txtAddress = new TextBox { Dock = DockStyle.Top };
            txtAddress.Text = url;
            Controls.Add(txtAddress);
            InitializeBrowser();
            _Init();
        }
        public BaseBrowserWin()
        {
            InitializeComponent();
        }

        protected virtual void _Init()
        {

        }

        public void LoadUrl(string url)
        {
            Url = url;
            if (chromeBrowser != null && !chromeBrowser.IsDisposed)
            {
                chromeBrowser.Load(url);
            }
            else
            {
                InitializeBrowser();
            }
        }
        // 窗体加载时，先不让地址栏拿焦点
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            chromeBrowser.Focus();
        }


        protected void InitializeBrowser()
        {

            chromeBrowser = new ChromiumWebBrowser(Url)
            {
                Dock = DockStyle.Fill,
            };
            //chromeBrowser.BrowserSettings.FileAccessFromFileUrls = CefState.Enabled;
            //chromeBrowser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Enabled;

            this.Controls.Add(chromeBrowser);

            //var Filter = new PicFilter()
            //{
            //    MinSize = 50 * 1024, // 50KB    
            //};
            //chromeBrowser.RequestHandler = new AllRequestsMonitorHandler()
            //{
            //    RequestFileFilter = Filter

            //};

            chromeBrowser.TitleChanged += ChromeBrowser_TitleChanged;
            chromeBrowser.AddressChanged += (s, e) =>
            {
                txtAddress.Invoke(new Action(() =>
                {
                    if (!txtAddress.Focused)
                        txtAddress.Text = e.Address;
                }));
            };
            chromeBrowser.LoadingStateChanged += async (sender, args) =>
            {
                if (!args.IsLoading)
                {
                    try
                    {
                        if (chromeBrowser == null || chromeBrowser.IsDisposed)
                            return;

                        this.Invoke(new Action(() =>
                        {
                            if (!chromeBrowser.IsDisposed)
                                chromeBrowser.Focus();
                        }));

                        // 加一个小延迟，确保 V8 上下文完全就绪
                        await Task.Delay(500);
                        if (chromeBrowser == null || chromeBrowser.IsDisposed)
                            return;
                        // 此时页面静态资源已加载完，再确认一下 DOM 状态
                        var response = await chromeBrowser.EvaluateScriptAsync("document.readyState");

                        if (response.Success && response.Result?.ToString() == "complete")
                        {
                            // 页面基本加载完成，可以开始提取图片 URL 了
                            //Console.WriteLine("页面基本加载完成");
                            //x.Say("页面基本加载完成");




                        }
                    }
                    catch(Exception ex)
                    {
                        Loger.Error("页面加载完成之后的处理出现异常", ex);
                    }

                
                }
            };

        }

        private void ChromeBrowser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            // throw new NotImplementedException();

           
            this.Invoke(new Action(() =>
            {
                this.Text = e.Title;
            }));
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 释放 CEF 资源
            //Cef.Shutdown();

            // 2. 释放浏览器实例
            chromeBrowser.Dispose();

            base.OnFormClosing(e);

        }


    }
}
