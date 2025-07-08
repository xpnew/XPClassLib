using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XP.Win.Components.Monitors
{
    public partial class BasicMonitorWin : Form
    {

        public List<BaseMonitorItemWin> ItemLst = new List<BaseMonitorItemWin>();
        public BasicMonitorWin()
        {
            x.Say("无参构造函数");
            InitializeComponent();
        }

        public BasicMonitorWin(List<BaseMonitorItemWin> itemLst):this()
        {
            x.Say("有参构造函数.....");
            this.ItemLst = itemLst;

            foreach (BaseMonitorItemWin item in itemLst)
            {
                ShowItem(item);
            }        
        }

        public void Add(BaseMonitorItemWin item)
        {
            ItemLst.Add(item);
            ShowItem(item);
        }

        protected virtual void ShowItem(BaseMonitorItemWin item)
        {
            item.MdiParent = this;
            item.Show();
        }

    }
}
