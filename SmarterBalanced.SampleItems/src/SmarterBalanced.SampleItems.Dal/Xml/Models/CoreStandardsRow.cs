using SmarterBalanced.SampleItems.Dal.Translations;
using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Xml.Models
{
    public sealed class CoreStandardsRow 
    {
        public string SubjectCode { get; }
        public string Key { get; }
        public string Name { get; }
        public string Description { get; }
        public string LevelType { get; }
        public StandardIdentifier StandardIdentifier { get; set; }
        public CoreStandardsRow(string type, string key, string name, string description, string levelType, StandardIdentifier identifier)
        {
            SubjectCode = type;
            Key = key;
            Name = name;
            Description = description;
            StandardIdentifier = identifier;
            LevelType = levelType;
        }
        public static CoreStandardsRow Create(
            string type = "",
            string key = "",
            string name = "",
            string description = "",
            string levelType = "",
            string subjectCode = "",
            StandardIdentifier identifier = null)
        {
            return new CoreStandardsRow(
                type: type,
                key: key,
                name: name,
                description: description,
                levelType: levelType,
                identifier: identifier);
        }

        public static CoreStandardsRow Create(XElement element)
        {

            var row = Create(
                type: (string)element.Attribute("Type"),
                key: (string)element.Attribute("Key"),
                name: (string)element.Attribute("Name"),
                description: (string)element.Attribute("Description"),
                levelType: (string)element.Attribute("Description"));

            row.StandardIdentifier = StandardIdentifierTranslation.CoreStandardToIdentifier(row);

            return row;
        }

    }

}
