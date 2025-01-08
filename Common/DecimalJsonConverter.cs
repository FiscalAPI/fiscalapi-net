using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Fiscalapi.Common
{
    public class DecimalJsonConverter : JsonConverter<decimal>
    {
        public override decimal ReadJson(JsonReader reader, Type objectType, decimal existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            return Convert.ToDecimal(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, decimal value, JsonSerializer serializer)
        {
            writer.WriteRawValue(value == Math.Floor(value)
                ? value.ToString("0")
                : value.ToString(CultureInfo.InvariantCulture));
        }
    }
}