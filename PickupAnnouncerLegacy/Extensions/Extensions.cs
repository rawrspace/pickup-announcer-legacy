using Newtonsoft.Json;
using System.Collections.Generic;

namespace PickupAnnouncerLegacy.Extensions
{
    public static class Extensions
    {
        public static T ToType<V, T>(this IDictionary<string, V> dictionary)
        {
            var json = JsonConvert.SerializeObject(dictionary);
            var config = JsonConvert.DeserializeObject<T>(json);

            return config;
        }
    }
}
