using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public class FileHelper<T> where T : new()
    {
        private string _xmlPath;
        public FileHelper(string path)
        {
            this._xmlPath = path;
        }
        public void SerializeToXml(T students)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var streamWriter = new StreamWriter(_xmlPath))
            {
                serializer.Serialize(streamWriter, students);
                streamWriter.Close();
            }
        }

        public T DeserializeFromXml()
        {
            if (!File.Exists(_xmlPath))
                return new T();

            var serializer = new XmlSerializer(typeof(T));

            using (var streamReader = new StreamReader(_xmlPath))
            {
                var students = (T)serializer.Deserialize(streamReader);
                streamReader.Close();
                return students;
            }
        }
    }
}
