using Newtonsoft.Json;
using TerralertAPI.Model;

namespace TerralertAPI.Helper;

public static class EventConversionHelper
{
    public static ResponseEvent ConvertEonetEvent(EonetEvent eonetEvent)
    {
        var responseEvent = new ResponseEvent
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
    
    private static ResponseGeometry ConvertStringCoordinates(EonetGeometry eonetGeometry)
    {
        var responseGeometry = new ResponseGeometry
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
                Console.WriteLine(eonetGeometry.Coordinates);
                var pointCoordinateFromJson = JsonConvert.DeserializeObject<List<double>>(eonetGeometry.Coordinates);
                var pointCoordinates = new ResponseCoordinates { PointCoordinates = pointCoordinateFromJson, PolygonCoordinates = null};
                responseGeometry.Coordinates = pointCoordinates;
                break;
            case "Polygon":
                Console.WriteLine(eonetGeometry.Coordinates);
                var polygonCoordinatesFromJson = JsonConvert.DeserializeObject<List<List<double>>>(eonetGeometry.Coordinates);
                var polygonCoordinates = new ResponseCoordinates { PolygonCoordinates = polygonCoordinatesFromJson, PointCoordinates = null};
                responseGeometry.Coordinates = polygonCoordinates;
                break;
        }

        return responseGeometry;
    }
}