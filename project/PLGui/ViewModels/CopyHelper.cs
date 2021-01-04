using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PLGui.Models.PO;

namespace PLGui.ViewModels
{
    public static class CopyHelper
    {
        public static void DeepCopyTo<S, T>(this S from, T to)
        {
            var fromType = from.GetType();
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = fromType.GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                    propTo.SetValue(to, value);
                else
                {
                    if (value == null)
                        continue;
                    var target = propTo.GetValue(to, null);
                    //if (target == null)
                    //    target = Activator.CreateInstance(propTo.PropertyType);

                    // If the property is a collection...
                    if (value is IEnumerable)
                    {
                        Type itemType = propTo.PropertyType.GetGenericArguments()[0];
                        propTo.PropertyType.GetMethod("Clear").Invoke(target, null);
                        foreach (var item in (value as IEnumerable))
                        {
                            var targetItem = Activator.CreateInstance(itemType);
                            item.DeepCopyTo(targetItem);
                            propTo.PropertyType.GetMethod("Add").Invoke(target, new object[] { targetItem });
                        }
                    }
                    else
                        value.DeepCopyTo(target);
                }
            }
        }
        public static Station Station_BO_PO(BO.Station from)
        {
            Station result = new Station();
            result.Code = from.Code;
            result.Address = from.Address;
            result.Latitude = from.Latitude;
            result.Longitude = from.Longitude;
            result.Name = from.Name;
            result.GetLines = new ObservableCollection<Line>(from.GetLines.Select(ln => Line_BO_PO(ln)));
            return result;
        }

        public static Line Line_BO_PO(BO.Line from)
        {
            Line result = new Line();
            result.ID = from.ID;
            result.LineNumber = from.LineNumber;
            result.Stations = new ObservableCollection<BO.LineStation>(from.Stations);
            result.Area = from.Area;
            return result;
        }
    }
}
