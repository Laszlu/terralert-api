using Microsoft.AspNetCore.Mvc.Testing;
using TerralertAPI;
using TerralertAPI.Model;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace EONET.API.IntegrationTests.EndpointTests.EventEndpointTests;

public class EventsEndpointTests : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _httpClient;
    
    private ITestOutputHelper _outputHelper;

    public EventsEndpointTests(WebApplicationFactory<IApiMarker> webApplicationFactory, ITestOutputHelper outputHelper)
    {
       _httpClient = webApplicationFactory.CreateClient();
       _outputHelper = outputHelper;
    }
    
    [Fact]
    public async Task GetCurrentEventsByCategory_ValidRequest_ReturnsCurrentEvents()
    {
        _outputHelper.WriteLine("GetCurrentEventsByCategory");
        
        foreach (var eventCategory in EventCategoryMapper.AllCategories)
        {
            var response = await _httpClient.GetAsync($"api/events/{eventCategory.Code}/current");
            
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task GetSingleEventByID_ValidRequest_ReturnsSingleEvent()
    {
        _outputHelper.WriteLine("GetSingleEventByID");

        var currentEvents =
            await _httpClient.GetAsync($"api/events/{EventCategoryMapper.AllCategories.First().Code}/current");
        
        var eventList = JsonConvert.DeserializeObject<List<Event>>(await currentEvents.Content.ReadAsStringAsync());

        if (eventList != null)
        {
            var singleEvent = eventList.FirstOrDefault();

            if (singleEvent != null)
            {
                var id = singleEvent.Id;
                var category = singleEvent.Categories?.FirstOrDefault();
                var response = await _httpClient.GetAsync($"api/events/{EventCategoryMapper.FromFullString(category.Id).Code}/{id}");
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
                response.Should().NotBeNull();
            }
        }
    }
}