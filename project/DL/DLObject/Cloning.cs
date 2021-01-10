using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;

namespace DL
{
    static class Cloning
    {
        internal static T Clone<T>(this T original) where T : new()
        {
            T copyToObject = new T();

            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                propertyInfo.SetValue(copyToObject, propertyInfo.GetValue(original, null), null);

            return copyToObject;
        }

        internal static void xelement_to_object<T>(XElement from, out T to) where T : new()
        {
            to = new T();
            foreach (var prop in typeof(T).GetType().GetProperties())
            {
                if (from.Element(prop.Name) == null)
                    continue;
                string val = from.Element(prop.Name).Value;
                switch (prop.PropertyType.Name)
                {
                    case "int":
                        prop.SetValue(to, int.Parse(val));
                        break;
                    case "DateTime":
                        prop.SetValue(to, DateTime.Parse(val));
                        break;
                    case "String":
                        prop.SetValue(to, val);
                        break;
                    case "bool":
                        prop.SetValue(to, bool.Parse(val));
                        break;
                    default:
                        throw new Exception($"need to add {prop.PropertyType.Name} to swich");
                        break;
                }
            }
        }

        internal static T xelement_to_new_object<T>(XElement from) where T : new()
        {
            xelement_to_object(from, out T newObj);
            return newObj;
        }

        internal static void object_to_xelement<T>(T from, XElement to) where T : new()
        {
            foreach (var prop in from.GetType().GetProperties())
            {
                string val = prop.GetValue(from).ToString();
                to.Element(prop.Name).Value = val;//insert new label <prop.Name> prop.GetValue(prop).ToString() </prop.Name>
            }
        }
    }
}
