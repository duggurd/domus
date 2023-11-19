namespace Domus;
public class RealEstate
{
    public int finnkodeId { get; set; }
    public int price { get; set; }
    public int squareMeters { get; set; }
    public float squareMeterPrice { get; set; }
    public string? address { get; set; }
    public float lat { get; set; }
    public float lon { get; set; }
}