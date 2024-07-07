using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Configuration;
using System.Configuration;
using System.IO;

namespace XP.Common.Serialization
{
    public class JsonHelper
    {
        public static string Serialize<T>(T data)
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer =
            new System.Runtime.Serialization.Json.DataContractJsonSerializer(data.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, data);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            if (String.IsNullOrEmpty(json))
                return obj;
            using (MemoryStream ms =
            new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer =
                new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());

                try
                {
                    obj = (T)serializer.ReadObject(ms);
                }
                catch (System.Runtime.Serialization.SerializationException ex)
                {
                    obj = default(T);
                }
                catch (ArgumentException ex)//兼容序列化以前的数据
                {
                    obj = default(T);
                }
                return obj;
            }
        }

    }


    public class JsSerializer
    {
        private int _Max;
        private bool _SetMax = false;
        public int Max
        {
            get
            {
                if (_SetMax)
                    return _Max;
                _Max = Int32.MaxValue;
                _SetMax = true;
                return _Max;
            }
            set { _Max = value; }
        }

        private JavaScriptSerializer _Serializer;

        public JavaScriptSerializer Serializer
        {
            get
            {
                if (null == _Serializer)
                    _Serializer = new JavaScriptSerializer();
                _Serializer.MaxJsonLength = Max;
                return _Serializer;
            }
            set { _Serializer = value; }
        }
        public JsSerializer(int max)
        {
            Max = max;
        }
        public JsSerializer()
        {

        }


        public string BuildString(object o)
        {
            return Serializer.Serialize(o);
        }


        public T BuildInstance<T>(string input) //where T:class
        {
            int TempMax = Max;
            if (Max < input.Length)
            {
                Max = input.Length;
            }
            T Result;
            try
            {
                Result = Serializer.Deserialize<T>(input);
            }
            catch (ArgumentException ex)//兼容序列化以前的数据
            {
                Result = default(T);

            }
            Max = TempMax;

            return Result;
        }

    }
}
