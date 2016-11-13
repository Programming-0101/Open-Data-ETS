using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace EdmontonTransit.Common
{
    /// <summary>
    /// The XMLFileIO class manages loading and saving any type to an XML file.
    /// </summary>
    public class XMLFileIO<T>
    {
        public string FilePath { get; private set; }

        public XMLFileIO(string filePath)
        {
            FilePath = filePath;
        }

        public List<T> LoadAll()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<T>));
            List<T> data;
            using (TextReader textReader = new StreamReader(FilePath))
            {
                data = (List<T>)deserializer.Deserialize(textReader);
                textReader.Close();
            }
            return data;
        }

        public void SaveAll(List<T> data, bool append)
        {
            if (append && File.Exists(FilePath))
            {
                List<T> existingData = LoadAll();
                existingData.AddRange(data);
                data = existingData;
            }
            using (Stream stream = File.Open(FilePath, FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                using (TextWriter writer = new StreamWriter(stream))
                {
                    serializer.Serialize(writer, data);
                    writer.Close();
                }
            }
        }
    }

}
