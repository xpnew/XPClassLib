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
    public class PageRightCollection
    {
        private Dictionary<int, List<Sys_RightV>> mItems = new Dictionary<int, List<Sys_RightV>>();

        public Dictionary<int, List<Sys_RightV>> Items
        {
            get { return mItems; }
            set { mItems = value; }
        }

        public void Add(int id, List<Sys_RightV> item)
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
        public List<Sys_RightV> this[int index]
        {
            get
            {
                if (Items.ContainsKey(index))
                {
                    return Items[index];
                }
                else
                {

                    bool HasLeft = false;
                    var list4LeftMenu = from u in ParentCache.LeftMenu.Items

                                        select u;

                    /*

                    //因为不要在登录之后直接缓存全部菜单，相应的也就是不缓存全部的页面权限。
                    //变成要在首次请求的时候缓存，所以增加了下面的代码。
                    if (ParentCache.TopMenu.Where(u => u.PageID.Value == index).Count() > 0)
                    {
                        
                        ISysService SysService = ControllerUtility.GetSysService();
                        List<View_RightGlobal> list;
                        //List<View_RightGlobal> list = SysService.GetPageByUserID(ParentCache.UserInfo.UserID, CookiesManage.GetCookie(CookiesManage.NameOfLanguage), index);
                        //因为获取权限的方法没有完成，这里只用一个空的对象代替。
                        list = new List<View_RightGlobal>();
                        ParentCache.PageRight.Add(index, list);
                        return list;
                    }
                    */
                    return null;
                }
            }
        }

        public List<Sys_RightV> this[string controller]
        {
            get
            {
                //从左边的菜单获取页面，读取不到3级菜单
                //var query = from u in ParentCache.LeftMenu.Items
                //           from o in u.Value
                //           from p in this.Items
                //           from w in p.Value
                //            where o.PageController.ToLower().Equals(controller.ToLower()) && o.PageID == w.PageID
                //           select w;
                //改为从所有的页面读取
                var query = from page in ParentCache.AllPageList
                                //from o in u.Value
                                //from p in this.Items
                                //from w in p.Value
                            from right in ParentCache.AllRightList
                            where page.PageController !=null &&  page.PageController.ToLower().Equals(controller.ToLower()) && page.Id == right.PageId
                            select right;


                if (0 < query.Count())
                {
                    return query.ToList();
                }
                return null;
            }
        }
        //检查缓存当中是否存在某个
        public bool EixstPageAction(string controller, string action)
        {
            var list = this[controller];
            if (null == list)
            {
                return false;
            }

            var result = (from u in list
                          where u.RightOperate.ToLower().Equals(action.ToLower())
                          select u).Any();
            return result;
        }

        public Sys_RightV GetRight(string controller, string action)
        {
            var list = this[controller];
            if (null == list)
            {
                return null;
            }

            var rightList = from u in list
                            where u.RightOperate.ToLower().Equals(action.ToLower())
                            select u;
            if (rightList.Any())
            {
                return rightList.First();
            }
            else
            {
                return null;
            }
        }

        public Sys_RightV GetRight(string area,string controller, string action)
        {
            var rightList = from u in ParentCache.AllRightList
                            where u.RightOperate.ToLower().Equals(action.ToLower()) &&
                                  u.PageArea.ToLower() == area.ToLower()&&
            u.PageController.ToLower() == controller.ToLower()
                            select u;
            if (rightList.Any())
            {
                return rightList.First();
            }
            else
            {
                return null;
            }
        }
        public IEnumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }

    }
}
