using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Console.Menus
{
    public class BaseMenuItem
    {

        public MenuCallTypeDef CallType { get; set; }

        public string MenuText { get; set; }

        public MenuCallFunction FunctionCall { get; set; }

        public MenuCallSub SubCall { get; set; }


        public BaseMenuItem(string txt, MenuCallFunction call)
        {
            MenuText = txt;
            FunctionCall = call;
            CallType = MenuCallTypeDef.Function;
        }

        public BaseMenuItem(string txt, MenuCallSub call)
        {
            MenuText = txt;
            SubCall = call;
            CallType = MenuCallTypeDef.Sub;
        }

        //public void MenuCall()
        //{

        //}


    }
}
