using Newtonsoft.Json;
using TerralertAPI.Model;

namespace TerralertAPI.Helper;

public static class EventConversionHelper
{
    public static TerralertEvent ConvertEonetEvent(EonetEvent eonetEvent)
    {
        var responseEvent = new TerralertEvent
        {
            Id = eonetEvent.Id,
            Title = eonetEvent.Title,
            Description = eonetEvent.Description,
            Link = eonetEvent.Link,
            Closed = eonetEvent.Closed,
            Categories = eonetEvent.Categories,
            Sources = eonetEvent.Sources,
            Geometry = []
        };

        foreach (var eonetGeometry in eonetEvent.Geometry)
        {
            responseEvent.Geometry.Add(ConvertStringCoordinates(eonetGeometry));
        }
        
        return responseEvent;
    }
    
    private static TerralertGeometry ConvertStringCoordinates(EonetGeometry eonetGeometry)
    {
        var responseGeometry = new TerralertGeometry
        {
            MagnitudeValue = eonetGeometry.MagnitudeValue,
            MagnitudeUnit = eonetGeometry.MagnitudeUnit,
            Date = eonetGeometry.Date,
            Type = eonetGeometry.Type,
            Coordinates = null
        };

        if (eonetGeometry.Coordinates == null) return responseGeometry;
        
        switch (responseGeometry.Type)
        {
            case "Point":
                var pointCoordinateFromJson = JsonConvert.DeserializeObject<List<double>>(eonetGeometry.Coordinates);
                var pointCoordinates = new TerralertCoordinates { PointCoordinates = pointCoordinateFromJson, PolygonCoordinates = null};
                responseGeometry.Coordinates = pointCoordinates;
                break;
            case "Polygon":
                var polygonCoordinatesFromJson = JsonConvert.DeserializeObject<List<List<List<double>>>>(eonetGeometry.Coordinates);
                var polygonCoordinates = new TerralertCoordinates { PolygonCoordinates = polygonCoordinatesFromJson, PointCoordinates = null};
                responseGeometry.Coordinates = polygonCoordinates;
                break;
        }

        return responseGeometry;
    }
}