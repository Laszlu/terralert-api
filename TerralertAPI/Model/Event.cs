using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    public List<EonetEvent>? Events { get; set; }
}

public class ResponseEventListResult
{
    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("link")]
    public string? Link { get; set; }

    [JsonProperty("events")]
    public List<ResponseEvent>? Events { get; set; }
}

public class EonetEvent
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
    public List<EonetGeometry>? Geometry { get; set; }
}

public class ResponseEvent
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
    public List<ResponseGeometry>? Geometry { get; set; }
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

public class EonetGeometry
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
    [JsonConverter(typeof(RawJsonStringConverter))]
    public string? Coordinates { get; set; }
}

public class ResponseGeometry
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
    public ResponseCoordinates? Coordinates { get; set; }
}

public class ResponseCoordinates
{
    public List<double>? PointCoordinates { get; set; }
    
    public List<List<List<double>>>? PolygonCoordinates { get; set; }
}

public class RawJsonStringConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(string);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartArray || reader.TokenType == JsonToken.StartObject)
        {
            JToken token = JToken.Load(reader);
            return token.ToString(Formatting.None);
        }
        return reader.Value?.ToString();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var str = value as string;
        if (string.IsNullOrWhiteSpace(str))
        {
            writer.WriteNull();
            return;
        }

        try
        {
            var token = JToken.Parse(str);
            token.WriteTo(writer);
        }
        catch
        {
            writer.WriteValue(str);
        }
    }
}

