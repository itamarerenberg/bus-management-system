using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DL
{
    static class Cloning
    {
        internal static T Clone<T>(this T original) where T : new()//@rimainder to check what's (where T : new()) means
        {
            T copyToObject = new T();
            //T copyToObject = (T)Activator.CreateInstance(typeof(T));

            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                propertyInfo.SetValue(copyToObject, propertyInfo.GetValue(original, null), null);

            return copyToObject;
        }


    }
}
