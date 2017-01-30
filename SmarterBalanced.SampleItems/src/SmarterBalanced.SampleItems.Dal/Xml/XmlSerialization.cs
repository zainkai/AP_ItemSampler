using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Xml
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
            if (!file.Exists)
                throw new FileNotFoundException(message: $"File does not exist: {file.FullName}", fileName: file.FullName);

            T xml = default(T);
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = file.OpenRead())
            {
                xml = (T)serializer.Deserialize(reader);
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

            // TODO: do these Task.Runs improve performance?
            await Task.Run(() => Parallel.ForEach(files, (file) => fileData.Add(DeserializeXml<T>(file))));
            return fileData;
        }

        /// <summary>
        /// Finds the paths for metadata.xml files in a directory
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static FileInfo[] FindMetadataXmlFiles(string directory)
        {
            var directoryInfo = new DirectoryInfo(directory);
            var files = directoryInfo.GetFiles("metadata.xml", SearchOption.AllDirectories);
            return files;
        }

        /// <summary>
        /// Finds the paths for item-BANK-KEY.xml files in a directory
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> FindContentXmlFiles(string directory)
        {
            Regex filePattern = new Regex(@"item-\d+-\d+.xml");
            var directoryInfo = new DirectoryInfo(directory);
            
            var files = directoryInfo
                .GetFiles("*.xml", SearchOption.AllDirectories)
                .Where(file => filePattern.IsMatch(file.Name));

            return files;
        }

        public static XElement GetXDocumentElement(string location, string rootElement)
        {
            return XDocument.Load(location).Element(rootElement);
        }

        public static XDocument GetXDocument(string location)
        {
            return XDocument.Load(location);
        }
    }
}
