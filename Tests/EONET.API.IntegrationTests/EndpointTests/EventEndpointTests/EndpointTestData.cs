using System.Collections;
using TerralertAPI.Model;

namespace EONET.API.IntegrationTests.EndpointTests.EventEndpointTests;

public class CategoryData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        foreach (var eventCategory in EventCategoryMapper.AllCategories)
        {
            yield return [eventCategory];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class CategoryRegionYearData : IEnumerable<object[]>
{
    private readonly int[] _testYears = [2014, 2015, 2018, 2020, 2023, 2025];
    
    public IEnumerator<object[]> GetEnumerator()
    {
        foreach (var eventCategory in EventCategoryMapper.AllCategories)
        {
            foreach (var year in _testYears)
            {
                switch (eventCategory.Code)
                {
                    case "st":
                        foreach (var region in RegionCoordinateMapper.StormRegions)
                        {
                            yield return [eventCategory, region, year];
                        }
                        break;
                    case "ea":
                        foreach (var region in RegionCoordinateMapper.EarthQuakeRegions)
                        {
                            yield return [eventCategory, region, year];
                        }

                        break;
                    case "vo":
                        foreach (var region in RegionCoordinateMapper.VolcanoeRegions)
                        {
                            yield return [eventCategory, region, year];
                        }
                        break;
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
}