using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLApi
{
    public static class CopyProperties
    {
        public static void CopyPropertiesTo<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                    propTo.SetValue(to, value);
            }

            //foreach (PropertyInfo propTo in to.GetType().GetProperties())
            //{
            //    PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
            //    if (propFrom == null)
            //        continue;
            //    var value = propFrom.GetValue(from, null);
            //    if (value is ValueType || value is string)
            //        propTo.SetValue(to, value);
            //    if (propTo.PropertyType.IsEnum && propTo.PropertyType.IsEnum)
            //    {
            //        propTo.SetValue(to, Enum.ToObject(propTo.PropertyType, value));
            //    }
            //}
        }
        public static object CopyPropertiesToNew<S>(this S from, Type type)
        {
            object to = Activator.CreateInstance(type); // new object of Type
            from.CopyPropertiesTo(to);
            return to;
        }
    }
}
