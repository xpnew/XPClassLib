using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using XP.Comm;
using XP.Comm.Msgs;
using XP.DB.Models;
using XP.DB.Models.UserModel;

namespace XP.Web.Permission.Controllers
{
    public class BMLoginPage:BMControllerRoot
    {

        private string NameOfLastLoginName = "CookiesName4LastLogin";
        private string mPageMessage;


        private WebUser _SessionUser;


        /// <summary>
        /// 使用用户管理
        /// </summary>
        public WebUser SessionUser
        {
            get
            {
                if (null == _SessionUser)
                {
                    _SessionUser = WebUser.CreateUser();
                }
                return _SessionUser;
            }
        }



        /// <summary>
        /// 登录页面默认界面
        /// </summary>
        /// <remarks>
        /// 新版本只提供了AJAX登录的方式
        /// </remarks>
        /// <returns></returns>
        public ActionResult Index()
        {

#if DEBUG
            ViewBag.DebugUser = "admin";
            ViewBag.DebugPwd = "111111";
            ViewBag.DebugDescode = "1111";
#else
            if (null != System.Web.HttpContext.Current.Request.Cookies[NameOfLastLoginName])
            {
                ViewBag.DebugUser = System.Web.HttpContext.Current.Request.Cookies[NameOfLastLoginName].Value;
            }
#endif

            //throw new System.ServiceModel.EndpointNotFoundException("sdddd");
            ViewBag.Message = mPageMessage;
            return View();
        }
        /// <summary>
        /// Ajax提交登录
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult AjaxPost(FormCollection collection)
        {

            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            MsgResult result = new MsgResult()
            {
                Name = "LoginPost",
                Body = "返回信息的主体。",
                StatusCode = 0//默认是0
            };
            res.Data = result;


            try
            {
                User_LoginInfo_ViewModel userinfo;
                bool IsLoged = false;
                int LoginStatusCode = 0;

                string username = collection["loginname"];

                string password = collection["loginpass"];

                password = Server.UrlDecode(password);

                string gencode = collection["gencode"];
                if (String.IsNullOrEmpty(gencode))
                {
#if DEBUG

#else
                    result.Body = "登录失败：验证码不能为空!";
                    result.StatusCode = -60103231;//失败
                    result.SetFail("登录失败：验证码不能为空!", -60103231);
                    return res;
#endif
                }
                string SessionGCC = Session["gencode"] as string;
                if (!String.IsNullOrEmpty(SessionGCC))
                {
                    if (gencode.ToLower() != SessionGCC.ToLower())
                    {
                        //开发期，暂时不要检查验证码
#if DEBUG

#else

                        //return SendAlert("验证码不正确！");
                        //LoginStatusCode = -60103232;
                        //result.Body = "登录失败：验证码不正确!";
                        //result.StatusCode = -60103232;
                        result.SetFail("登录失败：验证码不正确!", -60103232);

                        return res;
#endif
                    }
                }


                if (null == collection["loginpass"])
                {
                    //密码不能为空。
                    LoginStatusCode = -1105;
                    result.Body = "登录失败：密码不能为空!";
                    result.StatusCode = -1105;
                    return res;
                }
                if (null == collection["loginname"])
                {
                    //用户名为空
                    LoginStatusCode = -1104;
                    result.Body = "登录失败：用户名为空!";
                    result.StatusCode = -1104;
                    return res;
                }


                try
                {
                    #region 未加密的方式
                    //未加密的方式，因为现在的加密的方式，是不可重现的（因为每次加密都加入时间因子混淆），
                    //所以不能采用MD5的方式处理，即加完密的输入跟数据库里的字段比对。
                    var svc = this.UserService;
                    bool Return = svc.Login(username, password);
                    if (!Return)
                    {
                        res.Data = svc.Msg;
                        return res;
                    }

                    var MsgWithUser = svc.Msg as DataMsg<User_InfoV>;

                    var view = MsgWithUser.DataInfo;
                    if (null == view)
                    {
                        result.SetFail("出现意外，登录检查成功却没有获得用户信息。");
                        return res;
                    }

                    userinfo = svc.GetUserMode4Session(view);


                    #endregion
                }
                //catch (SA.DataObjects.Exceptions.CustomException ex)
                //{
                //    string LanguageMark = SA.WebUtil.Common.CookiesManage.GetLang();
                //    result.StatusCode = -1;
                //    result.Body = ex.Message;
                //    result.GlobalErrorMessage = GetErrorMsgeGlobal(ex, LanguageMark);
                //    return res;
                //}
                //catch (System.ServiceModel.FaultException<SA.DataObjects.FaultData> ex)
                //{
                //    string LanguageMark = SA.WebUtil.Common.CookiesManage.GetLang();
                //    result.StatusCode = -1;
                //    result.Body = ex.Message;
                //    result.GlobalErrorMessage = GetErrorMsgeGlobal(ex, LanguageMark);
                //    return res;
                //}
                catch (System.Net.WebException ex)
                {
                    result.Body = "登录失败：具体异常如下：" + ex.Message;
                    result.StatusCode = -60103291;//服务超时，稍后再试
                    return res;
                }
                //catch (System.ServiceModel.ServerTooBusyException ex)
                //{
                //    result.Body = "登录失败：具体异常如下：" + ex.Message;
                //    result.StatusCode = -60103291;//服务超时，稍后再试
                //    return res;
                //}
                //catch (System.ServiceModel.EndpointNotFoundException ex)
                //{
                //    result.Body = "登录失败：具体异常如下：" + ex.Message;
                //    result.StatusCode = -60103291;//服务超时，稍后再试
                //    return res;
                //}
                catch (Exception ex)
                {
                    string statuscode = ex.Message;
                    /*
                     1：用户名错误 -101   2:密码错误 -301  3:用户名被禁用 -201   
                     4:用户名为空 -10  5:密码为空  -11  6:所属商户没有系统  -401
                     * 
                     * */
                    //正常登录验证，验证失败返回的是数字
                    //if (System.Text.RegularExpressions.Regexs.IsNumber(statuscode))
                    //{
                    //    //TempData.Values["ErrorCode"] = ex.Message;
                    //    TempData.Add("ErrorCode", ex.Message);
                    //    ViewData.Add("ErrorCode", ex.Message);

                    //    userinfo = null;
                    //    result.Body = "登录失败：具体请看错误代码!";
                    //    result.StatusCode = int.Parse(statuscode);
                    //    return res;
                    //}
                    //else //返回的信息不是数字说明登录的过程出现了程序异常，例如“未将对象引用到实例”
                    //{

                    //    result.Body = "登录失败：具体异常如下：" + ex.Message;
                    //    result.StatusCode = -1;
                    //    return res;
                    //}
                    result.Body = "登录失败：具体异常如下：" + ex.Message;
                    result.StatusCode = -1;
                    return res;

                }
                finally
                {
                }


                IsLoged = true;
                try
                {
                    SessionUser.Login(userinfo);
                    //UpdateLoginTime(userinfo.UserID, userinfo.StoreID.Value);
                    HttpCookie NewCookie = System.Web.HttpContext.Current.Request.Cookies[NameOfLastLoginName];
                    if (null == NewCookie)
                    {
                        NewCookie = new HttpCookie(NameOfLastLoginName);
                        NewCookie.Value = userinfo.UserName;
                        Response.Cookies.Add(NewCookie);
                    }
                    else
                    {
                        NewCookie.Value = userinfo.UserName;

                        Response.AppendCookie(NewCookie);
                    }
                    //登录成功，在cookies当中存储登录名
                    //System.Web.HttpContext.Current.Request.Cookies[NameOfLastLoginName].Value = userinfo.UserName;
                    //System.Web.HttpContext.Current.Request.Cookies[NameOfLastLoginName].Expires = DateTime.Now.AddDays(10);
                    result.Body = "登录成功!";
                    result.StatusCode = 1;
                    return res;
                }
                catch (Exception ex)
                {
                    //
                    result.Body = "登录失败!登录程序在缓存用户数据时出错，具体信息如下：" + ex.Message;
                    result.StatusCode = -1;
                    return res;

                    //return SendAlert("登录程序在缓存用户数据时出错，具体信息如下：" + ex.Message);
                }
                finally
                {

                }





            }
            catch (Exception ex)
            {
                result.Body = "登录失败：具体异常如下：" + ex.Message;
                result.StatusCode = -1;
                return res;
            }
        }


