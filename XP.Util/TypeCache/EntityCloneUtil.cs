using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util.TypeCache
{
    /// <summary>
    /// 实体对象克隆工具，浅表复制一个对象的属性到一个新的对象
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    public class EntityCloneUtil<Entity> where Entity : class,new()
    {
        public Type EntityType { get; set; }

        public List<string> PropertyNames { get; set; }
        private DynamicMethod<Entity> _Copyer;
        /// <summary>
        /// 克隆的来源
        /// </summary>
        public Entity Source { get; set; }
        /// <summary>
        /// 需要排除的列名
        /// </summary>
        public List<string> ExcludeNames { get; set; }
        public EntityCloneUtil()
        {
            this.EntityType = typeof(Entity);
            EntityTypesCache Cache = EntityTypesCache.CreateInstance();
            PropertyNames = Cache.GetItem(EntityType).PropertyNames;
            ExcludeNames = new List<string>();
            _Copyer = new DynamicMethod<Entity>();
        }
        /// <summary>
        /// 进行克隆的方法
        /// </summary>
        /// <param name="source">克隆的来源</param>
        /// <returns>克隆出来的新对象</returns>
        public Entity Clone(Entity source)
        {
            if (null == source)
            {
                return null;
            }
            Entity Result = new Entity();

            List<string> CloneNames = PropertyNames.Except(ExcludeNames).ToList();

            foreach (string PropertyName in CloneNames)
            {
                if (PropertyNames.Exists(o => o == PropertyName))
                {
                    _Copyer.SetValue(Result, PropertyName, _Copyer.GetValue(source, PropertyName));
                }
            }
            return Result;
        }
        /// <summary>
        /// 将一个对的属性复制到另一个对象上。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void CloneTo(Entity source, Entity target)
        {
            List<string> CloneNames = PropertyNames.Except(ExcludeNames).ToList();

            foreach (string PropertyName in CloneNames)
            {
                if (PropertyNames.Exists(o => o == PropertyName))
                {
                    _Copyer.SetValue(target, PropertyName, _Copyer.GetValue(source, PropertyName));
                }
            }
        }


        //public Entity Clone()
        //{
        //    return Clone(Source);
        //}



    }
}
