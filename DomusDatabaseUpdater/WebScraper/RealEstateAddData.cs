namespace Domus;

public class FinnRealEstateAddData
    {
        public int finnkodeId { get; set; }
        public int price { get; set; }
        public int sqm { get; set; }
        public float sqmPrice => price / sqm;
        public string? address { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
    }
