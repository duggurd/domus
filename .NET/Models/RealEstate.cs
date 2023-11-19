using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domus.Models
{
    public class RealEstate
    {   
        public string? finnkode { get; set; }
        public Int32? price_nok { get; set; } 
        public int? area_sqm { get; set; }
        public string? address { get; set; } 
        public float? lat { get; set; }
        public float? lon { get; set; }
        public int? nok_sqm { get; set; } = 0;
        // public DateTime published { get; set; }
    }
}