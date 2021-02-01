using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PLGui.Models.PO;

namespace PLGui.utilities
{
    public static class CopyHelper
    {
        public static object DeepCopyTo<S, T>(this S from, T to)
        {
            if (to == null || from == null)
            {
                return null;
            }
            var fromType = from.GetType();
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                if(!propTo.CanWrite)
                {
                    continue;
                }
                PropertyInfo propFrom = fromType.GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                {
                    propTo.SetValue(to, value);
                    return to;
                }

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
                        if (target == null)
                        {
                            target = Activator.CreateInstance(propTo.PropertyType);

                        }
                        propTo.PropertyType.GetMethod("Clear").Invoke(target, null);
                        foreach (var item in (value as IEnumerable))
                        {
                            var targetItem = Activator.CreateInstance(itemType);
                            item.DeepCopyTo(targetItem);
                            propTo.PropertyType.GetMethod("Add").Invoke(target, new object[] { targetItem });
                        }
                        //propTo.SetValue(to, target);
                    }
                    else
                    {
                        if (target == null)
                        {
                            target = Activator.CreateInstance(propTo.PropertyType);
                        }
                    }
                }
            }
            return null;
        }
        public static object DeepCopyToNew<S>(this S from, Type type)
        {
            if (from == null)
            {
                if (type.IsClass)
                {
                    return null;
                }
            }
            object to = Activator.CreateInstance(type); // new object of Type
            from.DeepCopyTo(to);
            return to;
        }
        public static void DeepCopyToCollection<S, T>(this IEnumerable<S> from, Collection<T> to)
        {
            foreach (var fromItem in from)
            {
                T targetItem = (T)Activator.CreateInstance(typeof(T));
                fromItem.DeepCopyTo(targetItem);
                to.Add(targetItem);
                //to = to.Concat(new[] { targetItem });
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
            //result.GetLines = new ObservableCollection<Line>(from.GetLines.Select(ln => Line_BO_PO(ln)));
            result.LinesNums = new ObservableCollection<int>(from.LinesNums.Select( n => n));
            return result;
        }

        public static Line Line_BO_PO(this BO.Line from)
        {
            Line result = new Line
            {
                ID = from.ID,
                LineNumber = from.LineNumber,
                Area = (AreasEnum)from.Area,
                Stations = new ObservableCollection<BO.LineStation>(from.Stations),
                LineTrips = new ObservableCollection<BO.LineTrip>(from.LineTrips)
            };
            return result;
        }
        public static void CollectionLine_BO_PO(this IEnumerable<BO.Line> from, Collection<Line> to)
        {
            foreach (var fromLine in from)
            {
                Line line = fromLine.Line_BO_PO();
                to.Add(line);
            }
        }


        public static T FindParentOfType<T>(this Control aParent) where T : Control
        {
            if (aParent is T)
            {
                return (T)aParent;
            }
            if (aParent.Parent != null && aParent.Parent is Control bParent)
            {
                return FindParentOfType<T>(bParent);
            }
            return null;
        }

        public static T FindWindowOfType<T>(this Control aParent) where T : Control 
        {
            if (aParent.Parent is T)
            {
                return (T)aParent.Parent;
            }
            if (aParent.Parent != null && aParent.Parent is Control bParent)
            {
                return FindWindowOfType<T>(bParent);
            }
            if (aParent.Parent is Grid grid)
            {
                while (grid != null)
                {
                    if (grid.Parent is T)
                    {
                        return (T)grid.Parent;
                    }
                    if (grid.Parent is Grid temp)
                    {
                        grid = temp;
                    }
                    else
                    {
                        return null;
                    } 
                }
            }
            return null;
        }
    }

}
