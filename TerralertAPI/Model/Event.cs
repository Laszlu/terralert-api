using Newtonsoft.Json;

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
    public List<float>? Coordinates { get; set; }
}