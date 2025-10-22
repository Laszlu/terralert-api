namespace TerralertAPI.Model;

public record Region(string Name, double MinLongitude, double MaxLatitude, double MaxLongitude, double MinLatitude);

public static class RegionCoordinateMapper
{
    private static readonly Region NorthAtlantic = new("northatlantic", -80.00, 70.00, -20.00, 0.00);
    private static readonly Region NorthEastPacific = new("northeastpacific", -160.00, 70.00, -110.00, 0.00);
    private static readonly Region NorthWestPacific = new("northwestpacific", 110.00, 70.00, 180.00, 0.00);
    private static readonly Region NorthIndian = new("northindian", 20.00, 30.00, 100.00, 0.00);
    private static readonly Region SouthWestIndian = new("southwestindian", 20.00, 0.00, 100.00, -50.00);
    private static readonly Region AustraliaWithOcean = new("australiaocean", 110.00, -10.00, 170.00, -45.00);
    private static readonly Region SouthWestPacific = new("southwestpacific", -120.00, 0.00, 150.00, -50.00);
    private static readonly Region Europe = new("europe", -25.00, 72.00, 45.00, 35.00);
    private static readonly Region Asia = new("asia", 25.00, 80.00, 180.00, 0.00);
    private static readonly Region Africa = new("africa", -20.00, 38.00, 55.00, -38.00);
    private static readonly Region NorthAmerica = new("northamerica", -170.00, 83.00, -50.00, 10.00);
    private static readonly Region SouthAmerica = new("southamerica", -90.00, 15.00, -30.00, -60.00);
    private static readonly Region Australia = new("australia", 110.00, -10.00, 155.00, -45.00);
    private static readonly Region PacificRingOfFire = new("pacificringoffire", -140.00, 65.00, 150.00, -60.00);
    private static readonly Region CentralAmericaWithCaribbean = new("centralamericacaribbean", -120.00, 32.00, -60.00, 5.00);
    private static readonly Region Oceania = new("oceania", -120.00, 10.00, 110.00, -50.00);

    public static readonly IReadOnlyList<Region> AllRegions = new[]
    {
        NorthAtlantic, NorthEastPacific, NorthWestPacific, NorthIndian, SouthWestIndian, AustraliaWithOcean,
        SouthWestPacific, Europe, Asia, Africa, NorthAmerica, SouthAmerica, Australia, PacificRingOfFire,
        CentralAmericaWithCaribbean, Oceania
    };

    public static readonly IReadOnlyList<Region> StormRegions = new[]
    {
        NorthAtlantic, NorthEastPacific, NorthWestPacific, NorthIndian, SouthWestIndian, AustraliaWithOcean,
        SouthWestPacific
    };

    public static readonly IReadOnlyList<Region> EarthQuakeRegions = new[]
    {
        Europe, Asia, Africa, NorthAmerica, SouthAmerica, Australia
    };

    public static readonly IReadOnlyList<Region> VolcanoeRegions = new[]
    {
        Africa, Europe, PacificRingOfFire, NorthAmerica, CentralAmericaWithCaribbean, SouthAmerica, Asia, Oceania
    };

    public static Region? FromNameString(string name) =>
        AllRegions.FirstOrDefault(r => r.Name == name);
}