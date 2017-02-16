using SmarterBalanced.SampleItems.Dal.Translations;
using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public sealed class Target
    {
        public string Type { get; }
        public string Key { get; }
        public string Name { get; }
        public string Description { get; }
        public StandardIdentifier StandardIdentifier {get;}
        public Target(string type, string key, string name, string description, StandardIdentifier identifier)
        {
            Type = type;
            Key = key;
            Name = name;
            Description = description;
            StandardIdentifier = identifier;
        }
        public static Target Create(
            string type = "",
            string key = "",
            string name = "",
            string description = "",
            StandardIdentifier identifier = null)
        {
            return new Target(
                type: type,
                key: key,
                name: name,
                description: description,
                identifier: identifier);
        }
        public static Target Create(XElement element)
        {
            string key = (string)element.Attribute("Key");

            var standard = StandardIdentifierTranslation.StandardKeyToIdentifier(key);
            var target = Create(
                type: (string)element.Attribute("Type"),
                key: key,
                name: (string)element.Attribute("Name"),
                description: (string)element.Attribute("Description"),
                identifier: standard
                );

            return target;
        }

    }

}
