using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domus.Models
{
    public class RealEstateTypeConfiguration : IEntityTypeConfiguration<RealEstate>
    {
        public void Configure(EntityTypeBuilder<RealEstate> builder)
        {
            builder.Property(r => r.address).IsRequired();
            builder.Property(r => r.finnkode).IsRequired();
            builder.Property(r => r.price_nok).IsRequired();
            builder.Property(r => r.area_sqm).IsRequired();
            builder.Property(r => r.lat).IsRequired();
            builder.Property(r => r.lon).IsRequired();
        }
    }
}