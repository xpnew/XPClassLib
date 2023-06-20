using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Win.WaitWin
{
    public class WaitWinMng
    {


        public static WaitWinResult Show(EventHandler<WaitWinEventArgs> workerMethod, string message)
        {
            var ww = new WaitWindows();


            return ww.Show(workerMethod, message,new List<object>());

        }
        public static WaitWinResult Show(EventHandler<WaitWinEventArgs> workerMethod, EventHandler<EventArgs> cancelMechod, EventHandler<EventArgs> pauseMethod , string message= null)
        {
            var ww = new WaitWindows();


            return ww.Show(workerMethod, cancelMechod, pauseMethod, message);

        }


    }

}
