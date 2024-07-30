using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceApi.Database.Maps
{
    public class OrderItemMap : IEntityTypeConfiguration<OrderItemModel>
    {
        public void Configure(EntityTypeBuilder<OrderItemModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.OrderId).IsRequired();

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.OrderId);

            builder.HasOne(x => x.Products)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.ProductId);
        }       
    }
}
