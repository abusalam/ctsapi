using System.Text.Json;
using System.Text.Json.Serialization;

namespace CTS_BE.Helper
{
    public class JsonStringDateOnlyConverter : JsonConverter<DateOnly>
    {
        private readonly string _format;
        public JsonStringDateOnlyConverter(string format)
        {
            _format = format;
        }
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.ParseExact(reader.GetString() ?? "", _format, null);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }
    }
}