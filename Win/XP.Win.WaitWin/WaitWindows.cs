using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XP.Win.WaitWin
{
    public class WaitWindows
    {
        #region 事件和异步消息


        internal delegate void MethodInvoker<T>(T parameter1);


        internal EventHandler<WaitWinEventArgs> _WorkerMethod;

        internal List<object> _Args;

        /// <summary>
    	/// Updates the message displayed in the wait window.
    	/// </summary>
		public string Message
        {
            set
            {
                this._GUI.Invoke(new MethodInvoker<string>(this._GUI.SetMessage), value);
            }
        }


        /// <summary>
        /// Cancels the work and exits the wait windows immediately
        /// </summary>
        public void CloseDialog()
        {
            this._GUI.Invoke(new MethodInvoker(this._GUI.CloseDialog), null);
        }


        #endregion


        #region 基本数据




        #endregion



        #region 界面功能
        protected WaitInfoGUI _GUI { get; set; }



        public EventHandler<EventArgs> CancelEvent;

        public EventHandler<EventArgs> PauseEvent;





        protected void OnGuiCancel(object o, EventArgs arg)
        {

            //WinResult.IsCanceled = true;
            //WinResult.WorkTime = WinResult.CreateTime - DateTime.Now;
            CancelEvent?.Invoke(o, arg);

        }

        #endregion


        #region 核心工作


        public WaitWinResult Show(EventHandler<WaitWinEventArgs> workerMethod, string message, List<object> args)
        {

            if (workerMethod == null)
            {
                throw new ArgumentException("No worker method has been specified.", "workerMethod");
            }
            else
            {
                this._WorkerMethod = workerMethod;
            }

            this._GUI = new WaitInfoGUI(this);
            this._GUI.Label_Message.Text = message;


            //	Call it
            this._GUI.ShowDialog();

            //	clean up
            Exception _Error = this._GUI._Error;
            this._GUI.Dispose();


            return _GUI.WinResult;

        }

        public WaitWinResult Show(EventHandler<WaitWinEventArgs> workerMethod, EventHandler<EventArgs> cancelMethod, EventHandler<EventArgs> pauseMethod, string message)
        {

            if (workerMethod == null)
            {
                throw new ArgumentException("No worker method has been specified.", "workerMethod");
            }
            else
            {
                this._WorkerMethod = workerMethod;
            }

            this._GUI = new WaitInfoGUI(this);
            this._GUI.Label_Message.Text = message;

            CancelEvent += cancelMethod;
            PauseEvent += pauseMethod;

            //	Call it
            this._GUI.ShowDialog();

            //	clean up
            Exception _Error = this._GUI._Error;
            this._GUI.Dispose();


            return _GUI.WinResult;

        }

        #endregion


    }
}
