using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XP.Comm.Msgs;

namespace XP.Web.Permission.Controllers
{
    public class DevLoginPage: BMControllerRoot
    {

        private string NameOfLastLoginName = "CookiesName4LastLogin";

        private string mPageMessage;

        private DevUser _SessionUser;


        /// <summary>
        /// 使用用户管理
        /// </summary>
        public DevUser SessionUser
        {
            get
            {
                if (null == _SessionUser)
                {
                    _SessionUser = DevUser.CreateUser();
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
            ViewBag.DebugUser = "administrator";
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
                Name = "MerchantDelete",
                Body = "返回信息的主体。",
                StatusCode = 0//默认是0
            };
            res.Data = result;


            try
            {
                DB.Models.UserModel.User_LoginInfo_ViewModel userinfo;
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
                        LoginStatusCode = -60103232;
                        result.Body = "登录失败：验证码不正确!";
                        result.StatusCode = -60103232;
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
                        //用户名不存在	-1101
                        ViewData.Add("ErrorCode", svc.Msg.StatusCode);
                        result.Body = "登录失败：无法通过登录检查!";
                        //if (0 == LoginStatusCode)
                        //{
                        //    LoginStatusCode = -1;
                        //}
                        var Msg = svc.Msg;
                        res.Data = Msg;

                        result.StatusCode = Msg.StatusCode;
                        //if (Msg.FaultCode.HasValue)
                        //{
                        //    string LanguageMark = SA.WebUtil.Common.CookiesManage.GetLang();

                        //    result.StatusCode = -1;
                        //    result.Body = Msg.Body;
                        //    result.GlobalErrorMessage = GetErrorMsgeGlobal(Msg.FaultCode.Value, LanguageMark);

                        //}
                        //result.StatusCode = LoginStatusCode;
                        return res;
                    }

                    #endregion
                }
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
                    result.StatusCode = -1;
                    result.Body = "登录失败：具体异常如下：" + ex.Message;

                    return res;
                }
                finally
                {
                }


                IsLoged = true;
                try
                {
                    SessionUser.SetUserLogin();
                    SessionUser.UserName = username;
                    result.Body = "登录成功!";
                    result.StatusCode = 1;
                    return res;
                }
                catch (Exception ex)
                {
                    result.Body = "登录失败!登录程序在缓存用户数据时出错，具体信息如下：" + ex.Message;
                    result.StatusCode = -1;
                    return res;
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


    }
}
