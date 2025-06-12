using TerralertAPI.Model;

namespace TerralertAPI;

public interface IEonetService
{
    public string? ConvertEventCategory(string requestCategory);

    public Task<List<Event>?> EonetGetCurrentEventsForCategory(string category);

    public Task<Event?> EonetGetEventById(string id);
}