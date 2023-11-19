namespace DomusDatabase.Models;

///<summary>
///RealEstate model for Finn article data
///</summary>
public class RealEstate
{
    public int finnkodeId { get; set; }
    public int price { get; set; }
    public int sqm { get; set; }
    public float sqmPrice { get; set; }
    public string? address { get; set; }
    public float lat { get; set; }
    public float lon { get; set; }
}