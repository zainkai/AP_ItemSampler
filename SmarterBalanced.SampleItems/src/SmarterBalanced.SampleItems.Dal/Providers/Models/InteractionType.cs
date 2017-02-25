using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public sealed class InteractionType
    {
        public string Code { get; }
        public string Label { get; }
        public string Description { get; }
        public int Order { get; }

        public InteractionType(string code, string label, string description, int order)
        {
            Code = code;
            Label = label;
            Description = description;
            Order = order;
        }

        public static InteractionType Create(string code = "", string label = "", string description = "", int order = -1)
        {
            return new InteractionType(
                code,
                label,
                description,
                order
            );
        }
        public static InteractionType Create(XElement elem)
        {
            var interactionType = new InteractionType(
                code: (string)elem.Element("Code"),
                label: (string)elem.Element("Label"),
                description: (string)elem.Element("Description"),
                order: elem.Element("Order") == null ? 0 : (int)elem.Element("Order"));

            return interactionType;
        }

    }
}
