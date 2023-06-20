using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XP.Win.WaitWin
{
    public partial class WaitInfoGUI : Form
    {
        public WaitWinResult WinResult { get; set; } = new WaitWinResult();

        private IAsyncResult threadResult;

        protected WaitWindows _Parent;
        internal Exception _Error;
        internal object _Result;

        private delegate T FunctionInvoker<T>();


        public bool IsCanceled { get; set; }
        public WaitInfoGUI(WaitWindows p)
        {
            InitializeComponent();
            this._Parent = p;
        }


        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            //	Paint a 3D border
            ControlPaint.DrawBorder3D(e.Graphics, this.ClientRectangle, Border3DStyle.Raised);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            //   Create Delegate
            FunctionInvoker<object> threadController = new FunctionInvoker<object>(this.DoWork);

            //   Execute on secondary thread.
            this.threadResult = threadController.BeginInvoke(this.WorkComplete, threadController);
        }


        protected object DoWork()
        {
            WaitWinEventArgs e = new WaitWinEventArgs(this._Parent, this._Parent._Args);
            if ((this._Parent._WorkerMethod != null))
            {
                this._Parent._WorkerMethod(this, e);
            }
            return e.Result;

        }
        private void WorkComplete(IAsyncResult results)
        {
            if (!this.IsDisposed)
            {
                if (this.InvokeRequired)
                {
                    //if (results.IsCompleted)
                    //{
                    //    return;
                    //}
                    //if (this.IsDisposed)
                    //{
                    //    return;
                    //}
                    try
                    {
                        this.Invoke(new WaitWindows.MethodInvoker<IAsyncResult>(this.WorkComplete), results);

                    }
                    catch (Exception ex)
                    {
                        this._Error = ex;
                    }
                }
                else
                {
                    //	Capture the result
                    try
                    {
                        WinResult.IsCanceled = true;
                        WinResult.WorkTime = WinResult.CreateTime - DateTime.Now;

                        this._Result = ((FunctionInvoker<object>)results.AsyncState).EndInvoke(results);
                    }
                    catch (Exception ex)
                    {
                        //	Grab the Exception for rethrowing after the WaitWindow has closed.
                        this._Error = ex;
                    }
                    this.Close();
                }
            }
        }

        private void bt_Cancel_Click(object sender, EventArgs e)
        {
            IsCanceled = true;
            WinResult.IsCanceled = true;
            WinResult.WorkTime = WinResult.CreateTime - DateTime.Now;
            _Parent.CancelEvent?.Invoke(sender, e);

        }


        internal void SetMessage(string msg)
        {
            this.Label_Message.Text = msg;
        }

        internal void CloseDialog()
        {
            this.Invoke(new MethodInvoker(this.Close), null);
        }
    }

}


