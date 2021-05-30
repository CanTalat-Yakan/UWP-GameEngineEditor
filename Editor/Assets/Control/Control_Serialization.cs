using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Editor.Assets.Control
{
    internal enum EDataType
    {
        BINARY,
        XML,
        JSON,
    }

    static class Control_Serialization
    {
        internal static void DeleteFile(string _path)
        {
            if (File.Exists(_path))
                File.Delete(_path);
        }

        internal static void SaveFile<T>(T _object, string _path, EDataType _eDataType = EDataType.BINARY)
        {
            switch (_eDataType)
            {
                case EDataType.BINARY:
                    SaveBinary(_object, _path);
                    break;
                case EDataType.XML:
                    SaveXml(_object, _path);
                    break;
                case EDataType.JSON:
                    SaveJSON(_object, _path);
                    break;
                default:
                    break;
            }
        }

        internal static void SaveXml<T>(T _object, string _path)
        {
            using (FileStream fs = new FileStream(_path, File.Exists(_path) ? FileMode.Create : FileMode.CreateNew))
            {
                XmlSerializer serializer = new XmlSerializer(_object.GetType());
                serializer.Serialize(fs, _object);
            }
        }

        internal static void SaveBinary<T>(T _object, string _path)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, _object);
                File.WriteAllBytes(_path, ms.ToArray());
            }
        }

        internal static void SaveJSON<T>(T _object, string _path)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;

            string data = JsonSerializer.Serialize(_object, typeof(T), options);
            File.WriteAllText(_path, data);
        }


        internal static T LoadFile<T>(string _path, EDataType _eDataType = EDataType.BINARY)
        {
            object obj = null;

            switch (_eDataType)
            {
                case EDataType.BINARY:
                    obj = LoadBinary(_path);
                    break;
                case EDataType.XML:
                    obj = LoadXml(typeof(T), _path);
                    break;
                case EDataType.JSON:
                    obj = LoadJSON(typeof(T), _path);
                    break;
                default:
                    break;
            }
            return (T)obj;
        }

        internal static object LoadXml(Type _type, string _path)
        {
            object obj = null;

            using (FileStream fs = new FileStream(_path, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(_type);
                obj = serializer.Deserialize(fs);
            }

            return obj;
        }

        internal static object LoadBinary(string _path)
        {
            object obj = null;

            using (FileStream fs = new FileStream(_path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                obj = formatter.Deserialize(fs);
            }

            return obj;
        }

        internal static object LoadJSON(Type _type, string _path)
        {
            return JsonSerializer.Deserialize(File.ReadAllText(_path), _type);
        }
    }
}
