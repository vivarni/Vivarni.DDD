using System;
using Newtonsoft.Json;

namespace Vivarni.Domain.Core.Time
{
    public class DateSpanConverter : JsonConverter<DateSpan>
    {
        public override DateSpan ReadJson(JsonReader reader, Type objectType, DateSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;
            return DateSpan.FromString(s);
        }

        public override void WriteJson(JsonWriter writer, DateSpan value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
