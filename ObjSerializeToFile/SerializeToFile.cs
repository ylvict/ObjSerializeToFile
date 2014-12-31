using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tools
{
    /// <summary> 序列化数据对象工厂(序列化结果到指定文件)
    /// </summary>
    public class SerializeToFile
    {
        /// <summary> 序列化类型
        /// </summary>
        public enum FormatterType
        { soap, xml, binary }
        FileStream stream;
        IFormatter formatter;
        bool isSerialFmt = false;
        /// <summary> 初始化序列化器
        /// </summary>
        /// <param name="fmtType">序列化类型</param>
        public SerializeToFile(FormatterType fmtType)
        {
            switch (fmtType)
            {
                case FormatterType.soap: formatter = new SoapFormatter();
                    break;
                case FormatterType.xml: isSerialFmt = true;
                    break;
                case FormatterType.binary: formatter = new BinaryFormatter();
                    break;
                default:
                    break;
            }
        }
        /// <summary> 开始序列化
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="path">序列化后保存到的目标文件</param>
        public void serial(object obj, string path)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                stream = new FileStream(path, FileMode.Create);
                if (isSerialFmt)
                    serializer.Serialize(stream, obj);
                else
                    formatter.Serialize(stream, obj);
                stream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }
        /// <summary>开始反序列化
        /// </summary>
        /// <param name="path">反序列化源文件路径</param>
        /// <param name="type">反序列化的对象类型(仅XML序列化需要)</param>
        /// <returns></returns>
        public object deserial(string path, Type type)
        {
            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                object obj;
                if (type != null)
                {
                    XmlSerializer serializer = new XmlSerializer(type);
                    obj = serializer.Deserialize(stream);
                }
                else
                {
                    obj = formatter.Deserialize(stream);
                }
                stream.Close();
                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