        public ActionResult Timeout()
        {
            ViewBag.AlertMsg = "登录超时";//ControllerUtility.Transfer("Public_Login_LongTime");
            ViewBag.DumpUrl = SessionUser.LoginPage;

            return View("Alert");
        }


        public ActionResult CheckGraphCode()
        {
            bool valid = false;
            string code;
            if (!String.IsNullOrEmpty(Request["code"]))
            {
                code = Request["code"];
                string SessionGCC = Session["gencode"] as string;
                if (code == SessionGCC)
                {
                    valid = true;
                }
                else
                {
                    valid = false;
                }
                return Json(valid, JsonRequestBehavior.AllowGet);
            }
            else
            {
                valid = false;
                return Json(valid, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GraphicalCaptcha()
        {

            //ContentResult result = new ContentResult();
            //result.ContentType = "images/gif";
            //result.ContentEncoding = UTF8Encoding.UTF8;
            ////result.Content = html;
            //string SessionGCC = System.Web.ImageValidation.WriteImage(4, System.Web.ImageValidation.CodeRanges.NumericAndEnglishChar);
            //Session["gencode"] = SessionGCC.ToLower();
            return null;

        }
        public ActionResult GetCode()
        {
            ContentResult result = new ContentResult();
            result.ContentType = "images/jpg";
            result.ContentEncoding = UTF8Encoding.UTF8;
            //result.Content = html;
            var  imgCode = new Util.GdiPlus.ImgageCode();

            string SessionGCC = imgCode.CreateRandomCode(4);
            imgCode.CreateStream(SessionGCC);
            Session["gencode"] = SessionGCC.ToLower();

            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ContentType = "image/Jpeg";
            System.Web.HttpContext.Current.Response.BinaryWrite(imgCode.Stream.ToArray());

            return null;
        }

        public ActionResult ShowUserRigth()
        {
            var right = SessionUser.Cache.AllRightList;


            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            MsgResult result = new MsgResult()
            {
                Name = "LoginPost",
                Body = "返回信息的主体。",
                StatusCode = 0//默认是0
            };
            res.Data = right;

            return res;

        }

        public ActionResult Logout()
        {
            SessionUser.Logout();
            return new RedirectResult(SessionUser.LoginPage);
        }



    }
}
