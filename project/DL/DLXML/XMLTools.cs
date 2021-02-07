using DO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DL
{
    static class XMLTools
    {
        static string dir = @"xml\";
        static XMLTools()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        #region SaveLoadWithXElement
        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(dir + filePath);
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        public static XElement LoadListFromXMLElement(string filePath)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    return XElement.Load(dir + filePath);
                }
                else
                {
                    XElement rootElem = new XElement(dir + filePath);
                    rootElem.Save(dir + filePath);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion

        #region SaveLoadWithXMLSerializer
        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(dir + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }
        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(dir + filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else 
                    return new List<T>();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }

        #region my methods

        public static void xelement_to_object<T>(XElement from, out T to) where T : new()
        {
            to = new T();
            foreach (var prop in to.GetType().GetProperties())
            {
                if (from.Element(prop.Name) == null)
                    continue;
                string val = from.Element(prop.Name).Value;
                switch (prop.PropertyType.Name)
                {
                    case "Int32":
                        prop.SetValue(to, int.Parse(val));
                        break;
                    case "DateTime":
                        prop.SetValue(to, DateTime.Parse(val));
                        break;
                    case "String":
                        prop.SetValue(to, val);
                        break;
                    case "Boolean":
                        prop.SetValue(to, bool.Parse(val));
                        break;
                    case "Double":
                        prop.SetValue(to, double.Parse(val));
                        break;
                    case "Single":
                        prop.SetValue(to, float.Parse(val));
                        break;
                    case "AreasEnum":
                        prop.SetValue(to, Enum.Parse(typeof(AreasEnum), val));
                        break;
                    case "BusStatus":
                        prop.SetValue(to, Enum.Parse(typeof(BusStatus), val));
                        break;
                    case "Nullable`1"://int?
                        try
                        {
                            prop.SetValue(to, int.Parse(val));
                        }
                        catch (Exception)//if val is null
                        {
                            prop.SetValue(to, null);
                        }
                        break;
                    case "TimeSpan":
                        prop.SetValue(to, TimeSpan.Parse(val));
                        break;
                    default:
                        throw new Exception($"need to add {prop.PropertyType.Name} to swich");

                }
            }
        }

        public static T xelement_to_new_object<T>(XElement from) where T : new()
        {
            xelement_to_object(from, out T newObj);
            return newObj;
        }

        public static void object_to_xelement<T>(T from, XElement to) where T : new()
        {
            foreach (var prop in from.GetType().GetProperties())
            {
                if(prop.GetValue(from) == null)
                {
                    to.Element(prop.Name).Value = "NaN";
                    continue;
                }
                string val = prop.GetValue(from).ToString();
                to.Element(prop.Name).Value = val;//insert new label <prop.Name> prop.GetValue(prop).ToString() </prop.Name>
            }
        }

        /// <summary>
        /// return new XElement with Label = label, using object_to_xelement() to fill the XElement
        /// </summary>
        /// <param name="label">the label of the XElement to return</param>
        public static XElement to_new_xelement<T>(this T from, string label) where T : new()
        {
            XElement newXElement = new XElement(label);
            foreach(var prop in typeof(T).GetProperties())
            {
                if(prop.DeclaringType == typeof(T))
                    newXElement.Add(new XElement(prop.Name));
            }
            object_to_xelement(from, newXElement);
            return newXElement;
        }
        #endregion
        #endregion
    }


    //public static XElement ToXML(this Name n)
    //{
    //    return new XElement("Name",
    //        new XElement("FirstName", n.FirstName),
    //        new XElement("LastName", n.LastName)
    //    );
    //}

    //public static string ToXMLstring<T>(this T toSerialize)
    //{
    //    using (StringWriter textWriter = new StringWriter())
    //    {
    //        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
    //        xmlSerializer.Serialize(textWriter, toSerialize);
    //        return textWriter.ToString();
    //    }
    //}

}