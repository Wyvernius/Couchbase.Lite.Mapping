using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Couchbase.Lite.Mapping.Converters
{
    public class StringToDateOnlyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(DateOnly).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateOnly.Parse((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var newValue = value.ToString();
            serializer.Serialize(writer, newValue);
        }
    }
}
