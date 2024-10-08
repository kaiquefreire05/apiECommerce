﻿using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceApi.Database.Maps
{
    public class UserMap : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.UserEmail)
                   .HasMaxLength(200);

            builder.Property(x => x.Password)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Role)
                   .IsRequired();
        }
    }
}
