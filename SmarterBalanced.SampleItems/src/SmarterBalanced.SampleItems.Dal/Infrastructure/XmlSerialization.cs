using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Infrastructure
{
    public static class XmlSerialization
    {
       public static T DeserializeXml<T>(FileInfo file)
        {
            T xml = default(T);

            if (!file.Exists)
            {
                throw new Exception("File does not exist.");
            }

            var serializer = new XmlSerializer(typeof(T));

            using (var reader = file.OpenRead())
            {
                xml = (T)serializer.Deserialize(reader);
            }

            return xml;
        }


        public static IEnumerable<T> DeserializeXmlFiles<T>(IEnumerable<FileInfo> files)
        {
            BlockingCollection<T> fileData = new BlockingCollection<T>();

            Parallel.ForEach<FileInfo>(files, (file) =>
            {
                fileData.Add(DeserializeXml<T>(file));
            });

            return fileData;
        }
    }
}
