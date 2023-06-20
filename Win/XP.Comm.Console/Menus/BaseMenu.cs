using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Comm;
using XP.Comm.Msgs;
using XP.Util;

namespace XP.Comm.Console.Menus
{
    /// <summary>
    /// 带返回的调用
    /// </summary>
    /// <remarks>
    /// 执行结束返回菜单。
    /// </remarks>
    /// <returns></returns>
    public delegate CommMsg MenuCallFunction();

    /// <summary>
    /// 不带返回的调用
    /// </summary>
    /// <remarks>
    /// 控制权交给调用过程，然后不返回菜单
    /// 在VB当中，用sub和function区别带不带返回的过程调用
    /// </remarks>
    public delegate void MenuCallSub();


    /// <summary>
    /// 菜单调用类型
    /// </summary>
    public enum MenuCallTypeDef
    {
        Function = 2,

        Sub = 1,
    }
    /// <summary>
    /// 在控制台显示基本的菜单
    /// </summary>

    public class BaseMenu : BaseEntityVSMsg<CommMsg>
    {
        public List<BaseMenuItem> Items { get; set; }


        public string InputResult { get; set; }


        public string MenuName { get; set; }

        /// <summary>
        /// 连续输入错误的次数
        /// </summary>
        public int ErrorInputCount { get; set; }

        public BaseMenu() : base()
        {
            Items = new List<BaseMenuItem>();
            ErrorInputCount = 0;
        }

        public void AddFunction(string txt, MenuCallFunction call)
        {
            var item = new BaseMenuItem(txt, call);
            Items.Add(item);
        }
        public void AddSub(string txt, MenuCallSub call)
        {
            var item = new BaseMenuItem(txt, call);
            Items.Add(item);
        }

        protected void ShowMenu()
        {
            if (!String.IsNullOrEmpty(MenuName))
            {
                c.Write("【" + MenuName + "】");
            }
            c.Say("请选择一个菜单（q退出）：");
            for (int i = 0; i < Items.Count; i++)
            {
                c.Say((i + 1) + "、" + Items[i].MenuText);
            }


        }



        protected void WaitInput()
        {
            string input = c.ReadLine();
            if ("q" == input.ToLower())
            {
                InputResult = input;
                return;
            }
            if (2 < ErrorInputCount)
            {
                if ("cls" == input.ToLower())
                {
                    c.Clear();
                    Show();
                    return;
                }
            }

            if (!vbs.IsInt(input))
            {
                ErrorInputCount++;
                string NoticeText = "输入有误重新输入";

                if (2 < ErrorInputCount)
                {
                    NoticeText += "(输入cls清空并且重新显示菜单)";
                }
                c.Say(NoticeText);


                WaitInput();
                return;
            }

            int ChoosNum = int.Parse(input);
            if (ChoosNum < 1 || ChoosNum > Items.Count)
            {
                ErrorInputCount++;
                string NoticeText = "输入有误重新输入";

                if (2 < ErrorInputCount)
                {
                    NoticeText += "(输入cls清空并且重新显示菜单)";
                }
                c.Say(NoticeText);

                WaitInput();
                return;
            }

            var item = Items[ChoosNum - 1];

            if (item.CallType == MenuCallTypeDef.Function)
            {
                var msg = item.FunctionCall();


                //=0 输入Exit退出子程序
                //大于0 输入Quit退出
                //小于0 
                if (0 == msg.StatusCode)
                {
                    this.Msg.AddLog(msg);
                    Show();
                }

                if (0 > msg.StatusCode)
                {
                    MsgBase.Copy(msg, this.Msg);
                    return;
                }
            }
            else
            {
                item.SubCall();
            }

            InputResult = input;
        }

        public void Show()
        {
            ShowMenu();
            ErrorInputCount = 0;

            WaitInput();
        }

    }
}
