using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Win.WaitWin
{
    public class WaitWinEventArgs : EventArgs
    {

		/// <summary>
		/// Initialises a new intance of the WaitWindowEventArgs class.
		/// </summary>
		/// <param name="GUI">The associated WaitWindow instance.</param>
		/// <param name="args">A list of arguments to be passed.</param>
		public WaitWinEventArgs(WaitWindows GUI, List<object> args) : base()
		{
			this._Window = GUI;
			this._Arguments = args;
		}

		private WaitWindows _Window;
		private List<object> _Arguments;
		private object _Result;

		public WaitWindows Window
		{
			get { return _Window; }
		}

		public List<object> Arguments
		{
			get { return _Arguments; }
		}

		public object Result
		{
			get { return _Result; }
			set { _Result = value; }
		}
	}
}

