using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Json.JComment
{


    //https://www.cnblogs.com/yijiayi/p/10051284.html


    /// <summary>
    /// 使用注释的属性或者类
    /// </summary>
    public class UseCommentConverter : JsonConverter
    {

        //是否开启自定义反序列化，值为true时，反序列化时会走ReadJson方法，值为false时，不走ReadJson方法，而是默认的反序列化
        public override bool CanRead => false;
        //是否开启自定义序列化，值为true时，序列化时会走WriteJson方法，值为false时，不走WriteJson方法，而是默认的序列化
        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType)
        {
            object[] attrs = objectType.GetCustomAttributes(typeof(JsonCommentAttribute), true);


            if(0< attrs.Length)
            {
                //Attrs = new List<JsonCommentAttribute>();

                foreach(var a in attrs)
                {
                    Attrs.Add(a as JsonCommentAttribute);
                }
                return true;
            }
            return false;
            //return 0 < attrs.Length;

            //return typeof(Model) == objectType;
        }

        protected List<JsonCommentAttribute> Attrs { get; set; } = new List<JsonCommentAttribute>();



        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer jsonSerializer)
        {
            //new一个JObject对象,JObject可以像操作对象来操作json
            //var jobj = new JObject();
            ////value参数实际上是你要序列化的Model对象，所以此处直接强转
            //var model = value as Model;
            //if (model.ID != 1)
            //{
            //    //如果ID值为1，添加一个键位"ID"，值为false
            //    jobj.Add("ID", false);
            //}
            //else
            //{
            //    jobj.Add("ID", true);
            //}
            Type CurrentObject = value.GetType();


            //object[] attrs = value.GetType().GetCustomAttributes(typeof(JCommentEntityAttribute), true);
            foreach (var attr in Attrs)
            {
                ///JCommentEntityAttribute jca = attrs[0] as JCommentEntityAttribute;
                writer.WriteComment(attr.Comment);
            }

            //if (CurrentObject.IsClass)
            //{


            //}
            //if(CurrentObject.)

            JToken t = JToken.FromObject(value);

            Write(writer, t, jsonSerializer);
            //if (t.Type != JTokenType.Object)
            //{
            //    t.WriteTo(writer);
            //}
            //else
            //{
            //    //JObject jo = (JObject)t;
            //    //IList<string> propertyNameList = jo.Properties().Select(p => p.Name).ToList();

            //    //var pps = jo.Properties();

            //    //writer.WriteStartObject();
            //    //foreach (var p in pps)
            //    //{
            //    //    writer.WritePropertyName(p.Name);
            //    //    writer.WriteValue(p.Value);

            //    //}


            //    //writer.WriteEndObject();

            //    //writer.WriteStartObject();
            //    //writer.WritePropertyName("CPU");
            //    //writer.WriteValue("Intel");
            //    //writer.WritePropertyName("PSU");
            //    //writer.WriteValue("500W");
            //    //writer.WritePropertyName("Drives");
            //    //writer.WriteStartArray();
            //    //writer.WriteValue("DVD read/writer");
            //    //writer.WriteComment("(broken)");
            //    //writer.WriteValue("500 gigabyte hard drive");
            //    //writer.WriteValue("200 gigabyte hard drive");
            //    //writer.WriteEnd();
            //    //writer.WriteEndObject();
            //    //jo.AddFirst(new JProperty("Keys", new JArray(propertyNameList)));
            //    //jo.WriteTo(writer);
            //}


            //base.WriteJson(writer, value, serializer);

            //jsonSerializer.Serialize(writer, value);


            //通过ToString()方法把JObject对象转换成json
            //var jsonstr = jobj.ToString();
            ////调用该方法，把json放进去，最终序列化Model对象的json就是jsonstr，由此，我们就能自定义的序列化对象了
            //writer.WriteValue(jsonstr);
        }


        protected void Write(JsonWriter writer, JToken t, JsonSerializer jsonSerializer)
        {
            if (t.Type == JTokenType.Object)
            {
                writer.WriteStartObject();
                JObject jo = (JObject)t;
                foreach (var p in jo.Children())
                {
                    Write(writer, p, jsonSerializer);

                }
                writer.WriteEndObject();

            }
            else if (t.Type == JTokenType.Array)
            {
                writer.WriteStartArray();
                JArray ja = t as JArray;

                foreach (var item in ja)
                {
                    Write(writer, item, jsonSerializer);
                }


                writer.WriteEndArray();

            }
            else
            {


                t.WriteTo(writer);
            }
            // protected void Write2Array()

        }
    }
}
