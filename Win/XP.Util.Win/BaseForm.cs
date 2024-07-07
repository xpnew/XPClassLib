using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XP.Util.Win
{
    public class BaseForm : Form
    {


        protected void Alert(string str)
        {
            MessageBox.Show(str);
        } 
    }
}
