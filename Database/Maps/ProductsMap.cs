using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceApi.Database.Maps
{
    public class ProductsMap : IEntityTypeConfiguration<ProductsModel>
    {
        public void Configure(EntityTypeBuilder<ProductsModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(x => x.Price).HasColumnType("decimal(18, 2)").IsRequired();
            builder.Property(x => x.Stock).IsRequired();
            builder.Property(x => x.Category).IsRequired();
        }
    }
}
