

using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Par.CommandCenter.Application.Common
{
    public class JsonDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {

        public override void WriteJson(JsonWriter writer, DateTimeOffset value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(
                            "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture));
        }

        public override DateTimeOffset ReadJson(JsonReader reader, Type objectType, DateTimeOffset existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return DateTimeOffset.ParseExact(reader.Value.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}
