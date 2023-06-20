using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.Attributes;

namespace XP.Util.TypeCache
{
    /// <summary>
    /// 对DynamicMethod<T>的扩展，不支持泛型，但是支持type
    /// </summary>
    public class DynamicTypeMethod
    {

        private Type _Type;

        internal static Func<Type, object, string, object> GetValueDelegate;
        internal static Action<Type, object, string, object> SetValueDelegate;
        public object GetValue(object instance, string memberName)
        {
            return GetValueDelegate(_Type, instance, memberName);
        }

        public void SetValue(object instance, string memberName, object newValue)
        {
            SetValueDelegate(_Type, instance, memberName, newValue);
        }

        public DynamicTypeMethod(Type type)
        {
            _Type = type;
            GetValueDelegate = GenerateGetValue();
            SetValueDelegate = GenerateSetValue();

        }

        private static Func<Type, object, string, object> GenerateGetValue()
        {
            Type type = typeof(Type);
            //var type = typeof(T);
            var instance = Expression.Parameter(typeof(object), "instance");
            var memberName = Expression.Parameter(typeof(string), "memberName");
            var nameHash = Expression.Variable(typeof(int), "nameHash");
            var calHash = Expression.Assign(nameHash, Expression.Call(memberName, typeof(object).GetMethod("GetHashCode")));
            var cases = new List<SwitchCase>();
            foreach (var propertyInfo in type.GetProperties())
            {
                object[] attrsAllows = propertyInfo.GetCustomAttributes(typeof(EntitySkipColumnAttribute), true);
                if (0 < attrsAllows.Length)
                {
                   continue;
                }
                var property = Expression.Property(Expression.Convert(instance, type), propertyInfo.Name);
                var propertyHash = Expression.Constant(propertyInfo.Name.GetHashCode(), typeof(int));

                cases.Add(Expression.SwitchCase(Expression.Convert(property, typeof(object)), propertyHash));
            }
            var switchEx = Expression.Switch(nameHash, Expression.Constant(null), cases.ToArray());
            var methodBody = Expression.Block(typeof(object), new[] { nameHash }, calHash, switchEx);

            return Expression.Lambda<Func<Type, object, string, object>>(methodBody, instance, memberName).Compile();
        }

        private static Action<Type, object, string, object> GenerateSetValue()
        {
            //var type = typeof(T);
            Type type = typeof(Action<Type>);
            var instance = Expression.Parameter(typeof(object), "instance");
            var memberName = Expression.Parameter(typeof(string), "memberName");
            var newValue = Expression.Parameter(typeof(object), "newValue");
            var nameHash = Expression.Variable(typeof(int), "nameHash");
            var calHash = Expression.Assign(nameHash, Expression.Call(memberName, typeof(object).GetMethod("GetHashCode")));
            var cases = new List<SwitchCase>();
            foreach (var propertyInfo in type.GetProperties())
            {
                object[] attrsAllows = propertyInfo.GetCustomAttributes(typeof(EntitySkipColumnAttribute), true);
                if (0 < attrsAllows.Length)
                {
                    continue;
                }
                var property = Expression.Property(Expression.Convert(instance, type), propertyInfo.Name);
                var setValue = Expression.Assign(property, Expression.Convert(newValue, propertyInfo.PropertyType));
                var propertyHash = Expression.Constant(propertyInfo.Name.GetHashCode(), typeof(int));

                cases.Add(Expression.SwitchCase(Expression.Convert(setValue, typeof(object)), propertyHash));
            }
            var switchEx = Expression.Switch(nameHash, Expression.Constant(null), cases.ToArray());
            var methodBody = Expression.Block(typeof(object), new[] { nameHash }, calHash, switchEx);

            return Expression.Lambda<Action<Type, object, string, object>>(methodBody, instance, memberName, newValue).Compile();
        }
    }
}
