using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace PES.Application.Utilities
{
    public static class RedisDataConverter
    {
        public static T ConvertFromRedis<T>(HashEntry[] hashEntries) where T : new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var obj = new T();

            foreach (var property in properties)
            {
                HashEntry entry = hashEntries.FirstOrDefault(g => g.Name.ToString().Equals(property.Name, StringComparison.OrdinalIgnoreCase));

                if (entry.Equals(default(HashEntry))) continue;

                object value = entry.Value.ToString();
                Type propertyType = property.PropertyType;

                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (string.IsNullOrEmpty(value.ToString()))
                    {
                        property.SetValue(obj, null);
                        continue;
                    }
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }

                try
                {
                    property.SetValue(obj, Convert.ChangeType(value, propertyType));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting {value} to {propertyType}: {ex.Message}");
                }
            }

            return (T)obj;
        }

        public static HashEntry[] ToHashEntries(object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            return properties
                .Where(x => x.GetValue(obj) != null) // <-- PREVENT NullReferenceException
                .Select
                (
                      property =>
                      {
                          object propertyValue = property.GetValue(obj);
                          string hashValue;

                          // This will detect if given property value is 
                          // enumerable, which is a good reason to serialize it
                          // as JSON!
                          if (propertyValue is IEnumerable<object>)
                          {
                              // So you use JSON.NET to serialize the property
                              // value as JSON
                              hashValue = JsonConvert.SerializeObject(propertyValue);
                          }
                          else
                          {
                              hashValue = propertyValue.ToString();
                          }

                          return new HashEntry(property.Name, hashValue);
                      }
                )
                .ToArray();
        }
    }




}