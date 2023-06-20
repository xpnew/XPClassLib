using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.DB.CommRedis
{
    public class RegisterManage
    {

        //全局静态实例
        private static RegisterManage _instance;
        public static RegisterManage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RegisterManage();
                }
                return _instance;
            }
        }


        //服务列表
        private Dictionary<string, int> _serviceList;

        private List<Action<bool>> _eventList;

        //私有构造.不允许过多实例化.单例模式
        private RegisterManage()
        {
            _serviceList = new Dictionary<string, int>();
            _eventList = new List<Action<bool>>();
        }


        //设置事件完成的回调
        public void SetStopEvent(Action<bool> action)
        {
            _eventList.Add(action);
        }


        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="type">服务状态,0停止,1运行</param>
        /// <returns></returns>
        public bool RegisterService(string serviceName, int type = 1)
        {
            lock (_serviceList)
            {
                if (_serviceList.ContainsKey(serviceName))
                {
                    //已经存在相同的服务名称了,主册失败
                    _serviceList[serviceName] = type;
                }
                //不存在相同的服务名称,可以添加
                _serviceList.Add(serviceName, type);
            }

            string szlog = string.Format("注册 {0} 服务", serviceName);
            //LogOut.Instance.PrintLog(szlog);

            return true;
        }


        //注销服务
        public bool UnRegisterService(string serviceName)
        {
            lock (_serviceList)
            {
                if (_serviceList.ContainsKey(serviceName))
                {
                    _serviceList.Remove(serviceName);

                    string szlog = string.Format("注销 {0} 服务", serviceName);
                    //LogOut.Instance.PrintLog(szlog);
                }
            }
            CheckStop();
            return true;
        }


        //服务停止事件,0停止,1运行
        public bool ServiceEvent(string serviceName, int nType = 0)
        {
            lock (_serviceList)
            {
                if (_serviceList.ContainsKey(serviceName))
                {
                    _serviceList[serviceName] = nType;

                    string szlog = string.Format("服务 {0} 停止", serviceName);
                    //LogOut.Instance.PrintLog(szlog);
                }
            }

            CheckStop();
            return true;
        }


        public void Clear()
        {
            _serviceList.Clear();
            _eventList.Clear();
        }


        //检查是否可以停止
        private void CheckStop()
        {
            lock (_serviceList)
            {
                bool allStop = true;

                foreach (var item in _serviceList)
                {
                    if (item.Value > 0)
                    {
                        allStop = false;
                        break;
                    }
                }

                if (allStop)
                {
                    for (int i = 0; i < _eventList.Count; i++)
                    {
                        _eventList[i](true);
                    }
                }
            }
        }

    }
}
