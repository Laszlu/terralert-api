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
    
    [Theory]
    [ClassData(typeof(CategoryData))]
    public async Task GetCurrentEventsByCategory_ValidRequest_ReturnsCurrentEvents(EventCategory eventCategory)
    {
        _outputHelper.WriteLine("GetCurrentEventsByCategory");
        
        var response = await _httpClient.GetAsync($"api/events/{eventCategory.Code}/current");
        _outputHelper.WriteLine(response.Content.ReadAsStringAsync().Result);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
    }

    [Fact]
    public async Task GetSingleEventByID_ValidRequest_ReturnsSingleEvent()
    {
        _outputHelper.WriteLine("GetSingleEventByID");

        var currentEvents =
            await _httpClient.GetAsync($"api/events/{EventCategoryMapper.AllCategories.First().Code}/current");
        
        var eventList = JsonConvert.DeserializeObject<List<EonetEvent>>(await currentEvents.Content.ReadAsStringAsync());

        if (eventList != null)
        {
            var singleEvent = eventList.FirstOrDefault();

            if (singleEvent != null)
            {
                var id = singleEvent.Id;
                var category = singleEvent.Categories?.FirstOrDefault();
                var response = await _httpClient.GetAsync($"api/events/{EventCategoryMapper.FromFullString(category.Id).Code}/{id}");
                _outputHelper.WriteLine(response.Content.ReadAsStringAsync().Result);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
                response.Content.Should().NotBeNull();
            }
        }
    }

    [Theory]
    [ClassData(typeof(CategoryRegionYearData))]
    public async Task GetEventsByCategoryRegionAndYear_ValidRequest_ReturnsEventList(EventCategory category, Region region, int year)
    {
        _outputHelper.WriteLine("GetEventsByCategoryRegionAndYear");

        var response = await _httpClient.GetAsync($"api/events/{category.Code}/{region.Name}/{year}");

        _outputHelper.WriteLine(response.Content.ReadAsStringAsync().Result);
        
        if (year >= DateTime.Today.Year - 10)
        {
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Content.Should().NotBeNull();
        }
        else
        {
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}