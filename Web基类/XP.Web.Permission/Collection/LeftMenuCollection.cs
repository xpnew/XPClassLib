using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.DB.Models;

namespace XP.Web.Permission
{
    [Serializable]
    public class LeftMenuCollection : System.Collections.IEnumerable
    {
        private Dictionary<int, List<Sys_PageT>> mItems = new Dictionary<int, List<Sys_PageT>>();

        public Dictionary<int, List<Sys_PageT>> Items
        {
            get { return mItems; }
            set { mItems = value; }
        }

        public void Add(int id, List<Sys_PageT> item)
        {
            if (Items.ContainsKey(id))
            {
                Items[id] = item;
            }
            else
            {
                Items.Add(id, item);
            }
        }

        //作为父级对象的引用
        public UserDataItem4Session ParentCache { get; set; }
        public List<Sys_PageT> this[int index]
        {
            get
            {
                if (Items.ContainsKey(index))
                {
                    return Items[index];
                }
                else
                {
                    //2012年12月20日，改为缓存全部数据，所以下面的代码拿掉了。

                    /*

                    //因为不要在登录之后直接缓存全部菜单，而是要在首次请求的时候缓存，所以增加了下面的代码。
                    if (ParentCache.TopMenu.Where(u => u.PageID.Value == index).Count() > 0)
                    {
                        string langCookieSettion = CookiesManage.GetCookie(CookiesManage.NameOfLanguage);
                        int userid = ParentCache.UserInfo.UserID;
                        
                        ISysService SysService = ControllerUtility.GetSysService();
                        List<View_Page> list = SysService.GetPageByUserID(ParentCache.UserInfo.UserID, CookiesManage.GetCookie(CookiesManage.NameOfLanguage), index);
                        ParentCache.LeftMenu.Add(index, list);

                        //添加完左侧菜单之后缓存菜单的页面权限
                        foreach (View_Page leftItem in list)
                        {
                            List<View_RightGlobal> list4PageRight;
                            list4PageRight = SysService.GetRightByUserID(userid,1, langCookieSettion, leftItem.PageID.Value);
                            if (0 != list4PageRight.Count)
                            {
                                ParentCache.PageRight.Add(leftItem.PageID.Value, list4PageRight);
                            }
                        }


                        return list;
                    }
                    */
                    return null;
                }
            }
        }


        public bool ExistPageController(string controller)
        {
            /*
            bool  s = from u in this.Items
                    where u.Value.Any(o => o.PageController.Equals(controller))
                    select u;  
      
            */
            bool HasController = (from o in this.ParentCache.AllPageList

                                  where o.PageController.ToLower().Equals(controller.ToLower())
                                  select o).Any();

            return HasController;
            //return false;
        }
        public Sys_PageT GetPageByController(string controller)
        {
            var s = from u in this.Items
                    from o in u.Value
                    where o.PageController.ToLower().Equals(controller.ToLower())
                    select o;
            if (!s.Any())
            {
                return s.First();
            }
            return null;
        }

        //(!openWith.ContainsKey

        public IEnumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }

    }
}
