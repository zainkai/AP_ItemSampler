using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Infrastructure
{
    /// <summary>
    /// Methods to serialize XML files into objects
    /// </summary>
    public static class XmlSerialization
    {
        /// <summary>
        /// Deserializes the given XML file into the specified type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        public static T DeserializeXml<T>(FileInfo file)
        {
            T xml = default(T);

            if (file.Exists)
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var reader = file.OpenRead())
                {
                    xml = (T)serializer.Deserialize(reader);
                }
            }

            return xml;
        }

        /// <summary>
        /// Deserializes a list of XML files into a list of objects of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="files"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> DeserializeXmlFilesAsync<T>(IEnumerable<FileInfo> files)
        {
            BlockingCollection<T> fileData = new BlockingCollection<T>();

            await Task.Run(() => Parallel.ForEach<FileInfo>(files, (file) => { fileData.Add(DeserializeXml<T>(file)); }
            ));
            return fileData;
        }

        /// <summary>
        /// Finds the paths for metadata.xml files in a directory
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<FileInfo>> FindMetadataXmlFiles(string directory)
        {
            IEnumerable<FileInfo> files;
            var getFiles = Task.Run(() =>
            {
                return new DirectoryInfo(directory).GetFiles("metadata.xml", SearchOption.AllDirectories);
            });
            files = await getFiles;
            return files;
        }

        /// <summary>
        /// Finds the paths for item-BANK-KEY.xml files in a directory
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<FileInfo>> FindContentXmlFiles(string directory)
        {
            IEnumerable<FileInfo> files;
            Regex filePattern = new Regex(@"item-\d+-\d+.xml");
            var getFiles = Task.Run(() =>
            {
                return new DirectoryInfo(directory).GetFiles("*.xml", SearchOption.AllDirectories).Where(file => filePattern.IsMatch(file.Name));
            });
            files = await getFiles;
            return files;
        }
    }
}
