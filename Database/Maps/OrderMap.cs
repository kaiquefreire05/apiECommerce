using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Database.Maps
{
    public class OrderMap : IEntityTypeConfiguration<OrderModel>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<OrderModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OrderDate).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.HasOne(x => x.User)  // Specifies that each instance of OrdersModel has one related UserModel
                .WithMany(x => x.Orders)  // Specifies that each UserModel can have many related OrdersModel instances
                .HasForeignKey(x => x.UserId);  // Sets UserId as the foreign key in OrdersModel that references the Id in UserModel
        }
    }
}
