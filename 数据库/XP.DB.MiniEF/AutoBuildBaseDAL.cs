using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using XP.Comm.Attributes;
using XP.DB.Comm;
using XP.Util;
using XP.Util.TypeCache;

namespace XP.DB.MiniEF
{
    /// <summary>
    /// 支持自动生成的基类
    /// </summary>
    public class AutoBuildBaseDAL<T> : BaseDAL
        where T : class,new()
    {
        private Type _InnerType;

        protected Type InnerType { get; set; }

        /// <summary>
        /// 自增列列表
        /// </summary>
        public virtual List<string> IdentityList { get; set; }
        /// <summary>
        /// UI成员，数据属性不能插入和更新
        /// </summary>
        public virtual List<string> UIMemberList { get; set; }

        /// <summary>
        /// 主健列，暂时只支持单主键 
        /// </summary>
        public string PrimaryKeyColumn { get; set; }

        private bool _HasPrimaryKey = false;

        /// <summary>
        /// 属性值获取工具
        /// </summary>
        DynamicMethod<T> _PropertyValueGeter;


        public EntityTypesCacheItem TypeCache { get; set; }

        public AutoBuildBaseDAL()
            : base()
        {
            Init();

        }

        public AutoBuildBaseDAL(IProvider provider)
            : base(provider)
        {
            Init();
        }

        protected virtual void Init()
        {
            InnerType = typeof(T);
            var CacheManage = EntityTypesCache.CreateInstance();

            TypeCache = CacheManage.GetItem(InnerType);

            _PropertyValueGeter = new DynamicMethod<T>();


            InitIdentityList();
            InitUIMemberList();
            InitPrimaryKey();
        }

        private void InitPrimaryKey()
        {
            PrimaryKeyColumn = null;
            foreach (var P in TypeCache.PropertyArr)
            {
                object[] Attrs = P.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                if (0 < Attrs.Length)
                {
                    _HasPrimaryKey = true;
                    PrimaryKeyColumn = (P.Name);
                    return;
                }
            }

        }
        private void InitIdentityList()
        {
            IdentityList = new List<string>();

            object[] IdAttrs = InnerType.GetCustomAttributes(typeof(IdentityClassAttribute), true);
            if (0 == IdAttrs.Length)
                return;
            foreach (var P in TypeCache.PropertyArr)
            {
                object[] Attrs = P.GetCustomAttributes(typeof(IdentityFieldAttribute), true);
                if (0 < Attrs.Length)
                {
                    IdentityList.Add(P.Name);
                }
            }

        }

        private void InitUIMemberList()
        {
            UIMemberList = new List<string>();

            foreach (var P in TypeCache.PropertyArr)
            {
                object[] Attrs = P.GetCustomAttributes(typeof(UIMemberAttribute), true);
                if (0 < Attrs.Length)
                {
                    UIMemberList.Add(P.Name);
                }
            }

        }

        #region 调用SQL和LinqConextClass



        protected List<T> ConextRun(string sql)
        {
            List<T> ResultList = new List<T>();

            LinqConextClass context = new LinqConextClass(this.Provider);
            try
            {
                context.Open();
                DataContext dataContext = context.Context;
                IEnumerable collections = dataContext.ExecuteQuery((new T()).GetType(), sql);
                foreach (var item in collections)
                {
                    T temp = item as T;
                    ResultList.Add(temp);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                context.Close();
            }
            return ResultList;

        }

        public static List<TSelectModel> Select2Model<TSelectModel>(IProvider provider, string sql) where TSelectModel : class,new()
        {

            List<TSelectModel> ResultList = new List<TSelectModel>();

            LinqConextClass context = new LinqConextClass(provider);
            try
            {
                context.Open();
                DataContext dataContext = context.Context;
                IEnumerable collections = dataContext.ExecuteQuery((new TSelectModel()).GetType(), sql);
                foreach (var item in collections)
                {
                    TSelectModel temp = item as TSelectModel;
                    ResultList.Add(temp);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                context.Close();
            }
            return ResultList;

        }


        protected int ContextExec(string sql)
        {

            LinqConextClass context = new LinqConextClass(this.Provider);
            try
            {
                context.Open();
                DataContext dataContext = context.Context;
                int x = dataContext.ExecuteCommand(sql);

                return x;
            }
            catch (Exception exp)
            {
                return -1;

                throw exp;
            }
            finally
            {
                context.Close();
            }
            return -1;
        }


        #endregion



        #region  查询

        public List<T> GetAll()
        {
            StringBuilder strSql = new StringBuilder();
            //select [DepartmentID],[Name],[Budget],[StartDate],[Administrator] from Department

            strSql.Append("SELECT * FROM  ");
            strSql.Append(InnerType.Name);

            return ConextRun(strSql.ToString());
        }


        public List<T> SearchSql(string whereString)
        {
            List<T> ResultList = new List<T>();
            StringBuilder strSql = new StringBuilder();
            string LowerSql = whereString.Trim().ToLower();

            if (!LowerSql.StartsWith("select"))
            {
                strSql.Append("SELECT * FROM  ");
                strSql.Append(InnerType.Name);
            }
            strSql.Append(" ");

            if (!LowerSql.StartsWith("where"))
            {
                strSql.Append(" WHERE ");
            }
            strSql.Append(whereString);



            return ConextRun(strSql.ToString());

        

        }


        public T GetOneBySql(string whereString)
        {

            var List = SearchSql(whereString);

            if (null == List || 0 == List.Count)
            {

                return null;
            }

            var Model = List[0];

            return Model;
        }

        public T GetItemById(int id)
        {

            if (_HasPrimaryKey)
            {

                //    where u != PrimaryKeyColumn && !UIMemberList.Contains(u)

                string sql = PrimaryKeyColumn + "=" + id;
                var List = SearchSql(sql);
                if (null != List && 0 != List.Count)
                {
                    return List[0];
                }


            }

            return null;
        }

        #endregion

        #region  更新
        public int UpdateModel(T model)
        {
            #region  包含主键
            if (_HasPrimaryKey)
            {

                string ModelSql = Model2UpdateSql(model);

                return ContextExec(ModelSql);
            }
            #endregion

            return -1;
        }

        public string Model2UpdateSql(T model)
        {
            StringBuilder strSql = new StringBuilder();

            // "UPDATE [Class] SET [Sort]=5,[Name]='分类一', WHERE [ClassId]= 1
            strSql.Append("UPDATE [");
            strSql.Append(InnerType.Name);
            strSql.Append("] SET ");
            //strSql.Append("[MemberId],[IdCard],[FullName],[DatInsert] )");
            bool HasStart = false;
            var NameUsedList = from u in TypeCache.PropertyNames
                               where u != PrimaryKeyColumn && !UIMemberList.Contains(u)
                               select u;

            HasStart = false;

            #region 循环
            foreach (var name in NameUsedList)
            {
                if (TypeCache.IsListProperty(name))
                {
                    continue;
                }
                var Property = TypeCache.PropertyDic[name];
                object PropertyValue = GetProperty2Sql(model, name);
                if (HasStart)
                {
                    strSql.Append(",");

                }
                else
                {
                    HasStart = true;
                }
                strSql.Append("[");
                strSql.Append(name);
                strSql.Append("]=");

                if (null == PropertyValue)
                {
                    strSql.Append("null");
                }
                else
                {
                    strSql.Append(PropertyValue);
                }

            }
            #endregion
            strSql.Append(" WHERE [");
            strSql.Append(PrimaryKeyColumn);

            strSql.Append("] =");
            strSql.Append(GetProperty2Sql(model, PrimaryKeyColumn));

            return strSql.ToString();
        }


        /// <summary>
        /// 根据名称获得属性值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private object GetProperty2Sql(T model, string propertyName)
        {
            if (TypeCache.IsListProperty(propertyName))
            {
                x.Say(" 发现了List类型的数据：" + propertyName);
                return null;
            }
            //if (TypeCache.IsListProperty(propertyName))
            //    return null;
            var Property = TypeCache.PropertyDic[propertyName];
            Type PropertyType;
            Type PropertyTypeReal;
            object PropertyValue = null;
            PropertyType = Property.PropertyType;
            if (TypeCache.IsEnumProperty(propertyName))
            {
                var EnumValue = _PropertyValueGeter.GetValue(model, propertyName);
                x.Say(" 发现了枚举类型的数据：" + propertyName);
                return Convert.ToInt32(EnumValue);
            }
            if (EntityTypesCacheItem.IsNullableType(PropertyType))
            {
                PropertyValue = _PropertyValueGeter.GetValue(model, propertyName);
                if (null == PropertyValue)
                    return null;
                PropertyTypeReal = EntityTypesCacheItem.GetRealType(PropertyType);
                //日期时间，加前后引号
                if (typeof(DateTime) == PropertyTypeReal)
                {
                    return "'" + PropertyValue.ToString() + "'";
                }
            }
            //字符串，加前后引号
            if (typeof(String) == PropertyType)
            {

                PropertyValue = _PropertyValueGeter.GetValue(model, propertyName);
                if (null == PropertyValue)
                    return null;
                if (0 == ((string)PropertyValue).Length)
                    return null;

                PropertyValue = ((string)PropertyValue).Replace("\'", "\'");
                return "'" + PropertyValue + "'";
            }

            PropertyValue = _PropertyValueGeter.GetValue(model, propertyName);


            return PropertyValue;
        }

        //这是以前写的代码，参考一下获取属性值
        public object GetToPropertyValue(T source, string propertyName)
        {
            object Result;
            if (TypeCache.IsEnumProperty(propertyName))
            {
                var EnumValue = _PropertyValueGeter.GetValue(source, propertyName);
                x.Say(" 发现了枚举类型的数据：" + propertyName);
                Result = Convert.ToInt32(EnumValue);
            }
            else if (TypeCache.IsListProperty(propertyName))
            {
                x.Say(" 发现了List类型的数据：" + propertyName);
                Result = null;
            }
            else
            {
                Result = _PropertyValueGeter.GetValue(source, propertyName);

            }

            return Result;
        }
        #endregion


        #region 删除



        public int DeleteModel(T model)
        {
            var strSql = Model2DeleteSql(model);
            if (String.IsNullOrEmpty(strSql))
                return -1;

            int Result = base.ExcuteSql(strSql);

            return Result;
        }


        public int DeleteModel(string sqlWhere)
        {

            StringBuilder strSql = new StringBuilder();
            //strSql.Append("DELETE * FROM [NewsClass] WHERE [ClassId]=1");
            strSql.Append("DELETE ");
            if (Provider.DBType == DBTypeDefined.Access)
            {
                strSql.Append(" * ");
            }
            strSql.Append(" FROM [");
            strSql.Append(InnerType.Name);
            strSql.Append("]  ");
           string LowerSql = sqlWhere.ToLower();

           if (!LowerSql.StartsWith("where") && !LowerSql.StartsWith(" where"))
            {
                strSql.Append(" WHERE ");
            }
            strSql.Append(sqlWhere);



            int Result = base.ExcuteSql(strSql.ToString());

            return Result;

        }

        public string Model2DeleteSql(T model)
        {

            StringBuilder strSql = new StringBuilder();
            //strSql.Append("DELETE * FROM [NewsClass] WHERE [ClassId]=1");
            strSql.Append("DELETE ");
            if (Provider.DBType == DBTypeDefined.Access)
            {
                strSql.Append(" * ");
            }
            strSql.Append(" FROM [");
            strSql.Append(InnerType.Name);
            strSql.Append("]  WHERE [");
            strSql.Append(PrimaryKeyColumn);

            strSql.Append("] =");
            strSql.Append(GetProperty2Sql(model, PrimaryKeyColumn));


            return strSql.ToString();
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加一个实体对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual int InsertModel(T model)
        {

            string ModelSql = Model2InsertSql(model);

            return ContextExec(ModelSql);

          
        }

        public int InsertEntity(T model)
        {

            string ModelSql = Model2InsertSql(model);
            try
            {



                //int x = OleFactory.ExcuteInsert(ModelSql);
                int x = base.InsertAndId(ModelSql);

                if (0 < IdentityList.Count)
                {

                    var Property = TypeCache.PropertyDic[IdentityList[0]];
                    Property.SetValue(model, x, null);
                }


                return x;
            }
            catch (Exception exp)
            {
                return -1;

                throw exp;
            }
            finally
            {
            }
        }

        public string Model2InsertSql(T model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO ");
            strSql.Append(InnerType.Name);
            strSql.Append("(");

            //strSql.Append("[MemberId],[IdCard],[FullName],[DatInsert] )");
            bool HasStart = false;
            var NameUsedList = from u in TypeCache.PropertyNames
                               where !IdentityList.Contains(u) && !UIMemberList.Contains(u)
                               select u;


            foreach (var name in NameUsedList)
            {
                if (TypeCache.IsListProperty(name))
                {
                    continue;
                }

                if (HasStart)
                {
                    strSql.Append(",");

                }
                else
                {
                    HasStart = true;
                }

                strSql.Append("[");
                strSql.Append(name);
                strSql.Append("]");

            }
            strSql.Append(") values (");
            HasStart = false;
            foreach (var name in NameUsedList)
            {
                if (TypeCache.IsListProperty(name))
                {
                    continue;
                }
                var Property = TypeCache.PropertyDic[name];
                object PropertyValue = GetProperty2Sql(model, name);
                if (HasStart)
                {
                    strSql.Append(",");

                }
                else
                {
                    HasStart = true;
                }

                if (null == PropertyValue)
                {
                    strSql.Append("null");

                }
                else
                {
                    strSql.Append(PropertyValue);
                }

            }

            strSql.Append(")");

            return strSql.ToString();
        }



        #endregion


    }
}
