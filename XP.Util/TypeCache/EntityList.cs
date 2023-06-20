using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XP.Util.TypeCache
{
    public class EntityList
    {
        public static List<ToEntity> CopyList<FromEntity, ToEntity>(List<FromEntity> list)
            where FromEntity : class
            where ToEntity : class, new()
        {

            EntityCopyUtil<FromEntity, ToEntity> Copier = new EntityCopyUtil<FromEntity, ToEntity>();

            List<ToEntity> ResultList = new List<ToEntity>();
            if (null == list)
            {
                return ResultList;
            }
            foreach (FromEntity source in list)
            {
                ToEntity target = Copier.CopyEntity(source);
                ResultList.Add(target);
            }

            return ResultList;
        }

        public static List<ToEntity> Array2List<FromEntity, ToEntity>(FromEntity[] sourceArray)
            where FromEntity : class
            where ToEntity : class, new()
        {

            EntityCopyUtil<FromEntity, ToEntity> Copier = new EntityCopyUtil<FromEntity, ToEntity>();

            List<ToEntity> ResultList = new List<ToEntity>();
            if (null == sourceArray)
            {
                return ResultList;
            }
            foreach (FromEntity source in sourceArray)
            {
                ToEntity target = Copier.CopyEntity(source);
                ResultList.Add(target);
            }

            return ResultList;
        }


        public static ToEntity CopyEntity<FromEntity, ToEntity>(FromEntity from)
            where FromEntity : class
            where ToEntity : class, new()
        {
            EntityCopyUtil<FromEntity, ToEntity> Copier = new EntityCopyUtil<FromEntity, ToEntity>();

            ToEntity target = Copier.CopyEntity(from);
            return target;
        }


        public static void CopyToEntity<FromEntity, ToEntity>(FromEntity from, ToEntity to)
            where FromEntity : class
            where ToEntity : class

        {
            EntityPropertyCopy<FromEntity, ToEntity> Copier = new EntityPropertyCopy<FromEntity, ToEntity>();

            Copier.CopyToEntity(from, to);
        }




        /// <summary>
        /// 泛型的封装
        /// </summary>
        /// <remarks>
        /// 参考：
        /// http://blog.csdn.net/fgfg12345/article/details/45098639
        /// https://www.zhihu.com/question/22572368
        /// https://msdn.microsoft.com/zh-cn/library/system.reflection.methodinfo.makegenericmethod(v=vs.110).aspx
        /// http://blog.csdn.net/yan_hyz/article/details/46524497
        /// </remarks>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void CopyToType(Type fromType, Type toType, object from, object to)
        {
            Type ex = typeof(EntityList);

            MethodInfo mi = ex.GetMethod("CopyToEntity").MakeGenericMethod(fromType, toType);

            object[] args = { from, to };
            mi.Invoke(null, args);

        }

    }
}
