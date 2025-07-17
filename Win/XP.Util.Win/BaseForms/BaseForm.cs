using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XP.Util.Win.BaseForms;

namespace XP.Util.Win
{
    public class BaseForm : Form
    {

        public Action<BaseForm,FormCloseEventArgs> CloseEvent;

        protected void Alert(string str)
        {
            MessageBox.Show(str);
        }
        protected bool CheckNull(string inputString, string v, string tm = " {0} 不能为空")
        {
            if (String.IsNullOrEmpty(inputString))
            {
                Alert(String.Format(tm,v));
                return true;
            }
            return false;
        }

        public FormResultDef FormResult { get; set; } = FormResultDef.Default;


        
    }
}
