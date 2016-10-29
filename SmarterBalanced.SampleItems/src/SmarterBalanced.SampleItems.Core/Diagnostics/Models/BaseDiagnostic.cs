using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace SmarterBalanced.SampleItems.Core.Diagnostics.Models
{
    public class BaseDiagnostic
    {
        [XmlAttribute(AttributeName = "statusRating")]
        public int StatusRating { get; set; }

        [XmlAttribute(AttributeName = "statusText")]
        public string StatusText
        {
            get
            {
                return Enum.GetName(typeof(DiagnosticManager.StatusRating), StatusRating);
            }
            set { }
        }

        [XmlElement(ElementName = "error")]
        public List<string> ErrorMessages { get; set; }

        [XmlElement(ElementName = "warning")]
        public string WarningMessage { get; set; }


        public BaseDiagnostic()
        {
            StatusRating = (int)DiagnosticManager.StatusRating.Ideal;
        }

        public void AddErrorMessage(string message)
        {
            if (ErrorMessages == null)
            {
                ErrorMessages = new List<string>()
                {
                    message
                };
            }
            else
            {
                ErrorMessages.Add(message);
            }
        }
    }
}
