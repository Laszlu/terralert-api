namespace TerralertAPI.Model;

public record EventCategory(string Code, string FullCategoryString);

public static class EventCategoryMapper
{
    public static readonly EventCategory SevereStorms = new("st", "severeStorms");
    public static readonly EventCategory Wildfires = new("wi", "wildfires");
    public static readonly EventCategory Volcanoes = new("vo", "volcanoes");
    public static readonly EventCategory Earthquakes = new("ea", "earthquakes");

    public static readonly IReadOnlyList<EventCategory> AllCategories = new[]
    {
        SevereStorms, Wildfires, Volcanoes, Earthquakes
    };

    public static EventCategory? FromCode(string code) =>
        AllCategories.FirstOrDefault(c => c.Code == code);
    
    public static EventCategory? FromFullString(string fullCategoryString) => 
        AllCategories.FirstOrDefault(c => c.FullCategoryString == fullCategoryString);
}