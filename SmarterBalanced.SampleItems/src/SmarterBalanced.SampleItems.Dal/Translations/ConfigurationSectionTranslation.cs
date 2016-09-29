using Microsoft.Extensions.Configuration;
using SmarterBalanced.SampleItems.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public static class ConfigurationSectionTranslation
    {
        public static T ConfigurationSectionToObject<T>(this IConfigurationSection section) where T : new()
        {
            Type tType = typeof(T);

            T result = (T)Activator.CreateInstance(typeof(T));

            List<IConfigurationSection> sectionMembers = section.GetChildren().ToList();
            List<PropertyInfo> tMembers = tType.GetRuntimeProperties().ToList();

            foreach (var prop in sectionMembers)
            {
                var destField = tMembers.SingleOrDefault(t => t.Name == prop.Key);
                if (destField != null)
                {
                    destField.SetValue(result, Convert.ChangeType(prop.Value, destField.PropertyType), null);
                }
            }

            return result;
        }

      

    }
}
