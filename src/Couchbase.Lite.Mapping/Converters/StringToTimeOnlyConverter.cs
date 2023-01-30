using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Couchbase.Lite.Mapping.Converters
{
    internal class StringToTimeOnlyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(TimeOnly).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return TimeOnly.Parse((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var newValue = (TimeOnly)TimeOnly.Parse((string)value);
            serializer.Serialize(writer, newValue);
        }
    }
}
