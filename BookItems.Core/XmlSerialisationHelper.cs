using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace BookItems.Core
{
    public static class XmlSerialisationHelper
    {
        /// <summary>
        /// Serialises any object to XML
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        public static void SerialiseToFile(object obj, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlSerializer xs = new XmlSerializer(obj.GetType());
                xs.Serialize(fs, obj);
            }
        }

        /// <summary>
        /// Searialises an object to an XML string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerialiseToString(object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlSerializer xs = new XmlSerializer(obj.GetType());
                xs.Serialize(memoryStream, obj);

                memoryStream.Seek(0L, SeekOrigin.Begin);

                StreamReader sr = new StreamReader(memoryStream);
                string text = sr.ReadToEnd();

                return text;
            }
        }

        /// <summary>
        /// Deserializes to an object of given type <typeparamref name="T"/> from a file at <paramref name="path"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T DeserialiseFromFile<T>(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    T obj = (T)xs.Deserialize(fs);

                    return obj;
                }
            }
            catch (IOException)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Deserialises to an object of type <typeparamref name="T"/> from xml string <paramref name="xml"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeserialiseFromString<T>(string xml)
        {
            if (xml == null) return default(T);

            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    T obj = (T)xs.Deserialize(sr);

                    return obj;
                }
            }
            catch (IOException)
            {
                return default(T);
            }
        }
    }
}
