using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XP.Comm.Task.Visual4Win.UT
{
    public partial class VisualTestMain : Form
    {
        public VisualTestMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var win = new TextShowForm();

            win.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var win = new Test4AddBottonOnTitle();
            win.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var win = new ThemeForm();
            win.Show();

        }
    }
}
