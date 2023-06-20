using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Win.Components
{
    /// <summary>  
    /// 声明委托  
    /// </summary>  
    /// <param name="a">委托传递的参数</param>  
    public delegate void SendWinLogEventHander(WinLogDetails log);


    public class WinLogDetails : XP.Comm.CommMsg
    {

        public WinLogDetails() : base() { }
        public WinLogDetails(object entity) : base(entity) { }
        public WinLogDetails(Type type) : base(type) { }
        public WinLogDetails(string name) : base(name) { }


        public Color FontColor { get; set; } = Color.Black;

        public Color BgColor { get; set; } = Color.AliceBlue;

    }
}
