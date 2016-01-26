using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TicketManagement.ViewModels;

namespace TicketManagement.Helpers
{
    public static class FacebookHelpers
    {
        public class PropertyContainer
        {
            public string FacebookField { get; set; }
            public string FacebookParent { get; set; }
            public PropertyInfo FacebookMappedProperty { get; set; }
        }

        public static T ToStatic<T>(object dynamicObject)
        {
            var entity = Activator.CreateInstance<T>();

            var properties = dynamicObject as IDictionary<string, object>;

            if (properties == null)
                return entity;

            Dictionary<string, PropertyContainer> propertyLookup = new Dictionary<string, PropertyContainer>();

            var destinationFacebookMappingProperties = (from PropertyInfo property in entity.GetType().GetProperties()
                                                        where property.GetCustomAttributes(typeof(FacebookMapping), true).Length > 0
                                                        select property).ToList();

            foreach (PropertyInfo propInfo in destinationFacebookMappingProperties)
            {
                foreach (Attribute attribute in propInfo.GetCustomAttributes(typeof(FacebookMapping)))
                {
                    FacebookMapping facebookMapAttribute = attribute as FacebookMapping;

                    if (facebookMapAttribute != null)
                    {
                        var facebookLookupKey = string.IsNullOrEmpty(facebookMapAttribute.Parent) ? facebookMapAttribute.GetName() : facebookMapAttribute.Parent;

                        propertyLookup.Add(facebookLookupKey,
                            new PropertyContainer
                            {
                                FacebookField = facebookMapAttribute.GetName(),
                                FacebookParent = facebookMapAttribute.Parent,
                                FacebookMappedProperty = propInfo
                            });
                    }
                }
            }

            foreach (var entry in properties)
            {
                if (!propertyLookup.ContainsKey(entry.Key)) continue;
                PropertyContainer destinationPropertyInfo = propertyLookup[entry.Key];

                if (destinationPropertyInfo != null)
                {
                    object mappedValue;
                    if (entry.Value.GetType().Name == "JsonObject")
                    {
                        var childProperties = entry.Value as IDictionary<string, object>;

                        mappedValue = (from KeyValuePair<string, object> item in childProperties
                            where item.Key == destinationPropertyInfo.FacebookField
                            select item.Value).FirstOrDefault() ?? entry.Value;
                    }
                    else
                        mappedValue = entry.Value;

                    if (destinationPropertyInfo.FacebookMappedProperty.PropertyType.Name == "DateTime")
                    {
                        DateTime ukDateTime = DateTime.ParseExact(DateTime.Parse(mappedValue.ToString()).ToString(), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        destinationPropertyInfo.FacebookMappedProperty.SetValue(entity, ukDateTime, null);
                    }
                    else
                        destinationPropertyInfo.FacebookMappedProperty.SetValue(entity, mappedValue, null);
                }
            }

            return entity;
        }
    }
}
