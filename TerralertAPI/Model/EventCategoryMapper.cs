namespace TerralertAPI.Model;

public record EventCategory(string Code, string FullCategoryString);

public static class EventCategoryMapper
{
    private static readonly EventCategory SevereStorms = new("st", "severeStorms");
    //private static readonly EventCategory Wildfires = new("wi", "wildfires");
    private static readonly EventCategory Volcanoes = new("vo", "volcanoes");
    private static readonly EventCategory Earthquakes = new("ea", "earthquakes");

    public static readonly IReadOnlyList<EventCategory> AllCategories = new[]
    {
        SevereStorms, Volcanoes, Earthquakes
    };

    public static EventCategory? FromCode(string code) =>
        AllCategories.FirstOrDefault(c => c.Code == code);
    
    public static EventCategory? FromFullString(string fullCategoryString) => 
        AllCategories.FirstOrDefault(c => c.FullCategoryString == fullCategoryString);
}