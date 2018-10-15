using System;
using System.Reflection;

namespace UnityProBuilder
{
    public class TypeReader
    {
        private readonly Type type;
        private object obj;

        public string TypeName
        {
            get { return type.Name; }
        }

        public TypeReader(object o)
        {
            obj = o;
            type = o.GetType();
        }

        public T WriteToObjectProperties<T>(BindingFlags flags = BindingFlags.Public | BindingFlags.Instance, object dest = null)
        {
            var destObject = typeof(T);
            object g;
            if (ReadField(destObject.Name.ToFistLower(), out g))
            {
                var reader = new TypeReader(g);
                reader.CopyFieldsToProperties<T>(flags, dest);
            }
            return (T)dest;
        }

        public T WriteToStaticProperties<T>(BindingFlags flags = BindingFlags.Public | BindingFlags.Static, object dest = null)
        {
            var destObject = typeof(T);
            object g;
            if (ReadField(destObject.Name.ToFistLower(), out g))
            {
                var reader = new TypeReader(g);
                reader.CopyFieldsToProperties<T>(flags, dest);
            }
            return (T)dest;
        }

        public bool ReadField<T>(string fieldName, out T value)
        {
            value = default(T);
            var info = type.GetField(fieldName);
            if (info == null)
                return false;
            value = (T)info.GetValue(obj);
            return true;
        }

        public void CopyFieldsToProperties<T>(BindingFlags flags = BindingFlags.Public | BindingFlags.Instance, object dest = null)
        {
            var destType = typeof(T);
            var infos = destType.GetProperties(flags);
            for (int i = 0; i < infos.Length; i++)
            {
                var p = infos[i];
                object v;
                if (ReadField(p.Name, out v))
                {
                    p.SetValue(dest, v, null);
                }
            }
        }

        public void CopyFieldsToFields<T>(BindingFlags flags = BindingFlags.Public | BindingFlags.Instance, object dest = null)
        {
            var destType = typeof(T);
            var fields = destType.GetFields(flags);
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                object v;
                if (ReadField(field.Name, out v))
                {
                    field.SetValue(null, v);
                }
            }
        }
    }
}
