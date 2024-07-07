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


        public FormResultDef FormResult { get; set; } = FormResultDef.Default;



    }
}
