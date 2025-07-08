using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XP.Util;
using XP.Win.Components.Monitors;

namespace XP.Win.Components.MonitorsTest
{
    public partial class MainTestWin : Form
    {
        public MainTestWin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var win = new BasicMonitorWin();
            win.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<BaseMonitorItemWin> SubList = new List<BaseMonitorItemWin>();
            BaseMonitorItemWin  ItemWin = new BaseMonitorItemWin();

            ItemWin.Add("默认文字");
            ItemWin.Add("默认文字2");
            ItemWin.Add(new LevelLogMsg() { 
                 Level = DebugLevel.Warn,
                 Title= "警告信息 测试"
            });
            SubList.Add(ItemWin);
            var win = new BasicMonitorWin(SubList);
            win.Show();

        }
    }
}
