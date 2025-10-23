using System.Drawing;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonException = Newtonsoft.Json.JsonException;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace TerralertAPI.Model;

public class EonetEventListResult
{
    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("link")]
    public string? Link { get; set; }

    [JsonProperty("events")]
    public List<Event>? Events { get; set; }
}

public class Event
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("link")]
    public string? Link { get; set; }

    [JsonProperty("closed")]
    public DateTime? Closed { get; set; }

    [JsonProperty("categories")]
    public List<Category>? Categories { get; set; }

    [JsonProperty("sources")]
    public List<Source>? Sources { get; set; }

    [JsonProperty("geometry")]
    public List<Geometry>? Geometry { get; set; }
}

public class Source
{
    [JsonProperty("id")]
    public string? Id { get; set; }
    
    [JsonProperty("url")]
    public string? Url { get; set; }
}

public class Category
{
    [JsonProperty("id")]
    public string? Id { get; set; }
    
    [JsonProperty("title")]
    public string? Title { get; set; }
}

public class Geometry
{
    [JsonProperty("magnitudeValue")]
    public float? MagnitudeValue { get; set; }

    [JsonProperty("magnitudeUnit")]
    public string? MagnitudeUnit { get; set; }

    [JsonProperty("date")]
    public DateTime? Date { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("coordinates")]
    [JsonConverter(typeof(CoordinatesConverter))]
    public List<List<double>>? Coordinates { get; set; }
}

public class CoordinatesConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(List<List<double>>);

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);

        if (token.First?.Type == JTokenType.Float)
        {
            // [lon, lat]
            var lon = token[0].ToObject<double>();
            var lat = token[1].ToObject<double>();
            return new List<List<double>> { new() { lon, lat } };
        }
        else
        {
            // [[[lon, lat], ...]]
            return token[0].Select(p => p.Select(x => (double)x).ToList()).ToList();
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var valueCasted = value as List<List<double>>;
        if (valueCasted.Count == 1)
        {
            // Point: [lon, lat]
            writer.WriteStartArray();
            writer.WriteValue(valueCasted[0][0]);
            writer.WriteValue(valueCasted[0][1]);
            writer.WriteEndArray();
        }
        else
        {
            // Polygon: [[[lon, lat], ...]]
            writer.WriteStartArray();
            writer.WriteStartArray();
            foreach (var pair in valueCasted)
            {
                writer.WriteStartArray();
                writer.WriteValue(pair[0]);
                writer.WriteValue(pair[1]);
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
            writer.WriteEndArray();
        }
    }
}
