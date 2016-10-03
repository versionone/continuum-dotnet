using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ContinuumDotNet.Utilities
{
    public static class RedisUtils
    {
        //Serialize in Redis format:
        public static HashEntry[] ToHashEntries(this object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            return properties.Where(property => property.GetValue(obj) != null).Select(property => new HashEntry(property.Name, property.GetValue(obj).ToString())).ToArray();
        }

        //Deserialize from Redis format
        public static T ConvertFromRedis<T>(this HashEntry[] hashEntries)
        {
            var properties = typeof(T).GetProperties();
            var obj = Activator.CreateInstance(typeof(T));
            if (hashEntries == null) return default(T);
            foreach (var property in properties)
            {
                var entry = hashEntries.FirstOrDefault(g => g.Name.ToString().Equals(property.Name));
                if (entry.Equals(new HashEntry())) continue;
                property.SetValue(obj, Convert.ChangeType(entry.Value.ToString(), property.PropertyType));
            }
            return (T)obj;
        }
    }
}
