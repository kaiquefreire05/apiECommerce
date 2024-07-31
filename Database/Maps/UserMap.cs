using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Database.Maps
{
    public class UserMap : IEntityTypeConfiguration<UsersModel>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UsersModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.UserEmail).HasMaxLength(200);
            builder.Property(x => x.Password).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Role).IsRequired();
            builder.HasMany(x => x.Orders)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}
